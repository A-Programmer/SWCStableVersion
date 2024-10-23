using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Module_CoursesAndAds.Model;


namespace Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks
{
    public partial class Courses : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new ModuleCoursesAndAdsEntities())
            {
                dlCourses.DataSource =
                    (from a in context.Module_Commers_Items where a.CMKindID == 2 orderby a.CMID descending select a).
                        ToList();
                dlCourses.DataBind();
            }
        }
    }
}