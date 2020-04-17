using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop5
    {

        static int rows = 3;
        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defaultCheckTimingInterval);
        public static Sharp7.S7Client Client;
        PlcVars.Word watchdog5;

        public PlcVars.Word[] weekday = new PlcVars.Word[rows];
        public PlcVars.Word[] start = new PlcVars.Word[rows];
        public PlcVars.Word[] stop = new PlcVars.Word[rows];

        public PlcVars.Word rezim;
        public PlcVars.Bit rocno;

        readonly ushort startAddrs = 10;

        public Prop5(Sharp7.S7Client client)
        {
            Client = client;
            watchdog5 = new PlcVars.Word(Client, new PlcVars.WordAddress(5), "", "", false);

            for (int i = 0; i < rows; i++)
            {
                weekday[i] = new PlcVars.Word(Client, new PlcVars.WordAddress(startAddrs), "", "", true); 
                start[i] = new PlcVars.Word(Client, new PlcVars.WordAddress(startAddrs + 2), "", "", true);
                stop[i] = new PlcVars.Word(Client, new PlcVars.WordAddress(startAddrs + 4), "", "", true);
                startAddrs += 10;
            }


            rezim = new PlcVars.Word(Client, new PlcVars.WordAddress(40), "", "", true);
            rocno = new PlcVars.Bit(Client, new PlcVars.BitAddress(50,0), "", "", true);
        }
               
    }
}
