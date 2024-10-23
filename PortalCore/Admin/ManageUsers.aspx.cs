using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCommon;
using PortalCore.Logic;
using PortalCore.Models;

namespace PortalCore.Admin
{
    public partial class ManageUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetValues();
            }
            CheckPermissions();
        }

        public void Search(string key)
        {
            var uhelper = new UserHelper();
            grdUsers.DataSource = uhelper.SearchUsers(key).Where(u => u.UserName.ToLower() != "superadmin").ToList();
            grdUsers.DataBind();
        }
        
        protected void grdUsers_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DoEdit":
                {
                    if (PermissionControl.CheckPermission("EditUser"))
                    {
                        int id = Convert.ToInt32(e.CommandArgument);
                        var uhelper = new UserHelper();
                        var rhelper = new RoleHelper();
                        var role = rhelper.GetUserRoles(id).FirstOrDefault();
                        var user = uhelper.GetUserById(id);
                        txtUserName.Text = user.UserName;
                        txtPassword.Text = user.Password;
                        txtFirstName.Text = user.FirstName;
                        txtLastName.Text = user.LastName;
                        txtEmail.Text = user.Email;
                        txtPhone.Text = user.Phone;
                        chkActive.Checked = user.IsActive;
                        //chkSuperAdmin.Checked = user.IsSuperAdmin;
                        ddlRoles.SelectedValue = role.RoleId.ToString();
                        ViewState["Id"] = id;
                        ViewState["UserMode"] = "Edit";
                        txtUserName.Enabled = false;
                        txtPassword.Enabled = false;
                        mvUsers.SetActiveView(vwEditUser);
                    }
                    else
                    {
                        ltUserMessage.Text = "شما اجازه این عملیات را ندارید.";
                    }
                    break;
                }
                case "DoDelete":
                {
                    if (PermissionControl.CheckPermission("DeleteUser"))
                    {
                        int id = Convert.ToInt32(e.CommandArgument);
                        var uhelper = new UserHelper();
                        if (uhelper.DeleteUser(id))
                        {
                            ltUserMessage.Text = "کاربر مورد نظر با موفقیت حذف شد";
                            BindUsers();
                        }
                        else
                        {
                            ltUserMessage.Text = "خطا در حذف کاربر";
                        }
                    }
                    else
                    {
                        ltUserMessage.Text = "شما اجازه این عملیات را ندارید.";
                    }
                    break;
                }
                case "DoLockUnlock":
                {
                    if (PermissionControl.CheckPermission("LockUser"))
                    {
                        var id = Convert.ToInt32(e.CommandArgument);
                        var uhelper = new UserHelper();
                        if (uhelper.ChangeUserStatus(id))
                        {
                            ltUserMessage.Text = "وضعیت کاربر مورد تغییر کرد";
                            BindUsers();
                        }
                        else
                        {
                            ltUserMessage.Text = "خطا در تغییر وضعیت کاربر";
                        }
                    }
                    else
                    {
                        ltUserMessage.Text = "شما اجازه این عملیات را ندارید.";
                    }
                    
                    break;
                }
                //case "DoSuperAdmin":
                //{
                //    int id = Convert.ToInt32(e.CommandArgument);
                //    var uhelper = new UserHelper();
                //    if (uhelper.ChangeSuperAdminMode(id))
                //    {
                //        ltUserMessage.Text = "وضعیت کاربر ارشد تغییر کرد";
                //        BindUsers();
                //    }
                //    else
                //    {
                //        ltUserMessage.Text = "خطا در تغییر وضعیت کاربر ارشد";
                //    }
                //    break;
                //}
            }
        }

        public void BindRoles()
        {
            var rhelper = new RoleHelper();
            ddlRoles.DataSource = rhelper.GetRoles();
            ddlRoles.DataTextField = "RoleName";
            ddlRoles.DataValueField = "RoleId";
            ddlRoles.DataBind();
        }
        public void BindUsers()
        {
            var uhelper = new UserHelper();
            grdUsers.DataSource = uhelper.GetAllUsers().Where(u => u.UserName.ToLower() != "superadmin").ToList();
            grdUsers.DataBind();
        }
        public void SetValues()
        {
            //Bind Users Grid
            BindUsers();
            //Bind Roles Ddl
            BindRoles();
            var uhelper = new UserHelper();
            ltUsersCount.Text = (uhelper.GetAllUsers().Count - 1).ToString();

        }
        protected void grdUsers_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdUsers.PageIndex = e.NewPageIndex;
            BindUsers();
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            ResetValues();
            mvUsers.SetActiveView(vwUsers);
        }

        public void ResetValues()
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtPhone.Text = string.Empty;
            userinfo.InnerText = string.Empty;
        }

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            Search(txtSearchUser.Text);
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            ViewState["UserMode"] = "Add";
            ResetValues();
            txtUserName.Enabled = true;
            txtPassword.Enabled = true;
            mvUsers.SetActiveView(vwEditUser);
        }

        protected void btnSubmitUser_OnClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var uhelper = new UserHelper();
                switch (ViewState["UserMode"].ToString())
                {
                    case "Edit":
                    {
                        int id = Convert.ToInt32(ViewState["Id"]);
                        if (uhelper.AdminEditUser(id,txtUserName.Text, txtEmail.Text, txtFirstName.Text,
                            txtLastName.Text, txtPhone.Text,userinfo.InnerText/*,chkSuperAdmin.Checked*/,
                            chkActive.Checked,Convert.ToInt32(ddlRoles.SelectedItem.Value)))
                        {
                            ltUserMessage.Text = "مشخصات کاربر با موفقیت ویرایش شد.";
                            BindUsers();
                            mvUsers.SetActiveView(vwUsers);
                        }
                        else
                        {
                            ltProfileMessage.Text = "خطا در ویرایش مشخصات کاربر";
                        }
                        break;
                    }
                    case "Add":
                    {
                        switch(uhelper.CreateUser(txtUserName.Text, txtPassword.Text, txtEmail.Text, txtPhone.Text,
                            userinfo.InnerText, Convert.ToInt32(ddlRoles.SelectedItem.Value)
                            /*,chkSuperAdmin.Checked*/, chkActive.Checked, txtFirstName.Text, txtLastName.Text))
                        {
                            case 0:
                            {
                                ltUserMessage.Text = "کاربر با موفقیت ثبت شد.";
                                BindUsers();
                                mvUsers.SetActiveView(vwUsers);
                                    break;
                            }
                            case 1:
                            {
                                ltProfileMessage.Text = "نام کاربری یا ایمیل تکراری است.";
                                break;
                            }
                            case 2:
                            {
                                ltProfileMessage.Text = "خطا در ثبت مشخصات کاربر جدید.";
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        public bool GetAccess(string pname)
        {
            return PermissionControl.CheckPermission(pname);
        }

        public void CheckPermissions()
        {
            if (!PermissionControl.CheckPermission("ManageUsers"))
            {
                Response.Redirect("~/Error/403");
            }
            btnAdd.Enabled = PermissionControl.CheckPermission("AddUser");
            btnSubmitUser.Enabled = PermissionControl.CheckPermission("SubmitEditUser");
            pnlUsersStats.Visible = PermissionControl.CheckPermission("UsersStats");
        }

    }
}