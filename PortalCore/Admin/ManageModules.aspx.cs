using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using PortalCommon;
using PortalCore.Logic;
using PortalCore.Models;

namespace PortalCore.Admin
{
    public partial class ManageModules : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!PermissionControl.CheckPermission("ManageModules"))
            {
                Response.Redirect("~/Error/403");
            }
            CheckPermissions();
            if (!Page.IsPostBack)
            {
                BindModulesGrid();
            }
            LoadAdminUserControl();
        }

        public void BindModulesGrid()
        {
            var moduleHelper = new ModuleHelper();
            grdModules.DataSource = moduleHelper.GetModulesOnDisk();
            grdModules.DataBind();
        }

        public void LoadAdminUserControl()
        {
            var segments = Request.GetFriendlyUrlSegments().ToList();
            string moduleName = segments.FirstOrDefault();
            if (moduleName != null)
            {
                var db = new CoreDbContext();
                var module = db.Modules.FirstOrDefault(m => m.Name == moduleName);
                if (module != null)
                {
                    var adminFilePath = module.AdminFilePath;

                    if (File.Exists(Server.MapPath(adminFilePath)))
                    {
                        pnlModuleAdminFile.Controls.Add(Page.LoadControl(adminFilePath));
                        mvManageModules.SetActiveView(vwModuleAdmin);
                    }
                    else
                    {
                        ltModuleMessage.Text = "فایل مدیریت ماژول یافت نشد.";
                        mvManageModules.SetActiveView(vwModules);
                    }
                }
                else
                {
                    ltModuleMessage.Text = "ماژول مورد نظر نصب نشده است.";
                }
            }
        }

        public void CheckPermissions()
        {
            pnlUploadModule.Enabled = PermissionControl.CheckPermission("UploadModule");
            btnUploadModule.Enabled = PermissionControl.CheckPermission("UploadModule");
            grdModules.Visible = PermissionControl.CheckPermission("ModulesList");
        }

        protected void grdModules_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdModules.PageIndex = e.NewPageIndex;
            BindModulesGrid();
        }

        protected void grdModules_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "InstallUnInstall":
                {
                    var moduleHelper = new ModuleHelper();
                    var moduleName = e.CommandArgument.ToString();
                    var module = moduleHelper.GetModulesOnDisk().FirstOrDefault(m => m.ModuleName == moduleName);
                    if (module != null)
                    {
                        if (module.IsInstalled)
                        {
                            if (PermissionControl.CheckPermission("UninstallModule"))
                            {
                                #region Uninstall Mdule
                                    // Do UnInstall
                                    ltModuleMessage.Text = moduleHelper.UnInstallModule(moduleName) ? "ماژول مورد نظر با موفقیت لغو شد" : "خطا در لغو ماژول.";
                                #endregion
                            }
                            else
                            {
                                ltModuleMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                            }
                        }
                        else
                        {
                            if (PermissionControl.CheckPermission("InstallModule"))
                            {
                                #region Install Module
                                if (moduleHelper.InstallModule(moduleName))
                                {
                                    ltModuleMessage.Text = "ماژول مورد نظر با موفقیت نصب شد.";
                                    // 7 Days module license
                                    #region Add module License Key (for 7 days)
                                    var firstKeyName = moduleName;
                                    var siteName = Request.Url.Host.ToLower();
                                    var notEncryptedFirstKey = siteName + moduleName;
                                    var ed = new EncryptDecrypt();
                                    var firstKeyValue = ed.Encrypt(notEncryptedFirstKey);
                                    var seccondKeyName = moduleName + "ex";
                                    var seccondKeyValue = ed.Encrypt
                                        (DateTime.Now.AddDays(7) + "|" + siteName + "|" + moduleName);
                                    var licenseHelper = new LicenseHelper();
                                    if (licenseHelper.AddLicense(firstKeyName, firstKeyValue) && licenseHelper.AddLicense(seccondKeyName, seccondKeyValue))
                                    {
                                        ltModuleMessage.Text += "مجوز با موفقیت ثبت شد.";
                                    }
                                    else
                                    {
                                        ltModuleMessage.Text = "مشکلی در ثبت مجوز ها رخ داده است.";
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ltModuleMessage.Text = "خطا در نصب ماژول.";
                                }
                                #endregion
                            }
                            else
                            {
                                ltModuleMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                            }
                        }
                    }
                    else
                    {
                        ltModuleMessage.Text = "امکانات مورد نظر یافت نشد.";
                    }
                    BindModulesGrid();
                    BindMasterPageModulesList();
                    break;
                }
                case "DoDelete":
                {
                    if (PermissionControl.CheckPermission("DeleteModule"))
                    {
                        var moduleName = e.CommandArgument.ToString();
                        var moduleHelper = new ModuleHelper();
                        if (IsInstalled(moduleName))
                        {
                            ltModuleMessage.Text = "ماژول در حالت نصب است، برای حذف آن ابتدا آن را لغو کنید.";
                        }
                        else
                        {
                            var modulePath = Server.MapPath("~/Modules/" + moduleName);
                            var moduleFilesPath = Server.MapPath("~/ModuleFiles/" + moduleName);
                            #region Delete module license keys
                            var licenseHelper = new LicenseHelper();
                            var firstModuleKey = moduleName;
                            var seccondModuleKey = moduleName + "ex";
                            #endregion
                            if (moduleHelper.DeleteDirectory(modulePath) && moduleHelper.DeleteDirectory(moduleFilesPath))
                            {
                                ltModuleMessage.Text = "ماژول مورد نظر با موفقیت حذف شد.";
                                #region Delete module license Keys
                                if (licenseHelper.DeleteLicense(firstModuleKey) &&
                                    licenseHelper.DeleteLicense(seccondModuleKey))
                                {
                                    ltModuleMessage.Text += "مجوز ها با موفقیت حذف شدند.";
                                }
                                else
                                {
                                    ltModuleMessage.Text += "خطا در حذف مجوزها";
                                }
                                #endregion
                            }
                            else
                            {
                                ltModuleMessage.Text = "برخی از فایلهای ماژول حذف نشدند.";
                            }
                            BindModulesGrid();
                        }
                    }
                    else
                    {
                        ltModuleMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
            }
        }

        public bool GetAccessForInstallUnInstall(string moduleName)
        {
            var moduleHelper = new ModuleHelper();
            var module = moduleHelper.GetModulesOnDisk().FirstOrDefault(m => m.ModuleName == moduleName);
            if (module != null)
            {
                if (module.IsInstalled)
                {
                    return PermissionControl.CheckPermission("UninstallModule");
                }
                else
                {
                    return PermissionControl.CheckPermission("InstallModule");
                }
            }
            else
            {
                return false;
            }
        }

        public bool GetAccess(string permissionName)
        {
            return PermissionControl.CheckPermission(permissionName);
        }

        public string GetTitle(string modulename)
        {
            var moduleHelper = new ModuleHelper();
            var module = moduleHelper.GetModulesOnDisk().FirstOrDefault(m => m.ModuleName == modulename);
            if (module != null)
            {
                if (module.IsInstalled)
                {
                    return "لغو نصب";
                }
                else
                {
                    return "نصب";
                }
            }
            else
            {
                return "این امکان ناقص است";
            }
        }

        public bool IsInstalled(string moduleName)
        {
            var moduleHelper = new ModuleHelper();
            var module = moduleHelper.GetModulesOnDisk().FirstOrDefault(m => m.ModuleName == moduleName);
            if (module != null)
            {
                if (module.IsInstalled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        protected void btnUploadModule_OnClick(object sender, EventArgs e)
        {
            if (fuModule.HasFile)
            {
                var moduleExtension = Path.GetExtension(fuModule.FileName);
                if (!String.IsNullOrEmpty(moduleExtension) && moduleExtension.ToLower() == ".module")
                {
                    try
                    {
                        var moduleHelper = new ModuleHelper();
                        string filename = Path.GetFileNameWithoutExtension(fuModule.FileName);
                        fuModule.SaveAs(Server.MapPath("~/") + filename + ".zip");
                        moduleHelper.ExtractZipFile(Server.MapPath("~/") + filename + ".zip",
                            "1568KSModule6413",Server.MapPath("~/"));
                    }
                    catch (Exception ex)
                    {
                        var elogger = new ErrorLogger();
                        elogger.AddError("خطا در ارسال ماژول",
                            "در زمان ارسال فایل ماژول خطای زیر رخ داد:<br/>" + ex.Message,
                            "btnUploadModule","ManageModules.aspx");
                        ltModuleMessage.Text = "خطای ارسال ماژول :<br/>" + ex.Message;
                    }
                    string fileToDelete = Path.GetFileNameWithoutExtension(fuModule.FileName);
                    var filePath = Server.MapPath("~/" + fileToDelete + ".zip");
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    BindMasterPageModulesList();
                }
            }
            else
            {
                ltModuleMessage.Text = "فایلی برای ارسال انتخاب نشده است.";
            }
        }

        public void BindMasterPageModulesList()
        {
            var rpModules = (Repeater)Page.Master.FindControl("rptInstalledModule");
            var mhelper = new ModuleHelper();
            rpModules.DataSource = mhelper.GetModules();
            rpModules.DataBind();

        }
    }
}