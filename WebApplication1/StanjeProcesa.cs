using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1
{
    public class StanjeProcesa
    {        
        public static List<string> SporocilaZaPrikaz { get; private set; }
        public StanjeProcesa()
        {
            SporocilaZaPrikaz = new List<string>();

            Misc.SmartThread SporocilniSistem = new Misc.SmartThread(() => SporocilniSistem_method());
            SporocilniSistem.Start("SporocilniSistem", true);
        }

        void SporocilniSistem_method()
        {
            while (true)
            {
                foreach (var item in PlcVars.AllAlarmMessageVars)
                {
                    if (item != null)
                    {
                        if (item.Value == true)
                        {
                            if (!doesItExist(item.Message))
                            {
                                SporocilaZaPrikaz.Add(item.Message); // adds message to display if it does not exist yet   if alarm bool is == 1
                            }                            
                        }
                        else
                        {
                            if (doesItExist(item.Message))
                            {
                                SporocilaZaPrikaz.Remove(item.Message);// removes message to display if it exists   if alarm bool is == 0
                            }
                        }
                    }                    
                }                

                Thread.Sleep(500);
            }
        }

        bool doesItExist(string sporocilo)
        {
            foreach (var item in SporocilaZaPrikaz)
            {
                if (item == sporocilo)
                {
                    return true;
                }
            }
            return false;
            
        }

    }
}