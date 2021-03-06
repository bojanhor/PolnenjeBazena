﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1
{
    public class StanjeProcesa
    {
        static string NoconnectionMessagePrefix = "NI POVEZAVE S KRMILNIKOM ";
        public static List<PlcVars.AlarmBit> SporocilaZaPrikaz { get; private set; }
        public static List<string> SporocilaZaPrikaz_custom = new List<string>();

        public StanjeProcesa()
        {
            SporocilaZaPrikaz = new List<PlcVars.AlarmBit>();            

            Misc.SmartThread SporocilniSistem = new Misc.SmartThread(() => SporocilniSistem_method());
            SporocilniSistem.Start("SporocilniSistem", true);

            Misc.SmartThread AnalizeAndReport = new Misc.SmartThread(() => AnalizeAndReport_method());
            AnalizeAndReport.Start("AnalizeAndReport", true);
        }

        //
        public static  void NiPovezave(bool prikazi, int device)
        {
            var message = NoconnectionMessagePrefix + device +"!";
            if (prikazi)
            {
                ShowCustomMessage(message);
               WarningManager.AddMessageForUser_Warning(message);
            }
            else
            {
                HideCustomMessage(message);
                WarningManager.RemoveMessageForUser_Warning(message);
            }
        }
                
        private static void ShowCustomMessage(string message)
        {
            if (!DoesItExist(message, SporocilaZaPrikaz_custom))
            {
                SporocilaZaPrikaz_custom.Add(message);
            }
        }
        private static void HideCustomMessage(string message)
        {
            if (DoesItExist(message, SporocilaZaPrikaz_custom))
            {
                SporocilaZaPrikaz_custom.Remove(message);
            }
        }

        void AnalizeAndReport_method()
        {
            while (true)
            {
                if (Val.logocontroler != null && Val.logocontroler.LOGOConnection[1] != null)
                {
                    NiPovezave(Val.logocontroler.LOGOConnection[1].connectionStatusLOGO != Connection.Status.Connected, 1);
                }
                if (Val.logocontroler != null && Val.logocontroler.LOGOConnection[2] != null)
                {
                    NiPovezave(Val.logocontroler.LOGOConnection[2].connectionStatusLOGO != Connection.Status.Connected, 2);
                }

                Thread.Sleep(1000);
            }
        }
        void SporocilniSistem_method()
        {
            while (true)
            {

                try
                {
                    foreach (var item in PlcVars.AllAlarmMessageVars)
                    {
                        if (item != null)
                        {
                            if (item.Value == !item.InvertState)
                            {
                                if (!DoesItExist(item.Message, SporocilaZaPrikaz))
                                {
                                    SporocilaZaPrikaz.Add(item); // adds message to display if it does not exist yet   if alarm bool is == 1
                                }
                            }
                            else
                            {
                                if (DoesItExist(item.Message, SporocilaZaPrikaz))
                                {
                                    SporocilaZaPrikaz.Remove(item);// removes message to display if it exists   if alarm bool is == 0
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    SysLog.SetMessage("Napaka v sporočilnem sistemu: " + ex.Message);  
                }
                

               
                Thread.Sleep(500);
            }
        }

        static bool  DoesItExist(string sporocilo, List<string> collection)
        {
            foreach (var item in collection)
            {
                if (item == sporocilo)
                {
                    return true;
                }
            }
            return false;
            
        }
        static bool DoesItExist(string sporocilo, List<PlcVars.AlarmBit> collection)
        {
            foreach (var item in collection)
            {
                if (item.Message == sporocilo)
                {
                    return true;
                }
            }
            return false;

        }



    }
}