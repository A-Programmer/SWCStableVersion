using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCore.Logic;
using PortalCore.Models;
using PortalCommon;

namespace PortalCore.Admin
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("EditProfile"))
            {
                Response.Redirect("~/Error/403");
            }
            if (!Page.IsPostBack)
            {
                SetValues();
            }
            CheckPermission();
        }

        public void CheckPermission()
        {
            btnSubmitProfile.Enabled = PermissionControl.CheckPermission("SubmitProfileChanges");
        }

        public void SetValues()
        {
            var db = new CoreDbContext();
            string username = Page.User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                ltUserName.Text = username;
                var userroleid = db.UserRoles.FirstOrDefault(ur => ur.UserId == user.UserId);
                if (userroleid != null)
                {
                    var role = db.Roles.FirstOrDefault(r => r.RoleId == userroleid.RoleId);
                    if (role != null)
                    {
                        txtUserRole.Text = role.RoleTitle;
                        imgProfile.ImageUrl = user.UserPicture;
                        ViewState["UserImage"] = user.UserPicture;
                        txtFirstName.Text = user.FirstName;
                        txtLastName.Text = user.LastName;
                        txtEmail.Text = user.Email;
                        txtPhone.Text = user.Phone;
                        userinfo.InnerText = user.UserInfo;
                    }
                }
            }
        }

        protected void btnSubmitProfile_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (fuProfileImage.HasFile)
                {
                    HttpPostedFile profileImage = fuProfileImage.PostedFile;
                    profileImage.SaveAs(System.IO.Path.Combine(Server.MapPath("~/UserImage/"), Page.User.Identity.Name + "-" + profileImage.FileName));
                    ViewState["UserImage"] = "~/UserImage/" + Page.User.Identity.Name + "-" + profileImage.FileName;
                }
                var uhelper = new UserHelper();
                if (uhelper.EditUser(Page.User.Identity.Name, txtEmail.Text, txtFirstName.Text, txtLastName.Text,
                    txtPhone.Text, userinfo.InnerText, ViewState["UserImage"].ToString()))
                {
                    ltProfileMessage.Text = "مشخصات شما با موفقیت تغییر یافت";
                    var nhelper = new NotificationHelper();
                    nhelper.AddNotification("پروفایل شما تغییر کرد.", "کاربر گرامی "
                                                                      + Page.User.Identity.Name +
                                                                      " پروفایل شما با موفقیت تغییر کرد.",
                        Page.User.Identity.Name);
                    SetValues();
                }
                else
                {
                    ltProfileMessage.Text = "مشکلی در تغییر مشخصات رخ داده است، با مدیر وب سایت تماس بگیرید.";
                }
            }
        }
    }
}