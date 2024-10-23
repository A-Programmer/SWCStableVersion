using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCore.Logic;
using PortalCommon;

namespace PortalCore.Admin
{
    public partial class EmailSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("EmailSettings"))
            {
                Response.Redirect("~/Error/403");
            }
            CheckPermissions();
            if (!Page.IsPostBack)
            {
                GetData();
            }
        }

        public void CheckPermissions()
        {
            btnSubmitEmailSettings.Enabled = PermissionControl.CheckPermission("SubmitEmailSettings");
        }

        protected void btnSubmitEmailSettings_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var settingshelper = new SettingsHelper();
                var port = Convert.ToInt32(txtPort.Text);
                ltSettingsMessage.Text = settingshelper.EditEmailSettings(txtEmail.Text, txtPassword.Text, txtSmtpServer.Text, port) ? "تنظیمات ایمیل با موفقیت ثبت شد." : "ویرایش تنظیمات ایمیل با خطا مواجه شده است.";
            }
            else
            {
                ltSettingsMessage.Text = "فرم را با دقت پر کنید.";
            }
        }

        public void GetData()
        {
            var settingshelper = new SettingsHelper();
            var settings = settingshelper.GetEmailSettings();
            if (settings != null)
            {
                txtEmail.Text = settings.EmailAddress;
                txtPassword.Text = settings.EmailPassword;
                txtSmtpServer.Text = settings.EmailSmtpAddress;
                txtPort.Text = settings.EmailSmtpPort.ToString();
            }
        }

    }
}