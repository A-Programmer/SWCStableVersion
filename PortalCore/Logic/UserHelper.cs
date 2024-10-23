using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class UserHelper
    {
        public bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            using (var db = new CoreDbContext())
            {
                var user =
                    db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password && u.IsActive);

                if (user != null)
                {
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int Register(string username, string password, string email)
        {
            if (string.IsNullOrEmpty(username) || String.IsNullOrWhiteSpace(username))
            {
                return 2;
            }
            if (string.IsNullOrEmpty(password) || String.IsNullOrWhiteSpace(password))
            {
                return 3;
            }
            if (string.IsNullOrEmpty(email) || String.IsNullOrWhiteSpace(email))
            {
                return 4;
            }
            using (var db = new CoreDbContext())
            {
                var user =
                    db.Users.FirstOrDefault(u => u.UserName == username || u.Email == password);

                if (user != null)
                {

                    return 5;
                }
                else
                {
                    var settings = db.GeneralSettings.FirstOrDefault();
                    //اول چک میکنه که آیا اجازه ثبت نام داده شده یا خیر
                    if (settings != null && settings.EnableRegistration)
                    {
                        bool unlock = settings.EnableUnlockRegistratin;
                        //کاربر رو ثبت می کنه
                        db.Users.Add(new User()
                        {
                            Email = email,
                            UserName = username,
                            Password = password,
                            IsSuperAdmin = false,
                            UserPicture = "~/UserImage/DefaultProfilePicture.jpg",
                            IsActive = unlock
                        });
                        db.SaveChanges();
                        var registereduser =
                            db.Users.FirstOrDefault(u => u.UserName == username);
                        // نقش پیش فرض رو به کاربر میده
                        db.UserRoles.Add(new UserRole()
                        {
                            RoleId = settings.DefaultUserRoleId,
                            UserId = registereduser.UserId
                        });
                        var nhelper = new NotificationHelper();
                        nhelper.AddNotification("حساب کاربری شما ساخته شد",
                        "سلام " + username + " حساب کاربری شما با موفقیت ساخته شد اکنون میتوانید مشخصات خود را ویرایش کنید.", username);

                        db.SaveChanges();

                        FormsAuthentication.RedirectFromLoginPage(username, false);

                        return 0;
                    }
                    else
                    {
                        return 6;
                    }
                }
            }
        }

        public User GetUserByName(string username)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.Email == email);
            return user;
        }

        public User GetUserById(int id)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            return user;
        }

        public string Recovery(string email)
        {
            if (!string.IsNullOrEmpty(email) && !String.IsNullOrWhiteSpace(email))
            {
                using (var db = new CoreDbContext())
                {
                    var user =
                        db.Users.FirstOrDefault(u => u.Email == email);

                    if (user != null)
                    {
                        return sendEmail("بازیابی کلمه عبور",
                            "UserName : " + user.UserName + "<br/>Password : " + user.Password,
                            user.Email);

                    }
                    else
                    {

                        return "کاربری با ایمیل وارد شده یافت نشد.";
                    }
                }
            }
            else
            {
                return "ایمیل بازیابی وارد نشده است.";
            }

        }

        public string sendEmail(string title, string body, string address)
        {
            var db = new CoreDbContext();
            var mailsettings = db.PortalEmailSettings.FirstOrDefault();
            if (mailsettings != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(address);
                    mail.Subject = title;
                    mail.From = new MailAddress(mailsettings.EmailAddress);
                    string Body = body;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential(mailsettings.EmailAddress, mailsettings.EmailPassword);
                    smtp.Host = mailsettings.EmailSmtpAddress;
                    smtp.Port = mailsettings.EmailSmtpPort;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                    return "ایمیل شما با موفقیت ارسال شد.";
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("اشکال در ارسال ایمیل", ex.Message, "تابع sendEmail",
                        "UserHelper.cs");
                    return "مشکلی در ارسال ایمیل پیش آمده است، به مدیر سایت اطلاع دهید.";
                }
            }
            else
            {
                var elogger = new ErrorLogger();
                elogger.AddError("اشکال در ارسال ایمیل", "تنظیمات ایمیل وب سایت در بانک اطلاعاتی موجود نیست", "تابع sendEmail",
                    "UserHelper.cs");
                return "مشکلی در ارسال ایمیل رخ داده است با مدیر سایت تماس بگیرید.";
            }

        }

        public bool ChangePassword(string txtUserName, string txtCurrent, string txtNewPassword)
        {
            if (ValidateUser(txtUserName, txtCurrent))
            {
                var db = new CoreDbContext();
                var user = db.Users.FirstOrDefault(u => u.UserName == txtUserName && u.Password == txtCurrent);
                if (user != null)
                {
                    user.Password = txtNewPassword;
                    db.SaveChanges();
                    return true;
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

        public bool EditUser(string username,string email, string name, string lastname, string phone,string info,string pic)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            if(user != null)
            {
                try
                {
                    user.FirstName = name;
                    user.LastName = lastname;
                    user.Email = email;
                    user.Phone = phone;
                    user.UserInfo = info;
                    user.UserPicture = pic;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("خطا در ویرایش پروفایل کاربر",ex.Message,
                        HttpContext.Current.User.Identity.Name,"UserHelper.cs/EditUser");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool AdminEditUser(int id,string username, string email, string name, string lastname, string phone,
            string info/*,bool superadmin*/,bool active,int roleid)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                try
                {
                    user.FirstName = name;
                    user.LastName = lastname;
                    user.Email = email;
                    user.Phone = phone;
                    user.UserInfo = info;
                    user.IsActive = active;
                    //user.IsSuperAdmin = superadmin;
                    db.SaveChanges();

                    var rhelper = new RoleHelper();
                    rhelper.ChangeUserRole(user.UserId,roleid);

                    return true;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("خطا در ویرایش پروفایل کاربر", ex.Message,
                        HttpContext.Current.User.Identity.Name, "UserHelper.cs/EditUser");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public List<User> GetAllUsers()
        {
            var db = new CoreDbContext();
            var users = db.Users.OrderByDescending(u => u.UserId).ToList();
            return users;
        }

        public List<User> SearchUsers(string key)
        {
            var db = new CoreDbContext();
            var users = db.Users.Where(u => u.UserName.Contains(key) || u.Email.Contains(key)
                                            || u.FirstName.Contains(key) || u.LastName.Contains(key))
                .OrderByDescending(ui => ui.UserId).ToList();
            return users;
        }

        public int CreateUser(string username, string password, string email,string phone,string info,
            int roleid/*, bool issuperadmin*/, bool active, string name, string lastname,
            string pic = "~/UserImage/DefaultProfilePicture.jpg")
        {
            var db = new CoreDbContext();
            if (GetUserByName(username) == null && GetUserByEmail(email) == null)
            {
                try
                {
                    db.Users.Add(new User()
                    {
                        UserName = username,
                        Password = password,
                        Email = email,
                        FirstName = name,
                        IsActive = active,
                        //IsSuperAdmin = issuperadmin,
                        LastName = lastname,
                        Phone = phone,
                        UserInfo = info,
                        UserPicture = pic
                    });
                    var nhelper = new NotificationHelper();
                    nhelper.AddNotification("حساب کاربری شما ساخته شد",
                        "سلام " + name + " " + lastname + " حساب کاربری شما با موفقیت ساخته شد", username);

                    db.SaveChanges();
                    return 0;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("خطا در ثبت کاربر توسط مدیر", ex.Message, HttpContext.Current.User.Identity.Name,
                        "UserHelper.cs/CreateUser");
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public bool DeleteUser(int userid)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == userid);
            if (user != null)
            {
                var nhelper = new NotificationHelper();
                var rhelper = new RoleHelper();
                nhelper.DeleteUserNotifications(user.UserName);
                rhelper.DeleteUserRoles(user.UserId);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteCurrentUser()
        {
            var db = new CoreDbContext();
            string username = HttpContext.Current.User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                var nhelper = new NotificationHelper();
                var rhelper = new RoleHelper();
                nhelper.DeleteUserNotifications(user.UserName);
                rhelper.DeleteUserRoles(user.UserId);
                db.Users.Remove(user);
                db.SaveChanges();
                LogOut();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeUserStatus(int id)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                if (user.IsActive)
                {
                    user.IsActive = false;
                    db.SaveChanges();
                }
                else
                {
                    user.IsActive = true;
                    db.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeSuperAdminMode(int id)
        {
            var db = new CoreDbContext();
            var user = db.Users.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                if (user.IsSuperAdmin)
                {
                    user.IsSuperAdmin = false;
                    db.SaveChanges();
                }
                else
                {
                    user.IsSuperAdmin = true;
                    db.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}