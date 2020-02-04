using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop9
    {
        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public static Sharp7.S7Client Client;

        public Prop9(Sharp7.S7Client client)
        {
            Client = client;
        }
    }
}
