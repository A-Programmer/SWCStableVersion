using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCommon;
using PortalCore.Logic;
using PortalCore.Models;

namespace PortalCore.Admin
{
    public partial class ManagePermissions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("ManagePermissions"))
            {
                Response.Redirect("~/Error/403");
            }
            if (!Page.IsPostBack)
            {
                BindRoles();
            }
            CheckPermissions();
        }

        public void PopulateTree(int roleid = 0)
        {
            tvPermissionGroups.Nodes.Clear();
            var pghelper = new PermissionHelper();
            var pgs = pghelper.GetPermissionGroups();
            foreach (var pg in pgs)
            {
                tvPermissionGroups.Nodes
                    .Add(new TreeNode(pg.PermissionGroupTitle, pg.PermissionGroupId.ToString()));
                PopulatePermissions(pg.PermissionGroupId,roleid);
            }
            tvPermissionGroups.CollapseAll();
            
        }

        public void PopulatePermissions(int pgid,int roleid = 0)
        {
            var phelper = new PermissionHelper();
            var permissions = phelper.GetPermissionsForGroup(pgid);
            var granted = phelper.GetGrantedPermissions(pgid,roleid);
            int i = 0;
            foreach (var permission in permissions)
            {
                var tn = new TreeNode();
                tn = tvPermissionGroups.FindNode(pgid.ToString());
                if (tn != null)
                {

                    tn.ChildNodes.Add(new TreeNode(permission.PermissionTitle,
                        permission.PermissionId.ToString()));
                    
                    if (granted.Find(p => p.PermissionId == permission.PermissionId) != null)
                    {
                        tn.ChildNodes[i].Checked = true;
                    }
                    i++;
                }

            }
        }

        public bool GetAccess(string permissionname)
        {
            return PermissionControl.CheckPermission(permissionname);
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
                        int id = Convert.ToInt32(e.CommandArgument);
                        ViewState["Id"] = id;
                        var rhelper = new RoleHelper();
                        var role = rhelper.GetRole(id);
                        if (role != null)
                        {
                            PopulateTree(role.RoleId);
                        }
                        else
                        {
                            ltRolesMessage.Text = "گروه مورد نظر یافت نشد.";
                        }
                        mvPremissions.SetActiveView(vwEdirPermissions);
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

        protected void btnSubmitPermissions_OnClick(object sender, EventArgs e)
        {
            var checkeds = tvPermissionGroups.CheckedNodes;
            var phelper = new PermissionHelper();
            int roleid = Convert.ToInt32(ViewState["Id"]);
            var pids = new List<int>();
            foreach (TreeNode chk in checkeds)
            {
                pids.Add(Convert.ToInt32(chk.Value));
            }
            if (phelper.AddPermissionsForRole(roleid, pids))
            {
                ltRolesMessage.Text = "دسترسی های گروه مورد نظر با موفقیت ثبت شد.";
                BindRoles();
                mvPremissions.SetActiveView(vwRoles);
            }
            else
            {
                ltSubmitPermissionMessage.Text = "خطا در ثبت دسترسی های گروه";
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            mvPremissions.SetActiveView(vwRoles);
        }

        public void CheckPermissions()
        {
            btnSubmitPermissions.Enabled = PermissionControl.CheckPermission("SubmitRolePermissionChanges");
        }

    }
}