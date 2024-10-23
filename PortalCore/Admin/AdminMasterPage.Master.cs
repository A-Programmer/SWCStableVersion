using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.FriendlyUrls;
using PortalCore.Logic;
using PortalCore.Models;
using PortalCommon;

namespace PortalCore.Admin
{
    public partial class AdminMasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //بعد از تکمیل پروژه دسترسی ها رو ست کنم تو همین تابع و از حالت کامنت در بیاد

            CheckPermissions();


            string PageName = Path.GetFileNameWithoutExtension(Request.GetFriendlyUrlFileVirtualPath()).ToLower();
            if (PageName != "managelicense")
            {
                var lh = new LicenseHelper();
                if (!lh.CheckCoreLicense())
                {
                    Response.Redirect("~/Admin/ManageLicense");
                }
            }
            SetValues();
            BindModulesList();
        }

        public string GetUserName()
        {
            var username = Page.User.Identity.Name;
            using (var db = new CoreDbContext())
            {
                var user =
                    db.Users.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    if (!string.IsNullOrEmpty(user.FirstName) || !string.IsNullOrEmpty(user.LastName))
                    {
                        return user.FirstName + " " + user.LastName;
                    }
                    else
                    {
                        return user.UserName;
                    }
                }
                else
                {
                    return "کاربر ناشناس";
                }
            }
        }

        public User GetUser()
        {
            string username = Page.User.Identity.Name;
            var db = new CoreDbContext();
            return db.Users.FirstOrDefault(u => u.UserName == username);
        }

        public void CheckPermissions()
        {
            if (!Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Error/403");
            }

            //بقیه دسترسی ها رو چک کنم
            //...

            liManageUsers.Visible = PermissionControl.CheckPermission("ManageUsers");
            liManageRoles.Visible = PermissionControl.CheckPermission("ManageRoles");
            liManageModules.Visible = PermissionControl.CheckPermission("ManageModules");
            liManagePages.Visible = PermissionControl.CheckPermission("ManagePages");
            liManageTemplates.Visible = PermissionControl.CheckPermission("ManageTemplates");
            liCoreUpdate.Visible = PermissionControl.CheckPermission("CoreUpdates");
            liManageLicense.Visible = PermissionControl.CheckPermission("Licence");
            liTopSettings.Visible = PermissionControl.CheckPermission("GeneralSettings");
            liUserProfile.Visible = PermissionControl.CheckPermission("EditProfile");
            btnChangePassword.Enabled = PermissionControl.CheckPermission("SubmitChangePassword");
            liChangePassword.Visible = PermissionControl.CheckPermission("ChangePassword");
            liGeneralSettings.Visible = PermissionControl.CheckPermission("GeneralSettings");
            liEmailSettings.Visible = PermissionControl.CheckPermission("EmailSettings");
            liPermissions.Visible = PermissionControl.CheckPermission("ManagePermissions");

        }

        public bool CheckModulePermission(string modulename)
        {
            return PermissionControl.CheckPermission(modulename);
        }

        public void BindModulesList()
        {
            var mhelper = new ModuleHelper();
            rptInstalledModule.DataSource = mhelper.GetModules();
            rptInstalledModule.DataBind();
        }

        public void SetValues()
        {
            if (Page.User.Identity.IsAuthenticated)
            {
                var nhelper = new NotificationHelper();
                ltNotificationCount.Text = nhelper.GetNewNotificationsCount(Page.User.Identity.Name).ToString();
                ltNotifiesCount.Text = nhelper.GetNewNotificationsCount(Page.User.Identity.Name).ToString();
                BindNotifications();
                ltUserName.Text = GetUserName();
                imgProfile.Src = GetUser().UserPicture;
            }
            
        }

        public void BindNotifications()
        {
            var nhelper = new NotificationHelper();
            var username = Page.User.Identity.Name;
            rptNotifications.DataSource = nhelper.GetNewNotifications(username);
            rptNotifications.DataBind();
            rptContent.DataSource = nhelper.GetNewNotifications(username);
            rptContent.DataBind();
        }

        protected void btnChangePassword_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var uhelper = new UserHelper();
                if (uhelper.ChangePassword(Page.User.Identity.Name, txtCurrentPassword.Text, txtNewPassword.Text))
                {
                    //Thread.Sleep(2000);
                    var nheloper = new NotificationHelper();
                    nheloper.AddNotification("رمز شما با موفقیت تغییر یافت",
                        "کاربر عزیز " + Page.User.Identity.Name + " کلمه عبور شما با موفقیت تغییر یافت.",
                        Page.User.Identity.Name);
                    ltChangePasswordMessage.Text = "کلمه عبور شما با موفقیت تغییر یافت";
                    BindNotifications();
                }
                else
                {
                    ltChangePasswordMessage.Text = "تغییر کلمه عبور با مشکل مواجه شده است";
                }
            }
        }

        protected void lbSignOut_OnClick(object sender, EventArgs e)
        {
            var userHelper = new UserHelper();
            userHelper.LogOut();
        }
    }
}