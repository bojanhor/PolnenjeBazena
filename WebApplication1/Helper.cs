using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication1
{

    public class Helper
    {
        public static bool LogoControllerInitialized = false;
        public static bool GuiControllerInitialized = false;        
        public static bool WarningManagerInitialized = false;

        public static void deleteUnencriptedConfigFileMethod()
        {
            Thread.Sleep(10000);

            try
            {
                var p = XmlController.BaseDirectoryPath + Settings.pathToConfigFile;
                if (File.Exists(p))
                {
                    File.Delete(p);
                    XmlController.encriptedMode = true;
                }
            }
            catch (Exception) {  }
            
        }

        public static string getClockValue()
        {
            if (Val.logocontroler != null)
            {
                if (Val.logocontroler.Prop1 != null)
                {
                    var p1 = Val.logocontroler.Prop1.LogoClock.Value;
                    if (p1 != PropComm.NA)
                    {
                        return p1;
                    }
                }

                if (Val.logocontroler.Prop2 != null)
                {
                    var p2 = Val.logocontroler.Prop2.LogoClock.Value;
                    if (p2 != PropComm.NA)
                    {
                        return p2;
                    }
                }                
            }

            return "";
        }

        public static HtmlGenericControl PositionUnderTitle(WebControl element, Label Undertitle)
        {
            double offset = Convert.ToDouble(element.Style["width"].Replace("%", "")) * 2.6D; // set top offset for undertitle, to position it under image

            var div = GuiController.DIV.CreateDivAbsolute();

            var top = element.Style["top"]; // gets value from "top attribute
            top = top.Replace("%", ""); // removes %
            var posTop = Convert.ToDouble(top); // converts to int

            var left = element.Style["left"]; // gets value from "top attribute
            left = left.Replace("%", ""); // removes %
            var posLeft = Convert.ToInt32(left); // converts to int

            var wid = element.Style["width"]; // gets value from "top attribute            

            posTop += offset; // adds offset


            div.Style.Add(HtmlTextWriterStyle.Top, posTop.ToString("#.00").Replace(",", ".") + "%");
            div.Style.Add(HtmlTextWriterStyle.Left, posLeft + "%");
            div.Style.Add(HtmlTextWriterStyle.Width, wid);
            div.Style.Add(HtmlTextWriterStyle.Height, "10%");

            Undertitle.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            Undertitle.Style.Add(HtmlTextWriterStyle.Width, "100%");
            Undertitle.Style.Add(HtmlTextWriterStyle.Height, "100%");
            Undertitle.Style.Add(HtmlTextWriterStyle.Top, 0 + "%");
            Undertitle.Style.Add(HtmlTextWriterStyle.Left, 0 + "%");
            Undertitle.Style.Add(HtmlTextWriterStyle.TextAlign, "center");

            div.Controls.Add(Undertitle);

            return div;

        }

        public static void XML_Was_Changed()
        {
            Val.guiController.Reconstruct();
        }

        public static string GetBaseUrl()
        {
            return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, HttpRuntime.AppDomainAppVirtualPath);
        }

        public class Position
        {
            public float top;
            public float left;
            public float width;

            public Position()
            {

            }

            public Position(float top_, float left_)
            {
                top = top_;
                left = left_;
            }

            public Position(float top_, float left_, float width_)
            {
                top = top_;
                left = left_;
                width = width_;
            }
        }

        public sealed class WebSafeFonts
        {
            public static readonly string Arial = "Arial";
            public static readonly string Roboto = "Roboto";
            public static readonly string TimesNewRoman = "Times New Roman";
            public static readonly string Times = "Times";
            public static readonly string CourierNew = "Courier New";
            public static readonly string Courier = "Courier";
            public static readonly string Verdana = "Verdana";
            public static readonly string Georgia = "Georgia";
            public static readonly string Palatino = "Palatino";
            public static readonly string Garamond = "Garamond";
            public static readonly string Bookman = "Bookman";
            public static readonly string ComicSansMS = "Comic Sans MS";
            public static readonly string Candara = "Candara";
            public static readonly string ArialBlack = "Arial Black";
            public static readonly string Impact = "Impact";
        }

        public static string GetClientIP()
        {
            string ip;
            var thispage = Navigator.GetCurrentPage();

            try
            {
                ip = thispage.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = thispage.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (ip == "::1" || ip.Contains("127.0.0.1"))
                {
                    ip = "Local";
                }
            }
            catch
            {
                ip = "UNKNOWN IP";
            }

            return ip;
        }

        public static string FloatToStringWeb(float f, string postFix)
        {
            return f.ToString("0.##").Replace(",", ".") + postFix;
        }        

        public static string FloatToStringWeb(double f, string postFix)
        {
            return f.ToString("0.##").Replace(",", ".") + postFix;
        }

        public class Initialiser
        {            
            public Initialiser()
            {                
                Val.InitialiseClass();
            }
        }
        
        public class UserData
        {
            readonly string Username;
            readonly string Password;
            uint tries = 1;

            bool blocked = false;
            DateTime blocked_dt = new DateTime();

            public string GetUsername()
            {
                return Username;
            }

            public string GetPassword()
            {
                return Password;
            }

            public uint GetTries()
            {
                return tries;
            }

            public void AddTry()
            {
                if (tries >= Settings.MaxLoginTriesUser)
                {
                    blocked = true;
                    blocked_dt = DateTime.Now;                    
                }
                else
                {
                    tries++;
                }                
            }

            public void ResetTry()
            {
                tries = 1;
                blocked = false;
                blocked_dt = new DateTime();
            }
            
            public UserData(string U, string P)
            {
                Username = U;
                Password = P;
                tries = 1;
            }

            public bool Blocked()
            {
                if (blocked)
                {
                    if (blocked_dt < DateTime.Now.AddSeconds(-20))
                    {
                        unblock();
                    }
                }

                return blocked;
            }

            void unblock()
            {
                blocked = false;
                blocked_dt = new DateTime();
            }
        }

        public class UserDataManager
        {
            public static string ViewStateElement_LogingInUserTry = "##_LoggedInUser_##";
            public static List<UserData> ActiveUsers = new List<UserData>();

            public static bool ConfirmUsername(out UserCheckStatus chk, string UserName, string pwd, HttpSessionState session)
            {
                var usrsBuff = XmlController.GetUserData();
                UserData userBuff;
                var usr = UserName;
                var pswrd = pwd;

                session[ViewStateElement_LogingInUserTry] = UserName;

                foreach (var item in usrsBuff)
                {
                    if (item.GetUsername() == usr)
                    {
                        userBuff = ActiveUsersAdd(item);

                        if (userBuff.Blocked())
                        {
                            chk = UserCheckStatus.BlockedUser;
                            SysLog.SetMessage("Login Failed: User ("+ UserName +") was Blocked ");
                            return false;
                        }
                        //
                        if (userBuff.GetPassword().Equals(pswrd))
                        {
                            ActiveUserResetTry(userBuff);
                            chk = UserCheckStatus.OK;                            
                            return true;
                        }
                        else
                        {
                            ActiveUserAddTry(userBuff);
                            chk = UserCheckStatus.InvalidPassword;
                            SysLog.SetMessage("Login Failed: Password was invalid");
                            return false;
                        }
                    }
                }

                chk = UserCheckStatus.InvalidUserName;
                SysLog.SetMessage("Login Failed: Username ("+ UserName + ") was invalid");
                return false;
            }

            public enum UserCheckStatus
            {
                OK,
                InvalidUserName,
                InvalidPassword,
                BlockedUser
            }

            static UserData ActiveUsersAdd(UserData user)
            {
                foreach (var item in ActiveUsers)
                {
                    if (item.GetUsername() == user.GetUsername())
                    {
                        return item;
                    }
                }

                ActiveUsers.Add(user);
                return user;
            }

            static void ActiveUserAddTry(UserData user)
            {
                foreach (var item in ActiveUsers)
                {
                    if (item.GetUsername() == user.GetUsername())
                    {
                        item.AddTry();
                    }
                }
            }

            static void ActiveUserResetTry(UserData user)
            {
                foreach (var item in ActiveUsers)
                {
                    if (item.GetUsername() == user.GetUsername())
                    {
                        item.ResetTry();
                    }
                }
            }
            
        }

        public static void DownloadFile(Page thispage, string type, string content)
        {
            try
            {
                thispage.Response.Clear();
                thispage.Response.AddHeader("Content-Disposition", "attachment; filename=" + "config_downloaded." + type);
                thispage.Response.AddHeader("Content-Length", content.Length.ToString());
                thispage.Response.ContentType = "text/" + type;

                using (var streamWriter = new StreamWriter(thispage.Response.OutputStream))
                {
                    streamWriter.Write(content);

                    streamWriter.Flush();
                    streamWriter.Close();
                }

                thispage.Response.End();

            }
            catch (Exception ex)
            {
                throw new Exception("Can not prepare download file: " + ex.Message);
            }
        }

        public static int GetID_FromBtnObject(object ButtonFromSender_Event)
        {
            var btn = (GuiController.GControls.TransparentButton)ButtonFromSender_Event;
            string newID = new string(btn.ID.Where(char.IsDigit).ToArray()); // remove numbers to get ID
            return Convert.ToInt16(newID);
        }

        public static string GetNumbersOnlyFromString(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
    }
}