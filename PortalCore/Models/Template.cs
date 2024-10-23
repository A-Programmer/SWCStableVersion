using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class Template
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public bool IsInstalled { get; set; }
    }
}