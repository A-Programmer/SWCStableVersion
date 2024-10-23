using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class ErrorLogger
    {
        public void AddError(string title,string message,string source,string path)
        {
            try
            {
                string username = "Not registered";
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    username = HttpContext.Current.User.Identity.Name;
                }
                var db = new CoreDbContext();
                db.ErrorLogs.Add(new ErrorLog()
                {
                    Title = title,
                    Message = message,
                    Source = source,
                    Path = path,
                    UserName = username,
                    ErrorDate = DateTime.Now
                });
                db.SaveChanges();
            }
            catch (Exception)
            {
                // ignored
            }
        }



    }
}