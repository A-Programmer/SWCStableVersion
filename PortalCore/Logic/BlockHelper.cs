using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PortalCore.Models;

namespace PortalCore.Logic
{
    public class BlockHelper
    {
        public List<Block> GetModuleBlocks(int moduleid)
        {
            var db = new CoreDbContext();
            return db.Blocks.Where(b => b.ModuleId == moduleid).ToList();
        }



    }
}