using GST_Billing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace GST_Billing
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                // Get the forms authentication ticket.
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var identity = new GenericIdentity(authTicket.Name, "Forms");
                var principal = new MyPrincipal(identity);
                // Get the custom user data encrypted in the ticket.
                string userData = ((FormsIdentity)(Context.User.Identity)).Ticket.UserData;
                // Deserialize the json data and set it on the custom principal.
                var serializer = new JavaScriptSerializer();
                principal.User = (UserData)serializer.Deserialize(userData, typeof(UserData));
                // Set the context user.
                Context.User = principal;
            }
        }
        void Application_Error(object sender, EventArgs e)
        {
            //Exception ex = Server.GetLastError();
            //LogError(ex, HttpContext.Current.Request.Path);
            //Server.ClearError();
        }
        public void LogError(Exception ex, String source)
        {
            try
            {
                String LogFile = HttpContext.Current.Request.MapPath("~/logs.txt");
                if (LogFile != "")
                {
                    String Message = String.Format("{0}{0}=== {1} ==={0}{2}{0}{3}{0}{4}{0}{5}"
                             , Environment.NewLine
                             , DateTime.Now
                             , ex.Message
                             , source
                             , ex.InnerException
                             , ex.StackTrace);

                    byte[] binLogString = Encoding.Default.GetBytes(Message);

                    System.IO.FileStream loFile = new System.IO.FileStream(LogFile
                              , System.IO.FileMode.OpenOrCreate
                              , System.IO.FileAccess.Write, System.IO.FileShare.Write);
                    loFile.Seek(0, System.IO.SeekOrigin.End);
                    loFile.Write(binLogString, 0, binLogString.Length);
                    loFile.Close();
                }
            }
            catch { /*No need to catch the error here. */}
            Response.Redirect("~/Account/Error");
        }
    }
}
