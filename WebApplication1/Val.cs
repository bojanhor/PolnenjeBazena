using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WebApplication1
{

    public static class Val // used to hold values that are same for all instances / users
    {        
        public static Helper.Initialiser Initialiser = new Helper.Initialiser();
        public static LogoControler logocontroler = new LogoControler();
        public static string[] watchdog = new string[Settings.Devices + 1];        
        public static GuiController guiController = new GuiController();
        public static string ScrolToBottomTextboxScript = GetScript("ScrollToBottom.js");
        public static string RetainPositionTextboxScript = GetScript("RetainScrollPosition.js");
        public static ChartValues.ChartValuesLogger ChartValues;


        public static void InitialiseClass()
        {        
            ChartValues = new ChartValues.ChartValuesLogger();
        }

        public static void InitializeWDTable(int device)
        {
            if (watchdog[device] != null)
            {
                if (XmlController.GetEnabledLogo(device))
                {
                    watchdog[device] = "Not running";
                }
                else
                {
                    watchdog[device] = "Disabled";
                }
            }
        }
                               
      
        static string GetScript(string scriptName)
        {// Scripts must be registered as soon as posible after page load event
            try
            {
                StreamReader s;
                var path = Settings.pathToCustomJS + scriptName;
                s = new StreamReader(path);
                return s.ReadToEnd();
            }
            catch (Exception )
            {
                throw ;
            }
            
        }
    }
}