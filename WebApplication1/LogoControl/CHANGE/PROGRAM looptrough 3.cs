using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Sharp7;

using Linq = System.Xml.Linq;

namespace WebApplication1
{
    public partial class LogoControler
    {
        public void PROGRAM3(Prop3 prop)
        {
            prop.VrataGorPulse1.SyncWithPLC();
            prop.VrataDolPulse1.SyncWithPLC();
            prop.VrataStopPulse1.SyncWithPLC();

            prop.UporabljaKoncnaStikala1.SyncWithPLC();
            prop.KoncnoStikaloGor1.SyncWithPLC();
            prop.KoncnoStikaloDol1.SyncWithPLC();

            prop.CasPotovanja1.SyncWithPLC();

            //
            prop.VrataGorPulse2.SyncWithPLC();
            prop.VrataDolPulse2.SyncWithPLC();
            prop.VrataStopPulse2.SyncWithPLC();

            prop.UporabljaKoncnaStikala2.SyncWithPLC();
            prop.KoncnoStikaloGor2.SyncWithPLC();
            prop.KoncnoStikaloDol2.SyncWithPLC();

            prop.CasPotovanja2.SyncWithPLC();

            //
            prop.VrataGorPulse3.SyncWithPLC();
            prop.VrataDolPulse3.SyncWithPLC();
            prop.VrataStopPulse3.SyncWithPLC();

            prop.UporabljaKoncnaStikala3.SyncWithPLC();
            prop.KoncnoStikaloGor3.SyncWithPLC();
            prop.KoncnoStikaloDol3.SyncWithPLC();

            prop.CasPotovanja3.SyncWithPLC();


        }
    }
}
