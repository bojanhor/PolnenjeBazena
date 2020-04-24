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

        public class MessageManager : Dsps
        {
            List<string> PendingMessages = new List<string>();
            Misc.SmartThread LogWriter;


            static string LogFolderPath = Directory.GetParent(XmlController.BaseDirectoryPath).ToString() + "\\" + "Logs";
            public static string LogFilePath;
            static string tempLogFilePath;

            List<string> messageList = new List<string>();
                       
            public MessageManager()
            {
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                manageFiles();
                LogWriter = new Misc.SmartThread(() => WriteLogAsync());
                LogWriter.Start("LogWriter", ApartmentState.MTA, false);
            }

            private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
            {
                var message = "Fatal error" +
                    ":" + e.ExceptionObject.ToString();
                Message.SetMessage(message);
            }


            // Creates new line in Log file and in text box on GUI
            void SetMessage(string message, bool skippWritingToFile)
            {
                var LogMsg = DateTime.Now.ToString(Settings.defaultDateTimeFormatY) + ": " + message;
                messageList.Add(LogMsg);

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
            public string GetMessageForTB()
            {
                var lastIndex = messageList.Count - 1;
                var len = 4500; // TB is limited to 4500 lines to prevent long page loading time
                
                if (len > lastIndex)
                {
                    len = lastIndex ;
                }

                var tbMessages = messageList.GetRange(lastIndex - len, len); // gets last # messages
                string buff = "";

                foreach (var item in tbMessages) // converts to string suitable for textbox
                {
                    buff += item + Environment.NewLine;
                }

                return buff;
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
                    Navigator.MessageBox(message);
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
                    messageList = File.ReadLines(LogFilePath).ToList(); // reads lines to list

                }
                catch (Exception ex)
                {
                    var message = "File " + FilePath + " can not be opened. Error: " + ex.Message;
                    Navigator.MessageBox(message);
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
                    Navigator.MessageBox(message);
                    throw new Exception(message);
                }

            }

            void checkFileSize()
            {
                try
                {
                    FileInfo fi = new FileInfo(LogFilePath);
                    if (fi.Length > Settings.logFileMaxSizeKB * 1024) // file size limit cca 100MB
                    {
                        limitFileSize();
                    }
                }
                catch (Exception ex)
                {
                    SetMessage("Cant check or limit file size: " + LogFilePath + ". " + ex.Message);
                }

            }

            void limitFileSize()
            {
                try
                {// removes 20 lines from log file

                    var linesTmp = File.ReadLines(LogFilePath).ToList(); // reads lines to list
                    linesTmp.RemoveRange(0, 20);    // removes oldest 20 lines

                    File.WriteAllLines(tempLogFilePath, linesTmp); // writes to temporary file

                    File.Delete(LogFilePath);
                    File.Move(tempLogFilePath, LogFilePath); // replaces file

                }
                catch (Exception)
                {
                    throw;
                }
            }

            void WriteLogAsync()
            {
                Thread.Sleep(5000); // delayed start

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

                    }
                    catch (Exception ex)
                    {
                        var message = "ERROR WRITING TO FILE! " + ex.Message;
                        Navigator.MessageBox(message);
                        Message.SetMessage(message, true);
                        throw new Exception(message);
                    }

                    Thread.Sleep(500); // check every half second cca
                }
            }
        }

        public static void SetMessage(string message)
        {
            Message.SetMessage(message);
        }

        public static string GetMessagesTB()
        {
            return Message.GetMessageForTB();
        }
    }
}