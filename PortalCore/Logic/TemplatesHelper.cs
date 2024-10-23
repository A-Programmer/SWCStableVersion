using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using HtmlAgilityPack;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class TemplatesHelper
    {
        public List<Template> GetTemplates()
        {
            var db = new CoreDbContext();
            return db.Templates.OrderByDescending(t => t.Id).ToList();
        }

        public Template GetTemplateByName(string templatename)
        {
            var db = new CoreDbContext();
            return db.Templates.FirstOrDefault(t => t.Name == templatename);
        }

        public bool AddTemplate(string name,string title)
        {
            var db = new CoreDbContext();
            try
            {
                db.Templates.Add(new Template()
                {
                    Name = name,
                    Title = title,
                    IsActive = false,
                    IsInstalled = true
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا در ثبت مشخصات پوسته","متن خطا:<br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/AddTemplate", "TemplatesHelper.cs/AddTemplate");
                return false;
            }
        }

        public bool DeleteTemplate(int templateId)
        {
            var db = new CoreDbContext();
            var template = db.Templates.FirstOrDefault(t => t.Id == templateId);
            if (template != null && !template.IsActive)
            {
                try
                {
                    var templateDirectory = HttpContext.Current.Server.MapPath("~/Templates/" + template.Name);
                    DeleteDirectory(templateDirectory);
                    var zones = db.Zones.Where(z => z.TemplateName == template.Name).ToList();
                    foreach (var zone in zones)
                    {
                        var layouts = db.PageLayouts.Where(pl => pl.ZoneName == zone.Name).ToList();
                        foreach (var layout in layouts)
                        {
                            db.PageLayouts.Remove(layout);
                        }
                        db.Zones.Remove(zone);
                    }
                    db.Templates.Remove(template);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {

                    var elogger = new ErrorLogger();
                    elogger.AddError("خطا در حذف پوسته", "خطای زیر در زمان حذف پوسته رخ داد:<br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/DeleteTemplate","TemplatesHelper.cs/DeleteTemplate");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ActiveTemplate(int id)
        {
            var db = new CoreDbContext();
            try
            {
                var thisTemplate = db.Templates.FirstOrDefault(t => t.Id == id);
                if (thisTemplate != null)
                {
                    var homeTemplatePath = HttpContext.Current.Server.
                        MapPath("~/Templates/" + thisTemplate.Name + "/home.html");
                    var pagesTemplatePath = HttpContext.Current.Server.
                        MapPath("~/Templates/" + thisTemplate.Name + "/page.html");
                    if (File.Exists(homeTemplatePath))
                    {
                        var templates = db.Templates.ToList();
                        foreach (var template in templates)
                        {
                            template.IsActive = false;
                        }
                        db.SaveChanges();
                        thisTemplate.IsActive = true;
                        db.SaveChanges();
                        // Change MasterPage theme

                        if (EditHomeMasterPage(homeTemplatePath))
                        {
                            EditPagesMasterPage(File.Exists(pagesTemplatePath) ? pagesTemplatePath : homeTemplatePath);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا در فعال کردن پوسته",
                    "زمان فعال سازی پوسته خطای زیر رخ داد:<br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/ActiveTemplate","TemplatesHelper.cs");
                return false;
            }
        }

        public bool SubmitTemplateZones(string templateFilePath, string templateName)
        {
            if (File.Exists(templateFilePath))
            {
                var zoneHelper = new ZoneHelper();
                var templateHtml = File.ReadAllText(templateFilePath);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(templateHtml);
                var templateZones = htmlDoc.DocumentNode.SelectNodes("/zone");
                if (templateZones != null)
                {
                    var db = new CoreDbContext();
                    foreach (var zone in templateZones)
                    {
                        var existingZone = zoneHelper.GetZoneByName(zone.Id,templateName);
                        if (existingZone == null)
                        {
                            var cssclass = " ";
                            if (zone.Attributes["class"] != null)
                            {
                                cssclass = zone.Attributes["class"].Value;
                            }
                            db.Zones.Add(new Zone()
                            {
                                TemplateName = templateName,
                                Name = zone.Id,
                                Title = zone.InnerText,
                                CssClass = cssclass
                            });
                        }
                    }
                    db.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    byte[] buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا زمان گشودن فایل فشرده",
                    "هنگام گشودن فایل " + archiveFilenameIn + "خطای زیر رخ داد:<br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/ExtractZipFile"
                    , "ModuleHelper.cs/ExtractZipFile");
                return false;
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources

                }
            }
        }

        public bool DeleteDirectory(string targetDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(targetDirectory);
                string[] dirs = Directory.GetDirectories(targetDirectory);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }

                Directory.Delete(targetDirectory, false);
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در حذف پوشه ها و فایلها",
                    "زمان حذف فایل ها و پوشه های  " + targetDirectory + " خطای زیر رخ داد:<br/>" + ex.Message,
                     HttpContext.Current.User.Identity.Name + "/DeleteDirectory"
                     , "ModuleHelper.cs/DeleteDirectory");
                return false;
            }
        }

        public bool EditHomeMasterPage(string homeTemplatePath)
        {
            var eLogger = new ErrorLogger();
            if (File.Exists(homeTemplatePath))
            {
                try
                {
                    var homeHtml = File.ReadAllText(homeTemplatePath);
                    var homeDoc = new HtmlDocument();
                    homeDoc.LoadHtml(homeHtml);
                    var homeHead = homeDoc.DocumentNode.SelectSingleNode("/html/head");
                    var homeBodytag = homeDoc.DocumentNode.SelectSingleNode("/html/body");
                    var homeHeader = homeHead.InnerHtml;
                    var homeBody = homeBodytag.InnerHtml;
                    using (var sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Home.Master")))
                    {
                        using (var writer = new HtmlTextWriter(sw))
                        {
                            writer.Write("<%@ Master Language='C#' AutoEventWireup='true' CodeBehind='Home.master.cs' Inherits='PortalCore.Home' %>");
                            writer.Write("\n");
                            writer.RenderBeginTag(HtmlTextWriterTag.Html);
                            #region Create head tag
                            writer.Write(@"<head runat='server'>");
                            writer.Write(@homeHeader);
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"<asp:ContentPlaceHolder ID='head' runat='server'>
    </asp:ContentPlaceHolder>");
                            writer.Write("\n");
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"</head>");
                            #endregion
                            #region Create body tag
                            writer.Write("\n");
                            writer.RenderBeginTag(HtmlTextWriterTag.Body);
                            writer.Write(@"<form id='form1' runat='server'>");
                            writer.Write("\n");
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"<asp:ScriptManager ID='scManager' runat='server' LoadScriptsBeforeUI='true' 
        ScriptMode='Release'>
        <CompositeScript>
            <Scripts>
            </Scripts>
        </CompositeScript>
    </asp:ScriptManager>");
                            writer.Write("\n");
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"<asp:ContentPlaceHolder ID='cphMain' runat='server'>
    </asp:ContentPlaceHolder>");
                            writer.Write(@homeBody);
                            writer.Write("\n");
                            writer.Write(@"</form>");
                            writer.RenderEndTag();
                            #endregion
                            writer.RenderEndTag();
                        }
                    }
                    
                    return true;
                }
                catch (Exception ex)
                {
                    eLogger.AddError("خطا در تغییر پوسته صفحه اصلی.",
                        "زمان تغییر پوسته مادر صفحه اسلی خطای زیر رخ داد:<br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/EditHomeMasterPage",
                        "TemplatesHelper.cs/EditHomeMasterPage");
                    return false;
                }
            }
            else
            {
                eLogger.AddError("فایل پوسته صفحه اصلی یافت نشد.",
                    "فایل زیر یافت نشد:<br/>" + homeTemplatePath,
                    HttpContext.Current.User.Identity.Name + "/EditHomeMasterPage",
                    "TemplatesHelper.cs/EditHomeMasterPage");
                return false;
            }
        }

        public bool EditPagesMasterPage(string pageTemplatePath)
        {
            var eLogger = new ErrorLogger();
            if (File.Exists(pageTemplatePath))
            {
                try
                {
                    var pageHtml = File.ReadAllText(pageTemplatePath);
                    var pageDoc = new HtmlDocument();
                    pageDoc.LoadHtml(pageHtml);
                    var pageHead = pageDoc.DocumentNode.SelectSingleNode("/html/head");
                    var pageBodytag = pageDoc.DocumentNode.SelectSingleNode("/html/body");
                    var pageHeader = pageHead.InnerHtml;
                    var pageBody = pageBodytag.InnerHtml;
                    using (var sw = new StreamWriter(HttpContext.Current.Server.MapPath("~/Page.Master")))
                    {
                        using (var writer = new HtmlTextWriter(sw))
                        {
                            writer.Write("<%@ Master Language='C#' AutoEventWireup='true' CodeBehind='Page.master.cs' Inherits='PortalCore.Page' %>");
                            #region Create head tag
                            writer.Write("\n");
                            writer.RenderBeginTag(HtmlTextWriterTag.Html);
                            writer.Write(@"<head runat='server'>");
                            writer.Write(@pageHeader);
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"<asp:ContentPlaceHolder ID='head' runat='server'>
    </asp:ContentPlaceHolder>");
                            writer.Write("\n");
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"</head>");
                            #endregion
                            #region Create body tag
                            writer.Write("\n");
                            writer.RenderBeginTag(HtmlTextWriterTag.Body);
                            writer.Write(@"<form id='form1' runat='server'>");
                            writer.Write("\n");
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"<asp:ScriptManager ID='scManager' runat='server' LoadScriptsBeforeUI='true' 
        ScriptMode='Release'>
        <CompositeScript>
            <Scripts>
            </Scripts>
        </CompositeScript>
    </asp:ScriptManager>");
                            writer.Write("\n");
                            writer.Write(HtmlTextWriter.DefaultTabString);
                            writer.Write(@"<asp:ContentPlaceHolder ID='cphMain' runat='server'>
    </asp:ContentPlaceHolder>");
                            writer.Write(@pageBody);
                            writer.Write("\n");
                            writer.Write(@"</form>");
                            writer.RenderEndTag();
                            #endregion
                            writer.RenderEndTag();
                        }
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    eLogger.AddError("خطا در تغییر پوسته صفحات داخلی.",
                        "زمان تغییر پوسته مادر صفحات داخلی خطای زیر رخ داد:<br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/EditPagesMasterPage",
                        "TemplatesHelper.cs/EditPagesMasterPage");
                    return false;
                }
            }
            else
            {
                eLogger.AddError("فایل پوسته صفحات یافت نشد.",
                    "فایل زیر یافت نشد:<br/>" + pageTemplatePath,
                    HttpContext.Current.User.Identity.Name + "/EditPagesMasterPage",
                    "TemplatesHelper.cs/EditPagesMasterPage");
                return false;
            }
        }

    }
}