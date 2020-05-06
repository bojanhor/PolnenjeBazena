using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop2 : PropComm
    {
        // Ventilacija 
        public PlcVars.LogoClock LogoClock;

        // Top
        public PlcVars.Word Rezim_Prikaz;
        public PlcVars.Bit Rezim_Set_Auto;
        public PlcVars.Bit Rezim_Set_Man0;
        public PlcVars.Bit Rezim_Set_Man1;
        public PlcVars.Word Obrati_RocniNacin;

        public PlcVars.Word DejanskiRPM;

        // Left side menu
        public PlcVars.TemperatureShow TempNivo1;
        public PlcVars.TemperatureShow TempNivo2;
        public PlcVars.TemperatureShow TempNivo3;
        


        public PlcVars.Word ObratiTemperatura1;
        public PlcVars.Word ObratiTemperatura2;
        public PlcVars.Word ObratiTemperatura3;


        // Right side menu
        public PlcVars.Word Nocn_Nacin;
        public PlcVars.Word OmejiObrateNa;
        public PlcVars.TimeSet OmObrMedA;
        public PlcVars.TimeSet OmObrMedB;

        public PlcVars.TemperatureShow TempZunaj;
        public PlcVars.TemperatureShow TempZnotraj;
        public PlcVars.Word UpostevajZT;
        public PlcVars.Word PresezekZT;

        public PlcVars.Word PadavineZadnjaUra;

        public Prop2(Sharp7.S7Client client):base(client)
        {               
            LogoClock = new PlcVars.LogoClock(this);

            // Top            
            Rezim_Prikaz = new PlcVars.Word(this, new PlcVars.WordAddress(180), "", "%", false);
            Rezim_Set_Auto = new PlcVars.Bit(this, new PlcVars.BitAddress(184,0), true);
            Rezim_Set_Man0 = new PlcVars.Bit(this, new PlcVars.BitAddress(186,0), true); 
            Rezim_Set_Man1 = new PlcVars.Bit(this, new PlcVars.BitAddress(188,0), true);
            Obrati_RocniNacin = new PlcVars.Word(this, new PlcVars.WordAddress(84), "", "%", true);
            DejanskiRPM = new PlcVars.Word(this, new PlcVars.WordAddress(86), "", "%", false);

            // Left side menu
            TempNivo1 = new PlcVars.TemperatureShow(this, new PlcVars.WordAddress(50), "", "°C", 0, 0.1F, 0, true);
            TempNivo2 = new PlcVars.TemperatureShow(this, new PlcVars.WordAddress(54), "", "°C", 0, 0.1F, 0, true);
            TempNivo3 = new PlcVars.TemperatureShow(this, new PlcVars.WordAddress(58), "", "°C", 0, 0.1F, 0, true);
            


            ObratiTemperatura1 = new PlcVars.Word(this, new PlcVars.WordAddress(62), "", "%", true);
            ObratiTemperatura2 = new PlcVars.Word(this, new PlcVars.WordAddress(64), "", "%", true);
            ObratiTemperatura3 = new PlcVars.Word(this, new PlcVars.WordAddress(66), "", "%", true);


            // Right side menu
            Nocn_Nacin = new PlcVars.Word(this, new PlcVars.WordAddress(90), true);
            OmejiObrateNa = new PlcVars.Word(this, new PlcVars.WordAddress(104), "", "%", true);
            OmObrMedA = new PlcVars.TimeSet(this, new PlcVars.WordAddress(94), true);
            OmObrMedB = new PlcVars.TimeSet(this, new PlcVars.WordAddress(98), true);
            UpostevajZT = new PlcVars.Word(this, new PlcVars.WordAddress(70), true);
            PresezekZT = new PlcVars.Word(this, new PlcVars.WordAddress(74), "", "°C", true);

            //

            TempZunaj = new PlcVars.TemperatureShow(this, new PlcVars.WordAddress(110), "", "°C", 0, 0.1F, 1, true) {SyncEvery_X_Time = 2};
            TempZnotraj = new PlcVars.TemperatureShow(this, new PlcVars.WordAddress(114), "", "°C", 0, 0.1F, 1, true) {SyncEvery_X_Time = 2};

            PadavineZadnjaUra = new PlcVars.Word(this, new PlcVars.WordAddress(120), "", "mm/h", false);

        }

    }
}
