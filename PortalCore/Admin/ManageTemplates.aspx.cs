using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using PortalCommon;
using PortalCore.Logic;

namespace PortalCore.Admin
{
    public partial class ManageTemplates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!PermissionControl.CheckPermission("ManageTemplates"))
            {
                Response.Redirect("~/Error/403");
            }
            if (!Page.IsPostBack)
            {
                BindTemplates();
            }
            CheckPermissions();
        }
        public void BindTemplates()
        {
            var templateHelper = new TemplatesHelper();
            grdTemplates.DataSource = templateHelper.GetTemplates();
            grdTemplates.DataBind();
        }
        public bool GetAccess(string permissionName)
        {
            return PermissionControl.CheckPermission(permissionName);
        }
        public void CheckPermissions()
        {
            btnSubmitTemplate.Enabled = PermissionControl.CheckPermission("UploadTheme");
            grdTemplates.Visible = PermissionControl.CheckPermission("TemplatesList");
        }
        protected void grdTemplates_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdTemplates.PageIndex = e.NewPageIndex;
            BindTemplates();
        }
        protected void grdTemplates_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteTemplate":
                {
                    if (PermissionControl.CheckPermission("DeleteTheme"))
                    {
                        var id = Convert.ToInt32(e.CommandArgument);
                        var templateHelper = new TemplatesHelper();
                        ltTemplateMessages.Text = templateHelper.DeleteTemplate(id) ? "پوسته مورد نظر با موفقیت حذف شد." : "خطا در حذف پوسته مورد نظر.";
                    }
                    else
                    {
                        ltTemplateMessages.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    BindTemplates();
                    break;
                }
                case "ActiveTemplate":
                {
                    if (PermissionControl.CheckPermission("ActiveTheme"))
                    {
                        var id = Convert.ToInt32(e.CommandArgument);
                        var templateHelper = new TemplatesHelper();
                        ltTemplateMessages.Text = templateHelper
                            .ActiveTemplate(id) ? "پوسته مورد نظر با موفقیت فعال شد." : "پوسته مورد نظر فعال نشد.";
                    }
                    else
                    {
                        ltTemplateMessages.Text = "شما اجازه انجام این عملیات را ندارید.";
                    }
                    BindTemplates();
                    break;
                }
            }
        }
        protected void btnSubmitTemplate_OnClick(object sender, EventArgs e)
        {
            if (fuTemplate.HasFile)
            {
                var templateExtension = Path.GetExtension(fuTemplate.FileName);
                if (!String.IsNullOrEmpty(templateExtension) && templateExtension.ToLower() == ".template")
                {
                    try
                    {
                        var filename = Path.GetFileNameWithoutExtension(fuTemplate.FileName);
                        var filePath = Server.MapPath("~/" + filename + ".zip");
                        fuTemplate.SaveAs(filePath);
                        var templateHelper = new TemplatesHelper();
                        if (templateHelper.ExtractZipFile(filePath, "1568KSTemplate6413", Server.MapPath("~/")))
                        {
                            var templateName = filename.Replace(".zip", string.Empty);
                            var template = templateHelper.GetTemplateByName(templateName);
                            var homeTemplatePath = Server.MapPath("~/Templates/" + templateName + "/Home.html");
                            var pageTemplatePath = Server.MapPath("~/Templates/" + templateName + "/Page.html");
                            templateHelper.SubmitTemplateZones(homeTemplatePath, templateName);
                            templateHelper.SubmitTemplateZones(pageTemplatePath, templateName);
                            if (template == null)
                            {
                                var templateHtml = File.ReadAllText(homeTemplatePath);
                                var htmlDoc = new HtmlDocument();
                                htmlDoc.LoadHtml(templateHtml);
                                var templateTitle = htmlDoc.DocumentNode.SelectSingleNode("/html/head/title");
                                templateHelper.AddTemplate(templateName, templateTitle.InnerText);
                            }
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            if (File.Exists(filePath.Replace(".zip", ".template")))
                            {
                                File.Delete(filePath.Replace(".zip", ".template"));
                            }
                            BindTemplates();
                        }
                        else
                        {
                            ltTemplateMessages.Text = "خطا در گشودن فایل پوسته.";
                        }
                    }
                    catch (Exception ex)
                    {
                        var eLogger = new ErrorLogger();
                        eLogger.AddError("خطا در ارسال پوسته","خطای ارسال پوسته:<br/>" + ex.Message,
                            User.Identity.Name + "Upload Template", "ManageTemplates.aspx/btnSubmitTemplate_OnClick");
                        var filename = Path.GetFileNameWithoutExtension(fuTemplate.FileName);
                        var filePath = Server.MapPath("~/" + filename + ".zip");
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        if (File.Exists(filePath.Replace(".zip", ".template")))
                        {
                            File.Delete(filePath.Replace(".zip", ".template"));
                        }
                        ltTemplateMessages.Text = "خطای ارسال پوسته به سرور:<br/>" + ex.Message;
                    }
                }
                else
                {
                    ltTemplateMessages.Text = "فایل پوسته خراب است.";
                }
            }
            else
            {
                ltTemplateMessages.Text = "فایل پوسته انتخاب نشده است.";
            }
        }
    }
}