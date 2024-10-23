using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace PortalCore.Logic
{
    public class LicenseHelper
    {
        // Core First License : SiteName.Domain + "core"
        // Core Second License : DateTime.Now.AddMonths(12) + "|sitename.domain"
        //Module First License : sitename.domain + modulename
        // Module Second License : DateTime +"|" + siteName.Domain + "|" + ModuleName

        public bool CheckCoreLicense()
        {
            var sitename = HttpContext.Current.Request.Url.Host.ToLower();
            if (sitename.ToLower() == "localhost")
            {
                return true;
            }
            var keyname = sitename + "core";
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                var ed = new EncryptDecrypt();
                var coreLicenseKey = config.AppSettings.Settings[keyname].Value;
                var generatedKey = ed.Encrypt(keyname);
                if (coreLicenseKey == generatedKey)
                {
                    if (config.AppSettings.Settings[sitename + "ex"] != null)
                    {
                        var coreSeccondKeyValue = config.AppSettings.Settings[sitename + "ex"].Value;
                        var decryptedCoreSeccondKeyValue = ed.Decrypt(coreSeccondKeyValue).Split('|');
                        if (decryptedCoreSeccondKeyValue.Count() == 2)
                        {
                            if (decryptedCoreSeccondKeyValue[1].ToLower() == sitename)
                            {
                                var coreExpireDate = decryptedCoreSeccondKeyValue[0];
                                DateTime licensetime;
                                if (DateTime.TryParse(coreExpireDate, out licensetime))
                                {
                                    var today = DateTime.Now;
                                    if (today <= licensetime)
                                    {
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
                            else
                            {
                                return false; //Wrong Seccond License Key, It does not belong to core
                            }
                        }
                        else
                        {
                            return false; //Wrong Seccond License Key, It does not belong to core
                        }
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
            else
            {
                return false;
            }
        }
        public bool CheckCoreLicenseExistance(string keyName)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyName] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCoreLicenseStatus()
        {
            var sitename = HttpContext.Current.Request.Url.Host.ToLower();
            //if (sitename.ToLower() == "localhost")
            //{
            //    return "بدون محدودیت";
            //}
            var keyname = sitename + "core";
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                var ed = new EncryptDecrypt();
                var coreLicenseKey = config.AppSettings.Settings[keyname].Value;
                var generatedKey = ed.Encrypt(keyname);
                if (coreLicenseKey == generatedKey)
                {
                    if (config.AppSettings.Settings[sitename + "ex"] != null)
                    {
                        var coreSeccondKeyValue = config.AppSettings.Settings[sitename + "ex"].Value;
                        var decryptedCoreSeccondKeyValue = ed.Decrypt(coreSeccondKeyValue).Split('|');
                        if (decryptedCoreSeccondKeyValue.Count() == 2)
                        {
                            if (decryptedCoreSeccondKeyValue[1].ToLower() == sitename)
                            {
                                var coreExpireDate = decryptedCoreSeccondKeyValue[0];
                                DateTime licensetime;
                                if (DateTime.TryParse(coreExpireDate, out licensetime))
                                {
                                    var today = DateTime.Now;
                                    if (today <= licensetime)
                                    {
                                        var restTime = licensetime - today;
                                        return "زمان باقی مانده : " + restTime.Days
                                            + " روز و " + restTime.Hours
                                            + " ساعت و " + restTime.Minutes
                                            + " دقیقه و " + restTime.Seconds
                                            + "ثانیه";
                                    }
                                    else
                                    {
                                        return "منقضی شده است";
                                    }
                                }
                                else
                                {
                                    return "کلید دوم اشتباه است";
                                }
                            }
                            else
                            {
                                return "کلید دوم اشتباه است.";
                            }
                        }
                        else
                        {
                            return "کلید دوم اشتباه است.";
                        }
                    }
                    else
                    {
                        return "کلید دوم یافت نشد";
                    }
                }
                else
                {
                    return "کلید اول اشتباه است";
                }
            }
            else
            {
                return "کلید اول یافت نشد";
            }
        }

        public bool CheckModuleLicense(string modulename)
        {
            var ed = new EncryptDecrypt();
            var sitename = HttpContext.Current.Request.Url.Host.ToLower();
            if (sitename == "localhost")
            {
                return true; // No License needed
            }
            var keyname = modulename;
            var notencryptedvalue = sitename + modulename;
            var encryptedvalue = ed.Encrypt(notencryptedvalue);
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                var moduleLicenseKey = config.AppSettings.Settings[keyname].Value;
                var generatedKey = encryptedvalue;
                if (moduleLicenseKey == generatedKey)
                {
                    if (config.AppSettings.Settings[keyname + "ex"] != null)
                    {
                        var moduleSeccondKeyValue = config.AppSettings.Settings[sitename + "ex"].Value;
                        var decryptedModuleSeccondKeyValue = ed.Decrypt(moduleSeccondKeyValue).Split('|');
                        if (decryptedModuleSeccondKeyValue.Count() == 3)
                        {
                            if (decryptedModuleSeccondKeyValue[1].ToLower()
                                + decryptedModuleSeccondKeyValue[2].ToLower()
                                == sitename.ToLower() + modulename.ToLower())
                            {
                                var moduleExpireDate = decryptedModuleSeccondKeyValue[0];
                                DateTime licensetime;
                                if (DateTime.TryParse(moduleExpireDate, out licensetime))
                                {
                                    var today = DateTime.Now;
                                    if (today <= licensetime)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return false; // License expired
                                    }
                                }
                                else
                                {
                                    return false; // Wrong Expiration License Key
                                }
                            }
                            else
                            {
                                return false; // Wrong Seccond Module Key, it does not belong to module of this site
                            }
                        }
                        else
                        {
                            return false; // Wrong Seccond Module Key, it does not belong to module of this site
                        }
                    }
                    else
                    {
                        return false; // No Expiration License Key Found
                    }
                }
                else
                {
                    return false; // Wrong Module License Key
                }
            }
            else
            {
                return false; // No Module License Found
            }
        }

        public string GetModuleLicenseStatus(string modulename)
        {
            var ed = new EncryptDecrypt();
            var sitename = HttpContext.Current.Request.Url.Host.ToLower();
            //if (sitename == "localhost")
            //{
            //    return "رایگان"; // No License needed
            //}
            var keyname = modulename;
            var notencryptedvalue = sitename + modulename;
            var encryptedvalue = ed.Encrypt(notencryptedvalue);
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                var moduleLicenseKey = config.AppSettings.Settings[keyname].Value;
                var generatedKey = encryptedvalue;
                if (moduleLicenseKey == generatedKey)
                {
                    if (config.AppSettings.Settings[keyname + "ex"] != null)
                    {
                        var moduleSeccondKeyValue = config.AppSettings.Settings[sitename + "ex"].Value;
                        var decryptedModuleSeccondKeyValue = ed.Decrypt(moduleSeccondKeyValue).Split('|');
                        if (decryptedModuleSeccondKeyValue.Count() == 3)
                        {
                            if (decryptedModuleSeccondKeyValue[1].ToLower()
                                + decryptedModuleSeccondKeyValue[2].ToLower()
                                == sitename.ToLower() + modulename.ToLower())
                            {
                                var moduleExpireDate = decryptedModuleSeccondKeyValue[0];
                                DateTime licensetime;
                                if (DateTime.TryParse(moduleExpireDate, out licensetime))
                                {
                                    var today = DateTime.Now;
                                    if (today <= licensetime)
                                    {
                                        var restTime = licensetime - today;
                                        return "زمان باقی مانده : " + restTime.Days
                                            + " روز و " + restTime.Hours
                                            + " ساعت و " + restTime.Minutes
                                            + " دقیقه و " + restTime.Seconds
                                            + "ثانیه";
                                    }
                                    else
                                    {
                                        return "منقضی شده است."; // License expired
                                    }
                                }
                                else
                                {
                                    return "کلید دوم اشتباه است"; // Wrong Expiration License Key
                                }
                            }
                            else
                            {
                                return "کلید دوم اشتباه است.";
                            }
                        }
                        else
                        {
                            return "کلید دوم اشتباه است.";
                        }
                    }
                    else
                    {
                        return "کلید دوم موجود نیست"; // No Expiration License Key Found
                    }
                }
                else
                {
                    return "کلید اول اشتباه است"; // Wrong Module License Key
                }
            }
            else
            {
                return "کلید اول یافت نشد"; // No Module License Found
            }
        }

        public string FirstModuleLicense(string modulename)
        {
            string keyname = modulename;
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                return config.AppSettings.Settings[keyname].Value;
            }
            else
            {
                return "کلید اول یافت نشد.";
            }
        }

        public string SeccondModuleLicense(string modulename)
        {
            string keyname = modulename;
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname + "ex"] != null)
            {
                return config.AppSettings.Settings[keyname + "ex"].Value;
            }
            else
            {
                return "کلید دوم یافت نشد.";
            }
        }

        public bool CheckFirstModuleLicense(string modulename)
        {
            string keyname = modulename;
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckSeccondModuleLicense(string modulename)
        {
            string keyname = modulename;
            Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname + "ex"] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string CoreFirstLicense()
        {
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            var sitename = HttpContext.Current.Request.Url.Host.ToLower();
            var keyname = sitename + "core";
            if (config.AppSettings.Settings[keyname] != null)
            {
                return config.AppSettings.Settings[keyname].Value;
            }
            else
            {
                return "کلید اول یافت نشد.";
            }
        }

        public string CoreSeccondLicense()
        {
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            var sitename = HttpContext.Current.Request.Url.Host.ToLower();
            var expiredatekey = sitename + "ex";
            if (config.AppSettings.Settings[expiredatekey] != null)
            {
                return config.AppSettings.Settings[expiredatekey].Value;
            }
            else
            {
                return "کلید دوم یافت نشد.";
            }
        }

        public bool DeleteLicense(string keyname)
        {
            var config = WebConfigurationManager.OpenWebConfiguration
                (HttpContext.Current.Request.ApplicationPath + "Certifications");
            if (config.AppSettings.Settings[keyname] != null)
            {
                try
                {
                    config.AppSettings.Settings.Remove(keyname);
                    config.Save();
                    return true;
                }
                catch (Exception ex)
                {
                    var eLogger = new ErrorLogger();
                    eLogger.AddError("خطا در حذف مجوز", "خطا در حذف مجوز:<br/>" + ex.Message,
                        HttpContext.Current.User.Identity.Name + "/DeleteLicense"
                        , "LicenseHelper.cs/DeleteLicense");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool EditLicense(string keyname, string newlicense)
        {
            try
            {
                var config = WebConfigurationManager.OpenWebConfiguration
                    (HttpContext.Current.Request.ApplicationPath + "Certifications");
                config.AppSettings.Settings[keyname].Value = newlicense;
                config.Save();
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا در ویرایش مجوز", "خطا در ویرایش مجوز:<br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/EditLicense"
                    , "LicenseHelper.cs/EditLicense");
                return false;
            }
        }

        public bool AddLicense(string keyname, string license)
        {
            try
            {
                var config = WebConfigurationManager.OpenWebConfiguration
                    (HttpContext.Current.Request.ApplicationPath + "Certifications");
                config.AppSettings.Settings.Add(keyname, license);
                config.Save();
                return true;
            }
            catch (Exception ex)
            {
                var eLogger = new ErrorLogger();
                eLogger.AddError("خطا در ثبت مجوز","خطا در ثبت مجوز:<br/>" + ex.Message,
                    HttpContext.Current.User.Identity.Name + "/AddLicense"
                    ,"LicenseHelper.cs/AddLicens");
                return false;
            }
        }

    }
}