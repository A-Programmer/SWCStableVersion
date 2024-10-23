using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PortalCore.Models
{
    public class FaqCategory
    {
        [ScaffoldColumn(false)]
        [Key]
        public int CategoryId { get; set; }

        public string Title { get; set; }
        public virtual ICollection<Faq> Faqs { get; set; }
    }
}