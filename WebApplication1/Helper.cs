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
        public static string ViewStateElement_ScriptLoader = "ScriptLoader";
        public static string ViewStateElement_Response = "Response";
        public static string ViewStateElement_PageHistory = "PageHistory";
        public static string ViewStateElement_LoggedIn = "##_LoggedIn_##";
        static bool messagePending;

        static bool firstRequestover = false; // flag indicates if site is runned one cycle

        public static void Redirect(string site, Page _thisPage)
        {
            try
            {
                _thisPage.Response.Redirect(site, false);
            }
            catch (Exception ex)
            {
                SysLog.SetMessage("Error inside method Redirect(). " + ex.Message);
                var pageHistory = GetPageHistoryFromSession();
                if (pageHistory != null)
                {
                    var page = pageHistory.getPreviousPage();
                    _thisPage.Server.Transfer(page + "aspx");
                }
                
            }
            
        }

        private static void Refresh(System.Web.SessionState.HttpSessionState session, Page thisPage)
        {
            if (session != null)
            {
                var pageHistory = GetPageHistoryFromSession();
                var lastPage = pageHistory.getLastPage();

                Redirect(lastPage, thisPage);
            }
        }

        public static void Refresh()
        {
            Refresh(HttpContext.Current.Session, GetCurrentPage());
        }

        public static void RedirectBack()
        {
            try
            {
                var session = HttpContext.Current.Session;

                if (session != null)
                {
                    var pageHistory = GetPageHistoryFromSession();
                    var lastPage = pageHistory.getLastPage();
                    Redirect(pageHistory.getPreviousPage(), GetCurrentPage());
                }

            }
            catch (Exception ex)
            {
                SysLog.SetMessage("Error inside method RedirectBack(). " + ex.Message);
            }

        }

        static PageHistory GetPageHistoryFromSession()
        {
            var session = HttpContext.Current.Session;

            if (session != null)
            {
                var pageHistoryObj = session[ViewStateElement_PageHistory];
                var pageHistory = (PageHistory)pageHistoryObj;
                return pageHistory;                 
            }

            return null;
        }

        public static Page GetCurrentPage()
        {
            return (Page)HttpContext.Current.Handler;
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, System.Web.SessionState.HttpSessionState session, HtmlGenericControl TemplateClassID)
        {
            EveryPageProtocol(FriendlyPageNamePage, _thisPage, session, TemplateClassID, true, true);
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, System.Web.SessionState.HttpSessionState session, HtmlGenericControl TemplateClassID, bool hasMenuBtn, bool hasLogo)
        {
            EveryPageProtocol(FriendlyPageNamePage, _thisPage, session, TemplateClassID, hasMenuBtn, hasLogo, true, true);
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, System.Web.SessionState.HttpSessionState session, HtmlGenericControl TemplateClassID, bool hasMenuBtn, bool hasLogo, bool hasBackBtn, bool hasHomeBtn)
        {

            // Save response  
            session[ViewStateElement_Response] = _thisPage.Response;

            LoginChk(_thisPage, session);

            // Reconstruct whole page if button is clicked or timer is refreshing page
            if (_thisPage.IsPostBack)
            {
                Val.guiController.Reconstruct();
            }

            // Background
            GuiController.CreateEnclosingDivForReference(TemplateClassID);
            GuiController.Template.CreateBackground(_thisPage, TemplateClassID, hasLogo, hasHomeBtn);

            // Lable for page name
            Val.guiController.Template_.CreateTitleLabel(_thisPage, TemplateClassID, hasLogo, FriendlyPageNamePage);

            // back button - navigate
            if (hasBackBtn)
            {
                Val.guiController.Template_.CreateBackButton(_thisPage, session, TemplateClassID, hasLogo);
            }

            // menu button - navigate
            if (hasMenuBtn)
            {
                Val.guiController.Template_.CreateMenu(_thisPage, TemplateClassID);
            }

            // auto refresh page
            if (Settings.autoRefreshPageEvery_s != 0)
            {
                _thisPage.Response.AppendHeader("Refresh", Settings.autoRefreshPageEvery_s.ToString());
            }

            // font set
            Style style = new Style();
            style.Font.Name = Settings.DefaultFont;
            _thisPage.Header.StyleSheet.CreateStyleRule(style, null, "body");


            // Register scripts added   
            ScriptLoader scriptLoader;
            if (session[ViewStateElement_ScriptLoader] == null)
            {
                scriptLoader = new ScriptLoader();
            }
            else
            {
                scriptLoader = (ScriptLoader)session[ViewStateElement_ScriptLoader];
            }

            scriptLoader.RegisterAllScriptsNow(_thisPage, TemplateClassID);

            session[ViewStateElement_ScriptLoader] = scriptLoader;


            // Page history for back button
            var PageName = new System.IO.FileInfo(_thisPage.Request.Url.AbsolutePath).Name;


            PageHistory pageHistory;
            if (session[ViewStateElement_PageHistory] == null)
            {
                pageHistory = new PageHistory();
            }
            else
            {
                pageHistory = (PageHistory)session[ViewStateElement_PageHistory];
            }
            pageHistory.StorePage(PageName);
            session[ViewStateElement_PageHistory] = pageHistory;

            FirstRequestOver();
        }

        static void FirstRequestOver()
        {
            if (!firstRequestover)
            {
                firstRequestover = true;

                if (!HttpContext.Current.Request.IsLocal) // if site runs in debug mode leave config file - else delete unencripted file
                {
                    Misc.SmartThread deleteUnencriptedConfigFileThread = new Misc.SmartThread(() => deleteUnencriptedConfigFileMethod());
                    deleteUnencriptedConfigFileThread.Start("deleteUnencriptedConfigFileThread", System.Threading.ApartmentState.MTA, true);
                }                
            }
        }

        static void deleteUnencriptedConfigFileMethod()
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

        static void LoginChk(Page _thisPage, System.Web.SessionState.HttpSessionState session)
        {
            // Login determine
            var loginBuff = session[ViewStateElement_LoggedIn]; // gets login state from current session            


            if (loginBuff != null) // if there is no login state in session state
            {
                if (loginBuff.Equals(Val.LoggedIn))
                {
                    return; // if logged in continue 
                }
                else if (loginBuff.Equals(Val.LoggingIn)) // if value from sessionState is corect you can proceed on default page - if not than loginForm must be shown
                {
                    Redirect("Default", _thisPage); // value from sessionState was correct
                    session[ViewStateElement_LoggedIn] = Val.LoggedIn;

                    return;
                }
            }

            if (_thisPage.GetType().BaseType == typeof(Pages.test)) // if login form is (to be) shown proceed
            {
                return; // can proceed because login form will be shown
            }

            Redirect("Login", _thisPage);  // Conditions are not met for login

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

        public class Datasource : List<ListItem>
        {
            public Datasource()
            {

            }

            new public void Add(ListItem item)
            {
                base.Add(item);
            }

            public static int? GetValueFromText_int(string Text)
            {
                if (Text != null && Text != PropComm.NA)
                {
                    return Convert.ToInt32(GetNumbers(Text));
                }
                return null;
            }

            public static short? GetValueFromText_short(string Text)
            {
                if (Text != null && Text != PropComm.NA)
                {
                    return Convert.ToInt16(GetNumbers(Text));
                }
                return null;
            }

            static string GetNumbers(string input)
            {
                return new string(input.Where(c => char.IsDigit(c)).ToArray());
            }
        }

        public class TimeSelectorDatasource : Datasource
        {
            static ListItem buff;

            static string[] mins = { "00", "15", "30", "45" };
            static string[] hrs = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };


            public TimeSelectorDatasource()
            {
                Add(new ListItem(PropComm.NA, PropComm.NA));
                foreach (var item in hrs)
                {
                    foreach (var it in mins)
                    {
                        buff = new ListItem(item + ":" + it);
                        Add(buff);
                    }

                }

                Add(new ListItem("23:59", "23:59"));
            }

        }

        public class ChartViewSelectorDatasource : Datasource
        {
            static string[] values;

            public ChartViewSelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(10);
            }

            public void GetDatasource(int increment)
            {
                values = new string[]
                {
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._Today),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._1DayAgo),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._2DaysAgo),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._3DaysAgo),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._ThisWeek),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._ThisMonth)

                };


                for (int i = 0; i < values.Length; i++)
                {
                    CreateRow(values[i], i + ""); // Please make sure values are synchronised With enum: ChartData.ShowChartEnum
                }

            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem
                {
                    Text = text,
                    Value = value
                };
                Add(r); // adds to base class
                return r;
            }
        }

        public class DimmerSelectorDatasource : Datasource
        {
            static string[] percents;

            public DimmerSelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(10);
            }

            public void GetDatasource(int increment)
            {

                percents = new string[100 / increment];

                CreateRow(PropComm.NA, "0");

                var buff = 0;

                for (int i = 0; i < percents.Length + 1; i++)
                {
                    CreateRow(buff + "%", buff.ToString());
                    buff += increment;
                }

            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem
                {
                    Text = text,
                    Value = value
                };
                Add(r);// adds to base class
                return r;
            }
        }

        public class TimerSelectorDatasource : Datasource
        {
            public TimerSelectorDatasource(int from_seconds, int to_seconds, float increment, string unit)
            {
                GetDatasource(from_seconds, from_seconds, increment, unit);
            }

            public void GetDatasource(int from_seconds, int to_seconds, float increment, string unit)
            {
                var range = to_seconds - from_seconds;
                float valBuff;
                string txtBuff;
                float buff;

                valBuff = from_seconds;
                txtBuff = valBuff + unit;

                CreateRow(txtBuff, valBuff.ToString());

                for (int i = 1; i < 100; i++) // protection from  to many steps
                {
                    buff = from_seconds + increment;
                    if (buff > to_seconds)
                    {
                        break;
                    }

                    valBuff = buff;
                    txtBuff = buff + unit;

                    CreateRow(txtBuff, valBuff.ToString());
                }

            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem(text, value);
                Add(r);// adds to base class
                return r;
            }
        }

        public class HisteresisSelectorDatasource : Datasource
        {
            static string[] units;

            public HisteresisSelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(1);
            }

            public void GetDatasource(int increment)
            {

                try
                {
                    units = new string[5 / increment];
                }
                catch (Exception)
                {
                    units = new string[5 / increment];
                }

                CreateRow(PropComm.NA, "0");

                var buff = 1;

                for (int i = 0; i < units.Length; i++)
                {
                    CreateRow(buff + "°C", buff.ToString());
                    buff += increment;
                }


            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem(text, value);
                Add(r);
                return r;
            }
        }

        public class Temperature_10_30_SelectorDatasource : Datasource
        {
            static string[] units;

            public Temperature_10_30_SelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(2);
            }

            public void GetDatasource(int increment)
            {

                try
                {
                    units = new string[22 / increment];
                }
                catch (Exception)
                {
                    units = new string[22 / increment];
                }

                CreateRow(PropComm.NA, "0");

                var buff = 10;

                for (int i = 0; i < units.Length; i++)
                {
                    CreateRow(buff + "°C", buff.ToString());
                    buff += increment;
                }


            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem(text, value);
                Add(r);
                return r;
            }
        }

        public class YesNoSelectorDatasource : Datasource
        {
            public YesNoSelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, false);
                CreateRow("DA", true);
                CreateRow("NE", false);

            }

            ListItem CreateRow(string text, bool value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class RocnoAvtoSelectorDatasource : Datasource
        {
            public RocnoAvtoSelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, 0);
                CreateRow("Ročno", 1);
                CreateRow("Avtomatsko", 0);

            }

            ListItem CreateRow(string text, short value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class OnOffSelectorDatasource : Datasource
        {
            public OnOffSelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, false);
                CreateRow("VKLOP", true);
                CreateRow("IZKLOP", false);

            }

            ListItem CreateRow(string text, bool value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class WeekDaySelectorDatasource : Datasource
        {
            public WeekDaySelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, 0);
                CreateRow("IZKLOP", 0);
                CreateRow("Ponedeljek", 512);
                CreateRow("Torek", 1024);
                CreateRow("Sreda", 2048);
                CreateRow("Četrtek", 4096);
                CreateRow("Petek", 8192);
                CreateRow("Sobota", 16384);
                CreateRow("Nedelja", 256);               

            }

            ListItem CreateRow(string text, ushort value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
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

        public class ScriptLoader
        {// add scripts before RegisterScriptOnPageLoad()
            List<string> Scripts = new List<string>();
            List<string> Keys = new List<string>();

            public ScriptLoader()
            {

            }

            public void RegisterScriptOnPageLoad(string key, string script)
            { // Register scripts any time in Page Load (currently in EveryPageProtocol() )
                Scripts.Add(script);
                Keys.Add(key);
            }

            public void RegisterAllScriptsNow(Page page, HtmlGenericControl divForScript)
            {
                if (Scripts.Count > 0)
                {
                    for (int i = 0; i < Scripts.Count; i++)
                    {
                        page.ClientScript.RegisterStartupScript(typeof(Page), Keys[i], Scripts[i], true);
                    }

                    Scripts.Clear();
                    Keys.Clear();
                }
            }

            public static ScriptLoader GetScriptLoaderInstance()
            {
                try
                {
                    return (ScriptLoader)HttpContext.Current.Session[ViewStateElement_ScriptLoader];
                }
                catch
                {
                    return null;
                }

            }


        }

        public static void MessageBox(string message)
        {
            if (!messagePending)
            {
                var scriptLoader = ScriptLoader.GetScriptLoaderInstance();
                if (scriptLoader != null)
                {
                    var script = "alert('" + message + "');";
                    scriptLoader.RegisterScriptOnPageLoad("Info", script);
                    Helper.Refresh();
                }
                messagePending = true;
                Helper.Refresh();
            }
            else
            {
                messagePending = false;
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