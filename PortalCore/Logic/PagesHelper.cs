using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class PagesHelper
    {

        public List<Models.Page> GetAllPages()
        {
            var db = new CoreDbContext();
            return db.Pages.OrderBy(p => p.PageId).ToList();
        }

        public Models.Page GetPageById(int id)
        {
            var db = new CoreDbContext();
            return db.Pages.FirstOrDefault(p => p.PageId == id);
        }

        public List<PageLayout> GetPageLayouts(int pageid)
        {
            var db = new CoreDbContext();
            return db.PageLayouts.Where(p => p.PageId == pageid).ToList();
        }

        public bool DeletePageAndPageLayouts(int pageid)
        {
            var db = new CoreDbContext();
            var page = db.Pages.FirstOrDefault(p => p.PageId == pageid);
            if (page != null)
            {
                try
                {
                    var pageLayouts = db.PageLayouts.Where(pl => pl.PageId == pageid).ToList();
                    foreach (var pageLayout in pageLayouts)
                    {
                        db.PageLayouts.Remove(pageLayout);
                    }
                    db.Pages.Remove(page);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("خطا در حذف صفحه و چینش های آن",
                                     "زمان حذف صفحه و چینش های آن خطای زیر رخ داد:<br/>" + ex.Message,
                                      HttpContext.Current.User.Identity.Name + "/DeletePageAndPageLayouts", "PagesHelper.cs/DeletePageAndPageLayouts");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AddCorePage(string title,string name)
        {
            var db = new CoreDbContext();
            try
            {
                db.Pages.Add(new Models.Page()
                {
                    ModuleId = 0,
                    Name = name,
                    Title = title
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا زمان ثبت صفحه برای هسته",
                    "زمان ثبت صفحه برای هسته خطای زیر رخ داد:<br/>" + ex.Message,
                     HttpContext.Current.User.Identity.Name + "/AddCorePage", "PagesHelper.cs/AddCorePage");
                return false;
            }
        }
        public bool AddModulePage(int moduleId,string title,string name)
        {
            var moduleHelper = new ModuleHelper();
            var module = moduleHelper.GetModuleById(moduleId);
            string moduleTitle = "ماژول یافت نشد";
            if (module != null)
            {
                moduleTitle = module.Title;
            }
            var db = new CoreDbContext();
            try
            {
                db.Pages.Add(new Models.Page()
                {
                    ModuleId = moduleId,
                    Name = name,
                    Title = title
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا زمان ثبت صفحه برای ماژول " + moduleTitle,
                    "زمان ثبت صفحه برای ماژول " + moduleTitle + " خطای زیر رخ داد:<br/>" + ex.Message,
                     HttpContext.Current.User.Identity.Name + "/AddModulePage", "PagesHelper.cs/AddModulePage");
                return false;
            }
        }

        public bool EditPage(int pageId, string title)
        {
            var db = new CoreDbContext();
            try
            {
                var page = db.Pages.FirstOrDefault(p => p.PageId == pageId);
                if (page != null)
                {
                    page.Title = title;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا زمان ویرایش صفحه",
                    "زمان ویرایش صفحه خطای زیر رخ داد:<br/>" + ex.Message,
                     HttpContext.Current.User.Identity.Name + "/EditPage", "PagesHelper.cs/EditPage");
                return false;
            }
        }

        public bool DeletePageLayout(int pageLayoutId)
        {
            var db = new CoreDbContext();
            try
            {
                var pageLayout = db.PageLayouts.FirstOrDefault(pl => pl.PageLayoutId == pageLayoutId);
                db.PageLayouts.Remove(pageLayout);
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا در حذف چینش صفحه",
                    "زمان حذف چینش خطای زیر رخ داد:<br/>"
                    + ex.Message, HttpContext.Current.User.Identity.Name + "/DeletePageLayout","PagesHelper.cs");
                return false;
            }
        }

        public PageLayout GetPageLayout(int pageLayoutId)
        {
            var db = new CoreDbContext();
            return db.PageLayouts.FirstOrDefault(pl => pl.PageLayoutId == pageLayoutId);
        }

        public bool AddPageLayout(int pageId, int blockId, string zoneName)
        {
            var db = new CoreDbContext();
            try
            {
                db.PageLayouts.Add(new PageLayout()
                {
                    PageId = pageId,
                    BlockId = blockId,
                    ZoneName = zoneName,
                    AppearanceOrder = 0
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در ثبت چینش","متن خطا : <br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/AddPageLayout","PagesHelper.cs/AddPageLayout");
                return false;
            }
        }

        public bool EditAppearanceOrder(List<int> pageLayoutIds, List<int> appearenceOrder)
        {
            if (pageLayoutIds.Count == appearenceOrder.Count)
            {
                var i = 0;
                var db = new CoreDbContext();
                foreach (var pageLayoutId in pageLayoutIds)
                {
                    try
                    {
                        var pageLayout = db.PageLayouts.FirstOrDefault(pl => pl.PageLayoutId == pageLayoutId);
                        if (pageLayout != null)
                        {
                            pageLayout.AppearanceOrder = appearenceOrder[i];
                            db.SaveChanges();
                        }
                        i++;
                    }
                    catch (Exception ex)
                    {
                        var elogger = new ErrorLogger();
                        elogger.AddError("خطا در ثبت ترتیب نمایش بلاک ها",
                            "متن خطا :<br/>" + ex.Message,HttpContext.Current.User.Identity.Name + "/EditAppearanceOrder",
                            "PagesHelper.cs/EditAppearanceOrder");
                    }
                }
                return true;
            }
            else
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در ثبت ترتیب نمایش بلاک ها(تعداد ورودی ها)",
                    "متن خطا :<br/>" + "تعداد ورودی های ارسالی برابر نیست",HttpContext.Current.User.Identity.Name + "/EditAppearanceOrder",
                    "PagesHelper.cs/EditAppearanceOrder");
                return false;
            }
            
        }
    }
}