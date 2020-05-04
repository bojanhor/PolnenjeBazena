using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop4 : PropComm
    {
        // Mesala pehala           
        
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


        public Prop4(Sharp7.S7Client client):base(client)
        {                        
            for (int i = 0; i < rows; i++)
            {
                weekday[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(startAddrs), "", "", true);
                start[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(startAddrs + 2), "", "", true);
                stop[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(startAddrs + 4), "", "", true);
                startAddrs += 10;

                startAddrs += 100;
            }
            
            rezim = new PlcVars.Word(Client, this, new PlcVars.WordAddress(40), "", "", true);
            rocno = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(50, 0), "", "", true);
            aktivenMotor = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(60, 0), "", "", false);
            Zvezda = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(62, 0), "", "", false);
            Trikot = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(64, 0), "", "", false);
            SmerNaprej = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(66, 0), "", "", false);
        }
    }
}
