using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop3
    {
        // Vrata / zavese

        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public Sharp7.S7Client Client { get; set; }
        PlcVars.Word watchdog3;

        public PlcVars.Bit VrataGorPulse1;
        public PlcVars.Bit VrataDolPulse1;
        public PlcVars.Bit VrataStopPulse1;

        public PlcVars.Bit UporabljaKoncnaStikala1;
        public PlcVars.Bit KoncnoStikaloGor1;
        public PlcVars.Bit KoncnoStikaloDol1;

        // 2
        public PlcVars.Bit VrataGorPulse2;
        public PlcVars.Bit VrataDolPulse2;
        public PlcVars.Bit VrataStopPulse2;

        public PlcVars.Bit UporabljaKoncnaStikala2;
        public PlcVars.Bit KoncnoStikaloGor2;
        public PlcVars.Bit KoncnoStikaloDol2;



        public Prop3(Sharp7.S7Client client, LogoControler logoControler)
        {
            Client = client;
            watchdog3 = new PlcVars.Word(Client, "", "", "", false); // TODO adress


            VrataGorPulse1 = new PlcVars.Bit(Client, "", "", "", true); // TODO adress
            VrataDolPulse1 = new PlcVars.Bit(Client, "", "", "", true);
            VrataStopPulse1 = new PlcVars.Bit(Client, "", "", "", true);

            UporabljaKoncnaStikala1 = new PlcVars.Bit(Client, "", "", "", false); 
            KoncnoStikaloGor1 = new PlcVars.Bit(Client, "", "", "", false); 
            KoncnoStikaloDol1 = new PlcVars.Bit(Client, "", "", "", false);


            // 2
            VrataGorPulse2 = new PlcVars.Bit(Client, "", "", "", true); // TODO adress
            VrataDolPulse2 = new PlcVars.Bit(Client, "", "", "", true);
            VrataStopPulse2 = new PlcVars.Bit(Client, "", "", "", true);

            UporabljaKoncnaStikala2 = new PlcVars.Bit(Client, "", "", "", false);
            KoncnoStikaloGor2 = new PlcVars.Bit(Client, "", "", "", false);
            KoncnoStikaloDol2 = new PlcVars.Bit(Client, "", "", "", false);

        }

        public string GetWatchdog()
        {
            if (Client != null)
            {
                if (watchdog3 != null)
                {
                    return watchdog3.Value.ToString();
                }
            }
            return PropComm.NA;
        }


    }
}
