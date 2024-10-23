using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class Block
    {
        [Key]
        public int BlockId { get; set; }
        public int ModuleId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public bool IsAdminBlock { get; set; }
    }
}