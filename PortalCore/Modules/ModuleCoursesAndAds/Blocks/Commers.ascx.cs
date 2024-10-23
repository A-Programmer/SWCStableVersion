using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.FriendlyUrls;
using Module_CoursesAndAds.Model;

namespace Module_CoursesAndAds.Modules.ModuleCoursesAndAds.Blocks
{
    public partial class Commers : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var context = new ModuleCoursesAndAdsEntities())
            {
                IList<string> segments = Request.GetFriendlyUrlSegments();
                string[] slist = segments.ToArray();

                if (slist.Count() >= 2)
                {
                    try
                    {
                        int commersid = Convert.ToInt32(slist[1]);
                        var commers =
                            (from a in context.Module_Commers_Items where a.CMID == commersid select a).FirstOrDefault();
                        if(commers != null)
                        {
                            lbTitle.Text = commers.CmTitle;
                            lbContent.Text = commers.CMDetails;
                            Page.Title = commers.CmTitle;
                            Page.MetaDescription = commers.CMDescription;
                            Page.MetaKeywords = commers.CMTags;

                            string tags = commers.CMTags;
                            string[] tag = tags.Split(',');
                            int countTags = tag.Count();
                            foreach (string word in tag)
                            {
                                HyperLink hp = new HyperLink();
                                hp.ID = "hp" + countTags;
                                hp.Text = word;
                                hp.NavigateUrl = "~/ShowPage/Search/" + word;
                                pnlTags.Controls.Add(hp);
                                Literal lbl = new Literal();
                                lbl.ID = "lblComma" + countTags;
                                lbl.Text = ",";
                                pnlTags.Controls.Add(lbl);
                                countTags -= 1;
                            }

                        }
                        else
                        {
                            lbTitle.Text = "متاسفانه مورد موردنظر یافت نشد.";
                            lbContent.Text = "دوباره امتحان کنید!";
                        }
                    }
                    catch(Exception exp)
                    {
                        Response.Redirect("~/");
                        Response.Write(exp.InnerException.Message);
                        
                    }
                }
            }
        }
    }
}