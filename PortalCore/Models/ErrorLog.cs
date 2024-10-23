using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class ErrorLog
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Path { get; set; }
        public DateTime ErrorDate { get; set; }
        public string UserName { get; set; }
    }
}