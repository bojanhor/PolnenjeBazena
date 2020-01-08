using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop5
    {
        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public Sharp7.S7Client Client { get; set; }
        PlcVars.Word watchdog5;

        public Prop5(Sharp7.S7Client client, LogoControler logoControler)
        {
            Client = client;
            watchdog5 = new PlcVars.Word(Client, "DW 5", "", "", false);
        }

        public string GetWatchdog()
        {
            if (Client != null)
            {
                if (watchdog5 != null)
                {
                    return watchdog5.Value.ToString();
                }
            }
            return PropComm.NA;
        }
    }
}
