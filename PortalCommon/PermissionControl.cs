using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using PortalCommon.Models;

namespace PortalCommon
{
    public class PermissionControl
    {
        private static readonly HttpSessionState Session = HttpContext.Current.Session;
        private static readonly HttpApplicationState Application = HttpContext.Current.Application;

        public static bool CheckPermission(string PermissionName)
        {
            bool result = false;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                return false;
            }

            string Username = HttpContext.Current.User.Identity.Name;


            if (Application["AffectedUsers"] != null)
            {
                List<string> AffectedUsers = (List<string>)Application["AffectedUsers"];
                if (AffectedUsers.Contains(Username))
                {
                    Session["PermissionList"] = null;
                    AffectedUsers.Remove(Username);
                    Application["AffectedUsers"] = AffectedUsers;
                }

            }

            using (var db = new PortalDbContext())
            {
                var CurrentUser =
                    db.Users.FirstOrDefault(u => u.UserName == Username);
                if (CurrentUser != null)
                {
                    if (CurrentUser.IsSuperAdmin)
                    {
                        return true;
                    }
                    Session.Remove("PermissionList");
                    if (Session["PermissionList"] == null)
                    {
                        List<string> PermissionList = (from p in db.Permissions
                                                       join rp in db.RolePermissions on p.PermissionId equals
                                                           rp.PermissionId
                                                       join r in db.Roles on rp.RoleId equals r.RoleId
                                                       join ur in db.UserRoles on r.RoleId equals ur.RoleId
                                                       where ur.UserId == CurrentUser.UserId
                                                       select p.PermissionName).Distinct().ToList();

                        Session["PermissionList"] = PermissionList;


                        result = PermissionList.Contains(PermissionName);
                    }
                    else
                    {
                        var PermissionList = (List<string>)Session["PermissionList"];
                        result = PermissionList.Contains(PermissionName);

                    }
                }
                else
                {
                    return false;
                }
            }
            // remove following line later:
            return result;
        }
    }
}
