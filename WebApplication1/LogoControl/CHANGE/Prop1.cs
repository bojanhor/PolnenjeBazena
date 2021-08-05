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
        public PlcVars.Word PCWD; public PlcVars.AlarmBit PCWDFAIL;

        public PlcVars.Bit Ustavljeno;
        public PlcVars.Bit Rocno_read;
        public PlcVars.Bit Avtomat_read;
        public PlcVars.Bit CakanjeMateriala;
        public PlcVars.Bit Halt;

        public PlcVars.Word XImpulses; public PlcVars.Word YImpulses; public PlcVars.Word XImpulses2; public PlcVars.Word YImpulses2;
        public PlcVars.Word ImpulsesDisplayVal; public PlcVars.Word ImpulsesDisplayValRead;
        public PlcVars.Word XPos; public PlcVars.Word YPos;

        public PlcVars.Bit Start; public PlcVars.Bit Stop; public PlcVars.Bit Pause; public PlcVars.Bit StartInit;
        public PlcVars.Bit Initialized; public PlcVars.AlarmBit NotInitialized;

        public PlcVars.Bit Man_AutoReadState;

        public PlcVars.Bit Auto;        
        public PlcVars.Bit Inicializacija;

        public PlcVars.Bit DirX1; public PlcVars.Bit DirX2; public PlcVars.Bit DirY1; public PlcVars.Bit DirY2;

        
        public PlcVars.Bit TrakRead;

        public PlcVars.Bit Trak_muss;

        public PlcVars.AlarmBit SymPrisotMat;
        public PlcVars.Bit ReadPrisotMat;

        public PlcVars.Bit AutoDirX1; public PlcVars.Bit AutoDirX2; public PlcVars.Bit AutoDirY1; public PlcVars.Bit AutoDirY2;

        public PlcVars.AlarmBit ReadKSX1; public PlcVars.AlarmBit ReadKSX2; public PlcVars.AlarmBit ReadKSY1; public PlcVars.AlarmBit ReadKSY2;
                             
        public PlcVars.Bit JoyStickCommandX1; public PlcVars.Bit JoyStickCommandX2; public PlcVars.Bit JoyStickCommandY1; public PlcVars.Bit JoyStickCommandY2;

        public PlcVars.AlarmBit ReadJoyStickCommandX1; public PlcVars.AlarmBit ReadJoyStickCommandX2; public PlcVars.AlarmBit ReadJoyStickCommandY1; public PlcVars.AlarmBit ReadJoyStickCommandY2;
        public PlcVars.AlarmBit ReadJoyStickCommandActive;
        public PlcVars.Word SpeedX; public PlcVars.Word SpeedY;

        public PlcVars.Word SpeedSet; public PlcVars.Word SpeedRead;
        public PlcVars.Word SpeedSetTrak;

        public PlcVars.Bit SemaforGn; public PlcVars.Bit SemaforRd; public PlcVars.Bit SemaforYe;

        // Alarms
        public PlcVars.Bit AlarmInit;
        public PlcVars.Bit CriticalMalfunction;
        public PlcVars.Bit MotorThermalX; public PlcVars.Bit MotorThermalY; public PlcVars.Bit MotorThermalT;
        public PlcVars.AlarmBit Starting;

        // Kontrola 
        public PlcVars.Bit PermissionToRun;

        public PlcVars.Bit Polnenje_RobX1, Polnenje_RobX2, Polnenje_RobY1, Polnenje_RobY2,
            Polnenje_RobKroznoX1, Polnenje_RobKroznoX2, Polnenje_RobKroznoY1, Polnenje_RobKroznoY2,
            Polnenje_Krozno, Polnenje_ZigZag, Polnenje_ZigZagsKrogom;


        public Prop1(Sharp7.S7Client client):base(client)
        {
            //PC Watchdog
            PCWD = new PlcVars.Word(this, new PlcVars.WordAddress(796), true);
            PCWDFAIL = new  PlcVars.AlarmBit(this, new PlcVars.BitAddress(792,0),"Krmilnik ni zaznal Računalnika!", false, true);

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
            StartInit = new PlcVars.Bit(this, new PlcVars.BitAddress(17, 0), true); 

            Initialized = new PlcVars.Bit(this, new PlcVars.BitAddress(18, 0), false);
            NotInitialized = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(19,0), "Naprava ni inicializirana");

            Man_AutoReadState = new PlcVars.Bit(this, new PlcVars.BitAddress(13, 0), false) ;

            Auto = new PlcVars.Bit(this, new PlcVars.BitAddress(14, 0), true) ;
            Inicializacija = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(15, 0), "--- INICIALIZACIJA ---") ;

            DirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(20, 0), false) ;   // read data - motor runing direction X1
            DirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(22, 0), false);    // read data - motor runing direction X2
            DirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false);    // read data - motor runing direction Y1
            DirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(26, 0), false);    // read data - motor runing direction Y2
            TrakRead = new PlcVars.Bit(this, new PlcVars.BitAddress(28, 0), false) ;

            Trak_muss = new PlcVars.Bit(this, new PlcVars.BitAddress(29, 0), true) ;

            SymPrisotMat = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(30, 0),"Vključena je simulacija materiala!", false, false, true) ;
            ReadPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(32, 0), false) ;

            AutoDirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(40, 0), true); // writable - send command to move
            AutoDirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(42, 0), true); // writable - send command to move
            AutoDirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(44, 0), true); // writable - send command to move
            AutoDirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(46, 0), true); // writable - send command to move

            ReadKSX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(50, 0), "Končna pozicija X1", false, false, false) ;
            ReadKSX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(52, 0), "Končna pozicija X2", false, false, false) ;
            ReadKSY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(54, 0), "Končna pozicija Y1", false, false, false) ;
            ReadKSY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(56, 0), "Končna pozicija Y2", false, false, false) ;
            
            JoyStickCommandX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(90, 0), true) ;   // writable - joystick direction command
            JoyStickCommandX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(91, 0), true);    // writable - joystick direction command
            JoyStickCommandY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(92, 0), true);    // writable - joystick direction command
            JoyStickCommandY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(93, 0), true);    // writable - joystick direction command

            ReadJoyStickCommandX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(94, 0), "Ročni pomik X1", false, false, false);   // indicator of manual movement
            ReadJoyStickCommandX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(95, 0), "Ročni pomik X2", false, false, false);   // indicator of manual movement
            ReadJoyStickCommandY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(96, 0), "Ročni pomik Y1", false, false, false);   // indicator of manual movement
            ReadJoyStickCommandY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(97, 0), "Ročni pomik Y2", false, false, false);   // indicator of manual movement
            ReadJoyStickCommandActive = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(98, 0), "Daljinsko upravljanje aktivno", false, false, false);   // indicator of physical joystick active

            SpeedX = new PlcVars.Word(this, new PlcVars.WordAddress(100), false) ;
            SpeedY = new PlcVars.Word(this, new PlcVars.WordAddress(104), false) ;

            SpeedSet = new PlcVars.Word(this, new PlcVars.WordAddress(106), true) ;
            SpeedRead = new PlcVars.Word(this, new PlcVars.WordAddress(108), false) ;

            SpeedSetTrak = new PlcVars.Word(this, new PlcVars.WordAddress(226), true);

            SemaforGn = new PlcVars.Bit(this, new PlcVars.BitAddress(200, 0), false);
            SemaforRd = new PlcVars.Bit(this, new PlcVars.BitAddress(202, 0), false);
            SemaforYe = new PlcVars.Bit(this, new PlcVars.BitAddress(204, 0), false);

            // Kontrola 
            PermissionToRun = new PlcVars.Bit(this, new PlcVars.BitAddress(74, 0), false);

            Polnenje_RobX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(402, 0), "Polnenje Robu X1", false, false, true);
            Polnenje_RobX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(403, 0), "Polnenje Robu X2", false, false, true);
            Polnenje_RobY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(404, 0), "Polnenje Robu Y1", false, false, true);
            Polnenje_RobY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(405, 0), "Polnenje Robu Y2", false, false, true);
            Polnenje_RobKroznoX1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(406, 0), "Krožno Polnenje Robu X1", false, false, true);
            Polnenje_RobKroznoX2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(407, 0), "Krožno Polnenje Robu X2", false, false, true);
            Polnenje_RobKroznoY1 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(408, 0), "Krožno Polnenje Robu Y1", false, false, true);
            Polnenje_RobKroznoY2 = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(409, 0), "Krožno Polnenje Robu Y2", false, false, true);
            Polnenje_Krozno = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(410, 0), "Krožno Polnenje", false, false, true);
            Polnenje_ZigZag = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(411, 0), "Polnenje ZigZag", false, false, true);
            Polnenje_ZigZagsKrogom = new PlcVars.AlarmBit(this, new PlcVars.BitAddress(412, 0), "Polnenje ZigZag s Kroženjem", false, false, true);

        }

    }
}
