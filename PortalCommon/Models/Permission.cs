using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalCommon.Models
{
    public class Permission
    {
        [Key]
        public int PermissionId { get; set; }

        public int PermissionGroupId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionTitle { get; set; }
    }
}
