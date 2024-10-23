using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCore.Logic;

namespace PortalCore.UserControls
{
    public partial class PasswordRecovery : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRecoverPassword_OnClick(object sender, EventArgs e)
        {

            var uhelper = new UserHelper();
            System.Web.UI.Page page = this.Page;
            Label lbl = page.FindControl("lblMessage") as Label;
            if (uhelper.GetUserByName(txtRecoveryUserName.Text) != null)
            {
                
            if (lbl != null)
            {
                lbl.Visible = true;
                lbl.Text = uhelper.Recovery(uhelper.GetUserByName(txtRecoveryUserName.Text).Email);
            }
            }
            else
            {
                lbl.Visible = true;
                lbl.Text = "کاربری با این نام کاربری یافت نشد.";
            }
            
        }
    }
}