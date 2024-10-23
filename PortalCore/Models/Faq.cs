using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class Faq
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        
        public int? CategoryId { get; set; }
        public virtual FaqCategory Category { get; set; }
    }
}