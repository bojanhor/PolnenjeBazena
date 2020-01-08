using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Sharp7;
using Linq = System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Net;

namespace WebApplication1
{
    public class Connection
    {
        public enum Status : int
        {
            NotInitialised = -4,
            Error = -3,
            Warning = -2,
            Connecting = -1,
            Connected = 0,
        }

        private string _LocalTSAP;
        private string _RemoteTSAP;
        private ushort _LocalTSAP_asushort;
        private ushort _RemoteTSAP_asushort;

        public int errcodeLOGO;
        public int connectionStatusLOGO;
        public bool IsLogoConnected
        {
            get
            {
                if (connectionStatusLOGO == (int)Connection.Status.Connected)
                {
                    return true;
                }
                return false;
            }
        }

        public Connection()
        {
            connectionStatusLOGO = (int)Status.NotInitialised;
        }

        public IPAddress IpAddress { get; set; }

        public string LocalTSAP
        {
            get { return _LocalTSAP; }
            set
            {
                _LocalTSAP = value;
                var tmp = _LocalTSAP.Replace(".", "");
                tmp = tmp.Replace("\"", "");
                _LocalTSAP_asushort = Convert.ToUInt16(int.Parse(tmp, System.Globalization.NumberStyles.HexNumber));

            }

        }

        public string RemoteTSAP
        {
            get { return _RemoteTSAP; }
            set
            {
                _RemoteTSAP = value;
                var tmp = _RemoteTSAP.Replace(".", "");
                tmp = tmp.Replace("\"", "");
                _RemoteTSAP_asushort = Convert.ToUInt16(int.Parse(tmp, System.Globalization.NumberStyles.HexNumber));
            }


        }

        public ushort LocalTSAP_asushort { get { return _LocalTSAP_asushort; } }
        public ushort RemoteTSAP_asushort { get { return _RemoteTSAP_asushort; } }





        // Write to PLC
        public static void PLCwrite(S7Client Client, string typeAndAdress, short value, out int errCode)
        {
            try
            {
                int address = 0;
                byte[] b = new byte[6];  // temporary array for writing

                // In case if you want to write bit value
                if (typeAndAdress.Contains("bit at"))
                {
                    typeAndAdress = typeAndAdress.Replace("bit at", "");
                    typeAndAdress = typeAndAdress.Replace(" ", "");

                    bool boolval = false;
                    boolval = Convert.ToBoolean(value);


                    address = int.Parse(typeAndAdress.Split('.')[0]);
                    errCode = Client.DBRead(1, address, S7Consts.S7WLByte, b); // read whole byte as late as possible

                    if (errCode == S7Consts.err_OK)
                    {
                        S7.SetBitAt(ref b, 0, int.Parse(typeAndAdress.Split('.')[1]), boolval);
                        errCode = Client.DBWrite(1, address, 1, b); // write whole byte as soon as possible
                    }
                }

                // In case if you want to write DWORD value
                else if (typeAndAdress.Contains("DW "))
                {
                    typeAndAdress = typeAndAdress.Replace("DW ", "");
                    address = int.Parse(typeAndAdress);
                    S7.SetDWordAt(b, 0, (uint)value);
                    errCode = Client.DBWrite(1, address, 4, b);

                }

                // In case if you want to write Word value
                else if (typeAndAdress.Contains("W "))
                {
                    typeAndAdress = typeAndAdress.Replace("W ", "");
                    address = int.Parse(typeAndAdress);
                    S7.SetWordAt(b, 0, (ushort)value);
                    errCode = Client.DBWrite(1, address, 2, b);
                }

                else
                {
                    errCode = S7Consts.err_typeError;
                }
            }
            catch
            {
                errCode = S7Consts.err_Write;
            }





        }



        // Read from PLC
        public static short PLCread(S7Client Client, string typeAndAdress, out int errCode)
        {            
            short value = 0;
            try
            {
                int address = 0;                           
                byte[] b = new byte[6];  // temporary array for writing

                // In case if you want to read bit value
                if (typeAndAdress.Contains("bit at"))
                {
                    bool boolval = false;

                    typeAndAdress = typeAndAdress.Replace("bit at", "");
                    typeAndAdress = typeAndAdress.Replace(" ", "");

                    try
                    {
                        address = int.Parse(typeAndAdress.Split('.')[0]);
                        errCode = Client.DBRead(1, address, 1, b);                        
                        boolval = S7.GetBitAt(b, 0, int.Parse(typeAndAdress.Split('.')[1]));
                        if (boolval) { return 1; } return 0;
                    }
                    catch (Exception)
                    {
                        errCode = S7Consts.err_Read;
                    }

                }

                // In case if you want to read DWORD value
                else if (typeAndAdress.Contains("DW "))
                {
                    typeAndAdress = typeAndAdress.Replace("DW ", "");

                    try
                    {
                        address = int.Parse(typeAndAdress);
                        errCode = Client.DBRead(1, address, 4, b);
                        value = (short)Convert.ToInt32(S7.GetDWordAt(b, 0));
                    }
                    catch
                    {
                        errCode = S7Consts.err_Read;
                    }

                }
                

                // In case if you want to read WORD value
                else if (typeAndAdress.Contains("W "))
                {
                    typeAndAdress = typeAndAdress.Replace("W ", "");
                    
                    try
                    {
                        address = int.Parse(typeAndAdress);
                        errCode = Client.DBRead(1, address, 2, b);
                        value = (short)(S7.GetWordAt(b, 0));
                    }
                    catch 
                    {
                        errCode = S7Consts.err_Read;
                    }                   
                }
                else
                {
                    errCode = S7Consts.err_typeError;
                }

                return value;

            }

            catch
            {
                errCode = S7Consts.err_UnspecifiedHigherLayer;
                return value;
            }
        }
    }

}

