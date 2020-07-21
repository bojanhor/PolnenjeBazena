using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication1
{
    
    public class Global : HttpApplication
    {       

        void Application_Start(object sender, EventArgs e)
        {            

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            XmlController.XmlControllerInitialize();

           
        }

        // Handle all application errors
        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            var type = sender.GetType().BaseType;
            string pageVisited = "";

            if (type == typeof(Global))
            {
                var glbl = (Global)sender;
                if (glbl.Request != null)
                {
                    pageVisited = "Page visited: " + glbl.Request.Path + "... ";
                }
                
            }

            string excInnMess = "";

            if (exc.InnerException != null)
            {
                excInnMess = exc.InnerException.Message +"... ";
            }

            SysLog.SetMessage(pageVisited + exc.Message + " " + excInnMess + "Stack trace: "+ exc.StackTrace);
                        
        }        
    }
}
