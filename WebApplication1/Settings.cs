﻿using System;
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

        public static readonly byte autoRefreshPageEvery_s = 0;       // refreshes page automatically every __ seconds ( use 0 to disable )
               
        public static readonly int UpdateValuesPCms = 1000;          // Frekvenca osveževanja vrednosti

        public static readonly int Devices = 5;                      // how many devices supported (do not change)        
                
        public static readonly int XmlRefreshrate = 5000;                                // scans for changes in xml file (should be very high number - 60000)

        public static readonly bool EnableHoveronMenu = false;                           // enables hover on functionality on MENU icon

        public static readonly string DefaultFont = Helper.WebSafeFonts.Arial;         // Select default font for all pages

        public static readonly string ParentPathURL = "~/Pages/";                       // used for path strings

        public static readonly string DefaultPage = "Default";                          // used to redirect to home page or if something goes wrong

        public static readonly string XmlDeclaration = "<?xml version=\"1.0\" encoding=\"utf-8\"?>"; // used to recreate xml config file

        public static readonly string pathToCustomJS = HttpRuntime.AppDomainAppPath + "CustomScripts\\"; // path to scripts folder

        // less important (not very significant)

        public static int defultCheckTimingInterval = 40;      // used for loops checking - lower value means higher precision timing

        // colors
        public static readonly string RedColorHtmlHumar = "#C61720";
        public static readonly string LightBlackColor = "#303030";
    }



    

}