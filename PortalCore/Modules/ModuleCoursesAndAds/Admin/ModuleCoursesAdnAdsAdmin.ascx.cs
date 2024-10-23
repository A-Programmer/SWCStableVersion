using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Module_CoursesAndAds.Model;
using Telerik.Web.UI;

namespace Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Admin
{
    public partial class ModuleCoursesAdnAdsAdmin : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void BindGrid()
        {
            using (var context = new ModuleCoursesAndAdsEntities())
            {
                grdCommersList.DataSource =
                    (from a in context.Module_Commers_Items orderby a.CMID descending select a).ToList();
                grdCommersList.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvCommers.SetActiveView(veCommersList);
        }

        protected void btnSubmitChanges_Click(object sender, EventArgs e)
        {
            switch (ViewState["Mode"].ToString())
            {
                case "Insert":
                    {
                        using (var context = new ModuleCoursesAndAdsEntities())
                        {
                            context.Module_Commers_Items.AddObject(new Module_Commers_Items()
                                {
                                    CMDay = txtDay.Text,
                                    CMDescription = txtDes.Text,
                                    CMDetails = txtContent.Text,
                                    CMKindID = Convert.ToInt32(ddlKinds.SelectedValue),
                                    CMKind = ddlKinds.SelectedItem.Text,
                                    CMMonth = txtMonth.Text,
                                    CMPrice = Convert.ToInt32(txtPrice.Text),
                                    CMSendDate = DateTime.Now,
                                    CMTags = txtTags.Text,
                                    CmTitle = txtTitle.Text
                                });
                            context.SaveChanges();
                        }

                        BindGrid();
                        mvCommers.SetActiveView(veCommersList);
                        break;
                    }
                case "Edit":
                    {
                        int cid = Convert.ToInt32(ViewState["ID"]);
                        using (var context = new ModuleCoursesAndAdsEntities())
                        {
                            var item =
                                (from a in context.Module_Commers_Items where a.CMID == cid select a).FirstOrDefault();
                            if (item != null)
                            {
                                item.CmTitle = txtTitle.Text;
                                item.CMDetails = txtContent.Text;
                                item.CMTags = txtTags.Text;
                                item.CMDay = txtDay.Text;
                                item.CMDescription = txtDes.Text;
                                item.CMMonth = txtMonth.Text;
                                item.CMPrice = Convert.ToInt32(txtPrice.Text);
                                item.CMKindID = Convert.ToInt32(ddlKinds.SelectedValue);
                                context.SaveChanges();
                                BindGrid();
                                mvCommers.SetActiveView(veCommersList);
                            }
                        }
                        break;
                    }
            }
        }

        protected void grdCommersList_ItemCommand(object sender, GridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case RadGrid.InitInsertCommandName:
                    {
                        e.Canceled = true;

                        ViewState["Mode"] = "Insert";

                        txtTitle.Text = string.Empty;
                        txtContent.Text = string.Empty;
                        txtTags.Text = string.Empty;
                        txtDay.Text = string.Empty;
                        txtDes.Text = string.Empty;
                        txtMonth.Text = string.Empty;
                        txtPrice.Text = string.Empty;

                        mvCommers.SetActiveView(veCommersEdit);
                        break;
                    }
                case "DoEdit":
                    {
                        ViewState["Mode"] = "Edit";
                        int cid = Convert.ToInt32(e.CommandArgument);
                        ViewState["ID"] = cid;
                        using (var context = new ModuleCoursesAndAdsEntities())
                        {
                            var item =
                                (from a in context.Module_Commers_Items where a.CMID == cid select a).FirstOrDefault();
                            if (item != null)
                            {
                                txtTitle.Text = item.CmTitle;
                                txtContent.Text = item.CMDetails;
                                txtTags.Text = item.CMTags;
                                txtDay.Text = item.CMDay;
                                txtDes.Text = item.CMDescription;
                                txtMonth.Text = item.CMMonth;
                                txtPrice.Text = item.CMPrice.ToString();
                                ddlKinds.SelectedValue = item.CMKindID.ToString();

                                mvCommers.SetActiveView(veCommersList);
                            }
                        }

                        break;
                    }
                case "DoDelete":
                    {
                        int cid = Convert.ToInt32(e.CommandArgument);
                        using (var context = new ModuleCoursesAndAdsEntities())
                        {
                            (from a in context.Module_Commers_Items where a.CMID == cid select a).ToList().ForEach(context.DeleteObject);
                            context.SaveChanges();
                            BindGrid();
                        }
                        break;
                    }
            }
        }
    }
}