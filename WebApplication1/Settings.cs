using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public static class Settings
    {

        // importatnt settings (will reflect significant changes)

        public static readonly byte autoRefreshPageEvery_s = 60;       // refreshes page automatically every __ seconds ( use 0 to disable )

        public static readonly int UpdateValuesPCms = 500;          // Frekvenca osveževanja vrednosti

        public static readonly int UpdateValuesJoystick = 500;      // Frekvenca osveževanja gumbov za joystick

        public static readonly int Updateanimations = 200;          // Frekvenca osveževanja vrednosti za animacije

        public static readonly int Devices = 5;                      // how many devices supported (do not change)        

        public static readonly int XmlRefreshrate = 5000;                                // scans for changes in xml file (should be very high number - 60000)

        public static readonly bool EnableHighPerformanceSync = true;           // Enables tcp comunication without delay (disable if CPU usage is to high)

        public static int ChartUpdateRefreshRate = 60000;                           // updates chart every X[ms] specified (1000 - 60000)

        public static readonly bool EnableHoveronMenu = false;                           // enables hover on functionality on MENU icon

        public static readonly string DefaultFont = Helper.WebSafeFonts.Arial;         // Select default font for all pages

        public static readonly string ParentPathURL = "~/Pages/";                       // used for path strings

        public static readonly string DefaultPage = "Default";                          // used to redirect to home page or if something goes wrong

        public static readonly string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"; // used to recreate xml config file

        public static readonly string pathToCustomJS = HttpRuntime.AppDomainAppPath + "CustomScripts\\"; // path to scripts folder
        
        public static readonly string pathToConfigFileEncripted = "App_Data\\config.encripted";

        public static readonly string pathToConfigFile = "App_Data\\config.cnfg";

        public static readonly uint logFileMaxSizeKB = 4000;

        public static readonly uint MaxLoginTriesIP = 15;            // IP address is blocked after x failed attempts
        public static readonly uint MaxLoginTriesUser = 10;            // User is blocked after x failed attempts

        // less important (not very significant)

        public static readonly int defaultCheckTimingInterval = 40;      // used for loops checking - lower value means higher precision timing

        public static readonly string defaultDateFormat = "dd.MM";
        public static readonly string defaultDateFormatY = "dd.MM.yyyy";
        public static readonly string defaultTimeFormat = "HH:mm:ss";
        public static readonly string defaultDateTimeFormat = defaultDateFormat + "  " + defaultTimeFormat + " ";
        public static readonly string defaultDateTimeFormatY = defaultDateFormatY + "  " + defaultTimeFormat + " ";
        public static readonly bool PreventSearchEnginesFromIndexing = true;

        // colors
        public static readonly string RedColorHtmlHumar = "#C61720";
        public static readonly string LightBlackColor = "#303030";
    }



    

}