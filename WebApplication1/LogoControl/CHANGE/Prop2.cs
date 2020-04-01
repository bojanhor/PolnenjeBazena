using System;
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

        // Top
        public PlcVars.Word Obrati_RocniNacin;
        public PlcVars.Bit Vklop_RocniNacin;

        // Left side menu
        public PlcVars.TemperatureShow TempNivo1;
        public PlcVars.TemperatureShow TempNivo2;
        public PlcVars.TemperatureShow TempNivo3;
        public PlcVars.Bit UpostevajZT;


        public PlcVars.Word ObratiTemperatura1;
        public PlcVars.Word ObratiTemperatura2;
        public PlcVars.Word ObratiTemperatura3;


        // Right side menu
        public PlcVars.Bit Nocn_Nacin;
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

            // Top
            Obrati_RocniNacin = new PlcVars.Word(Client, new PlcVars.WordAddress(80), "", "%", true);
            Vklop_RocniNacin = new PlcVars.Bit(Client, new PlcVars.BitAddress(84,0), "Vklop", "Izklop", true);

            // Left side menu
            TempNivo1 = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(50), "", "°C", 0, 1, 0, true);
            TempNivo2 = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(54), "", "°C", 0, 1, 0, true);
            TempNivo3 = new PlcVars.TemperatureShow(Client, new PlcVars.WordAddress(58), "", "°C", 0, 1, 0, true);
            UpostevajZT = new PlcVars.Bit(Client, new PlcVars.BitAddress(70,0), "DA", "NE", true);


            ObratiTemperatura1 = new PlcVars.Word(Client, new PlcVars.WordAddress(62), "", "%", true);
            ObratiTemperatura2 = new PlcVars.Word(Client, new PlcVars.WordAddress(64), "", "%", true);
            ObratiTemperatura3 = new PlcVars.Word(Client, new PlcVars.WordAddress(66), "", "%", true);


            // Right side menu
            Nocn_Nacin = new PlcVars.Bit(Client, new PlcVars.BitAddress(90,0), "", "", true);
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
