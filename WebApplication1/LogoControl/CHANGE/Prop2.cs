﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop2
    {
        // Ventilacija 

        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defaultCheckTimingInterval);
        public static Sharp7.S7Client Client;
        PlcVars.Word watchdog2;
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
        public PlcVars.Word UpostevajZT;


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

        public PlcVars.Word PadavineZadnjaUra;

        public Prop2(Sharp7.S7Client client)
        {
            Client = client;
            watchdog2 = new PlcVars.Word(Client, new PlcVars.WordAddress(5), "", "", false);
            LogoClock = new PlcVars.LogoClock(Client);

            // Top            
            Rezim_Prikaz = new PlcVars.Word(Client, new PlcVars.WordAddress(180), "", "%", false);
            Rezim_Set_Auto = new PlcVars.Bit(Client, new PlcVars.BitAddress(184,0), "", "", true);
            Rezim_Set_Man0 = new PlcVars.Bit(Client, new PlcVars.BitAddress(186,0), "", "", true); 
            Rezim_Set_Man1 = new PlcVars.Bit(Client, new PlcVars.BitAddress(188,0), "", "", true);
            Obrati_RocniNacin = new PlcVars.Word(Client, new PlcVars.WordAddress(84), "", "%", true);
            DejanskiRPM = new PlcVars.Word(Client, new PlcVars.WordAddress(86), "", "%", false);

            // Left side menu
            TempNivo1 = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(50), "", "°C", 0, 0.1F, 0, true);
            TempNivo2 = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(54), "", "°C", 0, 0.1F, 0, true);
            TempNivo3 = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(58), "", "°C", 0, 0.1F, 0, true);
            UpostevajZT = new PlcVars.Word(Client, new PlcVars.WordAddress(70), "DA", "NE", true);


            ObratiTemperatura1 = new PlcVars.Word(Client, new PlcVars.WordAddress(62), "", "%", true);
            ObratiTemperatura2 = new PlcVars.Word(Client, new PlcVars.WordAddress(64), "", "%", true);
            ObratiTemperatura3 = new PlcVars.Word(Client, new PlcVars.WordAddress(66), "", "%", true);


            // Right side menu
            Nocn_Nacin = new PlcVars.Word(Client, new PlcVars.WordAddress(90), "", "", true);
            OmejiObrateNa = new PlcVars.Word(Client, new PlcVars.WordAddress(104), "", "%", true);
            OmObrMedA = new PlcVars.TimeSet(Client, new PlcVars.WordAddress(94), true);
            OmObrMedB = new PlcVars.TimeSet(Client, new PlcVars.WordAddress(98), true);

            //

            TempZunaj = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(110), "", "°C", 0, 0.1F, 1, true);
            TempZnotraj = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(114), "", "°C", 0, 0.1F, 1, true);

            PadavineZadnjaUra = new PlcVars.Word(Client, new PlcVars.WordAddress(120), "", "mm/h", false);

        }

    }
}
