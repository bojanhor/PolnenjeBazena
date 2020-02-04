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

        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
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



        public Prop2(Sharp7.S7Client client)
        {
            Client = client;
            watchdog2 = new PlcVars.Word(Client, "DW 5", "", "", false);

            // Top
            Obrati_RocniNacin = new PlcVars.Word(Client, "VW80", "", "%", true);
            Vklop_RocniNacin = new PlcVars.Bit(Client, "VW84", "Vklop", "Izklop", true);

            // Left side menu
            TempNivo1 = new PlcVars.TemperatureShow(Client, "VW50", "", "°C", 0, 1, 0, true);
            TempNivo2 = new PlcVars.TemperatureShow(Client, "VW54", "", "°C", 0, 1, 0, true);
            TempNivo3 = new PlcVars.TemperatureShow(Client, "VW58", "", "°C", 0, 1, 0, true);
            UpostevajZT = new PlcVars.Bit(Client, "VW70", "DA", "NE", true);


            ObratiTemperatura1 = new PlcVars.Word(Client, "VW62", "", "%", true);
            ObratiTemperatura2 = new PlcVars.Word(Client, "VW64", "", "%", true);
            ObratiTemperatura3 = new PlcVars.Word(Client, "VW66", "", "%", true);


            // Right side menu
            Nocn_Nacin = new PlcVars.Bit(Client, "VW90", "", "", true);
            OmejiObrateNa = new PlcVars.Word(Client, "VW104", "", "%", true);
            OmObrMedA = new PlcVars.TimeSet(Client, "VW94", true);
            OmObrMedB = new PlcVars.TimeSet(Client, "VW98", true);

            //

            TempZunaj = new PlcVars.TemperatureShow(Client, "VW110", "", "°C", 0, 0.1F, 1, true);
            TempZnotraj = new PlcVars.TemperatureShow(Client, "VW114", "", "°C", 0, 0.1F, 1, true);


        }

    }
}
