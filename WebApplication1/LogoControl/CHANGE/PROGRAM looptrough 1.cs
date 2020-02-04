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
        int cnt_prop1_1 = 0;
        int cnt_prop1_2 = 0;
        int cnt_prop1_3 = 0;

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
            
            

            switch (cnt_prop1_3)
            {
                case 1:
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
                    break;

                case 2:
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
                    break;

                case 3:
                    foreach (var item in prop.VklopUrnika)
                    {
                        if (item != null)
                        {
                            item.SyncWithPLC();
                        }

                    }
                    break;

                case 4:
                    foreach (var item in prop.DimmerDop)
                    {
                        if (item != null)
                        {
                            item.SyncWithPLC();
                        }
                    }

                    foreach (var item in prop.DimmerPop)
                    {
                        if (item != null)
                        {
                            item.SyncWithPLC();
                        }
                    }
                    break;
                    
                default:
                    cnt_prop1_3 = 0;
                    break;
            }


            switch (cnt_prop1_2)
            {
                case 1:
                    prop.IzklopKoJeDan.SyncWithPLC();
                    break;

                case 2:
                    prop.VzhodOffset_Write.SyncWithPLC();
                    break;

                case 3:
                    prop.ZahodOffset_Write.SyncWithPLC();
                    break;

                case 4:
                    prop.DayLightPercentOn.SyncWithPLC();
                    break;

                case 5:
                    prop.DayLightPercentOff.SyncWithPLC();
                    break;

                default:
                    cnt_prop1_2 = 0;
                    break;
            }


            switch (cnt_prop1_1)
            {
                case 1:
                    prop.DanNoc_Vrednost_An.SyncWithPLC();
                    break;

                case 2:
                    prop.DanNoc_Vrednost_Dig.SyncWithPLC();
                    break;

                case 3:
                    prop.Vzhod_Read.SyncWithPLC();
                    break;

                case 4:
                    prop.Zahod_Read.SyncWithPLC();
                    break;

                default:
                    cnt_prop1_1 = 0;
                    break;
            }   
            
            cnt_prop1_1++;
            cnt_prop1_2++;
            cnt_prop1_3++;

        }
    }
}
