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

        public PlcVars.Bit Trak_muss;

        public PlcVars.Bit SymPrisotMat;
        public PlcVars.Bit ReadPrisotMat;

        public PlcVars.Bit SymKSX1; public PlcVars.Bit SymKSX2; public PlcVars.Bit SymKSY1; public PlcVars.Bit SymKSY2;

        public PlcVars.Bit ReadKSX1; public PlcVars.Bit ReadKSX2; public PlcVars.Bit ReadKSY1; public PlcVars.Bit ReadKSY2;

        public PlcVars.Bit Inicializacija; public PlcVars.Bit Korak1; public PlcVars.Bit Korak2; public PlcVars.Bit Korak3; public PlcVars.Bit Korak4;

        public PlcVars.Bit InitCircle; public PlcVars.Bit KorakCircle1; public PlcVars.Bit KorakCircle2; public PlcVars.Bit KorakCircle3; public PlcVars.Bit KorakCircle4;

        public PlcVars.Bit JoyStickCommandX1; public PlcVars.Bit JoyStickCommandX2; public PlcVars.Bit JoyStickCommandY1; public PlcVars.Bit JoyStickCommandY2;

        public PlcVars.Word SpeedX; public PlcVars.Word SpeedY;

        public PlcVars.Word Trak_speedSet; public PlcVars.Word Trak_speedRead;

        public PlcVars.Word TimeZigY;

        public PlcVars.Bit SemaforZe; public PlcVars.Bit SemaforRd; public PlcVars.Bit SemaforYe;


        public Prop1(Sharp7.S7Client client):base(client)
        {
            Ustavljeno = new PlcVars.Bit(this, new PlcVars.BitAddress(1, 0), false);
            Rocno_read = new PlcVars.Bit(this, new PlcVars.BitAddress(2, 0), false);
            Avtomat_read = new PlcVars.Bit(this, new PlcVars.BitAddress(3, 0), false);
            CakanjeMateriala = new PlcVars.Bit(this, new PlcVars.BitAddress(4, 0), false);
            CakanjeUkaza = new PlcVars.Bit(this, new PlcVars.BitAddress(5, 0), false);

            Circle = new PlcVars.Bit(this, new PlcVars.BitAddress(9, 0), true);
            ZigZag = new PlcVars.Bit(this, new PlcVars.BitAddress(8, 0), true);
            Start = new PlcVars.Bit(this, new PlcVars.BitAddress(10, 0), true);
            Stop = new PlcVars.Bit(this, new PlcVars.BitAddress(12, 0), true);

            Man_AutoReadState = new PlcVars.Bit(this, new PlcVars.BitAddress(13, 0), false);

            Auto = new PlcVars.Bit(this, new PlcVars.BitAddress(14, 0), true);
            ResetPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(16, 0), true);

            DirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(20, 0), false);
            DirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(22, 0), false);
            DirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false);
            DirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(26, 0), false);
            TrakRead = new PlcVars.Bit(this, new PlcVars.BitAddress(28, 0), false);

            Trak_muss = new PlcVars.Bit(this, new PlcVars.BitAddress(29, 0), true);

            SymPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(30, 0), true);
            ReadPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(32, 0), false);

            SymKSX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(40, 0), true) {SyncEvery_X_Time = 2 };
            SymKSX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(42, 0), true) { SyncEvery_X_Time = 2 };
            SymKSY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(44, 0), true) { SyncEvery_X_Time = 2 };
            SymKSY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(46, 0), true) { SyncEvery_X_Time = 2 };

            ReadKSX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(50, 0), false);
            ReadKSX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(52, 0), false);
            ReadKSY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(54, 0), false);
            ReadKSY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(56, 0), false);

            Inicializacija = new PlcVars.Bit(this, new PlcVars.BitAddress(60, 0), false);
            Korak1 = new PlcVars.Bit(this, new PlcVars.BitAddress(60, 0), false);
            Korak2 = new PlcVars.Bit(this, new PlcVars.BitAddress(61, 0), false);
            Korak3 = new PlcVars.Bit(this, new PlcVars.BitAddress(62, 0), false);
            Korak4 = new PlcVars.Bit(this, new PlcVars.BitAddress(63, 0), false);

            InitCircle = new PlcVars.Bit(this, new PlcVars.BitAddress(70, 0), false);
            KorakCircle1 = new PlcVars.Bit(this, new PlcVars.BitAddress(71, 0), false);
            KorakCircle2 = new PlcVars.Bit(this, new PlcVars.BitAddress(72, 0), false);
            KorakCircle3 = new PlcVars.Bit(this, new PlcVars.BitAddress(73, 0), false);
            KorakCircle4 = new PlcVars.Bit(this, new PlcVars.BitAddress(74, 0), false);

            JoyStickCommandX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(90, 0), true);
            JoyStickCommandX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(91, 0), true);
            JoyStickCommandY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(92, 0), true);
            JoyStickCommandY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(93, 0), true);

            SpeedX = new PlcVars.Word(this, new PlcVars.WordAddress(100), false) { SyncEvery_X_Time = 2 };
            SpeedY = new PlcVars.Word(this, new PlcVars.WordAddress(104), false) { SyncEvery_X_Time = 2 };

            Trak_speedSet = new PlcVars.Word(this, new PlcVars.WordAddress(106), true);

            Trak_speedRead = new PlcVars.Word(this, new PlcVars.WordAddress(108), false);

            TimeZigY = new PlcVars.Word(this, new PlcVars.WordAddress(110), true) { SyncEvery_X_Time = 2 };

            SemaforZe = new PlcVars.Bit(this, new PlcVars.BitAddress(90, 0), false);
            SemaforRd = new PlcVars.Bit(this, new PlcVars.BitAddress(91, 0), false);
            SemaforYe = new PlcVars.Bit(this, new PlcVars.BitAddress(92, 0), false);

        }

    }
}
