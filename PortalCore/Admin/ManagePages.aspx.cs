using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCommon;
using PortalCore.Logic;
using PortalCore.Models;

namespace PortalCore.Admin
{
    public partial class ManagePages : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("ManagePages"))
            {
                Response.Redirect("~/Error/403");
            }
            if (!Page.IsPostBack)
            {
                BindPagesList();
            }
            CheckPermissions();
        }

        public void BindPagesList()
        {
            var phelper = new PagesHelper();
            grdPages.DataSource = phelper.GetAllPages();
            grdPages.DataBind();
        }
        public void BindModulesDdl()
        {
            var mhelper = new ModuleHelper();
            ddlModules.DataSource = mhelper.GetModules();
            ddlModules.DataTextField = "Title";
            ddlModules.DataValueField = "ModuleId";
            ddlModules.DataBind();
        }
        public void BindLayoutGrid(int pageid)
        {
            var db = new CoreDbContext();
            var layouts = (from pl in db.PageLayouts
                            where pl.PageId == pageid
                           join b in db.Blocks on pl.BlockId equals b.BlockId
                            select new
                            {
                                pl.AppearanceOrder,
                                pl.PageLayoutId,
                                pl.ZoneName,
                                b.BlockId,
                                b.Title,
                                b.IsAdminBlock
                            }).OrderBy(z => z.ZoneName).ThenBy(a => a.AppearanceOrder).ToList();
            grdPageLayouts.DataSource = layouts;
            grdPageLayouts.DataBind();
            
        }
        
        public List<Module> GetModulesData()
        {
            var modulehelper = new ModuleHelper();
            return modulehelper.GetModules();
        }

        public List<Block> GetModuleBlocks(int moduleid)
        {
            var blockhelper = new BlockHelper();
            return blockhelper.GetModuleBlocks(moduleid);
        } 
        public void PopulateModulesTreeView()
        {
            var modules = GetModulesData();
            foreach (var module in modules)
            {
                var thisNode = new TreeNode("</a>" + module.Title + "<a href=#>", module.ModuleId.ToString());
                tvModules.Nodes.Add(thisNode);
                var blocks = GetModuleBlocks(module.ModuleId);
                foreach (var block in blocks)
                {
                    var childNode = new TreeNode("</a>" + block.Title + "<a href=#>", block.BlockId.ToString());
                    thisNode.ChildNodes.Add
                        (childNode);
                    
                }
            }
        }

        public void BindZonesCheckBoxList()
        {
            var zonehelper = new ZoneHelper();
            chkZones.DataSource = zonehelper.GetCurrentTemplateZones();
            chkZones.DataTextField = "Title";
            chkZones.DataValueField = "Name";
            chkZones.DataBind();
        }
        public string GetZoneTitle(string zoneName)
        {
            var db = new CoreDbContext();
            var zone = db.Zones.FirstOrDefault(z => z.Name == zoneName);
            if (zone != null)
            {
                return zone.Title;
            }
            else
            {
                return "منطقه یافت نشد.";
            }
        }
        public bool GetAccess(string permissionname)
        {
            return PermissionControl.CheckPermission(permissionname);
        }

        public string GetPageType(int moduleId)
        {
            if (moduleId == 0)
            {
                return "صفحه هسته";
            }
            else
            {
                var moduleHelper = new ModuleHelper();
                return "صفحه " + moduleHelper.GetModuleById(moduleId).Title;
            }
        }
        public void CheckPermissions()
        {
            btnAddCorePage.Enabled = PermissionControl.CheckPermission("AddNewPage");
            btnAddModulePage.Enabled = PermissionControl.CheckPermission("AddNewPage");
            grdPages.Visible = PermissionControl.CheckPermission("PagesList");
            grdPageLayouts.Visible = PermissionControl.CheckPermission("LayoutsList");
            btnSubmitPageLayout.Enabled = PermissionControl.CheckPermission("SubmitPageLayout");
            btnSubmitPage.Enabled = PermissionControl.CheckPermission("SubmitPage");


        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            mvPagesAdmin.SetActiveView(vwPagesList);
        }

        protected void btnSubmitPage_OnClick(object sender, EventArgs e)
        {
            switch (ViewState["PageMode"].ToString())
            {
                case "AddCorePage":
                {
                    var pageHelper = new PagesHelper();
                    if (pageHelper.AddCorePage(txtPageTitle.Text,txtPageName.Text))
                    {
                        BindPagesList();
                        ltPagesMessage.Text = "صفحه هسته با موفقیت ثبت شد.";
                        txtPageName.Text = string.Empty;
                        txtPageTitle.Text = string.Empty;
                        mvPagesAdmin.SetActiveView(vwPagesList);
                    }
                    else
                    {
                        ltPagesMessage.Text = "خطا در ثبت صفحه برای هسته.";
                    }
                    break;
                }
                case "AddModulePage":
                {
                    var pageHelper = new PagesHelper();
                    int moduleId = Convert.ToInt32(ddlModules.SelectedItem.Value);
                    if (pageHelper.AddModulePage(moduleId,txtPageTitle.Text,txtPageName.Text))
                    {
                        BindPagesList();
                        txtPageName.Text = string.Empty;
                        txtPageTitle.Text = string.Empty;
                        ltPagesMessage.Text = "صفحه ماژول با موفقیت ثبت شد.";
                        mvPagesAdmin.SetActiveView(vwPagesList);
                    }
                    else
                    {
                        ltPagesMessage.Text = "خطا در ثبت صفحه ماژول.";
                    }
                    break;
                }
                case "Edit":
                {
                    var pageHelper = new PagesHelper();
                    int pageId = Convert.ToInt32(ViewState["PageId"]);
                    if (pageHelper.EditPage(pageId,txtPageTitle.Text))
                    {
                        BindPagesList();
                        txtPageName.Text = string.Empty;
                        txtPageTitle.Text = string.Empty;
                        ltPagesMessage.Text = "صفحه با موفقیت ویرایش شد.";
                        mvPagesAdmin.SetActiveView(vwPagesList);
                    }
                    else
                    {
                        ltPagesMessage.Text = "خطا در ویرایش صفحه.";
                    }
                    break;
                }
            }
        }

        protected void btnAddCorePage_OnClick(object sender, EventArgs e)
        {
            fgModules.Visible = false;
            fgPageName.Visible = true;
            fgPageTitle.Visible = true;
            ViewState["PageMode"] = "AddCorePage";
            txtPageName.Text = string.Empty;
            txtPageTitle.Text = string.Empty;
            mvPagesAdmin.SetActiveView(vwEditPage);

        }

        protected void btnAddModulePage_OnClick(object sender, EventArgs e)
        {
            BindModulesDdl();
            fgModules.Visible = true;
            fgPageName.Visible = true;
            fgPageTitle.Visible = true;
            ViewState["PageMode"] = "AddModulePage";
            txtPageName.Text = string.Empty;
            txtPageTitle.Text = string.Empty;
            mvPagesAdmin.SetActiveView(vwEditPage);
        }

        protected void grdPages_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPages.PageIndex = e.NewPageIndex;
            BindPagesList();
        }

        protected void grdPages_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DoEdit":
                {
                    if (PermissionControl.CheckPermission("EditPage"))
                    {
                        var pid = Convert.ToInt32(e.CommandArgument);
                        ViewState["PageId"] = pid;
                        var phelper = new PagesHelper();
                        var page = phelper.GetPageById(pid);
                        if (page != null)
                        {
                            ViewState["PageMode"] = "Edit";
                            fgModules.Visible = false;
                            fgPageName.Visible = false;
                            fgPageTitle.Visible = true;
                            txtPageTitle.Text = page.Title;
                            txtPageName.Text = page.Name;
                            mvPagesAdmin.SetActiveView(vwEditPage);

                        }
                        else
                        {
                            ltPagesMessage.Text = "صفحه مورد نظر یافت نشد.";
                        }
                    }
                    else
                    {
                        ltPagesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
                case "DoEditLayout":
                {
                    if (PermissionControl.CheckPermission("EditPageLayout"))
                    {
                        var pid = Convert.ToInt32(e.CommandArgument);
                        ViewState["PageId"] = pid;
                        BindLayoutGrid(pid);
                        PopulateModulesTreeView();
                        BindZonesCheckBoxList();
                        mvPagesAdmin.SetActiveView(vwPageDesigner);
                    }
                    else
                    {
                        ltPagesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
                case "DoDelete":
                {
                    if (PermissionControl.CheckPermission("DeletePage"))
                    {
                        var pid = Convert.ToInt32(e.CommandArgument);
                        var pageHelper = new PagesHelper();
                        if (pageHelper.DeletePageAndPageLayouts(pid))
                        {
                            ltPagesMessage.Text = "صفحه مورد نظر به همراه چینش های آن حذف شد.";
                            BindPagesList();
                        }
                        else
                        {
                            ltPagesMessage.Text = "خطا در حذف صفحه و چینش های آن";
                        }
                    }
                    else
                    {
                        ltPagesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
            }
        }

        protected void grdPageLayouts_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdPageLayouts.PageIndex = e.NewPageIndex;
            int pageid = Convert.ToInt32(ViewState["PageId"]);
            BindLayoutGrid(pageid);
        }

        protected void grdPageLayouts_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DoDelete":
                {
                    if (PermissionControl.CheckPermission("DeletePageLayout"))
                    {
                        var pageLayoutId = Convert.ToInt32(e.CommandArgument);
                        var pageHelper = new PagesHelper();
                        var pageLayout = pageHelper.GetPageLayout(pageLayoutId);
                        if (pageHelper.DeletePageLayout(pageLayoutId))
                        {
                            ltPagesMessage.Text = "چینش مورد نظر با موفقیت حذف شد.";
                            BindLayoutGrid(pageLayout.PageId);
                            mvPagesAdmin.SetActiveView(vwPageDesigner);
                        }
                    }
                    else
                    {
                        ltPagesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
                case "SubmitAppearanceOrder":
                {
                    if (PermissionControl.CheckPermission("SubmitPageLayout"))
                    {
                        var pageHelper = new PagesHelper();
                        var pageLayoutIds = new List<int>();
                        var appOrders = new List<int>();
                        foreach (GridViewRow row in grdPageLayouts.Rows)
                        {
                            var appOrder = row.FindControl("txtAppearanceOrder") as TextBox;
                            var pageLayoutId = row.FindControl("lblPageLayoutId") as Literal;
                            appOrders.Add(Convert.ToInt32(appOrder.Text));
                            pageLayoutIds.Add(Convert.ToInt32(pageLayoutId.Text));
                        }
                        pageHelper.EditAppearanceOrder(pageLayoutIds, appOrders);
                    }
                    else
                    {
                        ltPagesMessage.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    break;
                }
            }
        }

        protected void btnSubmitPageLayout_OnClick(object sender, EventArgs e)
        {
            var selectedZones = chkZones.Items.Cast<ListItem>().Where(li => li.Selected).ToList();
            int pageId = Convert.ToInt32(ViewState["PageId"]);
            if (pageId != 0)
            {
                foreach (var selectedZone in selectedZones)
                {
                    var selectedBlocks = tvModules.CheckedNodes.Cast<TreeNode>().ToList();
                    foreach (var selectedBlock in selectedBlocks)
                    {
                        var pageHelper = new PagesHelper();
                        pageHelper.AddPageLayout(pageId, Convert.ToInt32(selectedBlock.Value), selectedZone.Value);
                    }
                }
                BindLayoutGrid(pageId);
                BindZonesCheckBoxList();
                PopulateModulesTreeView();
                mvPagesAdmin.SetActiveView(vwPageDesigner);
            }
            else
            {
                ltPagesMessage.Text = "زمان بی کار ماندن صفحه بیش اندازه بوده، مجددا تلاش کنید.";
                mvPagesAdmin.SetActiveView(vwPagesList);
            }
            
        }



    }
}