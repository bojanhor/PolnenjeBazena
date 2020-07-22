using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace WebApplication1
{
    public class Prop2 : PropComm
    {
        // Alarms
        public PlcVars.Bit Alarm_goba;
        public PlcVars.Bit Alarm_zavesa1;
        public PlcVars.Bit Alarm_zavesa2;
        public PlcVars.Bit Alarm_FreqX;
        public PlcVars.Bit Alarm_FreqY;

        //// PositionSimulation            
        public PlcVars.Word PosX; public PlcVars.Word PosY;



        public Prop2(Sharp7.S7Client client):base(client)
        {
            // Alarms
            Alarm_goba = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(310, 0), "AKTIVIRANA JE GOBASTA TIPKA!", true, true) { SyncEvery_X_Time = 3 };
            Alarm_zavesa1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(311, 0), "AKTIVIRANA JE ZAVESA 1!", true, true) { SyncEvery_X_Time = 3 };
            Alarm_zavesa2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(312, 0), "AKTIVIRANA JE ZAVESA 2!", true, true) { SyncEvery_X_Time = 3 };
            Alarm_FreqX = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(313, 0), "NAPAKA FREKVENČNEGA PRETVORNIKA X OSI!", false, true) { SyncEvery_X_Time = 3 };
            Alarm_FreqY = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(314, 0), "NAPAKA FREKVENČNEGA PRETVORNIKA Y OSI!", false, true) { SyncEvery_X_Time = 3 };

            // PositionSimulation            
            PosX = new PlcVars.Word(this, new PlcVars.WordAddress(700), false);
            PosY = new PlcVars.Word(this, new PlcVars.WordAddress(702), false);
        }

    }
}
