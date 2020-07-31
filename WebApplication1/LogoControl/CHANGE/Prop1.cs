using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace WebApplication1
{
    public class Prop1 : PropComm
    {
        public PlcVars.Bit Ustavljeno;
        public PlcVars.Bit Rocno_read;
        public PlcVars.Bit Avtomat_read;
        public PlcVars.Bit CakanjeMateriala;
        public PlcVars.Bit CakanjeUkaza;

        public PlcVars.Bit Circle; public PlcVars.Bit ZigZag;
        public PlcVars.Bit Start; public PlcVars.Bit Stop;

        public PlcVars.Bit Man_AutoReadState;

        public PlcVars.Bit Auto;
        public PlcVars.Bit ResetPulse;

        public PlcVars.Bit DirX1; public PlcVars.Bit DirX2; public PlcVars.Bit DirY1; public PlcVars.Bit DirY2;
        public PlcVars.Bit TrakRead;

        public PlcVars.AlarmBit Trak_muss;

        public PlcVars.AlarmBit SymPrisotMat;
        public PlcVars.Bit ReadPrisotMat;

        public PlcVars.Bit SymKSX1; public PlcVars.Bit SymKSX2; public PlcVars.Bit SymKSY1; public PlcVars.Bit SymKSY2;

        public PlcVars.AlarmBit ReadKSX1; public PlcVars.AlarmBit ReadKSX2; public PlcVars.AlarmBit ReadKSY1; public PlcVars.AlarmBit ReadKSY2;

        public PlcVars.Bit Inicializacija; public PlcVars.Bit KorakZigZag1; public PlcVars.Bit KorakZigZag2;

        public PlcVars.Bit InitCircle; public PlcVars.Bit KorakCircle1; public PlcVars.Bit KorakCircle2; public PlcVars.Bit KorakCircle3; public PlcVars.Bit KorakCircle4;

        public PlcVars.Bit JoyStickCommandX1; public PlcVars.Bit JoyStickCommandX2; public PlcVars.Bit JoyStickCommandY1; public PlcVars.Bit JoyStickCommandY2;

        public PlcVars.Word SpeedX; public PlcVars.Word SpeedY;

        public PlcVars.Word SpeedSet; public PlcVars.Word SpeedRead;

        public PlcVars.Word TimeZigY;

        public PlcVars.Bit SemaforGn; public PlcVars.Bit SemaforRd; public PlcVars.Bit SemaforYe;

        // Alarms
        public PlcVars.Bit AlarmInit;
        public PlcVars.Bit CriticalMalfunction;
        public PlcVars.Bit MotorThermalX; public PlcVars.Bit MotorThermalY; public PlcVars.Bit MotorThermalT;
        public PlcVars.AlarmBit Starting;

        public Prop1(Sharp7.S7Client client):base(client)
        {

            // Alarms
            AlarmInit = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(315, 0), "NAPAKA Inicializacije!", false, true) { SyncEvery_X_Time = 3 };
            CriticalMalfunction = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(316, 0), "Napaka končnih stikal X!", false, true) { SyncEvery_X_Time = 3 };
            CriticalMalfunction = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(317, 0), "Napaka končnih stikal Y!", false, true) { SyncEvery_X_Time = 3 };
            Starting = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(318, 0), "Zaganjanje naprave...", false, true) { SyncEvery_X_Time = 3 };
            MotorThermalX = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(319, 0), "Pregretje motorja X!", false, true) { SyncEvery_X_Time = 3 };
            MotorThermalY = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(320, 0), "Pregretje motorja Y!", false, true) { SyncEvery_X_Time = 3 };
            MotorThermalT = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(321, 0), "Pregretje motorja T!", false, true) { SyncEvery_X_Time = 3 };

            // info proces
            Ustavljeno = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(1, 0), "USTAVLJENO", false, true) { SyncEvery_X_Time = 3 };
            Rocno_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(2, 0), "ROČNI NAČIN") { SyncEvery_X_Time = 3 };
            Avtomat_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(3, 0), "Avtomatski način") { SyncEvery_X_Time = 3 };
            CakanjeMateriala = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(4, 0), "ČAKANJE MATERIALA") { SyncEvery_X_Time = 3 };
            CakanjeUkaza = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(5, 0), "ČAKANJE UKAZA") { SyncEvery_X_Time = 3 };

            Inicializacija = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(60, 0), "ZigZag - Inicializacija") { SyncEvery_X_Time = 3 };
            KorakZigZag1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(61, 0), "ZigZag - Korak1") { SyncEvery_X_Time = 3 };
            KorakZigZag2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(63, 0), "ZigZag - Korak2") { SyncEvery_X_Time = 3 };

            InitCircle = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(70, 0), "Rob - Inicializacija") { SyncEvery_X_Time = 3 };
            KorakCircle1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(71, 0), "Rob - Korak1") { SyncEvery_X_Time = 3 };
            KorakCircle2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(72, 0), "Rob - Korak2") { SyncEvery_X_Time = 3 };
            KorakCircle3 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(73, 0), "Rob - Korak3") { SyncEvery_X_Time = 3 };
            KorakCircle4 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(74, 0), "Rob - Korak4") { SyncEvery_X_Time = 3 };

            Circle = new PlcVars.Bit(this, new PlcVars.BitAddress(9, 0), true) { SyncEvery_X_Time = 2 };
            ZigZag = new PlcVars.Bit(this, new PlcVars.BitAddress(8, 0), true) { SyncEvery_X_Time = 2 };
            Start = new PlcVars.Bit(this, new PlcVars.BitAddress(10, 0), true) { SyncEvery_X_Time = 2 };
            Stop = new PlcVars.Bit(this, new PlcVars.BitAddress(12, 0), true) { SyncEvery_X_Time = 2 };

            Man_AutoReadState = new PlcVars.Bit(this, new PlcVars.BitAddress(13, 0), false) { SyncEvery_X_Time = 3 };

            Auto = new PlcVars.Bit(this, new PlcVars.BitAddress(14, 0), true) { SyncEvery_X_Time = 2 };
            ResetPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(16, 0), true) { SyncEvery_X_Time = 2 };

            DirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(20, 0), false) { SyncEvery_X_Time = 2 };
            DirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(22, 0), false) { SyncEvery_X_Time = 2 };
            DirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false) { SyncEvery_X_Time = 2 };
            DirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(26, 0), false) { SyncEvery_X_Time = 2 };
            TrakRead = new PlcVars.Bit(this, new PlcVars.BitAddress(28, 0), false) { SyncEvery_X_Time = 3 };

            Trak_muss = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(29, 0),"Trak je prisilno vključen", false, false, true) { SyncEvery_X_Time = 3 };

            SymPrisotMat = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(30, 0),"Vključena je simulacija materiala!", false, false, true) { SyncEvery_X_Time = 3 };
            ReadPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(32, 0), false) { SyncEvery_X_Time = 3 };

            SymKSX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(40, 0), true) {SyncEvery_X_Time = 4 };
            SymKSX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(42, 0), true) { SyncEvery_X_Time = 4 };
            SymKSY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(44, 0), true) { SyncEvery_X_Time = 4 };
            SymKSY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(46, 0), true) { SyncEvery_X_Time = 4 };

            ReadKSX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(50, 0),"Končna pozicija X1", false,false, true) { SyncEvery_X_Time = 3 };
            ReadKSX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(52, 0), "Končna pozicija X2", false, false, true) { SyncEvery_X_Time = 3 };
            ReadKSY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(54, 0), "Končna pozicija Y1", false, false, true) { SyncEvery_X_Time = 3 };
            ReadKSY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(56, 0), "Končna pozicija Y2", false, false, true) { SyncEvery_X_Time = 3 };
                        
            JoyStickCommandX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(90, 0),"Ročni pomik X1" , false, false, true) { SyncEvery_X_Time = 3 };
            JoyStickCommandX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(91, 0), "Ročni pomik X2", false, false, true) { SyncEvery_X_Time = 3 };
            JoyStickCommandY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(92, 0), "Ročni pomik Y1", false, false, true) { SyncEvery_X_Time = 3 };
            JoyStickCommandY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(93, 0), "Ročni pomik Y2", false, false, true) { SyncEvery_X_Time = 3 };

            SpeedX = new PlcVars.Word(this, new PlcVars.WordAddress(100), false) { SyncEvery_X_Time = 4 };
            SpeedY = new PlcVars.Word(this, new PlcVars.WordAddress(104), false) { SyncEvery_X_Time = 4 };

            SpeedSet = new PlcVars.Word(this, new PlcVars.WordAddress(106), true) { SyncEvery_X_Time = 3 };
            SpeedRead = new PlcVars.Word(this, new PlcVars.WordAddress(108), false) { SyncEvery_X_Time = 3 };

            TimeZigY = new PlcVars.Word(this, new PlcVars.WordAddress(110),"","s", true) { SyncEvery_X_Time = 4 };

            SemaforGn = new PlcVars.Bit(this, new PlcVars.BitAddress(200, 0), false);
            SemaforRd = new PlcVars.Bit(this, new PlcVars.BitAddress(202, 0), false);
            SemaforYe = new PlcVars.Bit(this, new PlcVars.BitAddress(204, 0), false);

            

            

        }

    }
}
