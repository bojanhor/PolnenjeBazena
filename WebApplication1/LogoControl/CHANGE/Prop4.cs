using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop4
    {
        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public static Sharp7.S7Client Client;
        PlcVars.Word watchdog4;

        public PlcVars.Bit VrataGorPulse;
        public PlcVars.Bit VrataDolPulse;
        public PlcVars.Bit VrataStopPulse;

        public PlcVars.Bit UporabljaKoncnaStikala;
        public PlcVars.Bit KoncnoStikaloGor;
        public PlcVars.Bit KoncnoStikaloDol;

        public Prop4(Sharp7.S7Client client)
        {
            Client = client;
            watchdog4 = new PlcVars.Word(Client, "", "", "", false); // TODO adress


            VrataGorPulse = new PlcVars.Bit(Client, "", "", "", true); // TODO adress
            VrataDolPulse = new PlcVars.Bit(Client, "", "", "", true);
            VrataStopPulse = new PlcVars.Bit(Client, "", "", "", true);

            UporabljaKoncnaStikala = new PlcVars.Bit(Client, "", "", "", false);
            KoncnoStikaloGor = new PlcVars.Bit(Client, "", "", "", false);
            KoncnoStikaloDol = new PlcVars.Bit(Client, "", "", "", false);
        }

        public string GetWatchdog()
        {
            if (Client != null)
            {
                if (watchdog4 != null)
                {
                    return watchdog4.Value.ToString();
                }
            }
            return PropComm.NA;
        }
    }
}
