using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PortalCore.Logic;

namespace PortalCore.Admin
{
    public partial class ManageNotifications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindNotifications();
        }

        public void BindNotifications()
        {
            var nhelper = new NotificationHelper();
            var userName = Page.User.Identity.Name;
            grdNotifications.DataSource = nhelper.GetNotifications(userName);
            grdNotifications.DataBind();
        }


        protected void grdNotifications_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdNotifications.PageIndex = e.NewPageIndex;
            BindNotifications();
        }

        protected void grdNotifications_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ChangeStatus":
                {
                    var id = Convert.ToInt32(e.CommandArgument);
                    var nhelper = new NotificationHelper();
                    ltNotificationMessages.Text = nhelper.ChangeStatus(id) ? "اعلان مورد نظر با موفقیت تغییر وضعیت داده شد.." : "خطا در تغییر وضعیت اعلان";
                    BindMasterPageNotifications();
                    BindNotifications();
                    break;
                }
                case "DoDelete":
                {
                    var id = Convert.ToInt32(e.CommandArgument);
                    var nhelper = new NotificationHelper();
                    ltNotificationMessages.Text = nhelper.DeleteNotification(id) ? "اعلان مورد نظر با موفقیت حذف شد." : "خطا در حذف اعلان";
                    BindMasterPageNotifications();
                    BindNotifications();
                    break;
                }
            }
        }

        public void BindMasterPageNotifications()
        {
            var nHelper = new NotificationHelper();
            var rptNotifications = (Repeater)Page.Master.FindControl("rptNotifications");
            var rptContent = (Repeater)Page.Master.FindControl("rptContent");
            var ltNotificationCount = (Literal)Page.Master.FindControl("ltNotificationCount");
            var ltNotifiesCount = (Literal)Page.Master.FindControl("ltNotifiesCount");
            var username = Page.User.Identity.Name;
            rptNotifications.DataSource = nHelper.GetNewNotifications(username);
            rptNotifications.DataBind();
            rptContent.DataSource = nHelper.GetNewNotifications(username);
            rptContent.DataBind();
            ltNotificationCount.Text = nHelper.GetNewNotificationsCount(username).ToString();
            ltNotifiesCount.Text = nHelper.GetNewNotificationsCount(username).ToString();
            BindNotifications();
        }

        public string GetFontWeightStyle(int id)
        {
            var nHelper = new NotificationHelper();
            var notification = nHelper.GetNotificaion(id);
            if (notification != null)
            {
                return notification.Status ? "normal" : "bold";
            }
            else
            {
                return "normal";
            }
        }

    }
}