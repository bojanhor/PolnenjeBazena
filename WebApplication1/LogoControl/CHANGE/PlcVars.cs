using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace WebApplication1
{
    public class PlcVars
    {
        public class Word
        {
            public short Value
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (short)PLCval;
                    }
                    else
                    {
                        return 0;
                    }
                }
                set
                {
                    ReadFromPCtoBuffer(value, true);
                }
            }

            public string Value_string
            {
                get
                {
                    return _prefixToShow + Value.ToString() + _postFixToShow;
                }                
            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private string _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public Word(Sharp7.S7Client Client, string TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;
            }

            public void SyncWithPLC()
            {
                ReadFromPLCtoBuffer(false);
                WriteToPLCFromBuffer();
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) { PLCval = buffRead; buffRead = null; }
                    }
                }
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite; 
                                    }
                                }
                            }                            
                        }
                        directionToPLC = false;
                    }
                }
            }

            public void SyncWithPC(string value)
            {
                try
                {
                    WriteToPCFromBuffer(value);
                }
                catch { }

                try
                {
                    if (value != PropComm.NA)
                    {
                        if (value != null)
                        {
                            ReadFromPCtoBuffer(Convert.ToInt16(
                                RemoveFromBegining(
                                    RemoveFromEnd(
                                        value, _postFixToShow), _prefixToShow)), false);
                        }
                    }
                }
                catch
                {

                }

            }

            private void ReadFromPCtoBuffer(short? value, bool forceSet)
            {
                if (value != null)
                {
                    if (value != PCval || forceSet)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }


            private void WriteToPCFromBuffer(string value)
            {
                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted == true)
                        {
                            value = (_prefixToShow + PLCval + _postFixToShow).ToString();
                            datacorrupted = false;
                        }
                        if (value != (_prefixToShow + PLCval + _postFixToShow).ToString())
                        {
                            datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                        }
                    }
                }
            }
        }


        public class TimeSet
        {
            public string Value_ClockForSiemensLogoFormat
            {
                get
                {
                    if (PLCval != null)
                    {
                        if (PLCval != null)
                        {
                            return DecodeWordToTime((short)PLCval, 2).ToString();
                        }                      
                    }
                    return PropComm.NA;
                   
                }
                
            }

            public string Value_WeektimerForSiemensLogoFormat
            {
                get
                {
                    if (PLCval != null)
                    {
                        if (PLCval != null)
                        {
                            return DecodeWordToTime((short)PLCval, 1).ToString();
                        }
                    }
                    return PropComm.NA;

                }

            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private string _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public TimeSet(Sharp7.S7Client Client, string TypeAndAdress, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
            }

            public void SyncWithPLC()
            {
                ReadFromPLCtoBuffer(false);
                WriteToPLCFromBuffer();
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) { PLCval = buffRead; buffRead = null; }
                    }
                }
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                }
                            }                            
                        }                        
                        directionToPLC = false;
                    }
                }
            }

            /// <summary>
            /// method = 1 - Siemens Logo week timer
            /// method = 2 - Used to get clock from Siemens Logo
            /// </summary>
            /// <param name="value"></param>
            /// <param name="method"></param>
            public void SyncWithPC(string value, int method)
            {
                // method = 1 - Siemens Logo week timer
                // method = 2 - Used to get clock from Siemens Logo

                try
                {
                    WriteToPCFromBuffer(value,  method);
                }
                catch { }

                try
                {
                    if (value != PropComm.NA)
                    {
                        if (value != null)
                        {
                            ReadFromPCtoBuffer(EncodeWordToTime(value), false);
                        }
                    }
                }
                catch
                {

                }

            }

            private void ReadFromPCtoBuffer(short? value, bool forceSet)
            {
                if (value != null)
                {
                    if (value != PCval || forceSet)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }


            private void WriteToPCFromBuffer(string value, int method)
            {
                // method = 1 - Siemens Logo week timer
                // method = 2 - Used to get clock from Siemens Logo

                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted == true)
                        {
                            value = DecodeWordToTime((short)PLCval, method).ToString();
                            datacorrupted = false;
                        }
                        if (value != DecodeWordToTime((short)PLCval, method).ToString())
                        {                            
                            datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                        }
                    }
                }
            }
        }


        public static string DecodeWordToTime(short word, int method) //
        {
            if (method == 1) // Siemens logo week timer
            {
                //  1234 -> ##:##           
                var d1 = word / 4069;
                var leftover = word % 4069;
                if (d1 > 2)
                { return PropComm.NA; }
                var d2 = leftover / 256;
                leftover = word % 256;
                if (d2 > 9)
                { return PropComm.NA; }
                var d3 = leftover / 16;
                leftover = word % 16;
                if (d3 > 5)
                { return PropComm.NA; }
                var d4 = leftover;
                if (d4 > 9)
                { return PropComm.NA; }
                return ((d1.ToString() + d2.ToString()).ToString() + ":" + (d3.ToString() + d4.ToString()).ToString());
            }
            if (method == 2) // used to get clock from siemens logo
            {
                //  1234 -> ##:##           
                var d1 = word / 256;
                var leftover = word % 256;     
                return d1.ToString("00") + ":" + leftover.ToString("00");
            }

            return "ERR";
            
        }


        public static short? EncodeWordToTime(string val)
        {
            var s = val.ToCharArray();
            if (s.Length != 5)
            {
                return null;
            }

            short con;
            try
            {
                con = Convert.ToInt16((short.Parse(s[0].ToString()) * 4096) + (short.Parse(s[1].ToString()) * 256) + (short.Parse(s[3].ToString()) * 16) + (short.Parse(s[4].ToString())));
            }
            catch
            {
                return null;
            }

            return con;


        }


        public class WordForCheckBox
        {
            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private string _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public WordForCheckBox(Sharp7.S7Client Client, string TypeAndAdress, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
            }

            public void SyncWithPLC()
            {
                ReadFromPLCtoBuffer(false);
                WriteToPLCFromBuffer();
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) { if (buffRead > 0) { PLCval = 1; } else { PLCval = 0; } PLCval = buffRead; buffRead = null; }
                    }
                }
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            if (PCval > 0)
                            {
                                buffWrite = 1;
                            }
                            else
                            {
                                buffWrite = 0;
                            }


                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }
                                }
                            }                            
                        }
                        directionToPLC = false;
                    }
                }
            }

            public void SyncWithPC(bool? value)
            {
                try
                {
                    WriteToPCFromBuffer(value);
                }
                catch { }

                try
                {
                    if (value != null)
                    {
                        if (value != null)
                        {
                            ReadFromPCtoBuffer(Convert.ToInt16(value));
                        }
                    }
                }
                catch
                {

                }

            }

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    if (value != PCval)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }


            private void WriteToPCFromBuffer(bool? value)
            {
                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted == true)
                        {
                            if (PLCval > 0)
                            {
                                value = true;
                            }
                            else
                            {
                                value = false;
                            }

                            datacorrupted = false;
                        }
                        if (((bool)value == false && PLCval > 0) || ((bool)value == true && PLCval == 0))
                        {
                            datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                        }
                    }
                }
            }

            public void ToggleValue()
            {
                if (PLCval != null)
                {
                    if (PLCval > 0)
                    {
                        SyncWithPC(false);
                    }
                    else
                    {
                        SyncWithPC(true);
                    }
                }
            }
        }


        public class Bit
        {
            public bool Value
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (bool)PLCval;
                    }
                    else
                    {
                        return false;
                    }
                }
                set
                {
                    ReadFromPCtoBuffer(value, true);
                }
            }

            private bool? PLCval;
            private bool? PCval;
            private bool directionToPLC = false;
            private string _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _replacementTextIfTrue;
            string _replacementTextIfFalse;
            bool _IsWritable = false;
            bool datacorrupted = false;
            byte sendpulseState = 0;

            public Bit(Sharp7.S7Client Client, string TypeAndAdress, string replacementTextIfTrue, string replacementTextIfFalse, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _replacementTextIfTrue = replacementTextIfTrue;
                _replacementTextIfFalse = replacementTextIfFalse;
                _IsWritable = IsWritable;
            }

            public void SendPulse()
            {                
                sendpulseState = 1;
                Value = true;               
            }            

            private void stopSendPulse()
            {
                if (sendpulseState == 2)
                {
                    directionToPLC = false;
                    ReadFromPLCtoBuffer(true);

                    if (!Value)
                    {
                        sendpulseState = 0;                        
                    }
                }
            }

            public void SyncWithPLC()
            {
                
                if (sendpulseState == 1)
                {
                    WriteToPLCFromBuffer(true);
                    Value = false;
                    sendpulseState = 2;
                }
                else if (sendpulseState == 2)
                {
                    WriteToPLCFromBuffer(true);
                    stopSendPulse();
                }
                else
                {                    
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer(false);
                }                
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {                
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) { if (buffRead > 0) { PLCval = true; } else { PLCval = false; } buffRead = null; }
                    }
                }
            }

            private void WriteToPLCFromBuffer(bool forceWrite)
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval || forceWrite)
                        {
                            if ((bool)PCval) { buffWrite = 1; } else { buffWrite = 0; }

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = Convert.ToBoolean(buffWrite);
                                    }
                                }
                            }
                        }
                        directionToPLC = false;
                    }
                }
            }

            public void SyncWithPC(string value)
            {
                if (sendpulseState > 0) { return; }
                try
                {
                    WriteToPCFromBuffer(value);
                }
                catch { }

                try
                {

                    //if (value is DatagridTypes.CheckBox)
                    //{
                    //    if (value != null)
                    //    {
                    //        ReadFromPCtoBuffer(Convert.ToBoolean(value), false);
                    //        return;
                    //    }
                    //}
                    //else
                    {
                        if (value != PropComm.NA)
                        {

                            if (value != null)
                            {
                                if (value == _replacementTextIfTrue)
                                {
                                    ReadFromPCtoBuffer(true, false);
                                    return;
                                }
                                if (value == _replacementTextIfFalse)
                                {
                                    ReadFromPCtoBuffer(false, false);
                                    return;
                                }

                                throw new Exception("Data parsed from datagridview is not valid boolean!");
                            }
                        }
                    }

                }
                catch { }

            }

            public void SyncWithPC(bool value)
            {
                if (sendpulseState > 0) { return; }
                try
                {
                    WriteToPCFromBuffer(value);
                }
                catch { }

                try
                {
                    ReadFromPCtoBuffer(value, false);
                }
                catch { }

            }

            private void ReadFromPCtoBuffer(bool? value, bool forceSet)
            {
                if (value != null)
                {
                    if (value != PCval || forceSet)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }

            private void WriteToPCFromBuffer(string value)
            {
                if (sendpulseState > 0) { return; }

                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted)
                        {

                            if (PLCval == true)
                            {
                                value = _replacementTextIfTrue;
                                datacorrupted = false;
                                if (value != _replacementTextIfTrue)
                                {
                                    datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                                }
                            }
                            else
                            {
                                value = _replacementTextIfFalse;
                                datacorrupted = false;
                                if (value != _replacementTextIfFalse)
                                {
                                    datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                                }
                            }
                        }
                    }
                }

            }

            private void WriteToPCFromBuffer(bool value)
            {
                if (sendpulseState > 0) { return; }

                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted)
                        {

                            if (PLCval == true)
                            {
                                if (value != true)
                                {
                                    datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                                }
                                else
                                {
                                    datacorrupted = false;
                                }
                            }
                            else
                            {
                                if (value != false)
                                {
                                    datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                                }
                                else
                                {
                                    datacorrupted = false;
                                }
                            }
                        }
                    }
                }

            }

        }

        

        public class BitForCheckBox
        {
            public bool Value
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (bool)PLCval;
                    }
                    else
                    {
                        return false;
                    }
                }
                set
                {
                    ReadFromPCtoBuffer(value, true);
                }
            }

            private bool? PLCval;
            private bool? PCval;
            private bool directionToPLC = false;
            private string _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public BitForCheckBox(Sharp7.S7Client Client, string TypeAndAdress, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
            }

            public void SyncWithPLC()
            {
                ReadFromPLCtoBuffer(false);
                WriteToPLCFromBuffer();
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) { if (buffRead > 0) { PLCval = true; } else { PLCval = false; } buffRead = null; }
                    }
                }

            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            if ((bool)PCval) { buffWrite = 1; } else { buffWrite = 0; }

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = Convert.ToBoolean(buffWrite);
                                    }
                                }
                            }                            
                        }
                        directionToPLC = false;
                    }
                }
            }

            public void SyncWithPC(bool? value)
            {
                try
                {
                    if (value != null)
                    {
                        WriteToPCFromBuffer((bool)value);
                    }                        

                }
                catch { }

                try
                {

                    if (value != null)
                    {
                        ReadFromPCtoBuffer(Convert.ToBoolean(value), false);
                        return;
                    }
                }
                catch { }

            }

            private void ReadFromPCtoBuffer(bool? value, bool forceSet)
            {
                if (value != null)
                {
                    if (value != PCval || forceSet)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }

            private void WriteToPCFromBuffer(bool value)
            {
                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted)
                        {
                            if (PLCval == true)
                            {
                                value = true;
                                datacorrupted = false;
                                if (Convert.ToBoolean(value) != true)
                                {
                                    datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                                }
                            }
                            else
                            {
                                value = false;
                                datacorrupted = false;
                                if (Convert.ToBoolean(value) != false)
                                {
                                    datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop
                                }
                            }
                        }
                    }
                }
            }

        }

        public class PowerShow : TemperatureShow
        {
            public PowerShow(Sharp7.S7Client Client, string TypeAndAdress, string prefixToShow, string postFixToShow, float calibOffset, float calibMultiply, bool IsWritable)
                : base(Client, TypeAndAdress, prefixToShow, postFixToShow, calibOffset, calibMultiply, IsWritable)
            {

            }

            public override string Scalate(short? val)
            {
                if (val != null)
                {
                    return (_kx * (float)val + _n).ToString("F0");
                }
                else
                {
                    return PropComm.NA;
                }

            }
        }

        public class PhShow : TemperatureShow
        {

            public PhShow(Sharp7.S7Client Client, string TypeAndAdress, string prefixToShow, string postFixToShow, float calibOffset, float calibMultiply, bool IsWritable)
                : base(Client, TypeAndAdress, prefixToShow, postFixToShow, calibOffset, calibMultiply, IsWritable)
            {

            }

            public override string Scalate(short? val)
            {
                if (val != null)
                {
                    return (_kx * (float)val + _n).ToString("F1");
                }
                else
                {
                    return PropComm.NA;
                }

            }
        }


        public class TemperatureShow
        {

            public string Value
            {
                get
                {
                    return Scalate(PLCval);
                }
            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private string _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            bool datacorrupted = false;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            public float _kx;
            public float _n;
            bool _isWritable;


            public TemperatureShow(Sharp7.S7Client Client, string TypeAndAdress, string prefixToShow, string postFixToShow, float calibOffset, float calibMultiply, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _n = calibOffset;
                _kx = calibMultiply;
                _isWritable = IsWritable;

            }

            public void SyncWithPLC()
            {
                ReadFromPLCtoBuffer(false);
                WriteToPLCFromBuffer();
            }


            public void SyncWithPC(string value)
            {
                try
                {
                    WriteToPCFromBuffer(value);
                }
                catch
                {

                }

                try
                {
                    if (value != PropComm.NA)
                    {
                        if (value != null)
                        {
                            var a = DeScalate(value);
                            ReadFromPCtoBuffer(a);
                        }
                    }
                }
                catch
                {

                }
            }

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    if (value != PCval)
                    {
                        directionToPLC = true;
                        PCval = value;
                    }
                }
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null) { PLCval = buffRead; buffRead = null; }
                    }
                }
            }

            private void WriteToPLCFromBuffer()
            {
                if (PLCval == null)
                {
                    directionToPLC = false;
                }
                if (directionToPLC == true)
                {
                    if (PCval != null && PLCval != null)
                    {
                        if (PCval != PLCval)
                        {
                            buffWrite = PCval;

                            if (_Client != null)
                            {
                                if (_isWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = buffWrite;
                                    }                                    
                                }
                            }                            
                        }
                        directionToPLC = false;
                    }                   
                }
            }

            private void WriteToPCFromBuffer(string value)
            {
                if (directionToPLC == false)
                {
                    if (PLCval != null)
                    {
                        if (PLCval != PCval || datacorrupted == true)
                        {
                            value = (_prefixToShow + Scalate(PLCval) + _postFixToShow).ToString();
                            datacorrupted = false;
                        }

                        if (value != (_prefixToShow + Scalate(PLCval) + _postFixToShow).ToString())
                        {
                            datacorrupted = true; // if displayed data is not as it should be they are overwriten next loop                                                            
                        }
                    }
                }
            }

            public virtual string Scalate(short? val)
            {
                if (val != null)
                {
                    return (_kx * (float)val + _n).ToString("F1");
                }
                else
                {
                    return PropComm.NA;
                }

            }

            private short? DeScalate(string val)
            {
                string b;
                double c;
                short d;

                if (val != null && val != "")
                {
                    b = RemoveFromEnd(val, _postFixToShow);
                    b = RemoveFromBegining(b, _prefixToShow);
                    c = Convert.ToDouble(b);
                    d = (short)Misc.ToInt((c - _n) / _kx);

                    return d;
                }
                return null;
            }

        }


        public static string RemoveFromEnd(string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                var a = s.Substring(0, s.Length - suffix.Length);
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }

        public static string RemoveFromBegining(string s, string prefix)
        {
            if (s.StartsWith(prefix))
            {
                return s.Substring(prefix.Length, s.Length - prefix.Length);
            }
            else
            {
                return s;
            }
        }

    }
}
