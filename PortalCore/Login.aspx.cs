using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCore.Logic;
using PortalCore.Models;

namespace PortalCore
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            var db = new CoreDbContext();
            var generalsettings = db.GeneralSettings.FirstOrDefault();
            if (generalsettings != null)
            {
                //if (generalsettings.AllowExternalLogin)
                //{
                //    var ucExternalLogin = (Control)LoadControl("~/UserControls/ExternalLogin.ascx");
                //    LoadUserControl(ucExternalLogin, pnlExternalLogin);
                //}
                if (generalsettings.EnableRegistration)
                {
                    var ucRegister = (Control)LoadControl("~/UserControls/Register.ascx");
                    LoadUserControl(ucRegister, pnlRegister);
                }
                if (generalsettings.EnablePasswordRecovery)
                {
                    var ucPasswordRecovery = (Control)LoadControl("~/UserControls/PasswordRecovery.ascx");
                    LoadUserControl(ucPasswordRecovery, pnlRecoveryPassword);
                }
            }
            
        }


        public void LoadUserControl(Control uc, PlaceHolder ph)
        {
            ph.Controls.Add(uc);
        }


        protected void btnLogIn_OnClick(object sender, EventArgs e)
        {
            var uhelper = new UserHelper();
            if (uhelper.ValidateUser(txtUserName.Text, txtPassword.Text))
            {
                FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, chkRemember.Checked);
            }
            else
            {
                lblMessage.Visible = true;
                lblMessage.Text = "نام کاربری یا کلمه عبور اشتباه است.";
            }
        }
    }
}