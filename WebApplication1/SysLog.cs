using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1
{
    public static class SysLog
    {
        public static MessageManager Message = new MessageManager();
        

        public class MessageManager :Dsps
        {
            List<string> PendingMessages = new List<string>();
            Misc.SmartThread LogWriter;

            static string LogFolderPath = XmlController.BaseDirectoryPath + "\\" + "Logs";
            static string LogFilePath;
            static string tempLogFilePath;

            string Message_ = "";
            
            public MessageManager()
            {
                manageFiles();
                LogWriter = new Misc.SmartThread(() => WriteLogAsync());
                LogWriter.Start("LogWriter", ApartmentState.MTA, false);
            }

            // Creates new line in Log file and in text box on GUI
            void SetMessage(string message, bool skippWritingToFile)
            {
                var LogMsg = DateTime.Now.ToString(Settings.defaultDateTimeFormatY) + ": " + message;
                Message_ += message + Environment.NewLine;
                               
                if (!skippWritingToFile)
                {
                    PendingMessages.Add(LogMsg);
                }


            }

            public void SetMessage(string message)
            {
                SetMessage(message, false);
            }

            // gets textbox string to post on webpage
            public string GetMessage()
            {
                return Message_;
            }

            public static string GetLogFileContent()
            {
                return File.ReadAllText(LogFilePath);
            }

            // File management
            void manageFiles()
            {

                try
                {
                    if (!Directory.Exists(LogFolderPath))
                    {
                        Directory.CreateDirectory(LogFolderPath);
                    }

                     LogFilePath = LogFolderPath + "\\" + "Log.txt";
                     tempLogFilePath = LogFolderPath + "\\" + "Log_tmp.txt";

                    if (!ifFileExists(tempLogFilePath))
                    {
                        CreateFile(tempLogFilePath);
                    }

                    if (!ifFileExists(LogFilePath))
                    {
                        if (File.Exists(tempLogFilePath))
                        {
                            File.Move(tempLogFilePath, LogFilePath);
                        }
                        else
                        {
                            CreateFile(LogFilePath);
                        }                        
                    }
                    
                    // try file
                    LoadFile(LogFilePath);
                    
                }
                catch (Exception ex)
                {
                    var message = "Log file problem: " + ex.Message;
                    Helper.MessageBox(message);
                    throw new Exception(message);
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
                    using ( Microsoft.VisualBasic.FileIO.TextFieldParser parser = new Microsoft.VisualBasic.FileIO.TextFieldParser(FilePath))
                    {
                        parser.Close();
                    }
                }
                catch (Exception ex)
                {
                    var message = "File " + FilePath + " can not be opened. Error: " + ex.Message;
                    Helper.MessageBox(message);
                    throw new Exception(message);
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
                    var message = "File creation failed: " + ex.Message;
                    Helper.MessageBox(message);
                    throw new Exception(message);
                }

            }
            
            void checkFileSize()
            {
                try
                {                    
                    FileInfo fi = new FileInfo(LogFilePath);
                    if (fi.Length > 104875600) // file size limit cca 100MB
                    {
                        limitFileSize();                        
                    }                    
                }
                catch (Exception ex)
                {
                    SetMessage("Cant check or limit file size: " + LogFilePath + ". "+ ex.Message);
                }
               
            }

            void limitFileSize()
            {
                try
                {// removes 20 lines from log file
                    
                    var linesTmp = File.ReadLines(LogFilePath).ToList(); // reads lines to list
                    linesTmp.RemoveRange(0, 20);    // removes oldest 10 lines

                    File.WriteAllLines(tempLogFilePath, linesTmp); // writes to temporary file

                    File.Delete(LogFilePath);
                    File.Move(tempLogFilePath, LogFilePath); // replaces file
                    
                }
                catch (Exception )
                {                    
                    throw ;
                }                
            }

            void WriteLogAsync()
            {
                Thread.Sleep(500); // delayed start
               
                while (true)
                {
                    try
                    {
                        if (PendingMessages.Count > 0)
                        {
                            StreamWriter s = new StreamWriter(LogFilePath, true);

                            while (PendingMessages.Count > 0)
                            {
                                s.WriteLine(PendingMessages[0]);
                                PendingMessages.RemoveAt(0);
                            }

                            Thread.Sleep(Settings.defaultCheckTimingInterval);
                            checkFileSize();
                            s.Flush();
                            s.Close();
                        }
                        Thread.Sleep(500); // check every half second cca
                    }
                    catch (Exception ex)
                    {
                        var message = "ERROR WRITING TO FILE! " + ex.Message;
                        Helper.MessageBox(message);
                        Message.SetMessage(message, true);
                        throw new Exception(message);
                    }
                    
                }
            }
                        
        }
    }
}