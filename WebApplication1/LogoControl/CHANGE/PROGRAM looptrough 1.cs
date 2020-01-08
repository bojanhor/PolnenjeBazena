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
        

        public void PROGRAM1(Prop1 prop)
        {
            // This program is for LOGO1

            // GET/SET with plc    

            foreach (var item in prop.LucStatus_ReadToPC)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }
                
            }
            foreach (var item in prop.LucStatus_WriteToPLC)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }
                
            }

            prop.UgasniVseLuci.SyncWithPLC();

            foreach (var item in prop.VklopConadop)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }
            }

            foreach (var item in prop.VklopConapop)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }
            }

            foreach (var item in prop.IzklopConadop)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }
            }

            foreach (var item in prop.IzklopConapop)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }
            }

            foreach (var item in prop.VklopUrnika1)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }

            }

            foreach (var item in prop.VklopUrnika2)
            {
                if (item != null)
                {
                    item.SyncWithPLC();
                }

            }



        }
    }
}
