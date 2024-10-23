using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class NotificationHelper
    {

        // واکشی یک اعلان با آی دی
        public Notification GetNotificaion(int id)
        {
            var db = new CoreDbContext();
            return db.Notifications.FirstOrDefault(n => n.Id == id);
        }

        // تعداد اعلانهای جدید کاربر
        public int GetNewNotificationsCount(string reciever)
        {
            var db = new CoreDbContext();
            return db.Notifications.Count(n => n.Reciver == reciever && !n.Status);
        }
        // لیست همه اعلان ها
        public List<Notification> GetNotifications()
        {
            var db = new CoreDbContext();
            return db.Notifications.OrderByDescending(n => n.Id).ToList();
        }
        //لیست اهمه اعلان های یک کاربر
        public List<Notification> GetNotifications(string reciever)
        {
            var db = new CoreDbContext();
            return db.Notifications.Where(nf => nf.Reciver == reciever).OrderByDescending(n => n.Id).ToList();
        }
        //لیست اهمه اعلان های خوانده نشده یک کاربر
        public List<Notification> GetNewNotifications(string reciever)
        {
            var db = new CoreDbContext();
            return db.Notifications.Where(nf => nf.Reciver == reciever && !nf.Status).OrderByDescending(n => n.Id).ToList();
        }
        // تغییر حالت اعلان به عنوان خوانده شده
        public bool ChangeStatus(int id)
        {
            var db = new CoreDbContext();
            var notification = db.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification != null)
            {
                try
                {
                    notification.Status = !notification.Status;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("موفق به تغییر حالت اعلان نشد", "کاربر نتوانست اعلانی را به حالت خوانده شده در بیاورد <br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name, "NotificationHelper.cs/MarkAsRead");
                    return false;
                }
            }
            else
            {
                var elogger = new ErrorLogger();
                elogger.AddError("اعلان مورد نظر یافت نشد.","اعلان مورد نظر برای تغییر حالت آن به خوانده شده یافت نشد",
                    HttpContext.Current.User.Identity.Name,"NotificationHelper.cs/MarkAsRead");
                return false;
            }
        }
        //حذف اعلان
        public bool DeleteNotification(int id)
        {
            var db = new CoreDbContext();
            var notification = db.Notifications.FirstOrDefault(n => n.Id == id);
            if (notification != null)
            {
                try
                {
                    db.Notifications.Remove(notification);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var elogger = new ErrorLogger();
                    elogger.AddError("موفق به حذف اعلان نشد", "کاربر نتوانست اعلانی را حذف کند <br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name, "NotificationHelper.cs/DeleteNotification");
                    return false;
                }
            }
            else
            {
                var elogger = new ErrorLogger();
                elogger.AddError("اعلان مورد نظر یافت نشد.", "اعلانی برای حذف یافت نشد.",
                    HttpContext.Current.User.Identity.Name, "NotificationHelper.cs/DeleteNotification");
                return false;
            }
        }

        //حذف اعلان های یک کاربر
        public bool DeleteUserNotifications(string username)
        {
            var db = new CoreDbContext();
            try
            {
                var notifications = db.Notifications.Where(n => n.Reciver == username).ToList();
                foreach (var notification in notifications)
                {
                    db.Notifications.Remove(notification);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در حذف اعلان های یک کاربر", ex.Message,username,
                    "NotificationHelper.cs/DeleteUserNotifications");
                return false;
            }
        }

        //افزودن اعلان
        public bool AddNotification(string title, string message, string reciever)
        {
            var db = new CoreDbContext();
            try
            {
                db.Notifications.Add(new Notification()
                {
                    Status = false,
                    Reciver = reciever,
                    Title = title,
                    Message = message,
                    SentDate = DateTime.Now
                });
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                var elogger = new ErrorLogger();
                elogger.AddError("خطا در ثبت اعلان", "زمان ثبت اعلان خطای زیر رخ داد <br/>" + ex.Message,
                    "AddNotification", "NotificationHelper.cs");
                return false;
            }
        }


    }
}