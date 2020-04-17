using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace WebApplication1
{
    public class PlcVars
    {
        public static void ReportComunicatoonMessage(string message)
        {
            SysLog.SetMessage(message);
        }

        public class PlcAddress
        {
            public int Address;

            public int GetAddress()
            {
                return Address;
            }                       
        }

        public class BitAddress : PlcAddress
        {
            public ushort SubAddress;

            public BitAddress(int address, ushort subAddress)
            {
                SubAddress = subAddress;
                Address = address;
            }
            public int GetSubAddress()
            {
                return SubAddress;
            }

            public string GetStringRepresentation()
            {
                return "bit at " + GetAddress() + GetSubAddress();
            }

        }

        public class WordAddress : PlcAddress
        {
            public WordAddress(int address)
            {
                Address = address;
            }

            public string GetStringRepresentation()
            {
                return "VW" + GetAddress();
            }
        }

        public class DoubleWordAddress : PlcAddress
        {
            public DoubleWordAddress(int address)
            {
                Address = address;
            }

            public string GetStringRepresentation()
            {
                return "DW" + GetAddress();
            }

        }
        
        public class Word
        {
            public short? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value, true);
                    }
                }
            }

            public short? Value_short
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
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value, true);
                    }
                }
            }

            public string Value_string
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        return _prefixToShow + Value.ToString() + _postFixToShow;
                    }
                    return PropComm.NA;                    
                }                
            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private WordAddress _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public Word(Sharp7.S7Client Client, WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
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
                try
                {                    
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicatoonMessage(ex.Message);
                }
                
            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null)
                        {
                            PLCval = buffRead; buffRead = null;
                        }
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
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
                                    else
                                    {
                                        ReportError_throwException( "Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
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
                catch (Exception ex)
                {
                    ReportError_throwException("Write To PC failed." + ex.Message);
                }

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
                catch(Exception ex)
                {
                    ReportError_throwException("Read from PC failed." + ex.Message);
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

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {               
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }            
        }

        public class DWord
        {
            public short? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value, true);
                    }
                }
            }

            public short? Value_short
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
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value, true);
                    }
                }
            }

            public string Value_string
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        return _prefixToShow + Value.ToString() + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private DoubleWordAddress _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public DWord(Sharp7.S7Client Client, DoubleWordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
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
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicatoonMessage(ex.Message);
                }

            }

            private void ReadFromPLCtoBuffer(bool forceRead)
            {
                if (directionToPLC == false || forceRead)
                {
                    if (_Client != null)
                    {
                        buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                        if (ErrRead == 0 && buffRead != null)
                        {
                            PLCval = buffRead; buffRead = null;
                        }
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
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
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
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
                catch (Exception ex)
                {
                    ReportError_throwException("Write To PC failed." + ex.Message);
                }

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
                catch (Exception ex)
                {
                    ReportError_throwException("Read from PC failed." + ex.Message);
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

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
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
                set
                {
                    if (value != null)
                    {
                        if (value != PropComm.NA)
                        {
                            try
                            {                                
                                var buff = EncodeWordToTime(value);
                                ReadFromPCtoBuffer(buff, true);
                            }
                            catch (Exception ex)
                            {
                                ReportError_throwException("Error setting value for weektimer." + ex.Message);
                            }                            
                        }
                        
                    }
                }
                


            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private WordAddress _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            bool _IsWritable = false;
            bool datacorrupted = false;

            public TimeSet(Sharp7.S7Client Client, WordAddress TypeAndAdress, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
            }

            public void SyncWithPLC()
            {

                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicatoonMessage(ex.Message);
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
                    else
                    {
                        ReportError_throwException("Read from PLC failed.", null, forceRead);
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
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
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
                catch (Exception ex)
                {
                    ReportError_throwException("Write To PC failed." + ex.Message);
                }

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
                catch (Exception ex)
                {
                    ReportError_throwException("Read from PC failed." + ex.Message);
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

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
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
                       
        public class Bit
        {
            public bool? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    ReadFromPCtoBuffer(value, true);
                }
            }

            public bool Value_bool
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
            private BitAddress _TypeAndAdress;
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

            public Bit(Sharp7.S7Client Client, BitAddress TypeAndAdress, string replacementTextIfTrue, string replacementTextIfFalse, bool IsWritable)
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

                    if (!Value_bool)
                    {
                        sendpulseState = 0;                        
                    }
                }
            }

            public void SyncWithPLC()
            {
                try
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
                catch (Exception ex)
                {
                    ReportComunicatoonMessage(ex.Message);
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
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
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
                            if ((bool)PCval)
                            { buffWrite = 1; }
                            else { buffWrite = 0; }

                            if (_Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(_Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
                                    if (ErrWrite == 0)
                                    {
                                        PLCval = Convert.ToBoolean(buffWrite);
                                    }
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
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
                catch (Exception ex)
                {
                    ReportError_throwException("Write To PC failed." + ex.Message);
                }

                try
                {
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

                                throw new Exception("Data parsed is not valid boolean!");
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ReportError_throwException("Read from PC failed." + ex.Message);
                }

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

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }

                Flags += " isWritable: " + _IsWritable.ToString() + ";";


                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }

        }        
        
        public class TemperatureShow
        {

            public string Value
            {
                get
                {
                    return _prefixToShow + Scalate(PLCval) + _postFixToShow;
                }
            }

            private short? PLCval;
            private short? PCval;
            private bool directionToPLC = false;
            private WordAddress _TypeAndAdress;
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
            private int decimalPlaces;
            public int DecimalPlaces
            {
                get { return decimalPlaces; }
                set {
                    if (value <0)
                    {
                        decimalPlaces = 0;
                    }
                    if (value>5)
                    {
                        decimalPlaces = 5;
                    }
                    decimalPlaces = value;
                }
            }



            public TemperatureShow(Sharp7.S7Client Client, WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, float calibOffset, float calibMultiply, int decimals, bool IsWritable)
            {
                PLCval = null;
                PCval = null;

                decimalPlaces = decimals;
                
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
                try
                {
                    ReadFromPLCtoBuffer(false);
                    WriteToPLCFromBuffer();
                }
                catch (Exception ex)
                {
                    ReportComunicatoonMessage(ex.Message);
                }
            }


            public void SyncWithPC(string value)
            {
                try
                {
                    WriteToPCFromBuffer(value);
                }
                catch (Exception ex)
                {
                    ReportError_throwException("Write To PC failed." + ex.Message);
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
                catch (Exception ex)
                {
                    ReportError_throwException("Read from PC failed." + ex.Message);
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
                        else
                        {
                            ReportError_throwException("Read from PLC failed.", null, forceRead);
                        }
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
                                    else
                                    {
                                        ReportError_throwException("Write to PLC failed.");
                                    }
                                }
                                else
                                {
                                    ReportError_throwException("Write to PLC failed IsWritable flag must be true for writing to PLC.");
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
                    return (_kx * (float)val + _n).ToString("F" + decimalPlaces);
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

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);
                string ErrTyp_Write = _Client.ErrorText(ErrWrite);
                string Client = "Logo" + _Client.deviceID;
                string Flags;

                Flags = "directionToPLC: " + directionToPLC;

                if (forceSet_FlagToReport != null)
                {
                    Flags += " forceSet: " + forceSet_FlagToReport.ToString() + ";";
                }

                if (forceRead_FlagToReport != null)
                {
                    Flags += " forceRead: " + forceSet_FlagToReport.ToString() + ";";
                }
                
                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Write Error type: " + ErrTyp_Write + ", " +
                    "Client: " + Client + ". " +
                    "Flags: " + Flags);
            }

        }

        // ////////////////////////////////////////////////////////

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
