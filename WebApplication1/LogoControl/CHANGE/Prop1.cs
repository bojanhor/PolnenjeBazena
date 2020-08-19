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
        //PC Watchdog
        public PlcVars.Word PCWD;

        public PlcVars.Bit Ustavljeno;
        public PlcVars.Bit Rocno_read;
        public PlcVars.Bit Avtomat_read;
        public PlcVars.Bit CakanjeMateriala;
        public PlcVars.Bit CakanjeUkaza;
        public PlcVars.Bit Halt;

        public PlcVars.Bit ZigZag; public PlcVars.Bit ZigZagzRobom;
        public PlcVars.Bit RobX1; public PlcVars.Bit RobY1; public PlcVars.Bit RobX2; public PlcVars.Bit RobY2;
        public PlcVars.Bit KrozniRobX1; public PlcVars.Bit KrozniRobY1; public PlcVars.Bit KrozniRobX2; public PlcVars.Bit KrozniRobY2;
        public PlcVars.Bit Krozenje;

        public PlcVars.Word XImpulses; public PlcVars.Word YImpulses; public PlcVars.Word XImpulses2; public PlcVars.Word YImpulses2;
        public PlcVars.Word ImpulsesDisplayVal; public PlcVars.Word ImpulsesDisplayValRead;
        public PlcVars.Word XPos; public PlcVars.Word YPos;

        public PlcVars.Bit Start; public PlcVars.Bit Stop; public PlcVars.Bit Pause;

        public PlcVars.Bit Man_AutoReadState;

        public PlcVars.Bit Auto;        
        public PlcVars.Bit Inicializacija;

        public PlcVars.Bit DirX1; public PlcVars.Bit DirX2; public PlcVars.Bit DirY1; public PlcVars.Bit DirY2;

        public PlcVars.AlarmBit ZigZag_read; public PlcVars.AlarmBit ZigZagzRobom_read;
        public PlcVars.AlarmBit RobX1_read; public PlcVars.AlarmBit RobX2_read; public PlcVars.AlarmBit RobY1_read; public PlcVars.AlarmBit RobY2_read;
        public PlcVars.AlarmBit CircX1_read; public PlcVars.AlarmBit CircX2_read; public PlcVars.AlarmBit CircY1_read; public PlcVars.AlarmBit CircY2_read;
        public PlcVars.AlarmBit Krozenje_read;

        public PlcVars.Bit TrakRead;

        public PlcVars.Bit Trak_muss;

        public PlcVars.AlarmBit SymPrisotMat;
        public PlcVars.Bit ReadPrisotMat;

        public PlcVars.Bit AutoDirX1; public PlcVars.Bit AutoDirX2; public PlcVars.Bit AutoDirY1; public PlcVars.Bit AutoDirY2;

        public PlcVars.AlarmBit ReadKSX1; public PlcVars.AlarmBit ReadKSX2; public PlcVars.AlarmBit ReadKSY1; public PlcVars.AlarmBit ReadKSY2;
                             
        public PlcVars.Bit JoyStickCommandX1; public PlcVars.Bit JoyStickCommandX2; public PlcVars.Bit JoyStickCommandY1; public PlcVars.Bit JoyStickCommandY2;

        public PlcVars.Word SpeedX; public PlcVars.Word SpeedY;

        public PlcVars.Word SpeedSet; public PlcVars.Word SpeedRead;
        
        public PlcVars.Bit SemaforGn; public PlcVars.Bit SemaforRd; public PlcVars.Bit SemaforYe;

        // Alarms
        public PlcVars.Bit AlarmInit;
        public PlcVars.Bit CriticalMalfunction;
        public PlcVars.Bit MotorThermalX; public PlcVars.Bit MotorThermalY; public PlcVars.Bit MotorThermalT;
        public PlcVars.AlarmBit Starting;

        // Kontrola 
        public PlcVars.Bit PermissionToRun;


        public Prop1(Sharp7.S7Client client):base(client)
        {
            //PC Watchdog
            PCWD = new PlcVars.Word(this, new PlcVars.WordAddress(796), true) {SyncEvery_X_Time = 2 };

            // Alarms
            AlarmInit = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(315, 0), "NAPAKA INICIALIZACIJE!", false, true) ;
            CriticalMalfunction = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(316, 0), "Napaka končnih stikal X!", false, true) ;
            CriticalMalfunction = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(317, 0), "Napaka končnih stikal Y!", false, true) ;
            Starting = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(318, 0), "Zaganjanje naprave...", false, true) ;
            MotorThermalX = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(319, 0), "Pregretje motorja X!", false, true) ;
            MotorThermalY = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(320, 0), "Pregretje motorja Y!", false, true) ;
            MotorThermalT = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(321, 0), "Pregretje motorja T!", false, true) ;

            // info proces
            Ustavljeno = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(1, 0), "USTAVLJENO", false, true) ;
            Rocno_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(2, 0), "ROČNI NAČIN") ;
            CakanjeMateriala = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(4, 0), "ČAKANJE MATERIALA") ;
            CakanjeUkaza = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(5, 0), "ČAKANJE UKAZA") ;
            Halt = new PlcVars.Bit(this, new PlcVars.BitAddress(6,0), true) ;

            XImpulses = new PlcVars.Word(this, new PlcVars.WordAddress(210), true) ;
            YImpulses = new PlcVars.Word(this, new PlcVars.WordAddress(212), true) ;

            XImpulses2 = new PlcVars.Word(this, new PlcVars.WordAddress(218), true) ;
            YImpulses2 = new PlcVars.Word(this, new PlcVars.WordAddress(220), true) ;

            XPos = new PlcVars.Word(this, new PlcVars.WordAddress(310), false) ;
            YPos = new PlcVars.Word(this, new PlcVars.WordAddress(312), false) ;

            ImpulsesDisplayVal = new PlcVars.Word(this, new PlcVars.WordAddress(214), true) ;
            ImpulsesDisplayValRead = new PlcVars.Word(this, new PlcVars.WordAddress(216), false) ;

            Start = new PlcVars.Bit(this, new PlcVars.BitAddress(10, 0), true) ;
            Stop = new PlcVars.Bit(this, new PlcVars.BitAddress(12, 0), true) ;
            Pause = new PlcVars.Bit(this, new PlcVars.BitAddress(16, 0), true) ;

            Man_AutoReadState = new PlcVars.Bit(this, new PlcVars.BitAddress(13, 0), false) ;

            Auto = new PlcVars.Bit(this, new PlcVars.BitAddress(14, 0), true) ;
            Inicializacija = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(15, 0), "--- INICIALIZACIJA ---") ;

            DirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(20, 0), false) ;
            DirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(22, 0), false) ;
            DirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false) ;
            DirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(26, 0), false) ;
            TrakRead = new PlcVars.Bit(this, new PlcVars.BitAddress(28, 0), false) ;

            Trak_muss = new PlcVars.Bit(this, new PlcVars.BitAddress(29, 0), true) ;

            SymPrisotMat = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(30, 0),"Vključena je simulacija materiala!", false, false, true) ;
            ReadPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(32, 0), false) ;

            AutoDirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(40, 0), true);
            AutoDirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(42, 0), true);
            AutoDirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(44, 0), true);
            AutoDirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(46, 0), true);

            ReadKSX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(50, 0), "Končna pozicija X1", false, false, false) ; ;
            ReadKSX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(52, 0), "Končna pozicija X2", false, false, false) ; ;
            ReadKSY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(54, 0), "Končna pozicija Y1", false, false, false) ; ;
            ReadKSY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(56, 0), "Končna pozicija Y2", false, false, false) ; ;

            
            JoyStickCommandX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(90, 0),"Ročni pomik X1" , false, false, true) ;
            JoyStickCommandX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(91, 0), "Ročni pomik X2", false, false, true) ;
            JoyStickCommandY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(92, 0), "Ročni pomik Y1", false, false, true) ;
            JoyStickCommandY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(93, 0), "Ročni pomik Y2", false, false, true) ;

            SpeedX = new PlcVars.Word(this, new PlcVars.WordAddress(100), false) ;
            SpeedY = new PlcVars.Word(this, new PlcVars.WordAddress(104), false) ;

            SpeedSet = new PlcVars.Word(this, new PlcVars.WordAddress(106), true) ;
            SpeedRead = new PlcVars.Word(this, new PlcVars.WordAddress(108), false) ;
                       
            SemaforGn = new PlcVars.Bit(this, new PlcVars.BitAddress(200, 0), false);
            SemaforRd = new PlcVars.Bit(this, new PlcVars.BitAddress(202, 0), false);
            SemaforYe = new PlcVars.Bit(this, new PlcVars.BitAddress(204, 0), false);

            // Kontrola 
            PermissionToRun = new PlcVars.Bit(this, new PlcVars.BitAddress(74, 0), false);

            ZigZag = new PlcVars.Bit(this, new PlcVars.BitAddress(8, 0), true) ;
            ZigZag_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(167, 0), "AKTIVEN PROCES: ZigZag") ;

            ZigZagzRobom = new PlcVars.Bit(this, new PlcVars.BitAddress(7, 0), true) ;
            ZigZagzRobom_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(168, 0), "AKTIVEN PROCES: ZigZag") ;

            RobX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(33,0),true) ; 
            RobX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(34, 0), true) ; 
            RobY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(35, 0), true) ; 
            RobY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(36, 0), true) ;

            KrozniRobX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(63, 0), true);
            KrozniRobX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(64, 0), true);
            KrozniRobY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(65, 0), true);
            KrozniRobY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(66, 0), true);

            Krozenje = new PlcVars.Bit(this, new PlcVars.BitAddress(67, 0), true);

            RobX1_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(163, 0), "AKTIVEN PROCES: Polnenje robu X1") ;
            RobX2_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(164, 0), "AKTIVEN PROCES: Polnenje robu X2") ;
            RobY1_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(165, 0), "AKTIVEN PROCES: Polnenje robu Y1") ;
            RobY2_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(166, 0), "AKTIVEN PROCES: Polnenje robu Y2") ;

            CircX1_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(169, 0), "AKTIVEN PROCES: Krožno polnenje robu X1") ;
            CircX2_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(170, 0), "AKTIVEN PROCES: Krožno polnenje robu X2") ;
            CircY1_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(171, 0), "AKTIVEN PROCES: Krožno polnenje robu Y1") ;
            CircY2_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(172, 0), "AKTIVEN PROCES: Krožno polnenje robu Y2") ;

            Krozenje_read = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(173, 0), "AKTIVEN PROCES: Krožno polnenje robu");

        }

    }
}
