using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace WebApplication1
{
    public class Prop1
    {
        // Razsvetljava

        Misc.LoopTiming timing = new Misc.LoopTiming(Settings.UpdateValuesPCms, Settings.defultCheckTimingInterval);
        public Sharp7.S7Client Client;

        static int LucIconsTableLength = XmlController.GetHowManyLucIcons() + 1;

        public PlcVars.Bit[] LucStatus_ReadToPC = new PlcVars.Bit[LucIconsTableLength];
        public PlcVars.Bit[] LucStatus_WriteToPLC = new PlcVars.Bit[LucIconsTableLength];
        public PlcVars.Bit UgasniVseLuci;

        public PlcVars.WordForCheckBox[] VklopUrnika1 = new PlcVars.WordForCheckBox[LucIconsTableLength];
        public PlcVars.WordForCheckBox[] VklopUrnika2 = new PlcVars.WordForCheckBox[LucIconsTableLength];
        public PlcVars.TimeSet[] VklopConadop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.TimeSet[] VklopConapop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.TimeSet[] IzklopConadop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.TimeSet[] IzklopConapop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.Word[] DimmerDop = new PlcVars.Word[LucIconsTableLength];
        public PlcVars.Word[] DimmerPop = new PlcVars.Word[LucIconsTableLength];

        public PlcVars.TimeSet LogoClock;


        public Prop1(Sharp7.S7Client client, LogoControler logoControler)
        {
            Client = client;

            for (int i = 1; i < LucStatus_ReadToPC.Length; i++)
            {
                LucStatus_ReadToPC[i] = new PlcVars.Bit(Client, XmlController.GetLucAddress_ReadToPC(i),"", "", false);
                LucStatus_WriteToPLC[i] = new PlcVars.Bit(Client, XmlController.GetLucAddress_WriteToPLC(i), "", "", true);
                
                VklopConadop[i] = new PlcVars.TimeSet(Client, "VW550", true);  // TODO vpiši adress
                VklopConapop[i] = new PlcVars.TimeSet(Client, "VW554", true);  // TODO vpiši adress
                IzklopConadop[i] = new PlcVars.TimeSet(Client, "VW552", true); // TODO vpiši adress                
                IzklopConapop[i] = new PlcVars.TimeSet(Client, "VW556", true); // TODO vpiši adress
                VklopUrnika1[i] = new PlcVars.WordForCheckBox(Client, "", true); // TODO vpiši adress
                VklopUrnika2[i] = new PlcVars.WordForCheckBox(Client, "", true); // TODO vpiši adress
                DimmerDop[i] = new PlcVars.Word(Client, "", "", "", true); // TODO vpiši adress
                DimmerPop[i] = new PlcVars.Word(Client, "", "", "", true); // TODO vpiši adress

                LogoClock = new PlcVars.TimeSet(Client, "", false); // TODO vpiši address

            }

            UgasniVseLuci = new PlcVars.Bit(Client, "bit at 1.0", "", "", true);

            
                       
        }
                
    }
}
