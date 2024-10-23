using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCommon;
using PortalCore.Logic;

namespace PortalCore.Admin
{
    public partial class ManageRoles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("ManageRoles"))
            {
                Response.Redirect("~/Error/403");
            }
            CheckPermissions();
            if (!Page.IsPostBack && PermissionControl.CheckPermission("RolesList"))
            {
                BindRoles();
            }
        }

        protected void grdRoles_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdRoles.PageIndex = e.NewPageIndex;
            BindRoles();
        }

        protected void grdRoles_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DoEdit":
                {
                    if (PermissionControl.CheckPermission("EditRole"))
                    {
                        var id = Convert.ToInt32(e.CommandArgument);
                        ViewState["RoleMode"] = "Edit";
                        ViewState["Id"] = id;
                        var rhelper = new RoleHelper();
                        var role = rhelper.GetRole(id);
                        if (role != null)
                        {
                            txtRoleTitle.Text = role.RoleTitle;
                            txtRoleName.Text = role.RoleName;
                        }
                        else
                        {
                            ltRolesMessage.Text = "گروه مورد نظر یافت نشد.";
                        }
                        mvRoles.SetActiveView(vwEditRole);
                    }
                    else
                    {
                        ltRolesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
                case "DoDelete":
                {
                    if (PermissionControl.CheckPermission("DeleteRole"))
                    {
                        var id = Convert.ToInt32(e.CommandArgument);
                        var rhelper = new RoleHelper();
                        if (rhelper.DeleteRole(id))
                        {
                            ltRolesMessage.Text = "گروه مورد نظر به همراه دسترسی ها حذف شد.";
                            BindRoles();
                        }
                        else
                        {
                            ltRolesMessage.Text = "گروه مورد نظر یافت نشد.";
                        }
                    }
                    else
                    {
                        ltRolesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
            }
        }

        public void BindRoles()
        {
            var rhelper = new RoleHelper();
            grdRoles.DataSource = rhelper.GetRoles();
            grdRoles.DataBind();
        }

        public void CheckPermissions()
        {
            btnAdd.Enabled = PermissionControl.CheckPermission("AddNewRole");
            btnSubmitRole.Enabled = PermissionControl.CheckPermission("SubmitRoleChanges");
        }

        public bool IsDefaultRole(int id)
        {
            var rhelper = new RoleHelper();
            if (id == rhelper.GetDefaultRoleId())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetAccess(string permissionName)
        {
            return PermissionControl.CheckPermission(permissionName);
        }

        protected void btnAdd_OnClick(object sender, EventArgs e)
        {
            txtRoleName.Text = string.Empty;
            txtRoleName.Enabled = true;
            txtRoleTitle.Text = string.Empty;
            txtRoleTitle.Enabled = true;
            ViewState["RoleMode"] = "Add";
            mvRoles.SetActiveView(vwEditRole);
        }

        protected void btnSubmitRole_OnClick(object sender, EventArgs e)
        {
            switch (ViewState["RoleMode"].ToString())
            {
                case "Add":
                {
                    var rhelper = new RoleHelper();
                    if (rhelper.AddRole(txtRoleName.Text, txtRoleTitle.Text))
                    {
                        ltRolesMessage.Text = "گروه کاربری با موفقت ثبت شد.";
                        BindRoles();
                        mvRoles.SetActiveView(vwRoles);
                    }
                    else
                    {
                        ltRoleMessage.Text = "خطا در ثبت گروه کاربری جدید.";
                    }
                    break;
                }
                case "Edit":
                {
                    int id = Convert.ToInt32(ViewState["Id"]);
                    var rhelper = new RoleHelper();
                    if (rhelper.EditRole(id, txtRoleName.Text, txtRoleTitle.Text))
                    {
                        ltRolesMessage.Text = "گروه کاربری با موفقیت ویرایش شد.";
                        BindRoles();
                        mvRoles.SetActiveView(vwRoles);
                    }
                    else
                    {
                        ltRoleMessage.Text = "خطا در ویرایش گروه کاربری";
                    }
                    break;
                }
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            txtRoleName.Text = string.Empty;
            txtRoleTitle.Text = string.Empty;
            mvRoles.SetActiveView(vwRoles);
        }
    }
}