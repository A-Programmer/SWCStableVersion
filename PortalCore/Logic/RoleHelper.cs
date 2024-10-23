using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class RoleHelper
    {
        public bool DeleteUserRoles(int userid)
        {
            var db = new CoreDbContext();
            try
            {
                var userroles = db.UserRoles.Where(ur => ur.UserId == userid).ToList();
                foreach (var userrole in userroles)
                {
                    db.UserRoles.Remove(userrole);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در خذف نقش های کاربر", ex.Message,"حذف نقش های کاربر","RoleHelper.cs/DeleteUserRoles");
                return false;
            }
        }

        public List<Role> GetRoles()
        {
            var db = new CoreDbContext();
            var roles = db.Roles.OrderBy(r => r.RoleName).ToList();
            return roles;
        }

        public List<Role> GetUserRoles(int userid)
        {
            var db = new CoreDbContext();
            var userroleids = db.UserRoles.Where(ur => ur.UserId == userid).ToList();
            return userroleids.Select(urole => db.Roles.FirstOrDefault(u => u.RoleId == urole.RoleId)).ToList();
        }

        public void ChangeUserRole(int userid, int roleid)
        {
            var db = new CoreDbContext();
            var urole = db.UserRoles.FirstOrDefault(ur => ur.UserId == userid);
            if (urole != null)
            {
                urole.RoleId = roleid;
                db.SaveChanges();
            }
        }

        public Role GetRole(int roleid)
        {
            var db = new CoreDbContext();
            var role = db.Roles.FirstOrDefault(r => r.RoleId == roleid);
            return role;
        }

        public bool DeleteRole(int id)
        {
            var db = new CoreDbContext();
            var role = db.Roles.FirstOrDefault(r => r.RoleId == id);
            if (role != null)
            {
                var settings = db.GeneralSettings.FirstOrDefault();
                int defaultroleid = 1;
                if (settings != null)
                {
                    defaultroleid = settings.DefaultUserRoleId;
                }
                if (defaultroleid == id)
                {
                    return false;
                }
                var uroles = db.UserRoles.Where(ur => ur.RoleId == id).ToList();
                foreach (var urole in uroles)
                {
                    urole.RoleId = defaultroleid;
                }
                var rolepermissions = db.RolePermissions.Where(rp => rp.RoleId == id).ToList();
                foreach (var rolepermission in rolepermissions)
                {
                    db.RolePermissions.Remove(rolepermission);
                }
                db.Roles.Remove(role);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetDefaultRoleId()
        {
            var db = new CoreDbContext();
            var settings = db.GeneralSettings.FirstOrDefault();
            if (settings != null)
            {
                return settings.DefaultUserRoleId;
            }
            else
            {
                return 0;
            }
        }

        public bool AddRole(string name, string title)
        {
            var db = new CoreDbContext();
            try
            {
                db.Roles.Add(new Role()
                {
                    RoleName = name,
                    RoleTitle = title
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در ثبت گروه", "زمان ثبت گروه خطای زیر رخ داد <br/>" + ex.Message,
                    "AddRole", "RoleHelper.cs/AddRole");
                return false;
            }
        }

        public bool EditRole(int id, string name, string title)
        {
            var db = new CoreDbContext();
            var role = db.Roles.FirstOrDefault(r => r.RoleId == id);
            if (role != null)
            {
                role.RoleName = name;
                role.RoleTitle = title;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}