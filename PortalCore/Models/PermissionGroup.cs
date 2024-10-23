using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class PermissionGroup
    {
        [Key]
        public int PermissionGroupId { get; set; }
        public int? ModuleId { get; set; }
        public string PermissionGroupTitle { get; set; }
    }
}