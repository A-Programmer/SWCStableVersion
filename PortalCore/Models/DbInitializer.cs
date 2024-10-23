using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PortalCore.Logic;

namespace PortalCore.Models
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<CoreDbContext>
    {
        protected override void Seed(CoreDbContext context)
        {
            GetRoles().ForEach(r => context.Roles.Add(r));
            GetPermissionGroups().ForEach(pg => context.PermissionGroups.Add(pg));
            GetPermissions().ForEach(p => context.Permissions.Add(p));
            GetTemplates().ForEach(t => context.Templates.Add(t));
            GetPages().ForEach(p => context.Pages.Add(p));
            GetRolePermissions().ForEach(rp => context.RolePermissions.Add(rp));
            GeneralSetting().ForEach(gses => context.GeneralSettings.Add(gses));
            GetZones().ForEach(z => context.Zones.Add(z));
            GetUsers().ForEach(u => context.Users.Add(u));
            GetUserRoles().ForEach(ur => context.UserRoles.Add(ur));
            GetNotifications().ForEach(n => context.Notifications.Add(n));
            context.SaveChanges();
        }
        
        // Zone ha ro bayad check konam chon hanuz qalebe default nazashtam nemidunam che zone haii lazem dare
        private static List<Zone> GetZones()
        {
            var zones = new List<Zone>()
            {
                new Zone()
                {
                    Id = 1,
                    Name = "DefaultHeaderHomeZone",
                    Title = "منطقه بالایی صفحه اصلی",
                    TemplateName = "Default",
                    CssClass = ".homeHeader"
                },
                new Zone()
                {
                    Id = 2,
                    Name = "DefaulMainHomeZone",
                    Title = "منطقه میانی صفحه اصلی",
                    TemplateName = "Default",
                    CssClass = ".homeMainCol"
                },
                new Zone()
                {
                    Id = 3,
                    Name = "DefaultHeaderZone",
                    Title = "منطقه بالایی صفحات",
                    TemplateName = "Default",
                    CssClass = ".headerCol"
                },
                new Zone()
                {
                    Id = 4,
                    Name = "DefaultRightZone",
                    Title = "منطقه راست صفحات",
                    TemplateName = "Default",
                    CssClass = ".rightCol"
                },
                new Zone()
                {
                    Id = 5,
                    Name = "DefaultMainZone",
                    Title = " صفحاتمنطقه وسط",
                    TemplateName = "Default",
                    CssClass = ".mainCol"
                },
                new Zone()
                {
                    Id = 3,
                    Name = "DefaultLeftZone",
                    Title = "منطقه وسط صفحات",
                    TemplateName = "Default",
                    CssClass = ".leftCol"
                },
                new Zone()
                {
                    Id = 3,
                    Name = "DefaultFooterZone",
                    Title = "منطقه پانوشت صفحات",
                    TemplateName = "Default",
                    CssClass = ".footerCol"
                },
            };
            return zones;
        }
        private static List<GeneralSetting> GeneralSetting()
        {
            var generalsettings = new List<GeneralSetting>()
            {
                new GeneralSetting()
                {
                    Title = "عنوان وب سایت",
                    Name = " ",
                    ActiveTemplateId = 1,
                    ActiveTemplateName = "Default",
                    DefaultUserRoleId = 3,
                    Description = "توضیحات وب سایت",
                    EnableRegistration = true,
                    //AllowExternalLogin = true,
                    EnablePasswordRecovery = true,
                    EnableUnlockRegistratin = true,
                    InstallDate = DateTime.Now,
                    Status = true,
                    RunDate = DateTime.Now,
                    Tags = "برچسب های وب سایت",
                    HomeUrl = "http://localhost/"
                }
            };
            return generalsettings;
        }
        private static List<Template> GetTemplates()
        {
            var templates = new List<Template>()
            {
                new Template()
                {
                    Title = "قالب پیش فرض",
                    Name = "Default",
                    IsActive = true,
                    IsInstalled = true
                }
            };
            return templates;
        }
        private static List<Models.Page> GetPages()
        {
            var pages = new List<Page>()
            {
                new Page()
                {
                    Title = "صفحه اصلی",
                    Name = "Default",
                    ModuleId = null
                }
            };
            return pages;
        }
        private static List<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    RoleId = 1,
                    RoleName = "مدیران",
                    RoleTitle = "Admins"
                },
                new Role()
                {
                    RoleId = 2,
                    RoleName = "نماینده ها",
                    RoleTitle = "Managers"
                },
                new Role()
                {
                    RoleId = 3,
                    RoleName = "کاربران",
                    RoleTitle = "Users"
                },
                new Role()
                {
                    RoleId = 4,
                    RoleName = "کاربران نمایشی",
                    RoleTitle = "DemoUsers"
                }
            };
            return roles;
        }
        private static List<User> GetUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    UserId = 1,
                    Email = "YourEmailAddress@Domain.Com",
                    IsActive = true,
                    IsSuperAdmin = true,
                    Password = "123456",
                    UserName = "SuperAdmin",
                    UserPicture = "/UserImage/DefaultProfilePicture.jpg"
                },
                new User()
                {
                    UserId = 2,
                    FirstName = "کاربر",
                    LastName = "نمایشی",
                    Email = "YourEmailAddress@Domain.Com",
                    IsActive = true,
                    IsSuperAdmin = false,
                    Password = "123456",
                    UserName = "Demo",
                    UserPicture = "/UserImage/DefaultProfilePicture.jpg"
                },
            };
            return users;
        }
        private List<UserRole> GetUserRoles()
        {
            var userroles = new List<UserRole>()
            {
                new UserRole()
                {
                    UserId = 1,
                    RoleId = 1
                },
                new UserRole()
                {
                    UserId = 2,
                    RoleId = 4
                }
            };
            return userroles;
        }
        private static List<PermissionGroup> GetPermissionGroups()
        {
            var pgs = new List<PermissionGroup>()
            {
                new PermissionGroup()
                {
                    PermissionGroupId = 1,
                    ModuleId = null,
                    PermissionGroupTitle = "عمومی"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 2,
                    ModuleId = null,
                    PermissionGroupTitle = "مدیریت کاربران"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 3,
                    ModuleId = null,
                    PermissionGroupTitle = "مدیریت گروه ها"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 4,
                    ModuleId = null,
                    PermissionGroupTitle = "مدیریت دسترسی ها"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 5,
                    ModuleId = null,
                    PermissionGroupTitle = "مدیریت امکانات"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 6,
                    ModuleId = null,
                    PermissionGroupTitle = "مدیریت صفحات"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 7,
                    ModuleId = null,
                    PermissionGroupTitle = "مدیریت قالب ها"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 8,
                    ModuleId = null,
                    PermissionGroupTitle = "دسترسی به نمایه"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 9,
                    ModuleId = null,
                    PermissionGroupTitle = "مجوزها"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 10,
                    ModuleId = null,
                    PermissionGroupTitle = "به روز رسانی هسته"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 11,
                    ModuleId = null,
                    PermissionGroupTitle = "تنظیمات عمومی"
                },
                new PermissionGroup()
                {
                    PermissionGroupId = 12,
                    ModuleId = null,
                    PermissionGroupTitle = "تنظیمات ایمیل"
                }
            };
            return pgs;
        }
        private static List<Permission> GetPermissions()
        {
            var permissions = new List<Permission>()
            {
                new Permission()
                {
                    PermissionId = 1,
                    PermissionGroupId = 1,
                    PermissionName = "ManageUsers",
                    PermissionTitle = "مدیریت کاربران"
                },
                new Permission()
                {
                    PermissionId = 2,
                    PermissionGroupId = 1,
                    PermissionName = "ManageRoles",
                    PermissionTitle = "مدیریت گروه ها"
                },
                new Permission()
                {
                    PermissionId = 3,
                    PermissionGroupId = 1,
                    PermissionName = "GeneralSettings",
                    PermissionTitle = "تنظیمات عمومی"
                },
                new Permission()
                {
                    PermissionId = 4,
                    PermissionGroupId = 1,
                    PermissionName = "EmailSettings",
                    PermissionTitle = "تنظیمات ایمیل"
                },
                new Permission()
                {
                    PermissionId = 5,
                    PermissionGroupId = 1,
                    PermissionName = "ManagePermissions",
                    PermissionTitle = "مدیریت دسترسی ها"
                },
                new Permission()
                {
                    PermissionId = 6,
                    PermissionGroupId = 1,
                    PermissionName = "ManageModules",
                    PermissionTitle = "مدیریت امکانات"
                },
                new Permission()
                {
                    PermissionId = 7,
                    PermissionGroupId = 1,
                    PermissionName = "ManagePages",
                    PermissionTitle = "مدیریت صفحات"
                },
                new Permission()
                {
                    PermissionId = 8,
                    PermissionGroupId = 1,
                    PermissionName = "ManageTemplates",
                    PermissionTitle = "مدیریت قالب ها"
                },
                new Permission()
                {
                    PermissionId = 9,
                    PermissionGroupId = 1,
                    PermissionName = "EditProfile",
                    PermissionTitle = "ویرایش نمایه"
                },
                new Permission()
                {
                    PermissionId = 10,
                    PermissionGroupId = 1,
                    PermissionName = "CoreUpdates",
                    PermissionTitle = "به روزرسانی هسته"
                },
                new Permission()
                {
                    PermissionId = 11,
                    PermissionGroupId = 1,
                    PermissionName = "Licence",
                    PermissionTitle = "مجوزها"
                },
                new Permission()
                {
                    PermissionId = 12,
                    PermissionGroupId = 2,
                    PermissionName = "UsersList",
                    PermissionTitle = "لیست کاربران"
                },
                new Permission()
                {
                    PermissionId = 13,
                    PermissionGroupId = 2,
                    PermissionName = "AddUser",
                    PermissionTitle = "افزودن کاربر"
                },
                new Permission()
                {
                    PermissionId = 14,
                    PermissionGroupId = 2,
                    PermissionName = "EditUser",
                    PermissionTitle = "ویرایش کاربر"
                },
                new Permission()
                {
                    PermissionId = 15,
                    PermissionGroupId = 2,
                    PermissionName = "SubmitEditUser",
                    PermissionTitle = "ثبت تغییرات کاربر"
                },
                new Permission()
                {
                    PermissionId = 16,
                    PermissionGroupId = 2,
                    PermissionName = "DeleteUser",
                    PermissionTitle = "حذف کاربر"
                },
                new Permission()
                {
                    PermissionId = 17,
                    PermissionGroupId = 2,
                    PermissionName = "LockUser",
                    PermissionTitle = "تغییر وضعیت قفل کاربر"
                },
                new Permission()
                {
                    PermissionId = 18,
                    PermissionGroupId = 2,
                    PermissionName = "UsersStats",
                    PermissionTitle = "آمار کاربران"
                },
                new Permission()
                {
                    PermissionId = 19,
                    PermissionGroupId = 3,
                    PermissionName = "RolesList",
                    PermissionTitle = "لیست گروه ها"
                },
                new Permission()
                {
                    PermissionId = 20,
                    PermissionGroupId = 3,
                    PermissionName = "AddNewRole",
                    PermissionTitle = "افزودن گروه"
                },
                new Permission()
                {
                    PermissionId = 21,
                    PermissionGroupId = 3,
                    PermissionName = "EditRole",
                    PermissionTitle = "ویرایش گروه"
                },
                new Permission()
                {
                    PermissionId = 22,
                    PermissionGroupId = 3,
                    PermissionName = "EditRole",
                    PermissionTitle = "ثبت تغییرات گروه"
                },
                new Permission()
                {
                    PermissionId = 23,
                    PermissionGroupId = 3,
                    PermissionName = "DeleteRole",
                    PermissionTitle = "حذف گروه"
                },
                new Permission()
                {
                    PermissionId = 24,
                    PermissionGroupId = 4,
                    PermissionName = "ManagePermissions",
                    PermissionTitle = "مدیریت دسترسی ها"
                },
                new Permission()
                {
                    PermissionId = 25,
                    PermissionGroupId = 4,
                    PermissionName = "EditRolePermissions",
                    PermissionTitle = "ویرایش دسترسی های گروه"
                },
                new Permission()
                {
                    PermissionId = 26,
                    PermissionGroupId = 4,
                    PermissionName = "SubmitRolePermissionChanges",
                    PermissionTitle = "ثبت تغییرات دسترسی ها"
                },
                new Permission()
                {
                    PermissionId = 27,
                    PermissionGroupId = 5,
                    PermissionName = "ModulesList",
                    PermissionTitle = "لیست امکانات"
                },
                new Permission()
                {
                    PermissionId = 28,
                    PermissionGroupId = 5,
                    PermissionName = "UploadModule",
                    PermissionTitle = "ارسال امکانات"
                },
                new Permission()
                {
                    PermissionId = 29,
                    PermissionGroupId = 5,
                    PermissionName = "UninstallModule",
                    PermissionTitle = "لغو نصب امکانات"
                },
                new Permission()
                {
                    PermissionId = 30,
                    PermissionGroupId = 5,
                    PermissionName = "InstallModule",
                    PermissionTitle = "نصب امکانات"
                },
                new Permission()
                {
                    PermissionId = 31,
                    PermissionGroupId = 5,
                    PermissionName = "DeleteModule",
                    PermissionTitle = "حذف امکانات"
                },
                new Permission()
                {
                    PermissionId = 32,
                    PermissionGroupId = 6,
                    PermissionName = "PagesList",
                    PermissionTitle = "لیست صفحات"
                },
                new Permission()
                {
                    PermissionId = 33,
                    PermissionGroupId = 6,
                    PermissionName = "AddNewPage",
                    PermissionTitle = "افزودن صفحه"
                },
                new Permission()
                {
                    PermissionId = 34,
                    PermissionGroupId = 6,
                    PermissionName = "EditPage",
                    PermissionTitle = "ویرایش صفحه"
                },
                new Permission()
                {
                    PermissionId = 35,
                    PermissionGroupId = 6,
                    PermissionName = "SubmitPage",
                    PermissionTitle = "ثبت مشخصات صفحه"
                },
                new Permission()
                {
                    PermissionId = 36,
                    PermissionGroupId = 6,
                    PermissionName = "DeletePage",
                    PermissionTitle = "حذف صفحه"
                },
                new Permission()
                {
                    PermissionId = 37,
                    PermissionGroupId = 6,
                    PermissionName = "LayoutsList",
                    PermissionTitle = "لیست چینش صفحه"
                },
                new Permission()
                {
                    PermissionId = 38,
                    PermissionGroupId = 6,
                    PermissionName = "AddLayout",
                    PermissionTitle = "افزودن چینش جدید"
                },
                new Permission()
                {
                    PermissionId = 39,
                    PermissionGroupId = 6,
                    PermissionName = "EditPageLayout",
                    PermissionTitle = "ویرایش چینش صفحه"
                },
                new Permission()
                {
                    PermissionId = 40,
                    PermissionGroupId = 6,
                    PermissionName = "SubmitPageLayout",
                    PermissionTitle = "ثبت تغییرات چینش صفحه"
                },
                new Permission()
                {
                    PermissionId = 41,
                    PermissionGroupId = 6,
                    PermissionName = "DeletePageLayout",
                    PermissionTitle = "حذف چینش صفحه"
                },
                new Permission()
                {
                    PermissionId = 42,
                    PermissionGroupId = 7,
                    PermissionName = "UploadTheme",
                    PermissionTitle = "ارسال قالب جدید"
                },
                new Permission()
                {
                    PermissionId = 43,
                    PermissionGroupId = 7,
                    PermissionName = "TemplatesList",
                    PermissionTitle = "لیست قالب ها"
                },
                new Permission()
                {
                    PermissionId = 44,
                    PermissionGroupId = 7,
                    PermissionName = "ActiveTheme",
                    PermissionTitle = "فعال کردن قالب"
                },
                new Permission()
                {
                    PermissionId = 45,
                    PermissionGroupId = 7,
                    PermissionName = "DeleteTheme",
                    PermissionTitle = "حذف قالب"
                },
                new Permission()
                {
                    PermissionId = 46,
                    PermissionGroupId = 8,
                    PermissionName = "SubmitProfileChanges",
                    PermissionTitle = "ثبت تغییرات نمایه"
                },
                new Permission()
                {
                    PermissionId = 47,
                    PermissionGroupId = 8,
                    PermissionName = "ChangePassword",
                    PermissionTitle = "تغییر کلمه عبور"
                },
                new Permission()
                {
                    PermissionId = 48,
                    PermissionGroupId = 8,
                    PermissionName = "SubmitChangePassword",
                    PermissionTitle = "ثبت تغییر کلمه عبور"
                },
                new Permission()
                {
                    PermissionId = 49,
                    PermissionGroupId = 9,
                    PermissionName = "SubmitCoreLicense",
                    PermissionTitle = "ثبت مجوز هسته"
                },
                new Permission()
                {
                    PermissionId = 50,
                    PermissionGroupId = 9,
                    PermissionName = "SubmitModuleLicense",
                    PermissionTitle = "ثبت مجوز امکانات"
                },
                new Permission()
                {
                    PermissionId = 51,
                    PermissionGroupId = 9,
                    PermissionName = "EditModuleLicense",
                    PermissionTitle = "ویرایش مجوز امکانات"
                },
                new Permission()
                {
                    PermissionId = 52,
                    PermissionGroupId = 9,
                    PermissionName = "DeleteModuleLicense",
                    PermissionTitle = "حذف مجوز"
                },
                new Permission()
                {
                    PermissionId = 53,
                    PermissionGroupId = 10,
                    PermissionName = "SubmitUpdate",
                    PermissionTitle = "ارسال به روزرسانی"
                },
                new Permission()
                {
                    PermissionId = 54,
                    PermissionGroupId = 11,
                    PermissionName = "SubmitGeneralSettings",
                    PermissionTitle = "ثبت تنظیمات کلی"
                },
                new Permission()
                {
                    PermissionId = 55,
                    PermissionGroupId = 12,
                    PermissionName = "SubmitEmailSettings",
                    PermissionTitle = "ثبت تنظیمات ایمیل"
                }
            };
            return permissions;
        }
        private static List<RolePermission> GetRolePermissions()
        {
            var rp = new List<RolePermission>()
            {
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 1
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 2
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 3
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 4
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 5
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 6
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 7
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 8
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 9
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 10
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 11
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 12
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 13
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 14
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 19
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 20
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 21
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 24
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 25
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 27
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 32
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 33
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 34
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 37
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 38
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 39
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 43
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 47
                },
                new RolePermission()
                {
                    RoleId = 4,
                    PermissionId = 51
                }
            };
            return rp;
        }
        private List<Notification> GetNotifications()
        {
            var notifications = new List<Notification>()
            {
                new Notification()
                {
                    Status = false,
                    Reciver = "superadmin",
                    SentDate = DateTime.Now,
                    Title = "حساب کاربری شما با موفقیت ثبت شد.",
                    Message = "سلام!<br/>حساب شما با موفقیت ثبت شد، حالا می توانید از قسمت ویرایش نمایه اطلاعات خود را کامل یا ویرایش کنید."
                },
                new Notification()
                {
                    Status = false,
                    Reciver = "user",
                    SentDate = DateTime.Now,
                    Title = "حساب کاربری شما با موفقیت ثبت شد.",
                    Message = "سلام!<br/>حساب شما با موفقیت ثبت شد، حالا می توانید از قسمت ویرایش نمایه اطلاعات خود را کامل یا ویرایش کنید."
                },
                new Notification()
                {
                    Status = false,
                    Reciver = "manager",
                    SentDate = DateTime.Now,
                    Title = "حساب کاربری شما با موفقیت ثبت شد.",
                    Message = "سلام!<br/>حساب شما با موفقیت ثبت شد، حالا می توانید از قسمت ویرایش نمایه اطلاعات خود را کامل یا ویرایش کنید."
                },
                new Notification()
                {
                    Status = false,
                    Reciver = "admin",
                    SentDate = DateTime.Now,
                    Title = "حساب کاربری شما با موفقیت ثبت شد.",
                    Message = "سلام!<br/>حساب شما با موفقیت ثبت شد، حالا می توانید از قسمت ویرایش نمایه اطلاعات خود را کامل یا ویرایش کنید."
                },
                new Notification()
                {
                    Status = false,
                    Reciver = "banned",
                    SentDate = DateTime.Now,
                    Title = "حساب کاربری شما با موفقیت ثبت شد.",
                    Message = "سلام!<br/>حساب شما با موفقیت ثبت شد، حالا می توانید از قسمت ویرایش نمایه اطلاعات خود را کامل یا ویرایش کنید."
                }
            };
            return notifications;
        } 

    }
}