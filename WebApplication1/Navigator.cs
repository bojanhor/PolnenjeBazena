using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public class Navigator
    {
        public static string ViewStateElement_ScriptLoader = "ScriptLoader";
        public static string ViewStateElement_Response = "Response";
        public static string ViewStateElement_PageHistory = "PageHistory";
        public static string ViewStateElement_LoggedIn = "##_LoggedIn_##";
        public static string ViewStateElement_LoginAuth = "##_LogInAuth_##";

        static GuiController.GControls.PleaseWaitBanner PleaseWaitBanner;

        static bool messagePending;
        public static List<LoginTryData> LoginTryDataList = new List<LoginTryData>();
        
        static bool firstRequestover = false; // flag indicates if site is runned one cycle

        public static void Redirect(string site)
        {
            try
            {
                GetCurrentPage().Response.Redirect(site, false);
            }
            catch (Exception ex)
            {
                SysLog.SetMessage("Error inside method Redirect(). " + ex.Message);
                var pageHistory = GetPageHistoryFromSession();
                if (pageHistory != null)
                {
                    var page = pageHistory.getPreviousPage();
                    GetCurrentPage().Server.Transfer(page + "aspx");
                }

            }

        }

        private static void Refresh(HttpSessionState session, Page thisPage)
        {
            if (session != null)
            {
                var pageHistory = GetPageHistoryFromSession();
                var lastPage = pageHistory.getLastPage();

                Redirect(lastPage);
            }
        }

        public static void Refresh()
        {
            Refresh(GetSession(), GetCurrentPage());
        }

        public static void RedirectBack()
        {
            try
            {
                var session = GetSession();

                if (session != null)
                {
                    var pageHistory = GetPageHistoryFromSession();
                    var lastPage = pageHistory.getLastPage();
                    Redirect(pageHistory.getPreviousPage());
                }

            }
            catch (Exception ex)
            {
                SysLog.SetMessage("Error inside method RedirectBack(). " + ex.Message);
            }

        }

        static PageHistory GetPageHistoryFromSession()
        {
            var session = GetSession();

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

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClassID)
        {
            EveryPageProtocol(FriendlyPageNamePage, _thisPage, session, TemplateClassID, true, true);
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClassID, bool hasMenuBtn, bool hasLogo)
        {
            EveryPageProtocol(FriendlyPageNamePage, _thisPage, session, TemplateClassID, hasMenuBtn, hasLogo, true, true, true);
        }
                
        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClass, bool hasMenuBtn, bool hasLogo, bool hasBackBtn, bool hasHomeBtn, bool hasClock)
        {                       
            // Save response  
            session[ViewStateElement_Response] = _thisPage.Response;

            // Login management
            LoginChk(_thisPage, session);

            // ForceRefresh Management
            GlobalManagement.ForceRefreshManage();

            // Reconstruct whole page if button is clicked or timer is refreshing page
            if (_thisPage.IsPostBack)
            {
                Val.guiController.Reconstruct();
            }

            // Background
            GuiController.CreateEnclosingDivForReference(TemplateClass);
            GuiController.Template.CreateBackground(_thisPage, TemplateClass, hasLogo, hasHomeBtn);

            // Lable for page name
            Val.guiController.Template_.CreateTitleLabel(_thisPage, TemplateClass, hasLogo, FriendlyPageNamePage);

            // back button - navigate
            if (hasBackBtn)
            {
                Val.guiController.Template_.CreateBackButton(_thisPage, session, TemplateClass, hasLogo);
            }

            // menu button - navigate
            if (hasMenuBtn)
            {
                Val.guiController.Template_.CreateMenu(_thisPage, TemplateClass);
            }

            if (hasClock)
            {
                Clock(TemplateClass);
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

            scriptLoader.RegisterAllScriptsNow(_thisPage, TemplateClass);

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

            // Please Wait banner
            PleaseWait(TemplateClass);

            FirstRequestOver();
        }

        static void PleaseWait(HtmlGenericControl TemplateClass)
        {
            PleaseWaitBanner = new GuiController.GControls.PleaseWaitBanner();
            TemplateClass.Controls.Add(PleaseWaitBanner);
        }

        public static void ShowPleaseWait()
        {
            PleaseWaitBanner.ShowBanner();
        }

        static void FirstRequestOver()
        {
            if (!firstRequestover)
            {
                firstRequestover = true;

                if (!HttpContext.Current.Request.IsLocal) // if site runs in debug mode leave config file - else delete unencripted file
                {
                    Misc.SmartThread deleteUnencriptedConfigFileThread = new Misc.SmartThread(() => Helper.deleteUnencriptedConfigFileMethod());
                    deleteUnencriptedConfigFileThread.Start("deleteUnencriptedConfigFileThread", System.Threading.ApartmentState.MTA, true);
                }
            }
        }
                
        static void LoginChk(Page _thisPage, HttpSessionState session)
        {
            var ip = Helper.GetClientIP();

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
                    Redirect("Default"); // value from sessionState was correct
                    session[ViewStateElement_LoggedIn] = Val.LoggedIn;
                    LoginTryData.IPCleared(ip); // clears login failed tries
                    return;
                }                
            }
                        
            if (LoginTryData.isBlockedIPOrUser(ip, session))
            {
                if (_thisPage.GetType().BaseType == typeof(Pages.PageBlocked)) // if blocked form is (to be) shown proceed
                {
                    return; // can proceed because blocked form will be shown
                }
                                
                Redirect("Blocked"); // Ip is blocked
                return;
            }
            
            if (_thisPage.GetType().BaseType == typeof(Pages.PageLogin)) // if login form is (to be) shown proceed
            {
                return; // can proceed because login form will be shown
            }

            Redirect("Login");  // Conditions are not met for login

        }

        public static HttpSessionState GetSession()
        {
            return HttpContext.Current.Session;
        }

        static void Clock(HtmlGenericControl TemplateClass)
        {
            GuiController.GControls.UpdatePanelFull clkUpdt = new GuiController.GControls.UpdatePanelFull("clkUpdt", 5000);
            HtmlGenericControl div = GuiController.DIV.CreateDivAbsolute(0.7F, 90, 10, 3, "vw");
            div.ID = "clk";
            GuiController.GControls.SuperLabel clk = new GuiController.GControls.SuperLabel(
               Helper.getClockValue(), 0, 0, 100, 100); clk.FontWeightBold = true;
            clk.FontSize = 1.8F; clk.Style.Add(HtmlTextWriterStyle.Color, "grey");

            clkUpdt.Controls_Add(div);
            div.Controls.Add(clk);
            TemplateClass.Controls.Add(div);
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
                    return (ScriptLoader)GetSession()[ViewStateElement_ScriptLoader];
                }
                catch
                {
                    return null;
                }

            }            
        }

        public class LoginTryData
        {
            string ip;
            uint attempts = 1;
            DateTime dt;
            
            public static bool LoginTryIP(string ip)
            {
                foreach (var item in LoginTryDataList)
                {
                    if (item.ip == ip)
                    {
                        item.attempts++;
                        item.dt = DateTime.Now;

                        if (item.attempts > Settings.MaxLoginTriesIP)
                        {
                            return true;
                        }
                        return false;
                    }
                }

                var buff = new LoginTryData();
                buff.dt = DateTime.Now;
                buff.ip = ip;                
                LoginTryDataList.Add(buff);
                return false;
            }

            public static bool isBlockedIPOrUser(string ip, HttpSessionState session)
            {
                // IP blockage
                UnblockIPAfter(ip);

                foreach (var item in LoginTryDataList)
                {
                    if (item.ip == ip)
                    {                        
                        if (item.attempts >= Settings.MaxLoginTriesIP) // max login failed tries exceeded
                        {
                            return true;
                        }
                    }
                }

                // User Blockage

                var luserobj = session[Helper.UserDataManager.ViewStateElement_LogingInUserTry];

                if (luserobj != null)
                {
                    var luser = (string)luserobj;

                    foreach(var item in Helper.UserDataManager.ActiveUsers)
                    {
                        if (item.GetUsername() == luser)
                        {
                            return item.Blocked();                            
                        }
                    }
                }

                return false;
            }

            public static void UnblockIPAfter(string ip)
            {
                foreach (var item in LoginTryDataList)
                {
                    if (item.ip == ip)
                    {
                        if (item.dt < DateTime.Now.AddSeconds(-20))
                        {
                            IPCleared(ip);
                            return;
                        }
                    }
                }
            }

            public static void IPCleared(string ip)
            {
                foreach (var item in LoginTryDataList)
                {
                    if (item.ip == ip)
                    {
                        LoginTryDataList.Remove(item);
                        return;
                    }
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
                    Refresh();
                }
                messagePending = true;
                Refresh();
            }
            else
            {
                messagePending = false;
            }
        }
    }
}