using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCommon;

namespace ModuleCoursesAndAds.Modules.ModuleCoursesAndAds
{
    public partial class CCF : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string ModuleConfigFilePath = Server.MapPath("~/Modules/ModuleCoursesAndAds/Module.config");

            if (File.Exists(ModuleConfigFilePath))
            {
                File.Delete(ModuleConfigFilePath);
            }


            // Add Module
            pcModule pcm = new pcModule();

            pcm.ModuleName = "ModuleCoursesAndAds";
            pcm.ModuleTitle = "ماژول تجاری";

            pcm.SupportsDatabaseInstall = true;
            pcm.SupportsDatabaseUninstall = true;

            pcm.AdminFilePath = "~/Modules/ModuleCoursesAndAds/Admin/ModuleCoursesAdnAdsAdmin.ascx";


            // Add Blocks
            pcm.Blocks.Add(new pcModuleBlock()
            {
                BlockTitle = "بلاک تبلیغات",
                BlockDescription = "نمایش تبلیغات ثبت شده در سایت",
                BlockFilePath = "~/Modules/ModuleCoursesAndAds/Blocks/Ads.ascx",
                BlockName = "Ads"
            });

            pcm.Blocks.Add(new pcModuleBlock()
            {
                BlockTitle = "بلاک دوره ها",
                BlockDescription = "لیست دوره های در حال ثبت نام",
                BlockFilePath = "~/Modules/ModuleCoursesAndAds/Blocks/Courses.ascx",
                BlockName = "Courses"
            });

            pcm.Blocks.Add(new pcModuleBlock()
            {
                BlockTitle = "بلاک فروشگاه",
                BlockDescription = "لیست محصولات فروشگاه",
                BlockFilePath = "~/Modules/ModuleCoursesAndAds/Blocks/Store.ascx",
                BlockName = "Store"
            });

            pcm.Blocks.Add(new pcModuleBlock()
            {
                BlockTitle = "بلاک جزئیات",
                BlockDescription = "نمایش جزئیات محصول",
                BlockFilePath = "~/Modules/ModuleCoursesAndAds/Blocks/Commers.ascx",
                BlockName = "Commers"
            });

            // Add Permissions :
            pcm.Permissions.Add(new pcPermissions()
                                    {
                                        PermissionName = "EditCommers",
                                        PermissionTitle = "ویرایش آیتم"
                                    });

            pcm.Permissions.Add(new pcPermissions()
                                    {
                                        PermissionName = "DeleteCommers",
                                        PermissionTitle = "حذف آیتم"
                                    });

            // Add Pages
            pcm.Pages.Add(new pcModulePage()
                              {
                                  PageTitle = "نمایش جزئیات آیتم های تجاری",
                                  PageName = "Commers"
                              });
            

            // Check If This Project Has ConnectionStrings in Web.Config
            Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            if (config.ConnectionStrings.ConnectionStrings.Count != 0)
            {
                pcm.HasConnectionStrings = true;

                foreach (ConnectionStringSettings settings in config.ConnectionStrings.ConnectionStrings)
                {
                    if (settings.Name != "LocalSqlServer")
                    {
                        pcm.ConnectionStrings.Add(new pcConnectionString()
                        {
                            Name = settings.Name,
                            ConnectionString = settings.ConnectionString.Replace("\"", "&quot;"),
                            ProviderName = settings.ProviderName
                        });
                    }
                }
            }

            string serialized = SerializationUtils.ToXml(pcm, false).Replace("&amp;quot;", "&quot;");

            StreamWriter writer = new StreamWriter(ModuleConfigFilePath);
            writer.Write(serialized);
            writer.Close();
        }
    }
}