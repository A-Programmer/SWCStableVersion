using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class GeneralSetting
    {
        [Key]
        public int Id { get; set; }
        public string HomeUrl { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public DateTime InstallDate { get; set; }
        public bool Status { get; set; }
        public DateTime? RunDate { get; set; }
        public bool EnableRegistration { get; set; }
        public bool EnableUnlockRegistratin { get; set; }
        //public bool AllowExternalLogin { get; set; }
        public bool EnablePasswordRecovery { get; set; }
        public int DefaultUserRoleId { get; set; }
        public string ActiveTemplateName { get; set; }
        public int ActiveTemplateId { get; set; }

    }
}