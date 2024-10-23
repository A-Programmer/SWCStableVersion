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
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("GeneralSettings"))
            {
                Response.Redirect("~/Error/403");
            }
            CheckPermission();
            if (!Page.IsPostBack)
            {
                GetData();
                BindRoles();
                BindTemplates();
            }
        }

        public void CheckPermission()
        {
            btnSubmitSettings.Enabled = PermissionControl.CheckPermission("SubmitGeneralSettings");
        }

        protected void btnSubmitSettings_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var defaultRoleId = Convert.ToInt32(ddlRoles.SelectedItem.Value);
                    var activeTemplateId = Convert.ToInt32(ddlTemplates.SelectedItem.Value);
                    var pendingDays = Convert.ToInt32(txtPendingDays.Text);
                    var today = DateTime.Now;
                    var runDate = today.AddDays(pendingDays);
                    var settingsHelper = new SettingsHelper();
                    var homeUrl = Request.Url.Host.ToLower();
                    ltSettingsMessage.Text = settingsHelper.EditSettings(homeUrl,txtTitle.Text,chkStatus.Checked,runDate,chkEnableRegistration.Checked,
                        chkUnlockRegistration.Checked,/*chkEnableExternalLogin.Checked,*/chkEnablePasswordRecovery.Checked,
                        defaultRoleId,ddlTemplates.SelectedItem.Text,activeTemplateId,txtTags.Text,
                        txtDescription.Text) ? "تنظیمات با موفقیت ویرایش شدند." : "مشکلی در ویرایش تنظیمات به وجود آمده است.";
                }
                catch (Exception ex)
                {
                    var eLogger = new ErrorLogger();
                    eLogger.AddError("خطا در ویرایش تنظیمات عمومی",
                        "خطا در زمان ویرایش تنظیمات توسط کاربر:<br/>" + ex.Message,
                        Page.User.Identity.Name + "/btnSubmitSettings_OnClick",
                        "Settings.aspx/btnSubmitSettings_OnClick");
                    ltSettingsMessage.Text = "خطا در ویرایش تنظیمات :<br/>" + ex.Message;
                }
            }
        }

        public void GetData()
        {
            var settingsHelper = new SettingsHelper();
            var currentSettings = settingsHelper.GetSettings();
            if (currentSettings != null)
            {
                txtTitle.Text = currentSettings.Title;
                chkStatus.Checked = currentSettings.Status;
                chkEnableRegistration.Checked = currentSettings.EnableRegistration;
                chkUnlockRegistration.Checked = currentSettings.EnableUnlockRegistratin;
                //chkEnableExternalLogin.Checked = currentSettings.AllowExternalLogin;
                chkEnablePasswordRecovery.Checked = currentSettings.EnablePasswordRecovery;
                ddlRoles.SelectedValue = currentSettings.DefaultUserRoleId.ToString();
                ddlTemplates.SelectedValue = currentSettings.ActiveTemplateId.ToString();
                txtTags.Text = currentSettings.Tags;
                txtDescription.Text = currentSettings.Description;
                if (currentSettings.RunDate != null)
                {
                    var today = DateTime.Now;
                    var runDate = Convert.ToDateTime(currentSettings.RunDate);
                    var pendigDays = (runDate - today).Days;
                    if (pendigDays >= 0)
                    {
                        txtPendingDays.Text = pendigDays.ToString();
                    }
                    else
                    {
                        txtPendingDays.Text = "0";
                    }
                }
                else
                {
                    txtPendingDays.Text = "0";
                }

            }
        }

        public void BindRoles()
        {
            var roleHelper = new RoleHelper();
            ddlRoles.DataSource = roleHelper.GetRoles();
            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataValueField = "RoleId";
            ddlRoles.DataBind();
        }

        public void BindTemplates()
        {
            var templatesHelper = new TemplatesHelper();
            ddlTemplates.DataSource = templatesHelper.GetTemplates();
            ddlTemplates.DataTextField = "Title";
            ddlTemplates.DataValueField = "Id";
            ddlTemplates.DataBind();
        }

    }
}