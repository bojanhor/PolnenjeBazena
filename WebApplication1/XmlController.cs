using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Security;
using System.Xml;

namespace WebApplication1
{
    public static class XmlController
    {

        public static string BaseDirectoryPath = "";
        static int? howManyLucIconsBuff = null;
        static string XmlNotEncriptedPath;
        static string XmlEncriptedPath;
        static string XmlEncriptedPath_tmp;
        static bool forceRefresh = false;
        public static bool encriptedMode = false;
        static bool savingFilePleaseWait = false;

        public static bool XmlControllerInitialized = false;

        // xml file
        private static XDocument _XmlFile;
        public static XDocument XmlFile
        {
            get
            {
                if (_XmlFile != null)
                {
                    return _XmlFile;
                }
                return null;
            }

            private set
            {
                _XmlFile = value;
            }
        }

        // General section of xml file
        private static XElement _XmlGeneral;
        public static XElement XmlGeneral
        {
            get
            {
                return _XmlGeneral;
            }

            private set
            {
                _XmlGeneral = value;
            }
        }

        // GUI section of xml file
        private static XElement _XmlGUI;
        public static XElement XmlGUI
        {
            get
            {
                return _XmlGUI;
            }

            private set
            {
                _XmlGUI = value;
            }
        }

        // LOGO connection section of xml file
        private static XElement _XmlConn;
        public static XElement XmlConn
        {
            get
            {
                return _XmlConn;
            }

            private set
            {
                _XmlConn = value;
            }
        }

        // Users section of xml file
        private static XElement _XmlUsr;
        public static XElement XmlUsr
        {
            get
            {
                return _XmlUsr;
            }

            private set
            {
                _XmlUsr = value;
            }
        }

        // Statistics section of xml file
        private static XElement _XmlStat;
        public static XElement XmlStat
        {
            get
            {
                return _XmlStat;
            }

            private set
            {
                _XmlStat = value;
            }
        }


        private static XDocument LoadNotEncriptedXML(string XmlPath)
        {
            try
            {
                string read;

                using (StreamReader s = new StreamReader(XmlPath))
                {
                    read = s.ReadToEnd();
                }

                return XDocument.Parse(read);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading xml from file (before encription): " + ex.Message);
            }
        }

        public static string DownloadConfigFile()
        {
            try
            {
                string read;

                using (StreamReader s = new StreamReader(XmlEncriptedPath))
                {
                    read = s.ReadToEnd();
                }

                return XmlEncription.Decrypt(read);

            }
            catch (Exception ex)
            {
                throw new Exception("Error loading xml from encripted file: " + ex.Message);
            }
        }

        public static string DownloadLogFile()
        {
            try
            {
                string read;

                using (StreamReader s = new StreamReader(SysLog.MessageManager.LogFilePath))
                {
                    read = s.ReadToEnd();
                }

                return read;

            }
            catch (Exception ex)
            {
                throw new Exception("Error loading log file: " + ex.Message);
            }
        }
        public static void UploadConfigFile()
        {

        }

        private static XDocument LoadAndDecriptXml()
        {
            try
            {
                string read;

                using (StreamReader s = new StreamReader(XmlEncriptedPath))
                {
                    read = s.ReadToEnd();
                }

                var decripted = XmlEncription.Decrypt(read);

                return XDocument.Parse(decripted, LoadOptions.PreserveWhitespace);
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading xml from encripted file: " + ex.Message);
            }

        }

