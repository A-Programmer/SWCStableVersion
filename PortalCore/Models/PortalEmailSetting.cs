using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class PortalEmailSetting
    {
        [Key]
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string EmailPassword { get; set; }
        public string EmailSmtpAddress { get; set; }
        public int EmailSmtpPort { get; set; }
    }
}