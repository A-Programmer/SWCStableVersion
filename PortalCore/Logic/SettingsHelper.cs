using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class SettingsHelper
    {

        public GeneralSetting GetSettings()
        {
            var db = new CoreDbContext();
            return db.GeneralSettings.FirstOrDefault();
        }

        public PortalEmailSetting GetEmailSettings()
        {
            var db = new CoreDbContext();
            return db.PortalEmailSettings.FirstOrDefault();
        }

        public bool EditEmailSettings(string email, string password, string host, int port)
        {
            var db = new CoreDbContext();
            var esettings = db.PortalEmailSettings.FirstOrDefault();
            if (esettings != null)
            {
                try
                {
                    esettings.EmailAddress = email;
                    esettings.EmailPassword = password;
                    esettings.EmailSmtpAddress = host;
                    esettings.EmailSmtpPort = port;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var eLogger = new ErrorLogger();
                    eLogger.AddError("خطا در ویرایش تنظیمات ایمیل",
                        "زمان ویرایش تنظیمات ایمیل خطای زیر رخ داده است:<br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/EditEmailSettings",
                        "SettingsHelper.cs/EditEmailSettings");
                    return false;
                }
            }
            else
            {
                db.PortalEmailSettings.Add(new PortalEmailSetting()
                {
                    EmailAddress = email,
                    EmailPassword = password,
                    EmailSmtpAddress = host,
                    EmailSmtpPort = port
                });
                db.SaveChanges();
                return true;
            }
        }

        public bool EditSettings(string homeUrl,string title,bool status,DateTime runDate,bool enableRegistration,
            bool enableUnlockRegistration,/*bool allowExternalLogin,*/bool enablePasswordRecovery,int defaultRole,
            string templateName,int templateId,string tags,string description)
        {
            var db = new CoreDbContext();
            var settings = db.GeneralSettings.FirstOrDefault();
            if (settings != null)
            {
                try
                {
                    settings.HomeUrl = homeUrl;
                    settings.Title = title;
                    settings.Status = status;
                    settings.RunDate = runDate;
                    settings.EnableRegistration = enableRegistration;
                    settings.EnableUnlockRegistratin = enableUnlockRegistration;
                    //settings.AllowExternalLogin = allowExternalLogin;
                    settings.EnablePasswordRecovery = enablePasswordRecovery;
                    settings.DefaultUserRoleId = defaultRole;
                    settings.ActiveTemplateName = templateName;
                    settings.ActiveTemplateId = templateId;
                    settings.Tags = tags;
                    settings.Description = description;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    var eLogger = new ErrorLogger();
                    eLogger.AddError("خطا در ویرایش تنظیمات عمومی",
                        "زمان ویرایش تنظیمات عمومی خطای زیر رخ داد:<br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/EditSettings","SettingsHelper.cs/EditSettings");
                    return false;
                }
            }
            else
            {
                db.GeneralSettings.Add(new GeneralSetting()
                {
                    Name = " ",
                    Title = title,
                    HomeUrl = homeUrl,
                    Tags = tags,
                    Description = description,
                    InstallDate = DateTime.Now,
                    Status = status,
                    RunDate = runDate,
                    EnableRegistration = enableRegistration,
                    EnableUnlockRegistratin = enableUnlockRegistration,
                    //AllowExternalLogin = allowExternalLogin,
                    EnablePasswordRecovery = enablePasswordRecovery,
                    DefaultUserRoleId = defaultRole,
                    ActiveTemplateName = templateName,
                    ActiveTemplateId = templateId
                });
                db.SaveChanges();
                return true;
            }
        }

        public void CheckIfUnderConstruction()
        {
            var db = new CoreDbContext();
            var settings = GetSettings();
            if (settings != null)
            {
                if (!settings.Status)
                {
                    var runDate = Convert.ToDateTime(settings.RunDate);
                    var toDay = DateTime.Now;
                    if (toDay < runDate)
                    {
                        HttpContext.Current.Response.Redirect("~/Error/UnderConstruction");
                    }
                }
            }
            else
            {
                HttpContext.Current.Response.Redirect("~/Error/UnderConstruction");
            }
        }

    }
}