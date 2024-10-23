using System;
using System.Data.Entity;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using PortalCore.App_Start;
using PortalCore.Models;

namespace PortalCore
{
    public class Global : System.Web.HttpApplication
    {
        
        protected void Application_Start(object sender, EventArgs e)
        {
            Application["IsInstalled"] = false;
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // Database Initializer for base data
            //if (!Database.Exists("PortalConnectionString"))
            //{
                var db = new CoreDbContext();
                Database.SetInitializer<CoreDbContext>(new DbInitializer());
                db.Database.Initialize(true);
            //}
            RegisterCustomRoutes(RouteTable.Routes);

            //ScriptManager.ScriptResourceMapping.AddDefinition("jquery",
            //    new ScriptResourceDefinition
            //    {
            //        Path = "~/CoreUiFiles/jquery-1.11.3.min.js"
            //    }
            //);

        }

        private void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
              "Page",
              "ShowPage/{PageName}",
              "~/ShowPage.aspx",
              true,
              new RouteValueDictionary { {"PageName","Default"} }
          );
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //Change Default RoleManager Settings
            HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName.ToString()];
            if (authCookie != null)
            {
                FormsAuthenticationTicket myTicket;
                myTicket = FormsAuthentication.Decrypt(authCookie.Value);
                FormsIdentity MyIdentity = new FormsIdentity(myTicket);
                GenericPrincipal myPrincipal = new GenericPrincipal(MyIdentity, myTicket.UserData.Split(new char[] { ',' }));
                Context.User = myPrincipal;
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}