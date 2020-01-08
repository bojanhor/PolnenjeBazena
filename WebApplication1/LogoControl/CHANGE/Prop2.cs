using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop2
    {
        // Ventilacija 

        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public static Sharp7.S7Client Client { get; set; }
        PlcVars.Word watchdog2;

        // Left side menu
        public PlcVars.TemperatureShow TempNivo1 = new PlcVars.TemperatureShow(Client, "", "", "°C", 0,1, true); // TODO Address
        public PlcVars.TemperatureShow TempNivo2 = new PlcVars.TemperatureShow(Client, "", "", "°C", 0, 1, true); // TODO Address
        public PlcVars.TemperatureShow TempNivo3 = new PlcVars.TemperatureShow(Client, "", "", "°C", 0, 1, true); // TODO Address

        public PlcVars.Word Obrati1 = new PlcVars.Word(Client, "", "", "%", true); // TODO Address
        public PlcVars.Word Obrati2 = new PlcVars.Word(Client, "", "", "%", true); // TODO Address
        public PlcVars.Word Obrati3 = new PlcVars.Word(Client, "", "", "%", true); // TODO Address
        public PlcVars.Word Obrati_Omejevalnik_L = new PlcVars.Word(Client, "", "", "%", true); // TODO Address


        // Right side menu
        public PlcVars.Bit Obrati_Omejevalnik_R = new PlcVars.Bit(Client, "", "", "", true); // TODO Address
        public PlcVars.Word OmejiObrateNa = new PlcVars.Word(Client, "", "", "%", true); // TODO Address
        public PlcVars.TimeSet OmObrMedA = new PlcVars.TimeSet(Client, "", true); // TODO Address
        public PlcVars.TimeSet OmObrMedB = new PlcVars.TimeSet(Client, "", true); // TODO Address
        public PlcVars.Bit UpostevajZT = new PlcVars.Bit(Client, "", "DA", "NE", true);
        public PlcVars.Word Histereza = new PlcVars.Word(Client, "", "", "°C", true); // TODO Address


        public Prop2(Sharp7.S7Client client, LogoControler logoControler)
        {
            Client = client;
            watchdog2 = new PlcVars.Word(Client, "DW 5", "", "", false);
        }

        public string GetWatchdog()
        {
            if (Client != null)
            {
                if (watchdog2 != null)
                {
                    return watchdog2.Value.ToString();
                }
            }
            return PropComm.NA;
        }
    }
}
