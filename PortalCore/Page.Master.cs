using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using PortalCore.Logic;
using PortalCore.Models;

namespace PortalCore
{
    public partial class Page : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var settingsHelper = new SettingsHelper();
            settingsHelper.CheckIfUnderConstruction();
            var ActionName = Path.GetFileNameWithoutExtension(Request.GetFriendlyUrlFileVirtualPath()).ToLower();
            var PageName = "";
            if (ActionName.ToLower() == "showpage")
            {
                IList<string> segments = Request.GetFriendlyUrlSegments();
                if (!string.IsNullOrEmpty(segments.FirstOrDefault()))
                {
                    PageName = segments.FirstOrDefault();
                }
                else
                {
                    Response.Redirect("~/");
                }
            }
            else
            {
                Response.Redirect("~/Error/404");
            }

            var db = new CoreDbContext();
            var template = db.Templates.FirstOrDefault(t => t.IsActive);
            if (template != null)
            {

                //Get Zones Layout
                var zones = db.Zones.Where(z => z.TemplateName == template.Name).ToList();
                foreach (var zone in zones)
                {
                    Control Zone = this.FindControl(zone.Name);
                    var layouts =
                        (from l in db.PageLayouts
                         where l.ZoneName == zone.Name
                         join p in db.PageLayouts
                         on l.PageId equals p.PageId
                         where p.ZoneName == PageName
                         orderby l.AppearanceOrder
                         select l).ToList();

                    //Get zone blocks
                    foreach (var layout in layouts)
                    {
                        var Block =
                            db.Blocks.FirstOrDefault(b => b.BlockId == layout.BlockId);
                        Control BlockFile = LoadControl(Block.FilePath);
                        Zone.Controls.Add(BlockFile);
                    }
                }

                // Set Page Title from Pages table in database
                var page = db.Pages.FirstOrDefault(p => p.Name == PageName);
                if (page != null)
                {
                    Page.Title = page.Title;
                }
            }
            else
            {
                var elogger = new ErrorLogger();
                elogger.AddError("قالب فعالی یافت نشد","زمان اجرای سایت قالب فعالی یافت نشد.",
                    "ShowPage","~/ShowPage");
                Response.Redirect("~/Error/40");
            }

        }
    }
}