using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Module_CoursesAndAds.Model;

namespace Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks
{
    public partial class Store : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new ModuleCoursesAndAdsEntities())
            {
                dlStore.DataSource =
                    (from a in context.Module_Commers_Items where a.CMKindID == 3 orderby a.CMID descending select a).
                        ToList();
                dlStore.DataBind();
            }
        }
    }
}