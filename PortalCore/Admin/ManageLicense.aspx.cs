using System;
using System.Web.UI.WebControls;
using PortalCommon;
using PortalCore.Logic;

namespace PortalCore.Admin
{
    public partial class ManageLicense : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("Licence"))
            {
                Response.Redirect("~/Error/403");
            }
            CheckPermissions();
            if (!Page.IsPostBack)
            {
                GetCoreLicense();
                BindModules();
            }
            ltCoreLicenseStatus.Text = GetCoreLicenseStatus();
        }

        public bool GetAccess(string permissionName)
        {
            return PermissionControl.CheckPermission(permissionName);
        }

        public void CheckPermissions()
        {
            btnSubmitCoreLicense.Enabled = PermissionControl.CheckPermission("SubmitCoreLicense");
            btnSubmitModuleLicense.Enabled = PermissionControl.CheckPermission("SubmitModuleLicense");
        }

        public void BindModules()
        {
            var moduleHelper = new ModuleHelper();
            grdModules.DataSource = moduleHelper.GetModulesOnDisk();
            grdModules.DataBind();
        }

        public void GetCoreLicense()
        {
            var licenseHelper = new LicenseHelper();
            txtCoreFirstKey.Text = licenseHelper.CoreFirstLicense();
            txtCoreSeccondKey.Text = licenseHelper.CoreSeccondLicense();
        }

        public string CheckModuleLicense(string moduleName)
        {
            var licenseHelper = new LicenseHelper();
            return licenseHelper.GetModuleLicenseStatus(moduleName);
        }

        public string GetCoreLicenseStatus()
        {
            var licenseHelper = new LicenseHelper();
            return licenseHelper.GetCoreLicenseStatus();
        }
        public string GetModuleFirstLicense(string moduleName)
        {
            var licenseHelper = new LicenseHelper();
            return licenseHelper.FirstModuleLicense(moduleName);
        }
        public string GetModuleSeccondLicense(string moduleName)
        {
            var licenseHelper = new LicenseHelper();
            return licenseHelper.SeccondModuleLicense(moduleName);
        }

        protected void grdModules_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdModules.PageIndex = e.NewPageIndex;
            BindModules();
        }

        protected void grdModules_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            var moduleName = e.CommandArgument.ToString();
            switch (e.CommandName)
            {
                case "SetLicense":
                {
                    if (PermissionControl.CheckPermission("EditModuleLicense"))
                    {
                        ViewState["ModuleName"] = moduleName;
                        txtModuleFirstKey.Text = GetModuleFirstLicense(moduleName);
                        txtModuleSeccond.Text = GetModuleSeccondLicense(moduleName);
                        mvModulesLicense.SetActiveView(vwEditModuleLicense);
                    }
                    else
                    {
                        ltLicenseMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
                case "DeleteLicense":
                {
                    if (PermissionControl.CheckPermission("DeleteModuleLicense"))
                    {
                        var licenseHelper = new LicenseHelper();
                        if (!licenseHelper.CheckFirstModuleLicense(moduleName))
                        {
                            ltLicenseMessage.Text = "کلید اول یافت نشد.<br/>";
                        }
                        else
                        {
                            var firstKeyName = moduleName;
                            ltLicenseMessage.Text = licenseHelper.DeleteLicense(firstKeyName)
                                ? "کلید اول حذف شد.<br/>"
                                : "<br/>خطا در حذف کلید اول";
                            BindModules();
                        }
                        if (!licenseHelper.CheckSeccondModuleLicense(moduleName))
                        {
                            ltLicenseMessage.Text += "کلید دوم موجود نیست.";
                        }
                        else
                        {
                            var seccondKeyName = moduleName + "ex";
                            ltLicenseMessage.Text += licenseHelper.DeleteLicense(seccondKeyName)
                                ? "کلید دوم حذف شد."
                                : "خطا در حذف کلید دوم.";
                            BindModules();
                        }
                    }
                    else
                    {
                        ltLicenseMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
            }
        }

        protected void btnSubmitModuleLicense_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var moduleName = ViewState["ModuleName"].ToString();
                var firstKeyName = moduleName;
                var seccondKeyName = moduleName + "ex";
                var licenseHelper = new LicenseHelper();
                if (licenseHelper.CheckFirstModuleLicense(moduleName))
                {
                    ltLicenseMessage.Text = licenseHelper.EditLicense(firstKeyName, txtModuleFirstKey.Text) ? "کلید اول با موفقیت ویرایش شد.<br/>" : "خطا در ویرایش کلید اول.<br/>";
                }
                else
                {
                    ltLicenseMessage.Text = licenseHelper.AddLicense(firstKeyName, txtModuleFirstKey.Text) ? "کلید اول با موفقیت ثبت شد.<br/>" : "خطا در ثبت کلید اول.<br/>";
                }
                if (licenseHelper.CheckSeccondModuleLicense(moduleName))
                {
                    ltLicenseMessage.Text += licenseHelper.EditLicense(seccondKeyName, txtModuleSeccond.Text) ? "کلید دوم با موفقیت ویرایش شد." : "خطا در ویرایش کلید دوم.";
                }
                else
                {
                    ltLicenseMessage.Text += licenseHelper.AddLicense(seccondKeyName, txtModuleSeccond.Text) ? "کلید دوم با موفقیت ثبت شد.<br/>" : "خطا در ثبت کلید دوم.<br/>";
                }
                BindModules();
                mvModulesLicense.SetActiveView(vwModulesList);
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            txtModuleFirstKey.Text = string.Empty;
            txtModuleSeccond.Text = string.Empty;
            mvModulesLicense.SetActiveView(vwModulesList);
        }

        protected void btnSubmitCoreLicense_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var licenseHelper = new LicenseHelper();
                var firstKeyName = Request.Url.Host.ToLower() + "core";
                var seccondKeyName = Request.Url.Host.ToLower() + "ex";
                if (licenseHelper.CheckCoreLicenseExistance(firstKeyName))
                {
                    ltLicenseMessage.Text = licenseHelper.EditLicense(firstKeyName, txtCoreFirstKey.Text)
                        ? "کلید اول هسته با موفقیت ویرایش شد.<br/>"
                        : "خطا در ویرایش کلید اول هسته.<br/>";
                }
                else
                {
                    ltLicenseMessage.Text = licenseHelper.AddLicense(firstKeyName, txtCoreFirstKey.Text)
                        ? "کلید اول هسته با موفقیت ثبت شد.<br/>"
                        : "خطا در ثبت کلید اول هسته.<br/>";
                }
                if (licenseHelper.CheckCoreLicenseExistance(seccondKeyName))
                {
                    ltLicenseMessage.Text += licenseHelper.EditLicense(seccondKeyName, txtCoreSeccondKey.Text)
                        ? "کلید دوم هسته با موفقیت ویرایش شد."
                        : "خطا در ویرایش کلید دوم هسته.";
                }
                else
                {
                    ltLicenseMessage.Text += licenseHelper.AddLicense(seccondKeyName, txtCoreSeccondKey.Text)
                        ? "کلید دوم هسته با موفقیت ثبت شد."
                        : "خطا در ثبت کلید دوم هسته.";
                }
            }
        }

    }
}