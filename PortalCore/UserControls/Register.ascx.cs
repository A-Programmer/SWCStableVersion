using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCore.Logic;

namespace PortalCore.UserControls
{
    public partial class Register : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var uhelper = new UserHelper();
                System.Web.UI.Page page = this.Page;
                Label lbl = page.FindControl("lblMessage") as Label;
                lbl.Visible = true;
                int result = uhelper.Register(txtRegisterUserName.Text, txtRegisterPassword.Text,
                    txtRegisterEmail.Text);
                switch (result)
                {
                    case 0:
                    {
                        lbl.Text = "شما با موفقیت ثبت نام شدید";
                        break;
                    }
                case 2:
                    {
                        lbl.Text = "نام کاربری وارد نشده است";
                        break;
                    }
                case 3:
                    {
                        lbl.Text = "کلمه عبور وارد نشده است";
                        break;
                    }
                case 4:
                    {
                        lbl.Text = "ایمیل وارد نشده است";
                        break;
                    }
                case 5:
                    {
                        lbl.Text = "با این نام کاربری یا ایمیل قبلا ثبت نام شده است.";
                        break;
                    }
                }
            }
        }
    }
}