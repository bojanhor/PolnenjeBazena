using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication1
{

    public class Helper
    {
        public static void deleteUnencriptedConfigFileMethod()
        {
            System.Threading.Thread.Sleep(10000);

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
            var p1 = Val.logocontroler.Prop1.LogoClock.Value;
            if (p1 != PropComm.NA)
            {
                return p1;
            }

            var p2 = Val.logocontroler.Prop2.LogoClock.Value;
            if (p2 != PropComm.NA)
            {
                return p2;
            }
            else return "";
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

        public static string GetClientIP(Page page)
        {
            string ip;
            try
            {
                ip = page.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = page.Request.ServerVariables["REMOTE_ADDR"];
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
            Misc.SmartThread InitialiseClass;
            public Initialiser()
            {
                InitialiseClass = new Misc.SmartThread(
                    new System.Threading.ThreadStart(
                        () => Val.InitialiseClass()));
                InitialiseClass.Start("InitialiseClass", System.Threading.ApartmentState.MTA, true);

            }
        }
        
        public class UserData
        {
            string Username;
            string Password;

            public string GetUsername()
            {
                return Username;
            }

            public string GetPassword()
            {
                return Password;
            }

            public UserData(string U, string P)
            {
                Username = U;
                Password = P;
            }

            public enum UserCheckStatus
            {
                OK,
                InvalidUserName,
                InvalidPassword
            }

            public static bool ConfirmUsername(out UserCheckStatus chk, string UserName, string pwd)
            {
                var usrsBuff = XmlController.GetUserData();
                var usr = UserName;
                var pswrd = pwd;
                bool validUsername = false;

                foreach (var item in usrsBuff)
                {
                    if (item.GetUsername() == usr)
                    {
                        validUsername = true;
                        //
                        if (item.GetPassword().Equals(pswrd))
                        {
                            chk = UserCheckStatus.OK;
                            return true;
                        }
                    }
                }

                if (validUsername)
                {
                    chk = UserCheckStatus.InvalidPassword;
                }
                else
                {
                    chk = UserCheckStatus.InvalidUserName;
                }

                return false;

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
    }
}