using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Runtime.ExceptionServices;
using System.Security;

namespace WebApplication1
{

    public static class Val // used to hold values that are same for all instances / users
    {        
        public static Helper.Initialiser Initialiser = new Helper.Initialiser();
        public static LogoControler logocontroler;
        public static WarningManager WarningManager;
        public static GuiController guiController;
        public static List<WarningManager.Warning> Warnings;
        public static string[] watchdog = new string[Settings.Devices + 1];
        public static string ScrolToBottomTextboxScript;
        public static string RetainPositionTextboxScript;
        public static string FocusNextIfEnterKeyPressedScript;
        public static string LoggedIn = "!#LoggedIn#!";
        public static string LoggingIn = "!#LoggingIn#!";


        public static void InitialiseClass()
        {
            SysLog.Message = new SysLog.MessageManager();
            ScriptLoad();                                       // in new thread
            guiController = new GuiController();            
            WarningManager = new WarningManager();              // in new thread
            logocontroler = new LogoControler();                // in new thread
        }

        static void ScriptLoad()
        {
            Misc.SmartThread ScriptsLoaderThread = new Misc.SmartThread(() => ScriptsLoadMethod());
            ScriptsLoaderThread.Start("ScriptsLoaderThread", System.Threading.ApartmentState.MTA, true);
        }

        static void ScriptsLoadMethod()
        {
            FocusNextIfEnterKeyPressedScript = GetScript("FocusNextIfEnterKeyPressed.js");
            ScrolToBottomTextboxScript = GetScript("ScrollToBottom.js");
            RetainPositionTextboxScript = GetScript("RetainScrollPosition.js");            
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