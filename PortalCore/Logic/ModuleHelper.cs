using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using PortalCommon;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class ModuleHelper
    {
        public List<Module> GetModules()
        {
            var db = new CoreDbContext();
            return db.Modules.OrderBy(m => m.Title).ToList();
        }

        public List<pcModule> GetModulesOnDisk()
        {
            var modulesList = new List<pcModule>();
            const string modulesFolderRelativePath = "~/Modules";
            var di = new DirectoryInfo(HttpContext.Current.Server.MapPath(modulesFolderRelativePath));
            DirectoryInfo[] moduleFolders = di.GetDirectories();
            foreach (var folder in moduleFolders)
            {
                string currentModulePhysicalPath = HttpContext.Current.Server.MapPath
                    (Path.Combine(modulesFolderRelativePath, folder.Name));
                string moduleConfigPhysicalFilePath = Path.Combine(currentModulePhysicalPath, "Module.Config");
                if (File.Exists(moduleConfigPhysicalFilePath))
                {
                    var currentModule = (pcModule)SerializationUtils.XmlToFromFile(moduleConfigPhysicalFilePath, typeof(pcModule));

                    var db = new CoreDbContext();
                    var moduleInDb = db.Modules.FirstOrDefault(m => m.Name == currentModule.ModuleName);
                    currentModule.IsInstalled = moduleInDb != null;
                    currentModule.IsActive = moduleInDb != null && moduleInDb.IsActive;
                    modulesList.Add(currentModule);
                }
            }
            return modulesList;
        }

        public Module GetModuleById(int moduleId)
        {
            var db = new CoreDbContext();
            return db.Modules.FirstOrDefault(m => m.ModuleId == moduleId);
        }

        #region Install/UnInstall Module
        public bool InstallModule(string moduleName)
        {
            var elogger = new ErrorLogger();
            var modulesList = GetModulesOnDisk();
            var thisModule = modulesList.FirstOrDefault(m => m.ModuleName == moduleName);
            if (thisModule != null)
            {
                #region Add connection strings to root config file
                if (thisModule.HasConnectionStrings)
                {
                    try
                    {
                        var configuration = WebConfigurationManager.OpenWebConfiguration("~");
                        var section = (ConnectionStringsSection)configuration.GetSection("connectionStrings");
                        string constr = section.ConnectionStrings["PortalConnectionString"].ConnectionString;
                        var builder = new SqlConnectionStringBuilder(constr);
                        string user = builder.UserID;
                        string pass = builder.Password;
                        string server = builder.DataSource;
                        string database = builder.InitialCatalog;
                        var config = WebConfigurationManager.OpenWebConfiguration
                            (HttpContext.Current.Request.ApplicationPath);
                        foreach (pcConnectionString cs in thisModule.ConnectionStrings)
                        {
                            var settings = new ConnectionStringSettings
                            {
                                Name = cs.Name,
                                ProviderName = cs.ProviderName,
                                ConnectionString = cs.ConnectionString
                            };

                            switch (settings.ProviderName)
                            {
                                case "System.Data.EntityClient":
                                    // Az inja ConnectionStringi ke male EntityFramework bashe
                                    var builder3 = new EntityConnectionStringBuilder(cs.ConnectionString);
                                    string md = builder3.Metadata;
                                    builder3.ConnectionString = "provider=System.Data.SqlClient;provider connection string=\"data source=" + server + ";initial catalog=" + database + ";persist security info=True;User ID=" + user + ";Password=" + pass + ";multipleactiveresultsets=True;App=EntityFramework\"";
                                    settings.ConnectionString = "metadata=" + md + ";" + builder3.ConnectionString;
                                    // Ta inja
                                    config.ConnectionStrings.ConnectionStrings.Add(settings);
                                    break;
                                case "System.Data.SqlClient":

                                    settings.ConnectionString =
                                        "Data Source=" + server + ";Initial Catalog=" + database + 
                                        ";Persist Security Info=True;User ID=" + user + ";Password=" + pass;
                                    config.ConnectionStrings.ConnectionStrings.Add(settings);
                                    break;
                            }
                        }
                        config.Save();
                    }
                    catch (Exception ex)
                    {
                        elogger.AddError("خطا در ثبت رشته های اتصال ماژول",
                            "هنگام ثبت رشته های اتصال ماژول خطای زیر رخ داد:<br/>"
                            + ex.Message, "InstallModule/Add ConnectionStrings to Web.Config file", 
                            "ModuleHelper.cs");
                        return false;
                    }
                }
                #endregion

                var db = new CoreDbContext();

                try
                {
                    #region Add module to Modules table in database

                    var module = new Module()
                    {
                        Name = thisModule.ModuleName,
                        Title = thisModule.ModuleTitle,
                        AdminFilePath = thisModule.AdminFilePath,
                        IsActive = true,
                        IsVital = thisModule.IsVitalModule,
                        Description = thisModule.ModuleDescription
                    };
                    db.Modules.Add(module);
                    db.SaveChanges();

                    #endregion

                    #region Add module permissions
                    #region Add permission group
                    var permissionGroup = new PermissionGroup()
                    {
                        ModuleId = module.ModuleId,
                        PermissionGroupTitle = module.Title
                    };
                    db.PermissionGroups.Add(permissionGroup);
                    db.SaveChanges();
                    #endregion

                    #region Add permissions
                    foreach (var permission in thisModule.Permissions)
                    {
                        db.Permissions.Add(new Permission()
                        {
                            PermissionGroupId = permissionGroup.PermissionGroupId,
                            PermissionName = permission.PermissionName,
                            PermissionTitle = permission.PermissionTitle
                        });
                        db.SaveChanges();
                    }
                    #endregion
                    #endregion

                    #region Add blocks

                    foreach (var block in thisModule.Blocks)
                    {
                        var dbBlock = new Block()
                        {
                            ModuleId = module.ModuleId,
                            Name = block.BlockName,
                            Title = block.BlockTitle,
                            Description = block.BlockDescription,
                            FilePath = block.BlockFilePath,
                            IsAdminBlock = block.IsAdminBlock
                        };
                        db.Blocks.Add(dbBlock);
                        db.SaveChanges();
                    }
                    #endregion

                    #region Add pages and page Layouts

                    foreach (var page in thisModule.Pages)
                    {
                        var dbPage = new Models.Page()
                        {
                            ModuleId = module.ModuleId,
                            Name = page.PageName,
                            Title = page.PageTitle
                        };
                        db.Pages.Add(dbPage);
                        db.SaveChanges();
                        foreach (var layout in page.DefaultLayout)
                        {
                            int blockId = (from b in db.Blocks
                                           where b.Name == layout.BlockName
                                           select b.BlockId).FirstOrDefault();
                            var pLayout = new PageLayout()
                            {
                                PageId = dbPage.PageId,
                                BlockId = blockId,
                                ZoneName = layout.PageZone.ZoneName,
                                AppearanceOrder = layout.AppearanceOrder
                            };
                            db.PageLayouts.Add(pLayout);
                            db.SaveChanges();
                        }
                    }
                    #endregion

                    return InstallSqlScript(moduleName);
                }
                catch (Exception ex)
                {
                    elogger.AddError("خطا در ثبت اطلاعات ماژول در بانک", "متن خطا : <br/>" + ex.Message,
                            HttpContext.Current.User.Identity.Name + "/InstallModule"
                            , "ModuleHelper.cs");
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        public bool UnInstallModule(string moduleName)
        {
            var elogger = new ErrorLogger();
            var allModules = GetModulesOnDisk();
            var thisModule = (from m in allModules where m.ModuleName == moduleName select m)
                .FirstOrDefault<pcModule>();
            if (thisModule != null)
            {
                #region Delete Module ConnectionStrings from Web.Config file
                if (thisModule.HasConnectionStrings)
                {
                    Configuration config = WebConfigurationManager.OpenWebConfiguration
                        (HttpContext.Current.Request.ApplicationPath);
                    foreach (pcConnectionString cs in thisModule.ConnectionStrings)
                    {
                        config.ConnectionStrings.ConnectionStrings.Remove(cs.Name);
                    }
                    config.Save();
                }
                #endregion
            }
            var db = new CoreDbContext();
            var module =
                db.Modules.FirstOrDefault(m => m.Name == moduleName);
            if (module != null)
            {
                int moduleId = module.ModuleId;

                #region Delet permissions and permission groups and role permissions from database

                try
                {
                    var pgroup =
                    db.PermissionGroups.FirstOrDefault(pg => pg.ModuleId == moduleId);
                    int pgroupid = 0;
                    if (pgroup != null)
                    {
                        pgroupid = pgroup.PermissionGroupId;
                    }

                    var permissions =
                        db.Permissions.Where(p => p.PermissionGroupId == pgroupid).ToList();
                    //Delete Permissions and Role Permissions
                    foreach (var permission in permissions)
                    {
                        var rolepermissions = db.RolePermissions
                            .Where(rp => rp.PermissionId == permission.PermissionId).ToList();
                        foreach (var rolepermission in rolepermissions)
                        {
                            db.RolePermissions.Remove(rolepermission);
                        }
                        db.Permissions.Remove(permission);
                    }
                    // Delete Permission Group
                    db.PermissionGroups.Remove(pgroup);
                }
                catch (Exception ex)
                {
                    elogger.AddError("خطا در حذف دسترسی های ماژول " + moduleName + " از بانک اطلاعاتی",
                        "زمان حذف دسترسی های ماژول خطای زیر رخ داده است : <br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/UnInstallModule/Delete Permissions,PermissionGroups,RolePermissions",
                        "ManageModules.aspx/UnInstallModule");
                }
                #endregion


                #region Delete Pages and PageLayouts from database

                try
                {
                    var pages =
                    db.Pages.Where(p => p.ModuleId == moduleId).ToList();
                    if (pages != null)
                    {
                        foreach (var page in pages)
                        {
                            var pageLayouts =
                                db.PageLayouts.Where(pl => pl.PageId == page.PageId).ToList();
                            foreach (var pageLayout in pageLayouts)
                            {
                                db.PageLayouts.Remove(pageLayout);
                            }
                            db.Pages.Remove(page);
                        }
                    }
                }
                catch (Exception ex)
                {
                    elogger.AddError("خطا در حذف صفحات ماژول " + moduleName + " از بانک اطلاعاتی",
                        "زمان حذف صفحات ماژول خطای زیر رخ داده است : <br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/UnInstallModule/Delete Pages and PageLayouts",
                        "ManageModules.aspx/UnInstallModule");
                }
                #endregion

                #region Delete Blocks from database

                try
                {
                    var blocks =
                    db.Blocks.Where(b => b.ModuleId == moduleId).ToList();
                    foreach (var block in blocks)
                    {
                        var pageLayouts =
                            db.PageLayouts.Where(pl => pl.BlockId == block.BlockId).ToList();
                        foreach (var pageLayout in pageLayouts)
                        {
                            db.PageLayouts.Remove(pageLayout);
                        }
                        db.Blocks.Remove(block);
                    }
                }
                catch (Exception ex)
                {
                    elogger.AddError("خطا در حذف بلاک های ماژول " + moduleName + " از بانک اطلاعاتی",
                        "زمان حذف بلاک های ماژول خطای زیر رخ داده است : <br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/UnInstallModule/Delete Blocks",
                        "ManageModules.aspx/UnInstallModule");
                }
                #endregion

                #region Delete Module from Modules table in database

                try
                {
                    db.Modules.Remove(module);
                }
                catch (Exception ex)
                {
                    elogger.AddError("خطا در حذف اطلاعات ماژول " + moduleName + " از جدول ماژول ها",
                        "زمان حذف اطلاعات ماژول از جدول ماژول ها خطای زیر رخ داده است : <br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/UnInstallModule/Delete Module From Database"
                        , "ManageModules.aspx/UnInstallModule");
                }

                #endregion

                #region Save all changes to database

                db.SaveChanges();

                #endregion

                #region Call UnInstallSqlScript for deleting Module tables from databas

                if (!UninstallSqlScript(moduleName))
                {
                    elogger.AddError("خطا در حذف جداول ماژول از بانک اطلاعاتی",
                        "زمان حذف جداول ماژول " + moduleName + " از بانک اطلاعاتی خطایی رخ داده است",
                        HttpContext.Current.User.Identity.Name + "/UnInstallModule/UnInstallModuleSqlScript"
                        , "ManageModules.aspx/UnInstallModule");
                }

                #endregion

                return true;

                // Zamane UnInstall Kardane Module Ma Nemikhaim File haye Module Hazf Beshan
                // Chon Ke Dobare Mikhad Install Kone
                // Tou Hazfe Kolie Module Miyaim Hameye Fileha va Directoriha Ro Hazf mikonim

                //#region Delete module files and directories
                //string moduleFilesPath = Server.MapPath("~/ModuleFiles/" + moduleName);
                //DeleteDirectory(moduleFilesPath);
                //#endregion
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region Install SQL Scripts for Module
        private bool InstallSqlScript(string moduleName)
        {
            var elogger = new ErrorLogger();
            bool result = false;
            string modulePath = "~/Modules/" + moduleName;
            string sqlScriptFileName = modulePath + "/Install.sql";
            string sqlFullFileName = HttpContext.Current.Server.MapPath(sqlScriptFileName);
            if (File.Exists(sqlFullFileName))
            {
                StreamReader stream = File.OpenText(sqlFullFileName);
                string sql = stream.ReadToEnd();

                string[] sqlCommands = sql.Split(new string[] { "GO" }, StringSplitOptions.None);

                SqlConnection connection = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["PortalConnectionString"].ConnectionString);

                try
                {
                    connection.Open();

                    foreach (string cmdText in sqlCommands)
                    {
                        if (!String.IsNullOrEmpty(cmdText))
                        {
                            SqlCommand cmd = new SqlCommand(cmdText, connection);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    result = true;
                }
                catch (Exception exp)
                {
                    result = false;
                    elogger.AddError("خطا در ساخت جداول ماژول " + moduleName,
                        "زمان اجرای اسکریپت ساخت جداول خطای زیر رخ داد : <br/>" + exp.Message,
                        HttpContext.Current.User.Identity.Name + "/InstallSqlScript"
                        , "ManageModules.aspx/InstallSqlScript");
                }
                finally
                {
                    connection.Close();
                }

            }
            return result;
        }
        private bool UninstallSqlScript(string moduleName)
        {
            var elogger = new ErrorLogger();
            bool result = false;
            string modulePath = "~/Modules/" + moduleName;
            string sqlScriptFileName = modulePath + "/Uninstall.sql";
            string sqlFullFileName = HttpContext.Current.Server.MapPath(sqlScriptFileName);
            if (File.Exists(sqlFullFileName))
            {
                StreamReader stream = File.OpenText(sqlFullFileName);
                string sql = stream.ReadToEnd();

                string[] sqlCommands = sql.Split(new string[] { "GO" }, StringSplitOptions.None);

                SqlConnection connection = new SqlConnection(ConfigurationManager
                    .ConnectionStrings["PortalConnectionString"].ConnectionString);

                try
                {
                    connection.Open();

                    foreach (string cmdText in sqlCommands)
                    {
                        if (!String.IsNullOrEmpty(cmdText))
                        {
                            SqlCommand cmd = new SqlCommand(cmdText, connection);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    result = true;
                }
                catch (Exception exp)
                {
                    result = false;
                    elogger.AddError("خطا در حذف جداول ماژول " + moduleName,
                        "زمان اجرای اسکریپت حذف جداول خطای زیر رخ داد : <br/>" + exp.Message,
                        HttpContext.Current.User.Identity.Name + "/UnInstallSqlScript"
                        , "ManageModules.aspx/UnInstallSqlScript");
                }
                finally
                {
                    connection.Close();
                }

            }
            return result;
        }
        #endregion


        #region Extra Methods

        // Delete Directory and Files
        public bool DeleteDirectory(string targetDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(targetDirectory);
                string[] dirs = Directory.GetDirectories(targetDirectory);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(targetDirectory, false);
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در حذف پوشه ها و فایلها",
                    "زمان حذف فایل ها و پوشه های  " + targetDirectory + " خطای زیر رخ داد:<br/>" + ex.Message,
                     HttpContext.Current.User.Identity.Name + "/DeleteDirectory"
                     ,"ModuleHelper.cs/DeleteDirectory");
                return false;
            }
        }

        // Unzip Uploaded Module Method
        public bool ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا زمان گشودن فایل فشرده",
                    "هنگام گشودن فایل " + archiveFilenameIn + "خطای زیر رخ داد:<br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/ExtractZipFile"
                    , "ModuleHelper.cs/ExtractZipFile");
                return false;
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                    
                }
            }
        }
        #endregion
    }
}