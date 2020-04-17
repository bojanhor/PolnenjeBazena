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

            SysLog.SetMessage(exc.Message + " " + exc.InnerException.Message + " " + exc.StackTrace);

            Helper.RedirectBack();

        }        
    }
}
