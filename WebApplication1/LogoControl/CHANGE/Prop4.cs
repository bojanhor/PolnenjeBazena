using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop4
    {
        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defaultCheckTimingInterval);
        public static Sharp7.S7Client Client;
        PlcVars.Word watchdog4;

        public PlcVars.Bit VrataGorPulse1;
        public PlcVars.Bit VrataDolPulse1;
        public PlcVars.Bit VrataStopPulse1;

        public PlcVars.Bit UporabljaKoncnaStikala1;
        public PlcVars.Bit KoncnoStikaloGor1;
        public PlcVars.Bit KoncnoStikaloDol1;

        public PlcVars.Word CasPotovanja1;

        // 2
        public PlcVars.Bit VrataGorPulse2;
        public PlcVars.Bit VrataDolPulse2;
        public PlcVars.Bit VrataStopPulse2;

        public PlcVars.Bit UporabljaKoncnaStikala2;
        public PlcVars.Bit KoncnoStikaloGor2;
        public PlcVars.Bit KoncnoStikaloDol2;

        public PlcVars.Word CasPotovanja2;

        public Prop4(Sharp7.S7Client client)
        {
            Client = client;
            watchdog4 = new PlcVars.Word(Client, new PlcVars.WordAddress(5), "", "", false);


            VrataGorPulse1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(10,0), "", "", true);
            VrataDolPulse1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(11,0), "", "", true);
            VrataStopPulse1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(12,0), "", "", true);

            UporabljaKoncnaStikala1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(20,0), "", "", true);
            KoncnoStikaloGor1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(21,0), "", "", false);
            KoncnoStikaloDol1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(22,0), "", "", false);

            CasPotovanja1 = new PlcVars.Word(Client, new PlcVars.WordAddress(25), "", "", true);

            // 2
            VrataGorPulse2 = new PlcVars.Bit(Client, new PlcVars.BitAddress(110,0), "", "", true);
            VrataDolPulse2 = new PlcVars.Bit(Client, new PlcVars.BitAddress(111,0), "", "", true);
            VrataStopPulse2 = new PlcVars.Bit(Client, new PlcVars.BitAddress(112,0), "", "", true);

            UporabljaKoncnaStikala2 = new PlcVars.Bit(Client, new PlcVars.BitAddress(120,0), "", "", true);
            KoncnoStikaloGor2 = new PlcVars.Bit(Client, new PlcVars.BitAddress(121,0), "", "", false);
            KoncnoStikaloDol2 = new PlcVars.Bit(Client, new PlcVars.BitAddress(122,0), "", "", false);

            CasPotovanja2 = new PlcVars.Word(Client, new PlcVars.WordAddress(125), "", "", true);
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
