using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AdminFilePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsVital { get; set; }
    }
}