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
        static char[] newLine = Environment.NewLine.ToCharArray();
        
        ChartData ChartData1;
        public static string DirectoryPath = XmlController.BaseDirectoryPath + "\\" + "ChartValues";
        static readonly string FileNameToday = "ChartValues_Today.csv";
        static readonly string FileName1DayAgo = "ChartValues_1DayAgo.csv";
        static readonly string FileName2DaysAgo = "ChartValues_2DaysAgo.csv";
        static readonly string FileName3DaysAgo = "ChartValues_3DaysAgo.csv";
        static readonly string FileNameWeek = "ChartValues_Week.csv";

        public static string CsvFileToday = DirectoryPath + "\\" + FileNameToday;
        public static string CsvFile_1DayAgo = DirectoryPath + "\\" + FileName1DayAgo;
        public static string CsvFile_2DaysAgo = DirectoryPath + "\\" + FileName2DaysAgo;
        public static string CsvFile_3DaysAgo = DirectoryPath + "\\" + FileName3DaysAgo;
        public static string CsvFileWeek = DirectoryPath + "\\" + FileNameWeek;
               
        DateTime DateTimeNow;
        short? danNoc;
        short? padavineH;
        short? Tzunanja;
        short? Tnotranja;

        List<DateTime> DateTimeNow_lst = new List<DateTime>();
        List<short?> danNoc_lst = new List<short?>();
        List<short?> padavineH_lst = new List<short?>();
        List<short?> Tzunanja_lst = new List<short?>();
        List<short?> Tnotranja_lst = new List<short?>();

        string oldDataLine;

        float calcMax1day = 60 * 60 / (Settings.ChartUpdateRefreshRate / 1000F) * 24; // sets max number of lines to match 1 day (60sec per minure   *   60minutes per hour   ...   * 24hours per day)
        float calcMax1week = 60 * 60 / (Settings.ChartUpdateRefreshRate / 1000F) * 24; // sets max number of lines to match 1 week (hardcoded 7 times less resolution   and 7 times more data  .. number is the same)

        public ChartData.ShowChartEnum ShowChart { get; set; }

        ChartDataWriter CDataWrt = new ChartDataWriter(CsvFileToday, CsvFile_1DayAgo, CsvFile_2DaysAgo, CsvFile_3DaysAgo, CsvFileWeek);

        public ChartValuesLogger()
        {
            Misc.SmartThread ValueLogger = new Misc.SmartThread(() => m());
            ValueLogger.Start("ChartValLogger", ApartmentState.MTA, true);

        }

        void m()
        {
            //ChartData1 = new ChartData();
            Thread.Sleep(100); // frees up resources for webserver to load   
            
            manageFiles(); // finds csv file or creates it
            waitForInitialisation(); // waits for program to initialize itself if necessarry
            LimitPeriod();     
                        
            var tm = new Misc.SmartThread(()=>timing());
            tm.Start("ChartTiming");
            
        }
                
        public ChartData GetDataForChart()
        {
            ShowChart = (ChartData.ShowChartEnum)XmlController.GetShowChartMode();
            ChartData1 = new ChartData(ShowChart);

            try
            {
                var buff = ChartData1.lines;
                var splitted = new string[5];
                for (int i = 0; i < buff.Count; i++)
                {
                    splitted = buff[i].Split(delimeter_);
                    if (splitted[0] != "")
                    {
                        ChartData1.datetimes[i] = splitted[0]; // 1st column - datetime
                        ChartData1.Svetlost[i] = Convert.ToSingle(splitted[1]); // 2nd column - datetime
                        ChartData1.padavineH[i] = Convert.ToSingle(splitted[2]); // 3rd column - datetime
                        ChartData1.Tzunanja[i] = Convert.ToSingle(splitted[3]); // 4th column - datetime
                        ChartData1.Tnotranja[i] = Convert.ToSingle(splitted[4]); // 5th column - datetime
                    }                    
                }

                return ChartData1;
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Preparing data for writing to csv failed. " + ex.Message);
                return null;
            }
           
        }

        void timerDoStuf()
        {
            try
            {
                NewEntryDay();
                ChartData1 = new ChartData(ShowChart);
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Timer interval failed to complete. Timer will try next period. " + ex.Message);
            }
            
        }

        void timing()
        {
            var loopTimer = new Misc.LoopTiming(Settings.ChartUpdateRefreshRate, Settings.defaultCheckTimingInterval);

            while (true)
            {
                loopTimer.WaitForIt();
                timerDoStuf();                
            }
        }

        int counterForWeekLimiter = 0;
        void NewEntryWeek(string DataLine)
        {
            if (counterForWeekLimiter >= 7)
            {
                CDataWrt.Write(DataLine, CsvFileWeek);
                counterForWeekLimiter = 0;
            }
            else
            {
                counterForWeekLimiter++;
            }
                        
        }

        void NewEntryDay()
        {
            DateTimeNow = DateTime.Now;
            danNoc = Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value;
            padavineH = Val.logocontroler.Prop2.PadavineZadnjaUra.Value;
            Tzunanja = Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value;
            Tnotranja = Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value;
                       
            DateTimeNow_lst.Add(DateTime.Now);
            danNoc_lst.Add(Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value);
            padavineH_lst.Add(Val.logocontroler.Prop2.PadavineZadnjaUra.Value);
            Tzunanja_lst.Add(Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value);
            Tnotranja_lst.Add(Val.logocontroler.Prop1.DanNoc_Vrednost_An.Value);

            //
            string concat_ = concat(DateTimeNow, danNoc, padavineH, Tzunanja, Tnotranja, delimeter_); // data to string - csv
            //

            try
            {         
                // write todays file
                oldDataLine = LimitData(Convert.ToInt32(calcMax1day), CsvFileToday); // removes oldest line from csv file if there are too many lines - returns lines that were removed
                CDataWrt.Write(concat_, CsvFileToday);

                
                if (oldDataLine != "")
                {// write yesterdays file
                    oldDataLine = LimitData(Convert.ToInt32(calcMax1day), CsvFile_1DayAgo);
                    CDataWrt.Write(concat_, CsvFile_1DayAgo);

                    if (oldDataLine != "")
                    {// write 2 days ago file
                        oldDataLine = LimitData(Convert.ToInt32(calcMax1day), CsvFile_2DaysAgo);
                        CDataWrt.Write(concat_, CsvFile_2DaysAgo);

                        if (oldDataLine != "")
                        {// write 3 days ago file
                            oldDataLine = LimitData(Convert.ToInt32(calcMax1day), CsvFile_3DaysAgo);
                            CDataWrt.Write(concat_, CsvFile_3DaysAgo);
                        }
                    }
                }


                // averages for 1week
                if (DateTimeNow_lst.Count >= 7) // prepares data for one week
                {
                    // calc averages
                    DateTimeNow = DateTimeNow_lst.Last(); // reused buffer
                    danNoc = Average(danNoc_lst);// reused buffer
                    padavineH = Average(padavineH_lst);// reused buffer
                    Tzunanja = Average(Tzunanja_lst);// reused buffer
                    Tnotranja = Average(Tnotranja_lst);// reused buffer

                    concat_ = concat(DateTimeNow, danNoc, padavineH, Tzunanja, Tnotranja, delimeter_); // data to string - csv

                    // write
                    oldDataLine = LimitData(Convert.ToInt32(calcMax1week), CsvFileToday); // removes oldest line from csv file if there are too many lines - returns lines that were removed
                    CDataWrt.Write(concat_, CsvFileWeek);

                    //clear lists
                    DateTimeNow_lst.Clear();
                    danNoc_lst.Clear();
                    padavineH_lst.Clear();
                    Tzunanja_lst.Clear();
                    Tnotranja_lst.Clear();
                }
                
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Write to csv file failed. " + ex.Message);
            }
           
        }

        string concat(DateTime DateTimeNow, short? danNoc, short? padavineH, short? Tzunanja, short? Tnotranja, char delimeter_)
        {
            return 
                DateTimeNow.ToString(Settings.defaultDateTimeFormat) + delimeter_ + 
                danNoc.ToString() + delimeter_ + 
                padavineH.ToString() + delimeter_ +
                Tzunanja.ToString() + delimeter_ + 
                Tnotranja.ToString() + delimeter_;
        }

        short Average(List<short?> values)
        {
            double buff = 0;
            foreach (var item in values) // sum
            {
                if (item != null)
                {
                    buff += (short)item;
                }
            }

            if (buff == 0)
            {
                return 0;
            }

            return Misc.ToShort(buff/values.Count); // avg
        }        
        
        public static List<string> FileToarray(string FilePath)
        {
            var arr = new List<string>();
            var arr2 = new List<string>();

            try
            {                
                using (StreamReader r = new StreamReader(FilePath))
                {
                    arr = r.ReadToEnd().Split(newLine).ToList();
                    foreach (var item in arr)
                    {
                        if (item != "")
                        {
                            arr2.Add(item);
                        }
                    }                    
                    return arr2;
                }
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Error reading csv file: " + ex.Message);
            }            
            return null;
            
        }

        string LimitData(int MaxLines, string FilePath)
        {
            string[] overflowText = new string[1] {""}; // new empty table

            try
            {
                var lines = FileToarray(FilePath); // Reads file and puts content in table

                if (lines != null)
                {
                    if (lines.Count >= MaxLines)
                    {
                        var overflow = lines.Count - MaxLines + 1;
                        overflowText = new string[overflow];
                        overflowText = lines.Skip(overflow).ToArray();

                        foreach (var item in overflowText)
                        {
                            CDataWrt.Write(item, FilePath);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Limiting data for csv file failed (method name: LimitData(int MaxLines)). " + ex.Message); 
            }

            return overflowText[0];
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
                Thread.Sleep(50);
            }
        }

        void manageFiles()
        {
           
            try
            {
                if (!folderExists())
                {
                    createFldr();
                }

                if (!ifFileExists(CsvFileToday))
                {
                    CreateFile(CsvFileToday);
                }

                if (!ifFileExists(CsvFile_1DayAgo))
                {
                    CreateFile(CsvFile_1DayAgo);
                }

                if (!ifFileExists(CsvFile_2DaysAgo))
                {
                    CreateFile(CsvFile_2DaysAgo);
                }

                if (!ifFileExists(CsvFile_3DaysAgo))
                {
                    CreateFile(CsvFile_3DaysAgo);
                }

                if (!ifFileExists(CsvFileWeek))
                {
                    CreateFile(CsvFileWeek);
                }

                // try file
                LoadFile(CsvFileToday);
                LoadFile(CsvFile_1DayAgo);
                LoadFile(CsvFile_2DaysAgo);
                LoadFile(CsvFile_3DaysAgo);
                LoadFile(CsvFileWeek);
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Error with file: *.csv. " + ex.Message);                
            }
        }

        bool ifFileExists(string FilePath)
        {
            return File.Exists(FilePath);
        }

        void LoadFile(string FilePath)
        {
            try
            {
                Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(FilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("File " + FilePath + " can not be opened, Program will create new file. Error: " + ex.Message);
            }
                   
        }

        void CreateFile(string FilePath)
        {
            try
            {
                FileStream fs = File.Create(FilePath, 32768, FileOptions.WriteThrough);
                fs.Close();
            }
            catch (Exception ex)
            {

                throw new Exception("File creation failed: " + ex.Message);
            }
            
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

    //
    public class ChartDataWriter
    {       
        public readonly string CsvFileToday;
        public readonly string CsvFile_1DayAgo;
        public readonly string CsvFile_2DaysAgo;
        public readonly string CsvFile_3DaysAgo;
        public readonly string CsvFileWeek;

        List<string> line_Pending = new List<string>(); // pending writes
        List<string> PathCsvFile_Pending = new List<string>(); //pending writes;

        public ChartDataWriter(string CsvFileToday, string CsvFile_1DayAgo, string CsvFile_2DaysAgo, string CsvFile_3DaysAgo, string CsvFileWeek)
        {
            Misc.SmartThread t = new Misc.SmartThread( () => writer() );
            t.Start("ChartDataWriter", ApartmentState.MTA, false);

            this.CsvFileToday = CsvFileToday;
            this.CsvFile_1DayAgo = CsvFile_1DayAgo;
            this.CsvFile_2DaysAgo = CsvFile_2DaysAgo;
            this.CsvFile_3DaysAgo = CsvFile_3DaysAgo;
            this.CsvFileWeek = CsvFileWeek;
    }

        void writer()
        {
            while (true)
            {
                Thread.Sleep(Settings.defaultCheckTimingInterval);
                if (line_Pending.Count > 0)
                {
                    Write_Thread(line_Pending[0], PathCsvFile_Pending[0]);
                    line_Pending.RemoveAt(0);
                    PathCsvFile_Pending.RemoveAt(0);
                }
            }
        }
        
        public void Write(string line, string PathCsvFile)
        {
            line_Pending.Add(line);
            PathCsvFile_Pending.Add(PathCsvFile);

        }

        void Write_Thread(string line, string PathCsvFile)
        {
            if (line == null) { return; }
            if (line == "" || line == " ") { return; }

            try
            {               
                using (StreamWriter s = new StreamWriter(PathCsvFile, true))
                {
                    s.WriteLine(line);
                    s.Flush();
                    s.Close();
                }               
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Writing values to csv file failed (" + PathCsvFile + "). " + ex.Message);
                
            }
        }
        
    }

    //
    public class ChartData
    {
        public List<string> lines;
        public string[] datetimes;
        public float[] Svetlost;
        public float[] padavineH;
        public float[] Tzunanja;
        public float[] Tnotranja;

        public ChartData(ShowChartEnum showchartEnum)
        {
            var FilePath = GetPathShowchart(showchartEnum);

            try
            {                
                lines = ChartValuesLogger.FileToarray(FilePath);
                datetimes = new string[lines.Count];
                Svetlost = new float[lines.Count];
                padavineH = new float[lines.Count];
                Tzunanja = new float[lines.Count];
                Tnotranja = new float[lines.Count];
            }
            catch (Exception ex)
            {
                Val.Message.Setmessage("Reading chart data from file failed ("+ FilePath + "): " + ex.Message);
            }
             
        }

        public enum ShowChartEnum
        {
           _Today,
           _1DayAgo,
           _2DaysAgo,
           _3DaysAgo,
           _ThisWeek
        }

        public static string GetPathShowchart(ShowChartEnum showchartEnum)
        {
            if (showchartEnum == ShowChartEnum._Today)
            {
                return ChartValuesLogger.CsvFileToday;
            }
            else if (showchartEnum == ShowChartEnum._1DayAgo)
            {
                return ChartValuesLogger.CsvFile_1DayAgo;
            }
            else if (showchartEnum == ShowChartEnum._2DaysAgo)
            {
                return ChartValuesLogger.CsvFile_2DaysAgo;
            }
            else if (showchartEnum == ShowChartEnum._3DaysAgo)
            {
                return ChartValuesLogger.CsvFile_3DaysAgo;
            }
            else if (showchartEnum == ShowChartEnum._ThisWeek)
            {
                return ChartValuesLogger.CsvFileWeek;
            }
            else
            {
                return ChartValuesLogger.CsvFileToday;
            }

        }
        
    }
}