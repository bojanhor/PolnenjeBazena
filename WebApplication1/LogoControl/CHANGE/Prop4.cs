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

        public PlcVars.Byte[] weekday = new PlcVars.Byte[rows];
        public PlcVars.TimeSet[] start = new PlcVars.TimeSet[rows];
        public PlcVars.TimeSet[] stop = new PlcVars.TimeSet[rows];

        //bit	40	SetAuto
        //bit	42	SetMan0
        //bit	44	SetMan1
        //bit	46	ShowVal

        public PlcVars.Bit SetAuto;
        public PlcVars.Bit SetMan0;
        public PlcVars.Bit SetMan1;
        public PlcVars.Word ShowVal_Rezim;

        public PlcVars.Bit aktivenMotor;        

        readonly ushort startAddrs = 10;


        public Prop4(Sharp7.S7Client client) : base(client)
        {
            for (int i = 0; i < rows; i++)
            {
                weekday[i] = new PlcVars.Byte(this, new PlcVars.ByteAddress(startAddrs), true);
                start[i] = new PlcVars.TimeSet(this, new PlcVars.WordAddress(startAddrs + 2), true);
                stop[i] = new PlcVars.TimeSet(this, new PlcVars.WordAddress(startAddrs + 4), true);

                startAddrs += 100;
            }


            SetAuto = new PlcVars.Bit(this, new PlcVars.BitAddress(40, 0), true);
            SetMan0 = new PlcVars.Bit(this, new PlcVars.BitAddress(42, 0), true);
            SetMan1 = new PlcVars.Bit(this, new PlcVars.BitAddress(44, 0), true);
            ShowVal_Rezim = new PlcVars.Word(this, new PlcVars.WordAddress(46), false);

            aktivenMotor = new PlcVars.Bit(this, new PlcVars.BitAddress(60, 0), false);
            
        }
    }
}
