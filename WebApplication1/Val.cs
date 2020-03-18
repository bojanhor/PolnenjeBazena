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
        public static MessageToWeb Message = new MessageToWeb();
        public static GuiController guiController = new GuiController();
        public static string ScrolToBottomTextboxScript = null;
        public static string RetainPositionTextboxScript = null;
        
        public static void InitialiseClass()
        {
            getScrolDownTextboxScript();
            ChartValues.ChartValuesLogger CVL = new ChartValues.ChartValuesLogger();

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

        public class MessageToWeb
        {
            string Message = "";

            public MessageToWeb()
            {

            }

            public void Setmessage(string message)
            {
                Message += message + Environment.NewLine;
            }

            public string GetMessage()
            {
                return Message;
            }
        }
                
        static void getScrolDownTextboxScript()
        {
            ScrolToBottomTextboxScript = GetScript("ScrollToBottom.js");
            RetainPositionTextboxScript = GetScript("RetainScrollPosition.js");
        }

        static string GetScript(string scriptName)
        {
            try
            {
                StreamReader s;
                var path = Settings.pathToCustomJS + scriptName;
                s = new StreamReader(path);
                return s.ReadToEnd();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}