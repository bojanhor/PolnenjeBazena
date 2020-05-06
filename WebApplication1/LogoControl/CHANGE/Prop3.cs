using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace WebApplication1
{
    public class Prop3 : PropComm
    {
        // Vrata / zavese
       
        public PlcVars.Bit VrataGorPulse1;
        public PlcVars.Bit VrataDolPulse1;
        public PlcVars.Bit VrataStopPulse1;

        public PlcVars.Bit UporabljaKoncnaStikala1;
        public PlcVars.Bit KoncnoStikaloGor1;
        public PlcVars.Bit KoncnoStikaloDol1;

        public PlcVars.Word CasPotovanja1;

        // 2
        public PlcVars.Bit VrataGorPulse2;
        public PlcVars.Bit VrataDolPulse2;
        public PlcVars.Bit VrataStopPulse2;

        public PlcVars.Bit UporabljaKoncnaStikala2;
        public PlcVars.Bit KoncnoStikaloGor2;
        public PlcVars.Bit KoncnoStikaloDol2;

        public PlcVars.Word CasPotovanja2;

        // 3
        public PlcVars.Bit VrataGorPulse3;
        public PlcVars.Bit VrataDolPulse3;
        public PlcVars.Bit VrataStopPulse3;

        public PlcVars.Bit UporabljaKoncnaStikala3;
        public PlcVars.Bit KoncnoStikaloGor3;
        public PlcVars.Bit KoncnoStikaloDol3;

        public PlcVars.Word CasPotovanja3;



        //
        public PlcVars.Bit AktivenMotor;
        public PlcVars.Bit Zvezda;
        public PlcVars.Bit Trikot;
        public PlcVars.Bit SmerNaprej;

        public Prop3(Sharp7.S7Client client):base(client)
        {
                       
            //1
            VrataGorPulse1 = new PlcVars.Bit(this, new PlcVars.BitAddress(12, 0), true);
            VrataDolPulse1 = new PlcVars.Bit(this, new PlcVars.BitAddress(13, 0), true);
            VrataStopPulse1 = new PlcVars.Bit(this, new PlcVars.BitAddress(14, 0), true);

            UporabljaKoncnaStikala1 = new PlcVars.Bit(this, new PlcVars.BitAddress(20, 0), true);
            KoncnoStikaloGor1 = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false);
            KoncnoStikaloDol1 = new PlcVars.Bit(this, new PlcVars.BitAddress(26, 0), false);

            CasPotovanja1 = new PlcVars.Word(this, new PlcVars.WordAddress(30), true);

            // 2
            VrataGorPulse2 = new PlcVars.Bit(this, new PlcVars.BitAddress(112, 0), true);
            VrataDolPulse2 = new PlcVars.Bit(this, new PlcVars.BitAddress(113, 0), true);
            VrataStopPulse2 = new PlcVars.Bit(this, new PlcVars.BitAddress(114, 0), true);

            UporabljaKoncnaStikala2 = new PlcVars.Bit(this, new PlcVars.BitAddress(120, 0), true);
            KoncnoStikaloGor2 = new PlcVars.Bit(this, new PlcVars.BitAddress(124, 0), false);
            KoncnoStikaloDol2 = new PlcVars.Bit(this, new PlcVars.BitAddress(126, 0), false);

            CasPotovanja2 = new PlcVars.Word(this, new PlcVars.WordAddress(130), true);

            // 3
            VrataGorPulse3 = new PlcVars.Bit(this, new PlcVars.BitAddress(212, 0), true);
            VrataDolPulse3 = new PlcVars.Bit(this, new PlcVars.BitAddress(213, 0), true);
            VrataStopPulse3 = new PlcVars.Bit(this, new PlcVars.BitAddress(214, 0), true);

            UporabljaKoncnaStikala3 = new PlcVars.Bit(this, new PlcVars.BitAddress(220, 0), true);
            KoncnoStikaloGor3 = new PlcVars.Bit(this, new PlcVars.BitAddress(224, 0), false);
            KoncnoStikaloDol3 = new PlcVars.Bit(this, new PlcVars.BitAddress(226, 0), false);

            CasPotovanja3 = new PlcVars.Word(this, new PlcVars.WordAddress(230), true);


            //
            AktivenMotor = new PlcVars.Bit(this, new PlcVars.BitAddress(60, 0), false);
            Zvezda = new PlcVars.Bit(this, new PlcVars.BitAddress(62, 0), false);
            Trikot = new PlcVars.Bit(this, new PlcVars.BitAddress(64, 0), false);
            SmerNaprej = new PlcVars.Bit(this, new PlcVars.BitAddress(66, 0), false);

        }

       
    }
}
