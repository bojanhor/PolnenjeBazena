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
        public void PROGRAM4(Prop4 prop)
        {
            for (int i = 0; i < prop.weekday.Length; i++)
            {
                prop.weekday[i].SyncWithPLC();
                prop.start[i].SyncWithPLC();               
                prop.stop[i].SyncWithPLC();
                
            }

            prop.rezim.SyncWithPLC();
            prop.rocno.SyncWithPLC();
            prop.aktivenMotor.SyncWithPLC();
            prop.Zvezda.SyncWithPLC();
            prop.Trikot.SyncWithPLC();
            prop.SmerNaprej.SyncWithPLC();

        }
    }
}
