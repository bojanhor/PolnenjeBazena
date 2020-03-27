using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    public static class SysLog
    {
        public static MessageManager Message = new MessageManager();

        public class MessageManager
        {            
            static string LogFilePath = XmlController.BaseDirectoryPath + "\\" + "Log.txt";
            static string tempLogFilePath = XmlController.BaseDirectoryPath + "\\" + "Log_tmp.txt";
            string Message_ = "";
            StreamWriter s;            

            public MessageManager()
            {
                manageFiles();                
            }

            // Creates new line in Log file and in text box on GUI
            public void SetMessage(string message)
            {
                Message_ += message + Environment.NewLine;
                var LogMsg = DateTime.Now.ToString(Settings.defaultDateTimeFormatY) + ": " + message;

                try
                {
                    
                    s = new StreamWriter(LogFilePath, true);
                    s.WriteLine(LogMsg);
                    s.Flush();
                    s.Close();
                    checkFileSize();                   
                }
                catch (Exception ex)
                {                  
                    Message_ += "ERROR WRITING MESSAGE TO FILE (Log.txt)" + ex.Message + Environment.NewLine;
                }
                
            }

            // gets textbox string to post on webpage
            public string GetMessage()
            {
                return Message_;
            }

            // File management
            void manageFiles()
            {

                try
                {
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
                    throw new Exception("Log file problem: " + ex.Message);
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
                    throw new Exception("File " + FilePath + " can not be opened. Error: " + ex.Message);
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
                catch (Exception ex)
                {                    
                    throw ex;
                }                
            }
        }
    }
}