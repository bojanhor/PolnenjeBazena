﻿using System;
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

        public class ByteAddress : PlcAddress
        {
            public ByteAddress(int address)
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

        // ////////////////////////////////////////////////////////

        public abstract class PlcType
        {            
            public Sharp7.S7Client Client;

            private ushort _syncEvery_X_Time = 1;
            public ushort SyncEvery_X_Time // set to 1 for syncing to occur every loop -- 0 = turn sync off -- 2 = every 2nd loop...
            {
                get { return _syncEvery_X_Time;  }
                set { chk_SyncEvery_X_Time_Val(value); }
            }

            public ushort skipNextLoop = 1; // alghoritem syncs variable only if this value is 1;
          
            public PlcType(PropComm this_prop)
            {
                // adding base types of all variables to list "sorted" by prop to efficiently call sync procedure
                this_prop.AutoSync.Add(this);                
                Client = this_prop.Client;
            }
                        
            public abstract void SyncWithPLC();            

            void chk_SyncEvery_X_Time_Val(ushort val)
            {
                // limit
                if (val >= 0)
                {
                    if (val <= 5)
                    {
                        _syncEvery_X_Time = val;                        
                    }
                    else
                    {
                        _syncEvery_X_Time = 5;
                    }

                }
                else
                {
                    _syncEvery_X_Time = 1;
                }               

            }
        }
        
        
        public class Word : PlcType
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
                        ReadFromPCtoBuffer(value);
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
                        ReadFromPCtoBuffer(value);
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
            int ErrRead;
            int ErrWrite;
            short? buffRead;
            short? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;

            public Word(PropComm prop, WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, prefixToShow, postFixToShow, IsWritable);
            }

            public Word(PropComm prop, WordAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, "", "", IsWritable);
            }

            void Ctor(WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
            {
                PLCval = null;
                PCval = null;


                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;
            }

            public override void SyncWithPLC()
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
                    if (Client != null)
                    {
                        buffRead = Connection.PLCread(Client, _TypeAndAdress, out ErrRead);
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

                            if (Client != null)
                            {
                                if (_IsWritable)
                                {                                    
                                    Connection.PLCwrite(Client, _TypeAndAdress, (short)buffWrite, out ErrWrite);
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

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }
                                 
            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {               
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = Client.ErrorText(ErrRead);
                string ErrTyp_Write = Client.ErrorText(ErrWrite);
                string ClientName = "Logo" + Client.deviceID;
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

        public class Byte : PlcType
        {
            public byte? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(value);
                    }
                }
            }

            public byte? Value_short
            {
                get
                {
                    if (PLCval != null)
                    {
                        return (byte)PLCval;
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
                        ReadFromPCtoBuffer(value);
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

            private byte? PLCval;
            private byte? PCval;
            private bool directionToPLC = false;
            private ByteAddress _TypeAndAdress;
            int ErrRead;
            int ErrWrite;
            byte? buffRead;
            byte? buffWrite;
            string _prefixToShow;
            string _postFixToShow;
            bool _IsWritable = false;

            public Byte(PropComm prop, ByteAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, prefixToShow, postFixToShow, IsWritable);
            }

            public Byte(PropComm prop, ByteAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                Ctor(TypeAndAdress, "", "", IsWritable);
            }

            void Ctor(ByteAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable)
            {
                PLCval = null;
                PCval = null;


                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;
            }

            public override void SyncWithPLC()
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
                    if (Client != null)
                    {
                        buffRead = (byte)Connection.PLCread(Client, _TypeAndAdress, out ErrRead);
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

                            if (Client != null)
                            {
                                if (_IsWritable)
                                {
                                    Connection.PLCwrite(Client, _TypeAndAdress, (byte)buffWrite, out ErrWrite);
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

            private void ReadFromPCtoBuffer(byte? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
                }
            }

            public void ReportError_throwException(string Message)
            {
                ReportError_throwException(Message, null, null);
            }
            public void ReportError_throwException(string Message, bool? forceSet_FlagToReport, bool? forceRead_FlagToReport)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = Client.ErrorText(ErrRead);
                string ErrTyp_Write = Client.ErrorText(ErrWrite);
                string ClientName = "Logo" + Client.deviceID;
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

        public class DWord : PlcType
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
                        ReadFromPCtoBuffer(value);
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
                        ReadFromPCtoBuffer(value);
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
           
            public DWord(Sharp7.S7Client Client, PropComm prop, DoubleWordAddress TypeAndAdress, string prefixToShow, string postFixToShow, bool IsWritable) : base(prop)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _prefixToShow = prefixToShow;
                _postFixToShow = postFixToShow;
                _IsWritable = IsWritable;
            }

            public override void SyncWithPLC()
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

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
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

        public class TimeSet : PlcType
        {            
            public string Value
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
                                ReadFromPCtoBuffer(buff);
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
            
            public TimeSet(PropComm prop, WordAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;
                _IsWritable = IsWritable;
            }

            public override void SyncWithPLC()
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

            private void ReadFromPCtoBuffer(short? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
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

        public class TemperatureShow : PlcType
        {
            public float? Value
            {
                get
                {
                    if (PLCval != null)
                    {
                        return Scalate(PLCval);
                    }
                    return 0;
                }
                set
                {
                    if (value != null)
                    {
                        ReadFromPCtoBuffer(DeScalate(value));
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
                        float val = (float)Value;
                        return _prefixToShow + val + _postFixToShow;
                    }
                    return PropComm.NA;
                }
            }

            public string Value_string_formatted
            {
                get
                {
                    var buff = Value;
                    if (buff != null)
                    {
                        float val = (float)Value;
                        return _prefixToShow + val.ToString("0.0") + _postFixToShow;
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
            public float _kx;
            public float _n;
            bool _isWritable;
            private int decimalPlaces;
            public int DecimalPlaces
            {
                get { return decimalPlaces; }
                set
                {
                    if (value < 0)
                    {
                        decimalPlaces = 0;
                    }
                    if (value > 5)
                    {
                        decimalPlaces = 5;
                    }
                    decimalPlaces = value;
                }
            }

            public TemperatureShow(PropComm prop, WordAddress TypeAndAdress, string prefixToShow, string postFixToShow, float calibOffset, float calibMultiply, int decimals, bool IsWritable) : base(prop)
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

            public override void SyncWithPLC()
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
                               
            public virtual float? Scalate(short? val)
            {
                if (val != null)
                {
                    var value = (float)val;
                    return (_kx * value + _n);
                }
                return 0;
            }
                        
            private short? DeScalate(float? val)
            {  
                if (val != null)
                {
                    return Misc.ToShort(((float)val - _n) / _kx);
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
                       
        public class Bit : PlcType
        {
            public bool? Value
            {
                get
                {
                    return PLCval;
                }
                set
                {
                    ReadFromPCtoBuffer(value);
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
                    ReadFromPCtoBuffer(value);
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
            bool _IsWritable = false;           
            byte sendpulseState = 0;

            public Bit(PropComm prop, BitAddress TypeAndAdress, bool IsWritable) : base(prop)
            {
                PLCval = null;
                PCval = null;

                _Client = Client;
                _TypeAndAdress = TypeAndAdress;                
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

            public override void SyncWithPLC()
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

            private void ReadFromPCtoBuffer(bool? value)
            {
                if (value != null)
                {
                    directionToPLC = true;
                    PCval = value;
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
        
        public class LogoClock : PlcType
        {            
            public string Value
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

            private short? PLCval;                   
            private WordAddress _TypeAndAdress;
            private Sharp7.S7Client _Client;
            int ErrRead;           
            short? buffRead;           
            
            public LogoClock(PropComm prop) : base(prop)
            {
                PLCval = null;               
                _Client = Client;
                _TypeAndAdress = new WordAddress(988);
                base.SyncEvery_X_Time = 5;
            }

            public override void SyncWithPLC()
            {
                try
                {
                    ReadFromPLCtoBuffer();                   
                }
                catch (Exception ex)
                {
                    ReportComunicatoonMessage(ex.Message);
                }
            }

            private void ReadFromPLCtoBuffer()
            {
                if (_Client != null)
                {
                    buffRead = Connection.PLCread(_Client, _TypeAndAdress, out ErrRead);
                    if (ErrRead == 0 && buffRead != null) { PLCval = buffRead; buffRead = null; }
                }
                else
                {
                    ReportError_throwException("Reading Clock / Time from PLC failed.");
                }
            }                      
                       
            public void ReportError_throwException(string Message)
            {
                string Address = _TypeAndAdress.GetStringRepresentation();
                string ErrTyp_Read = _Client.ErrorText(ErrRead);               
                string Client = "Logo" + _Client.deviceID;
                
                throw new Exception(
                    Message + " " +
                    "Address: " + Address + ", " +
                    "Read Error type: " + ErrTyp_Read + ", " +
                    "Client: " + Client + ". " );
                    
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

    }
}
