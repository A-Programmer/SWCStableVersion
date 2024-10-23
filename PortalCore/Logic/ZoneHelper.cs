using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class ZoneHelper
    {
        public List<Zone> GetCurrentTemplateZones()
        {
            var db = new CoreDbContext();
            var currentTemplate = db.Templates.FirstOrDefault(t => t.IsActive);
            if (currentTemplate != null)
            {
                string templatename = currentTemplate.Name;
                return db.Zones.Where(z => z.TemplateName == templatename).OrderBy(zn => zn.Id).ToList();
            }
            else
            {
                return null;
            }
        }

        public Zone GetZoneByName(string zoneName,string templateName)
        {
            var db = new CoreDbContext();
            return db.Zones.FirstOrDefault(z => z.Name == zoneName && z.TemplateName == templateName);
        }

        
    }
}