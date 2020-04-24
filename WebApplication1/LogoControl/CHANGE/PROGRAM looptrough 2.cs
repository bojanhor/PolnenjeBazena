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
        int cnt_prop2 = 0 ;
        int clksync2 = 5;

        public void PROGRAM2(Prop2 prop)
        {
            prop.Rezim_Prikaz.SyncWithPLC();
            prop.Rezim_Set_Auto.SyncWithPLC();
            prop.Rezim_Set_Man0.SyncWithPLC();
            prop.Rezim_Set_Man1.SyncWithPLC();
            prop.Obrati_RocniNacin.SyncWithPLC();
            prop.DejanskiRPM.SyncWithPLC();
            prop.TempNivo1.SyncWithPLC();
            prop.TempNivo2.SyncWithPLC();
            prop.TempNivo3.SyncWithPLC();
            prop.UpostevajZT.SyncWithPLC();
            prop.ObratiTemperatura1.SyncWithPLC();
            prop.ObratiTemperatura2.SyncWithPLC();
            prop.ObratiTemperatura3.SyncWithPLC();
            prop.Nocn_Nacin.SyncWithPLC();
            prop.OmejiObrateNa.SyncWithPLC();
            prop.OmObrMedA.SyncWithPLC();
            prop.OmObrMedB.SyncWithPLC();


            switch (cnt_prop2)
            {
                case 1:
                    prop.TempZunaj.SyncWithPLC();
                    break;

                case 2:
                    // skip
                    break;

                case 3:
                    prop.TempZnotraj.SyncWithPLC();
                    break;

                case 4:
                    // skip
                    break;

                default:
                    cnt_prop2 = 0;
                    break;
            }

            if (clksync2 >= 5)
            {
                prop.LogoClock.SyncWithPLC();
                clksync1 = 0;
            }
         
            clksync2++;
            cnt_prop2++;
        }
    }
}
