using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class PageLayout
    {
        [Key]
        public int PageLayoutId { get; set; }
        public int PageId { get; set; }
        public int BlockId { get; set; }
        public string ZoneName { get; set; }
        public int AppearanceOrder { get; set; }
    }
}