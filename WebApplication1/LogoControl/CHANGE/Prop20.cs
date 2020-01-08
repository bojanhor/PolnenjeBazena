using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop20
    {
        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public Sharp7.S7Client Client { get; set; }

        public Prop20(Sharp7.S7Client client)
        {
            Client = client;
        }
    }
}
