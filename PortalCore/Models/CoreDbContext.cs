using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;

namespace PortalCore.Models
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext() 
            : base("PortalConnectionString")
        {
             
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Models.GeneralSetting> GeneralSettings { get; set; }
        public DbSet<PortalEmailSetting> PortalEmailSettings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Template> Templates { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<PageLayout> PageLayouts { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<FaqCategory> FaqCategories { get; set; }
    }
}
