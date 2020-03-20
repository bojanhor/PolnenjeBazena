using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Threading;

namespace WebApplication1.ChartValues
{   
    public class ChartValuesLogger
    {
        char delimeter_ = ';'; // delimeter


        ChartData ChartData1;
        public static string DirectoryPath = XmlController.BaseDirectoryPath + "\\" + "ChartValues";
        static readonly string FileName = "ChartValues.csv";
        public static string CsvFile = DirectoryPath + "\\" + FileName;

        public ChartValuesLogger()
        {
            Misc.SmartThread ValueLogger = new Misc.SmartThread(() => m());
            ValueLogger.Start("ChartValLogger", ApartmentState.MTA, true);

        }

        void m()
        {
            ChartData1 = new ChartData();
            Thread.Sleep(100); // frees up resources for webserver to load   
            
            manageFile(); // finds csv file or creates it
            waitForInitialisation(); // waits for program to initialize itself if necessarry
            LimitPeriod();            
            TimerCallback tc = new TimerCallback(timerDoStuf);
            Timer t = new Timer(tc, null,0, Settings.ChartUpdateRefreshRate);
            
            
        }
        
        public ChartData GetDataForChart()
        {

            try
            {
                var buff = ChartData1.lines;
                for (int i = 0; i < buff.Length; i++)
                {
                    ChartData1.datetimes[i] = buff[i].Split(delimeter_)[0]; // 1st column - datetime
                    ChartData1.Svetlost[i] = Convert.ToSingle(buff[i].Split(delimeter_)[1]); // 2nd column - datetime
                    ChartData1.padavineH[i] = Convert.ToSingle(buff[i].Split(delimeter_)[2]); // 3rd column - datetime
                    ChartData1.Tzunanja[i] = Convert.ToSingle(buff[i].Split(delimeter_)[3]); // 4th column - datetime
                    ChartData1.Tnotranja[i] = Convert.ToSingle(buff[i].Split(delimeter_)[4]); // 5th column - datetime
                }

                return ChartData1;
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Preparing data for writing to csv failed. " + ex.Message);
                return null;
            }
           
        }

        void timerDoStuf(object sender)
        {
            NewEntry();
            ChartData1 = new ChartData();
        }        

        void NewEntry()
        {
            string DateTimeNow = DateTime.Now.ToString(Settings.defaultDateTimeFormat);
            string danNoc = Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value.ToString();
            string padavineH = Val.logocontroler.Prop2.PadavineZadnjaUra.Value.ToString();            
            string Tzunanja = Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value.ToString();
            string Tnotranja = Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value.ToString();
                        
            //
            string concat = DateTimeNow + delimeter_ + danNoc + delimeter_ + padavineH + delimeter_ + Tzunanja + delimeter_ + Tnotranja + delimeter_; // dodaj po potrebi
            //

            try
            {
                var calcMax1day = 60 / (Settings.ChartUpdateRefreshRate / 1000F) * 24; // sets max number of lines to match 1 day
                LimitData(Convert.ToInt32(calcMax1day));

                try
                {
                    using (StreamWriter s = new StreamWriter(CsvFile, true))
                    {
                        s.WriteLine(concat);
                        s.Flush();
                        s.Close();
                    }
                }
                catch (Exception ex)
                {
                    Val.Message.Setmessage("Writing values to csv file failed. " +ex.Message);
                }
                
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Write to csv file failed. " + ex.Message);
            }
           
        }

        void LimitData(int MaxLines)
        {
            try
            {
                var lines = File.ReadAllLines(CsvFile);
                if (lines.Length >= MaxLines)
                {
                    var overflow = lines.Length - MaxLines + 1;
                    File.WriteAllLines(CsvFile, lines.Skip(overflow).ToArray());
                }
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Limiting data for csv file failed (method name: LimitData(int MaxLines)). " + ex.Message); 
            }
           
            
        }

        void LimitPeriod()
        {
            if (Settings.ChartUpdateRefreshRate < 1000)
            {
                Settings.ChartUpdateRefreshRate = 1000;
            }
            else if (Settings.ChartUpdateRefreshRate > 60000)
            {
                Settings.ChartUpdateRefreshRate = 60000;
            }
        }

        void waitForInitialisation()
        {
            while (true)
            {
                if (XmlController.XmlControllerInitialized)
                {
                    break;
                }
            }
        }

        void manageFile()
        {
           
            try
            {
                if (!folderExists())
                {
                    createFldr();
                }

                if (!ifFileExists())
                {
                    CreateFile();
                }

                LoadFile();
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Error with file: ChartValues.csv. " + ex.Message);
                CreateFile();
                LoadFile();
            }
        }

        bool ifFileExists()
        {
            return File.Exists(CsvFile);
        }

        void LoadFile()
        {
            try
            {
                Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(CsvFile);
            }
            catch (Exception ex)
            {
                throw new Exception("File " + CsvFile + " can not be opened, Program will create new file. Error: " + ex.Message);
            }
                   
        }

        void CreateFile()
        {
            FileStream fs = File.Create(CsvFile, 32768, FileOptions.WriteThrough);
            fs.Close();
        }

        bool folderExists()
        {
            return Directory.Exists(DirectoryPath);
        }

        void createFldr()
        {
            Directory.CreateDirectory(DirectoryPath);
        }
    }

    public class ChartData
    {
        public string[] lines;
        public string[] datetimes;
        public float[] Svetlost;
        public float[] padavineH;
        public float[] Tzunanja;
        public float[] Tnotranja;

        public ChartData()
        {
            try
            {
                lines = File.ReadAllLines(ChartValuesLogger.CsvFile);
                datetimes = new string[lines.Length];
                Svetlost = new float[lines.Length];
                padavineH = new float[lines.Length];
                Tzunanja = new float[lines.Length];
                Tnotranja = new float[lines.Length];
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Reading chart data from file failed: " + ex.Message);
            }
             
        }

    }
}