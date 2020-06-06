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
               
        public Prop1(Sharp7.S7Client client):base(client)
        {

            PlcVars.Bit Ustavljeno = new PlcVars.Bit(this, new PlcVars.BitAddress(1, 0), false);
            PlcVars.Bit Rocno_read = new PlcVars.Bit(this, new PlcVars.BitAddress(2, 0), false);
            PlcVars.Bit Avtomat_read = new PlcVars.Bit(this, new PlcVars.BitAddress(3, 0), false);
            PlcVars.Bit CakanjeMateriala = new PlcVars.Bit(this, new PlcVars.BitAddress(4, 0), false);
            PlcVars.Bit CakanjeUkaza = new PlcVars.Bit(this, new PlcVars.BitAddress(5, 0), false);
            PlcVars.Bit Circle = new PlcVars.Bit(this, new PlcVars.BitAddress(9, 0), true);
            PlcVars.Bit ZigZag = new PlcVars.Bit(this, new PlcVars.BitAddress(8, 0), true);
            PlcVars.Bit Start = new PlcVars.Bit(this, new PlcVars.BitAddress(10, 0), true);
            PlcVars.Bit Stop = new PlcVars.Bit(this, new PlcVars.BitAddress(12, 0), true);
            PlcVars.Bit Man_AutoReadState = new PlcVars.Bit(this, new PlcVars.BitAddress(13, 0), false);
            PlcVars.Bit Auto = new PlcVars.Bit(this, new PlcVars.BitAddress(14, 0), true);
            PlcVars.Bit ResetPulse = new PlcVars.Bit(this, new PlcVars.BitAddress(16, 0), true);

            PlcVars.Bit DirX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(20, 0), false);
            PlcVars.Bit DirX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(22, 0), false);
            PlcVars.Bit DirY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(24, 0), false);
            PlcVars.Bit DirY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(26, 0), false);
            PlcVars.Bit TrakRead = new PlcVars.Bit(this, new PlcVars.BitAddress(28, 0), false);
            PlcVars.Bit Trak_muss = new PlcVars.Bit(this, new PlcVars.BitAddress(29, 0), true);

            PlcVars.Bit SymPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(30, 0), true);
            PlcVars.Bit ReadPrisotMat = new PlcVars.Bit(this, new PlcVars.BitAddress(32, 0), false);

            PlcVars.Bit SymKSX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(40, 0), true);
            PlcVars.Bit SymKSX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(42, 0), true);
            PlcVars.Bit SymKSY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(44, 0), true);
            PlcVars.Bit SymKSY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(46, 0), true);

            PlcVars.Bit ReadKSX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(50, 0), false);
            PlcVars.Bit ReadKSX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(52, 0), false);
            PlcVars.Bit ReadKSY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(54, 0), false);
            PlcVars.Bit ReadKSY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(56, 0), false);

            PlcVars.Bit Inicializacija = new PlcVars.Bit(this, new PlcVars.BitAddress(60, 0), false);
            PlcVars.Bit Korak1 = new PlcVars.Bit(this, new PlcVars.BitAddress(60, 0), false);
            PlcVars.Bit Korak2 = new PlcVars.Bit(this, new PlcVars.BitAddress(61, 0), false);
            PlcVars.Bit Korak3 = new PlcVars.Bit(this, new PlcVars.BitAddress(62, 0), false);
            PlcVars.Bit Korak4 = new PlcVars.Bit(this, new PlcVars.BitAddress(63, 0), false);

            PlcVars.Bit InitCircle = new PlcVars.Bit(this, new PlcVars.BitAddress(70, 0), false);
            PlcVars.Bit KorakCircle1 = new PlcVars.Bit(this, new PlcVars.BitAddress(71, 0), false);
            PlcVars.Bit KorakCircle2 = new PlcVars.Bit(this, new PlcVars.BitAddress(72, 0), false);
            PlcVars.Bit KorakCircle3 = new PlcVars.Bit(this, new PlcVars.BitAddress(73, 0), false);
            PlcVars.Bit KorakCircle4 = new PlcVars.Bit(this, new PlcVars.BitAddress(74, 0), false);

            PlcVars.Bit JoyStickCommandX1 = new PlcVars.Bit(this, new PlcVars.BitAddress(90, 0), true);
            PlcVars.Bit JoyStickCommandX2 = new PlcVars.Bit(this, new PlcVars.BitAddress(91, 0), true);
            PlcVars.Bit JoyStickCommandY1 = new PlcVars.Bit(this, new PlcVars.BitAddress(92, 0), true);
            PlcVars.Bit JoyStickCommandY2 = new PlcVars.Bit(this, new PlcVars.BitAddress(93, 0), true);

            PlcVars.Word SpeedX = new PlcVars.Word(this, new PlcVars.WordAddress(100), false);
            PlcVars.Word SpeedY = new PlcVars.Word(this, new PlcVars.WordAddress(104), false);
            PlcVars.Word Trak_speedSet = new PlcVars.Word(this, new PlcVars.WordAddress(106), true);
            PlcVars.Word Trak_speedRead = new PlcVars.Word(this, new PlcVars.WordAddress(108), false);
            PlcVars.Word TimeZigY = new PlcVars.Word(this, new PlcVars.WordAddress(110), true);

            PlcVars.Bit SemaforZe = new PlcVars.Bit(this, new PlcVars.BitAddress(90, 0), false);
            PlcVars.Bit SemaforRd = new PlcVars.Bit(this, new PlcVars.BitAddress(91, 0), false);
            PlcVars.Bit SemaforYe = new PlcVars.Bit(this, new PlcVars.BitAddress(92, 0), false);

        }

    }
}
