using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class Page
    {
        [Key]
        public int PageId { get; set; }
        public int? ModuleId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}