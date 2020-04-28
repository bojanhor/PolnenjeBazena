using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop4
    {
        // Mesala pehala    
        public static Sharp7.S7Client Client;
        PlcVars.Word watchdog4;

        static int rows = 3;

        public PlcVars.Word[] weekday = new PlcVars.Word[rows];
        public PlcVars.Word[] start = new PlcVars.Word[rows];
        public PlcVars.Word[] stop = new PlcVars.Word[rows];

        public PlcVars.Word rezim;
        public PlcVars.Bit rocno;

        public PlcVars.Bit aktivenMotor;
        public PlcVars.Bit Zvezda;
        public PlcVars.Bit Trikot;
        public PlcVars.Bit SmerNaprej;

        readonly ushort startAddrs = 10;


        public Prop4(Sharp7.S7Client client)
        {
            Client = client;
            watchdog4 = new PlcVars.Word(Client, new PlcVars.WordAddress(5), "", "", false);

            for (int i = 0; i < rows; i++)
            {
                weekday[i] = new PlcVars.Word(Client, new PlcVars.WordAddress(startAddrs), "", "", true);
                start[i] = new PlcVars.Word(Client, new PlcVars.WordAddress(startAddrs + 2), "", "", true);
                stop[i] = new PlcVars.Word(Client, new PlcVars.WordAddress(startAddrs + 4), "", "", true);
                startAddrs += 10;

                startAddrs += 100;
            }


            rezim = new PlcVars.Word(Client, new PlcVars.WordAddress(40), "", "", true);
            rocno = new PlcVars.Bit(Client, new PlcVars.BitAddress(50, 0), "", "", true);
            aktivenMotor = new PlcVars.Bit(Client, new PlcVars.BitAddress(60, 0), "", "", false);
            Zvezda = new PlcVars.Bit(Client, new PlcVars.BitAddress(62, 0), "", "", false);
            Trikot = new PlcVars.Bit(Client, new PlcVars.BitAddress(64, 0), "", "", false);
            SmerNaprej = new PlcVars.Bit(Client, new PlcVars.BitAddress(66, 0), "", "", false);
        }
    }
}
