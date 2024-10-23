using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class PermissionHelper
    {

        public List<PermissionGroup> GetPermissionGroups()
        {
            var db = new CoreDbContext();
            var pgs = db.PermissionGroups.OrderBy(pg => pg.PermissionGroupId).ToList();
            return pgs;
        }

        public List<Permission> GetPermissionsForGroup(int id)
        {
            var db = new CoreDbContext();
            var permissions = db.Permissions.Where(p => p.PermissionGroupId == id)
                .OrderBy(ps => ps.PermissionId).ToList();
            return permissions;
        }

        public List<Permission> GetGrantedPermissions(int pgid, int roleid = 0)
        {
            var db = new CoreDbContext();
            var permissions = (from p in db.Permissions
                               join rp in db.RolePermissions on p.PermissionId equals rp.PermissionId
                               where rp.RoleId == roleid && p.PermissionGroupId == pgid
                               select p).ToList();

            return permissions;
        }

        public bool DeleteRolePermissions(int roleid)
        {
            var db = new CoreDbContext();
            var rps = db.RolePermissions.Where(r => r.RoleId == roleid).ToList();
            try
            {
                foreach (var rp in rps)
                {
                    db.RolePermissions.Remove(rp);
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در حذف دسترسی های گروه", "زمان حذف دسترسی های گروه خطای زیر رخ داد <br/>" + ex.Message,
                    "DeleteRolePermissions", "PermissionHelper.cs/DeleteRolePermissions");
                return false;
            }
        }

        public bool AddPermissionsForRole(int roleid, List<int> permissionids)
        {
            var db = new CoreDbContext();
            if (DeleteRolePermissions(roleid))
            {
                try
                {
                    foreach (var permissionid in permissionids)
                    {
                        db.RolePermissions.Add(new RolePermission()
                        {
                            RoleId = roleid,
                            PermissionId = permissionid
                        });
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                elogger.AddError("خطا در افزودن دسترسی های گروه", "زمان افودن دسترسی های گروه خطای زیر رخ داد <br/>" + ex.Message,
                    "AddPermissionsForRole", "PermissionHelper.cs/AddPermissionsForRole");
                return false;
            }
                }
            else
            {
                return false;
            }
            
        }


    }
}