        public static void SaveXML(string newContent)
        {
            string text;

            savingFilePleaseWait = true;

            try
            {
                text = Settings.XmlDeclaration + newContent;
                var encriptedText = XmlEncription.Encrypt(text);

                using (StreamWriter s = new StreamWriter(XmlEncriptedPath_tmp, false, Encoding.UTF8))
                {
                    s.Write(Environment.NewLine + encriptedText);
                    s.Flush();
                    s.Dispose();
                }

                SysLog.SetMessage("ConfigFile was changed, by user.");
            }
            catch (Exception ex)
            {
                savingFilePleaseWait = false;
                var message = "Problem saving encripted config File." + ex.Message;
                Navigator.MessageBox(message);
                throw new Exception(message);
            }

            try
            {
                if (!encriptedMode)
                {
                    if (File.Exists(XmlNotEncriptedPath))
                    {
                        using (StreamWriter s = new StreamWriter(XmlNotEncriptedPath, false, Encoding.UTF8))
                        {
                            s.Write(text);
                            s.Flush();
                            s.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                savingFilePleaseWait = false;
                var message = "Problem saving unecripted config File." + ex.Message;
                Navigator.MessageBox(message);
                throw new Exception(message);
            }

            TmpToFile();
            savingFilePleaseWait = false;
        }

        static void TmpToFile()
        {
            try
            {
                if (File.Exists(XmlEncriptedPath_tmp))
                {
                    File.Copy(XmlEncriptedPath_tmp, XmlEncriptedPath,true);
                    File.Delete(XmlEncriptedPath_tmp);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Problem replacing tmp config file: " + ex.Message);
            }
        }

        static void FindFileAndEncript()
        {
            XmlNotEncriptedPath = BaseDirectoryPath + Settings.pathToConfigFile;
            XmlEncriptedPath = BaseDirectoryPath + Settings.pathToConfigFileEncripted;
            XmlEncriptedPath_tmp = BaseDirectoryPath + Settings.pathToConfigFileEncripted + "_tmp";

            var ii = "\"";

            // if there is Nonencripted config file (will be deleted automatically after publish)

            try
            {
                if (File.Exists(XmlNotEncriptedPath))
                {
                    if (File.Exists(XmlEncriptedPath))
                    {
                        try
                        {
                            File.Delete(XmlEncriptedPath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error deleting encripted config file: " + ex.Message);
                        }
                        
                    }

                    FileEncript(XmlNotEncriptedPath);
                }
                else
                {
                    if (!File.Exists(XmlEncriptedPath))
                    {
                        throw new Exception("Config file could not be found. Search was performed at locations: " + ii + XmlEncriptedPath + ii + " and " + ii + XmlNotEncriptedPath + ii + ".");
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Procedure failed: " + ex.Message);
            }
        }

        static void FileEncript(string XmlPath)
        {
            string xmlText;
            string encriptedText;

            try
            {
                xmlText = LoadNotEncriptedXML(XmlPath).ToString();
                encriptedText = XmlEncription.Encrypt(xmlText);

                using (StreamWriter s = new StreamWriter(XmlEncriptedPath))
                {
                    s.Write(encriptedText);
                    s.Flush();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Config file encription failed: " + ex.Message);
            }

        }

        public static void XmlControllerInitialize()
        {
            try
            {
                BaseDirectoryPath = AppDomain.CurrentDomain.BaseDirectory; // get XML path                

                FindFileAndEncript();

                XmlFile = LoadAndDecriptXml();
                SetClass();

                Misc.SmartThread refresher = new Misc.SmartThread(() => Refresher_Thread());
                refresher.Start("XmlRefresher", System.Threading.ApartmentState.MTA, true);

            }
            catch (Exception e)
            {
                var message = "Method XmlController() encountered an error with configuration file. " +
                    "Please copy proper xml file inside Config folder and name it: config.cnfg. Error description:" + e.Message;
                Navigator.MessageBox(message);
                throw new Exception(message);
            }

        }

        static void Refresher_Thread()
        {
            DateTime dt1;
            XDocument newXml;


            while (true)
            {
                try
                {
                    dt1 = DateTime.Now;

                    if (!savingFilePleaseWait)
                    {
                        try
                        {
                            newXml = LoadAndDecriptXml();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error loading XML file: " + ex.Message);
                        }


                        if (newXml.Element("root").Value != XmlFile.Element("root").Value)
                        {
                            RefreshCache(newXml); // refresh if different
                        }

                        forceRefresh = false;  // reset flag (notifies other methods that fresh copy was aquired)
                    }                    


                    while (DateTime.Now < (dt1 + TimeSpan.FromMilliseconds(Settings.XmlRefreshrate))) // wait for some time
                    {
                        System.Threading.Thread.Sleep(Settings.defaultCheckTimingInterval);

                        if (forceRefresh)  // periodically check for force refresh flag
                        {
                            break;
                        }
                    }

                    XmlControllerInitialized = true;
                    System.Threading.Thread.Sleep(1); // mandatory wait
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while refreshing data from xml file. Please check XML path and data. More info: location of error - Refresher_Thread() method - Error message: " + ex.Message);
                }

            }
        }

        static void RefreshCache(XDocument FreshLoadedXML)
        {
            XmlFile = FreshLoadedXML;
            SetClass();
            Helper.XML_Was_Changed();
        }

        public static string GetXMLTextAndStopRefreshing()
        {
            return XmlFile.ToString();
        }

        public static void SaveCurrentTB(string value)
        {
            SaveXML(value);
        }

        public static void RefreshFile_readAgain()
        {
            forceRefresh = true;

            while (forceRefresh)
            {
                System.Threading.Thread.Sleep(Settings.defaultCheckTimingInterval);
            }
        }

        private static void SetClass()
        {
            try
            {
                XmlGeneral = XmlFile.Element("root").Element("GENERAL");
                XmlGUI = XmlFile.Element("root").Element("GUI");
                XmlConn = XmlFile.Element("root").Element("CONNECTION");
                XmlUsr = XmlFile.Element("root").Element("USERS");
                XmlStat = XmlFile.Element("root").Element("STATS");
            }

            catch (Exception)
            {
                throw;
            }
        }

        // PUBLIC

        public static IPAddress GetLogoIP(int n)
        {
            if (n < 0 || n > Settings.Devices)
            {
                throw new Exception("getLogoIP() method internal error. Index out of range");
            }

            try
            {
                var IP = XmlConn.Element("LOGO" + n).Element("serverIP").Value;

                if (!string.IsNullOrEmpty(IP) && IPAddress.TryParse(IP, out IPAddress result))
                {
                    return result;
                }

                else
                {
                    throw new Exception("IP addres in config file is not valid IP. " +
                        "Correct the IP address in config.cnfg file at LOGO" + n + " entry");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static string GetLogoLocalTsap(int n)
        {
            if (n < 0 || n > Settings.Devices)
            {
                throw new Exception("getLogoLocalTsap() method internal error. Index out of range");
            }

            try
            {
                var LocalTSAP = XmlConn.Element("LOGO" + n).Element("localTSAP").Value;

                if (LocalTSAP.Length != 5 && !LocalTSAP.Contains("."))
                {
                    throw new Exception("LocalTSAP addres in config file is not valid LocalTSAP. " +
                        "Correct the LocalTSAP address in config.cnfg file at LOGO" + n + " entry. format must be ##.## (03.00).");
                }

                return LocalTSAP;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetLogoRemoteTsap(int n)
        {
            if (n < 0 || n > Settings.Devices)
            {
                throw new Exception("getLogoRemoteTsap() method internal error. Index out of range");
            }

            try
            {
                var RemoteTSAP = XmlConn.Element("LOGO" + n).Element("remoteTSAP").Value;

                if (RemoteTSAP.Length != 5 && !RemoteTSAP.Contains("."))
                {
                    throw new Exception("remoteTSAP addres in config file is not valid remoteTSAP. " +
                        "Correct the remoteTSAP address in config.cnfg file at LOGO" + n + " entry. format must be ##.## (02.00).");
                }

                return RemoteTSAP;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool IsDebugEnabled()
        {
            try
            {
                if (Convert.ToBoolean(XmlGeneral.Element("debugToConsole").Value))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return true;
            }

        }

        public static PlcVars.DoubleWordAddress GetWDAddress(int device)
        {
            if (device < 0 || device > Settings.Devices)
            {
                throw new Exception("GetWDAddress() method internal error. Index out of range");
            }

            try
            {
                var xmlVal = XmlConn.Element("LOGO" + device).Element("watchdogAddress").Value;
                return new PlcVars.DoubleWordAddress(ushort.Parse(xmlVal));
            }
            catch (Exception)
            {
                throw new Exception(
                    "watchdogAddress value in config file is not valid watchdogAddress. " +
                    "Correct the watchdogAddress value in config.cnfg file at LOGO" + device + " entry. " +
                    "format must be 300.");
            }
        }

        public static bool GetEnabledLogo(int device)
        {

            try
            {
                if (device < 1 || device > Settings.Devices)
                {
                    throw new Exception();
                }
                else
                {
                    return Convert.ToBoolean(XmlConn.Element("LOGO" + device).Element("enabled").Value);
                }

            }
            catch (Exception)
            {

                throw new Exception(
                    "enabled value in config file is not valid enabled value. " +
                    "Correct the enabled value in config.cnfg file: at LOGO" + device + " entry. " +
                    "format must be true ore false.");
            }

        }

        public static int GetReadWriteCycle(int device)
        {
            try
            {
                if (device < 1 || device > Settings.Devices)
                {
                    throw new Exception();
                }
                else
                {
                    return Convert.ToInt16(XmlConn.Element("LOGO" + device).Element("ReadWriteCycle").Value);
                }
            }
            catch (Exception)
            {

                throw new Exception(
                    "ReadWriteCycle value in config file is not valid ReadWriteCycle value. " +
                    "Correct the ReadWriteCycle value in config.cnfg file at LOGO" + device + " entry. " +
                    "format must be number (example: 500).");
            }

        }

        public static short GetHowManyMenuDDItems()
        {
            var searchValue = "HowManyDDMenuItems";

            try
            {
                return Convert.ToInt16(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3).");
            }

        }

        public static string GetMenuDDItemName(short index)
        {
            var searchValue = "DDMenuItemName" + index;

            try
            {

                if (index < 0 || index > 25)
                {
                    throw new Exception();
                }
                return XmlGUI.Element(searchValue).Value;
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value 25.");
            }

        }

        public static string GetMenuDDItemLink(short index)
        {
            var searchValue = "DDMenuItemLink" + index;

            try
            {

                if (index < 0 || index > 25)
                {
                    throw new Exception();
                }
                return XmlGUI.Element(searchValue).Value;
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value 25.");
            }

        }

        public static short GetHowManyMenuItems()
        {
            var searchValue = "HowManyMenuItems";

            try
            {
                return Convert.ToInt16(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3).");
            }

        }

        public static string GetMenuItemLink(int index)
        {
            var searchValue = "MenuItemLink" + index;

            try
            {

                if (index < 1 || index > 5)
                {
                    throw new Exception();
                }
                return XmlGUI.Element(searchValue).Value;
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value 25.");
            }

        }

        public static short GetMasterWindowScaleX()
        {
            var searchValue = "scaleGuiFactorX";

            try
            {
                return Convert.ToInt16(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value 25.");
            }

        }

        public static short GetMasterWindowScaleY()
        {
            var searchValue = "scaleGuiFactorY";

            try
            {
                return Convert.ToInt16(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value 25.");
            }

        }

        public static Helper.Position GetPositionLucForDefaultScreen(int index)
        {
            var prefix = "PositionLuc";
            string buff;
            var searchValue = "";
            var arr = new string[3];
            Helper.Position pos = new Helper.Position();

            if (index < 1 || index > GetHowManyLucIcons())
            {
                throw new Exception("Index out of range inside method GetPositionLucForDefaultScreen(int index);");
            }

            try
            {

                searchValue = prefix + index;
                buff = XmlGUI.Element(searchValue).Value; // gets string from xml
                arr = buff.Split(';');  // splits value in 3 parts
                pos.top = Convert.ToSingle(arr[0]); // distance from top
                pos.left = Convert.ToSingle(arr[1]); // distance from left
                pos.width = Convert.ToSingle(arr[2]); // last part is size

            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                "format must be #;#;#; .  Example: 50;50;5;");
            }

            return pos;
        }

        public static PlcVars.BitAddress GetLucAddress_ReadToPC(int ID)
        {
            var searchValue = "AddressLuc_ReadToPC" + ID;

            try
            {
                if (ID < 1 || ID > GetHowManyLucIcons())
                {
                    throw new Exception();
                }

                var xmlVal = XmlGUI.Element(searchValue).Value.Split('.');
                return new PlcVars.BitAddress(ushort.Parse(xmlVal[0]), ushort.Parse(xmlVal[1]));
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be profinet bit adress (example: 10.0) - min value 1, max value 800.");
            }

        }

        public static PlcVars.BitAddress GetLucAddress_WriteToPLC(int ID)
        {
            var searchValue = "AddressLuc_WriteToPLC" + ID;

            try
            {
                if (ID < 1 || ID > GetHowManyLucIcons())
                {
                    throw new Exception();
                }
                var xmlVal = XmlGUI.Element(searchValue).Value.Split('.');
                return new PlcVars.BitAddress(ushort.Parse(xmlVal[0]), ushort.Parse(xmlVal[1]));
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be profinet bit adress (example: 10.0) - min value 1, max value 800.");
            }

        }

        public static int GetHowManyLucIcons()
        {
            int cnt = 0;
            var prefix = "PositionLuc";
            string buff;
            var searchValue = "";
            var arr = new string[3];
            Helper.Position pos = new Helper.Position();

            try
            {
                if (howManyLucIconsBuff != null)
                {
                    if (howManyLucIconsBuff > 0)
                    {
                        return (int)howManyLucIconsBuff; // get value from buffer
                    }
                }

                for (int i = 1; i <= 12; i++) // scan 12 items
                {
                    searchValue = prefix + i;
                    buff = XmlGUI.Element(searchValue).Value; // gets string from xml
                    arr = buff.Split(';'); // splits value in 3 parts
                    var w = pos.width = Convert.ToSingle(arr[2]); // last part is size
                    if (w > 0) // if size exists (is not disabled)
                    {
                        cnt++;
                    }
                    else
                    {
                        howManyLucIconsBuff = cnt; // save to buffer
                        break; // breaks on first disabled icon
                    }
                }
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                "format must be #;#;#; .  Example: 50;50;5;");
            }

            return cnt; // returns number of items that have size greater than 0

        }

        public static string GetDeviceName(int index)
        {
            var searchValue = "devicename";

            try
            {
                if (index < 1 || index > Settings.Devices)
                {
                    throw new Exception();
                }
                return XmlConn.Element("LOGO" + index).Element(searchValue).Value;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value  " + Settings.Devices + ". " + "Exception message: " + ex.Message);
            }
        }

        public static string GetShowName(int index)
        {
            var searchValue = "showname";

            try
            {
                if (index < 1 || index > Settings.Devices)
                {
                    throw new Exception();
                }
                return XmlConn.Element("LOGO" + index).Element(searchValue).Value;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "format must be number (example: 3) - min value 1, max value " + Settings.Devices + ". " + "Exception message: " + ex.Message);
            }
        }

        public static bool GetHasDimmer(int lucID)
        {

            var searchValue = "HasDimmer" + lucID;

            try
            {
                if (lucID < 1 || lucID > GetHowManyLucIcons())
                {
                    throw new Exception();
                }
                return Convert.ToBoolean(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be \"true\" or \"false\"");
            }


        }

        public static bool GetHasWeekTimer(int lucID)
        {

            var searchValue = "HasWeekTimer" + lucID;

            try
            {
                if (lucID < 1 || lucID > GetHowManyLucIcons())
                {
                    throw new Exception();
                }
                return Convert.ToBoolean(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {

                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be \"true\" or \"false\"");
            }


        }

        public static bool GetEnableCharts_Svetlost()
        {
            var searchValue = "EnableChart_Svetlost";

            try
            {
                return Convert.ToBoolean(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be \"true\" or \"false\"");
            }
        }

        public static bool GetEnableCharts_Padavine()
        {
            var searchValue = "EnableChart_Padavine";

            try
            {
                return Convert.ToBoolean(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be \"true\" or \"false\"");
            }
        }

        public static bool GetEnableCharts_Tzunanja()
        {
            var searchValue = "EnableChart_Tzunanja";

            try
            {
                return Convert.ToBoolean(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be \"true\" or \"false\"");
            }
        }

        public static bool GetEnableCharts_Tnotranja()
        {
            var searchValue = "EnableChart_Tnotranja";

            try
            {
                return Convert.ToBoolean(XmlGUI.Element(searchValue).Value);
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be \"true\" or \"false\"");
            }
        }

        public static void SetEnableCharts_Svetlost(bool value)
        {
            var searchValue = "EnableChart_Svetlost";

            try
            {
                XmlFile.Element("root").Element("GUI").Element(searchValue).Value = value.ToString();
                SaveXML(XmlFile.ToString());
            }

            catch (Exception ex)
            {
                throw new Exception(
                   "Error while setting value: " + searchValue + ". Error info: " + ex.Message);
            }

        }

        public static void SetEnableCharts_Padavine(bool value)
        {
            var searchValue = "EnableChart_Padavine";

            try
            {
                XmlFile.Element("root").Element("GUI").Element(searchValue).Value = value.ToString();
                SaveXML(XmlFile.ToString());
            }

            catch (Exception ex)
            {
                throw new Exception(
                   "Error while setting value: " + searchValue + ". Error info: " + ex.Message);
            }

        }

        public static void SetEnableCharts_Tzunanja(bool value)
        {
            var searchValue = "EnableChart_Tzunanja";

            try
            {
                XmlFile.Element("root").Element("GUI").Element(searchValue).Value = value.ToString();
                SaveXML(XmlFile.ToString());
            }

            catch (Exception ex)
            {
                throw new Exception(
                   "Error while setting value: " + searchValue + ". Error info: " + ex.Message);
            }

        }

        public static void SetEnableCharts_Tnotranja(bool value)
        {
            var searchValue = "EnableChart_Tnotranja";

            try
            {
                XmlFile.Element("root").Element("GUI").Element(searchValue).Value = value.ToString();
                SaveXML(XmlFile.ToString());
            }

            catch (Exception ex)
            {
                throw new Exception(
                    "Error while setting value: " + searchValue + ". Error info: " + ex.Message);
            }

        }

        public static int GetShowChartMode()
        {
            var searchValue = "ShowChartMode";

            try
            {
                var buff = Convert.ToInt32(XmlGUI.Element(searchValue).Value);
                if (buff > 10 || buff < 0)
                {
                    throw new Exception();
                }
                return buff;
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be between 0 and 10");
            }
        }

        public static void SetShowChart(int value)
        {
            var searchValue = "ShowChartMode";

            try
            {
                XmlFile.Element("root").Element("GUI").Element(searchValue).Value = value.ToString();
                SaveXML(XmlFile.ToString());
            }

            catch (Exception ex)
            {
                throw new Exception(
                   "Error while setting value: " + searchValue + ". Error info: " + ex.Message);
            }
        }

        public static int GetPageVremeWidth()
        {
            var searchValue = "VremeWidth";

            try
            {
                var buff = Convert.ToInt32(XmlGUI.Element(searchValue).Value);
                if (buff > 100 || buff < 20)
                {
                    throw new Exception();
                }
                return buff;
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be between 20 and 100");
            }
        }

        public static string GetPageVremeLink()
        {
            var searchValue = "VremeLink";

            try
            {
                var buff = XmlGUI.Element(searchValue).Value;

                Uri uriResult;
                bool result = Uri.TryCreate(buff, UriKind.Absolute, out uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if (!result)
                {
                    throw new Exception();
                }

                return buff;
            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config.cnfg file at GUI entry. " +
                    "value must be valid link to internet page");
            }
        }

        public static List<Helper.UserData> GetUserData()
        {
            var searchValue = "User";
            List<Helper.UserData> de = new List<Helper.UserData>();
            Helper.UserData buff;


            try
            {
                string buffName;
                string buffPwd;

                for (int i = 1; i <= 30; i++)
                {
                    buffName = XmlUsr.Element(searchValue + i).Element("Name").Value;
                    buffPwd = XmlUsr.Element(searchValue + i).Element("Pwd").Value;
                    buff = new Helper.UserData(buffName, buffPwd);
                    de.Add(buff);
                }

                return de;

            }
            catch (Exception)
            {
                throw new Exception(
                    searchValue + " value in config file is not valid " + searchValue + " value. " +
                    "Correct the " + searchValue + " value in config or call your support administrator. ");
            }
        }
    }
}