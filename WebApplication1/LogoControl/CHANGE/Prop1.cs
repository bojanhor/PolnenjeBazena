using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace WebApplication1
{
    public class Prop1 : PropComm
    {
        // Razsvetljava
                
        static int LucIconsTableLength = XmlController.GetHowManyLucIcons() + 1;

        public PlcVars.Bit[] LucStatus_ReadToPC = new PlcVars.Bit[LucIconsTableLength];
        public PlcVars.Bit[] LucStatus_WriteToPLC = new PlcVars.Bit[LucIconsTableLength];
        public PlcVars.Bit UgasniVseLuci;

        public PlcVars.Word[] VklopUrnika = new PlcVars.Word[LucIconsTableLength];
        public PlcVars.Word IzklopKoJeDan;

        public PlcVars.TimeSet[] VklopConadop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.TimeSet[] VklopConapop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.TimeSet[] IzklopConadop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.TimeSet[] IzklopConapop = new PlcVars.TimeSet[LucIconsTableLength];
        public PlcVars.Word[] DimmerDop = new PlcVars.Word[LucIconsTableLength];
        public PlcVars.Word[] DimmerPop = new PlcVars.Word[LucIconsTableLength];
        public PlcVars.Word[] DimmerActual = new PlcVars.Word[LucIconsTableLength];

        public PlcVars.LogoClock LogoClock;

        public PlcVars.TimeSet Vzhod_Read;
        public PlcVars.TimeSet Zahod_Read;
        public PlcVars.TimeSet VzhodOffset_Write;
        public PlcVars.TimeSet ZahodOffset_Write;
        public PlcVars.Word DanNoc_Vrednost_An;
        public PlcVars.Bit DanNoc_Vrednost_Dig;
        public PlcVars.Word DayLightPercentOn;
        public PlcVars.Word DayLightPercentOff;

        public Prop1(Sharp7.S7Client client):base(client)
        {                        
            LogoClock = new PlcVars.LogoClock(Client, this);

            const ushort inc = 10;
            ushort vklopUrnikabuff = 210;

            IzklopKoJeDan = new PlcVars.Word(Client, this, new PlcVars.WordAddress(744), "", "", true) { SyncEvery_X_Time = 5 };

            for (int i = 1; i < LucIconsTableLength; i++)
            {
                LucStatus_ReadToPC[i] = new PlcVars.Bit(Client, this, XmlController.GetLucAddress_ReadToPC(i),"", "", false);
                LucStatus_WriteToPLC[i] = new PlcVars.Bit(Client, this, XmlController.GetLucAddress_WriteToPLC(i), "", "", true);
                     
                VklopUrnika[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(vklopUrnikabuff), "", "", true);
                vklopUrnikabuff += inc;

            }

            ushort buffCona = 16;
            ushort buffConaIzkInc = 2;
            ushort buffConaPopInc = 100;
            for (int i = 1; i < LucStatus_ReadToPC.Length; i++)
            {
                VklopConadop[i] = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress(buffCona), true) { SyncEvery_X_Time = 5 }; 
                IzklopConadop[i] = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress((ushort)(buffCona + buffConaIzkInc)), true) { SyncEvery_X_Time = 5 }; 

                VklopConapop[i] = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress((ushort)(buffCona + buffConaPopInc)), true) { SyncEvery_X_Time = 5 }; 
                IzklopConapop[i] = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress((ushort)(buffCona + buffConaPopInc + buffConaIzkInc)), true) { SyncEvery_X_Time = 5 };

                buffCona += 10;
            }

            ushort buffDimmD = 310, buffDimmP = 314, buffDimmActual = 410;
            for (int i = 1; i <= 4; i++)
            {
                DimmerDop[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(buffDimmD), "", "", true) { SyncEvery_X_Time = 5 }; 
                DimmerPop[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(buffDimmP), "", "", true) { SyncEvery_X_Time = 5 }; 
                DimmerActual[i] = new PlcVars.Word(Client, this, new PlcVars.WordAddress(buffDimmActual), "", "%", false) { SyncEvery_X_Time = 2 }; 
                buffDimmD += inc; buffDimmP += inc; buffDimmActual += inc;
            }

            UgasniVseLuci = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(700,0), "", "", true);

            Vzhod_Read = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress(712), false) { SyncEvery_X_Time = 5 };
            Zahod_Read = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress(714), false) { SyncEvery_X_Time = 5 };
            VzhodOffset_Write = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress(716), false) { SyncEvery_X_Time = 5 };
            ZahodOffset_Write = new PlcVars.TimeSet(Client, this, new PlcVars.WordAddress(718), false) { SyncEvery_X_Time = 5 };
            DanNoc_Vrednost_An = new PlcVars.Word(Client, this, new PlcVars.WordAddress(736), "", "%", false) { SyncEvery_X_Time = 5 };
            DanNoc_Vrednost_Dig = new PlcVars.Bit(Client, this, new PlcVars.BitAddress(740,0), "Dan", "Noč", false) { SyncEvery_X_Time = 5 };
            DayLightPercentOn = new PlcVars.Word(Client, this, new PlcVars.WordAddress(724), "", "%", true) { SyncEvery_X_Time = 3 };
            DayLightPercentOff = new PlcVars.Word(Client, this, new PlcVars.WordAddress(728), "", "%", true) { SyncEvery_X_Time = 3 };

        }


        //
        public bool GetIzklopiKoJeDan(int id)
        {
            try
            {
                if (id <= 0 || id > 16 || IzklopKoJeDan.Value == null)
                {
                    return false;
                }

                char[] b = getBitsFromWord((short)IzklopKoJeDan.Value);
                return (b[id - 1]) != '0' ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside GetIzklopiKoJeDan(int id) method: " + ex.Message);
            }
            
        }

        public void SetIzklopKoJeDan(int id, bool value)
        {
            try
            {
                if (id <= 0 || id > 16 || IzklopKoJeDan.Value == null)
                {
                    return;
                }

                var buff = IzklopKoJeDan.Value ?? 0;
                var bits = getBitsFromWord(buff);
                bits[id - 1] = value ? '1' : '0';
                
                IzklopKoJeDan.Value = getWordFromBits(bits); 
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside SetIzklopKoJeDan(int id, bool value) method: " + ex.Message);
            }
            


        }

        char[] getBitsFromWord(short value)
        {
            try
            {                
                var buff = Convert.ToInt16(value);
                string s = Convert.ToString(buff, 2);
                char[] b;
                b = s.ToCharArray();
                b = b.Reverse().ToArray();
                char[] c = new char[16];
                for (int i = 0; i < c.Length; i++)
                {
                    if (b.Length > i)
                    {
                        c[i] = b[i];
                    }
                    else
                    {
                        c[i] = '0';
                    }
                    
                }
                return c;//Reverse().ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside getBitsFromWord(short value) method: " + ex.Message);
            }
            
        }

        short getWordFromBits(char[] bitArray)
        {
            try
            {
                short buff = 0;
                double m;
                for (int i = 0; i < bitArray.Length; i++)
                {
                    if (bitArray[i] != '0')
                    {
                        m = Math.Pow(2, i);
                        buff += Convert.ToInt16(m);
                    }
                }
                return buff;
            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside getWordFromBits(char[] bitArray) method: " + ex.Message);
            }
            
        }

        public void GetVklopUrnika(out bool Dop, out bool Pop, int id)
        {
            //0 - Disable both;   
            //1 - Enable 1  Disable 2;   
            //2 - Enable both;   
            //3 - Disable 1  Enable 2; 

            var val = VklopUrnika[id].Value;

            if (val == 1)
            {
                Dop = true;  Pop = false;
            }

            else if (val == 2)
            {
                Dop = true;  Pop = true;
            }

            else if (val == 3)
            {
                Dop = false;  Pop = true; // 
            }

            else
            {
                Dop = false;  Pop = false;
            }
        }

        public void SetVklopUrnika(int id, bool value_Dop, bool value_Pop)
        {
            //0 - Disable both;   
            //1 - Enable 1  Disable 2;   
            //2 - Enable both;   
            //3 - Disable 1  Enable 2; 
                      
            if (value_Dop && !value_Pop)
            {
                VklopUrnika[id].Value = 1;
            }

            else if (value_Dop && value_Pop)
            {
                VklopUrnika[id].Value = 2;
            }

            else if (!value_Dop && value_Pop)
            {
                VklopUrnika[id].Value = 3;
            }

            else
            {
                VklopUrnika[id].Value = 0;
            }
                        
        }

    }
}
