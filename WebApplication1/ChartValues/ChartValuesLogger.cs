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

        public string Path
        {
            get;
            set;
        }

        public string DirectoryPath
        {
            get;
            set;
        }

        readonly string FileName = "ChartValues.csv";

        public ChartValuesLogger()
        {
            Misc.SmartThread ValueLogger = new Misc.SmartThread(() => m());
            ValueLogger.Start("ChartValLogger", ApartmentState.MTA, true);

        }

        void m()
        {
            Thread.Sleep(1); // frees up resources for webserver to load           
            manageFile(); // finds csv file or creates it
            waitForInitialisation(); // waits for program to initialize itself if necessarry

            // TODO Logger
            
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
            DirectoryPath = XmlController.BaseDirectoryPath + "\\" + "ChartValues";
            Path = DirectoryPath + "\\" + FileName;

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
            return File.Exists(Path);
        }

        void LoadFile()
        {
            try
            {
                Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(Path);
            }
            catch (Exception ex)
            {
                throw new Exception("File " + Path + " can not be opened, Program will create new file. Error: " + ex.Message);
            }
                   
        }

        void CreateFile()
        {
            FileStream fs = File.Create(Path, 32768, FileOptions.WriteThrough);
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
}