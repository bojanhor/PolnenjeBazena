using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public class Navigator
    {
        static bool Initialized = false;

        static GuiController.GControls.PleaseWaitBanner PleaseWaitBanner;

        static WarningManagerWebControl WarningManagerWebControl_;

        static bool messagePending;
        public static List<LoginTryData> LoginTryDataList = new List<LoginTryData>();

        static bool firstRequestover = false; // flag indicates if site is runned one cycle

        public static void Redirect(string site)
        {
            try
            {
                GetCurrentPage().Response.Redirect(site, false);
                if (SysLog.Message != null)
                {
                    SysLog.Message.SetMessage("Redirected to: " + site);
                }               
            }
            catch (Exception ex)
            {
                try
                {
                    var pageHistory = GetPageHistoryFromSession();
                    if (pageHistory != null)
                    {
                        var page = pageHistory.getPreviousPageName();
                        GetCurrentPage().Server.Transfer(page + "aspx");
                    }
                }
                catch (Exception)
                {
                    SysLog.SetMessage("Error inside method Redirect(). " + ex.Message);
                }                  
            }
        }
        
        private static bool IsInitialized()
        {
            if (Initialized)
            {
                return true;
            }

            if (
                Helper.WarningManagerInitialized &&
                Helper.LogoControllerInitialized &&
                Helper.GuiControllerInitialized
                )
            {
                Initialized = true;
                return true;
            }
            return false;
        }

        public static void Refresh()
        {
            var page = GetCurrentPage();
            if (page != null)
            {
                try
                {
                    page.Response.Redirect(page.Request.RawUrl, false);
                    SysLog.Message.SetMessage("Page refreshed.");
                }
                catch (Exception)
                {
                    Redirect("Default");
                    SysLog.Message.SetMessage("Refresh page failed.");
                }
            }
        }

        public static void RedirectBack()
        {
            try
            {
                var session = GetSession();

                if (session != null)
                {
                    var pageHistory = GetPageHistoryFromSession();
                    if (pageHistory != null)
                    {
                        Redirect(pageHistory.getPreviousPageName());
                    }
                    else
                    {
                        Redirect("Default");
                    }
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
                var pageHistoryObj = session[SessionHelper.PageHistory];
                var pageHistory = (PageHistory)pageHistoryObj;
                return pageHistory;
            }

            return null;
        }

        public static Page GetCurrentPage()
        {

            IHttpHandler buff;
            Page pg;
            string exceptionMessage = "Method GetCurrentPage() is reporting error. ";

            try
            {
                buff = getCurrentHttpContextHandler();

                if (buff != null)
                {
                    pg = (Page)buff;

                    if (pg.Response != null)
                    {
                        return pg;
                    }
                }

            }
            catch (Exception ex)
            {
                SysLog.SetMessage("1- " + exceptionMessage + ex.Message);
            }
            
            //
            try
            {
                pg = GetCurrentPageAlternative();

                if (pg != null)
                {
                    if (pg.Response != null)
                    {
                        return pg;
                    }
                }
            }
            catch (Exception ex)
            {
                SysLog.SetMessage("2- " + exceptionMessage + ex.Message);
            }
            exceptionMessage = "GetCurrentPage() is reporting an error: Can not get Page object from anywhere.";
            SysLog.SetMessage(exceptionMessage);
            throw new Exception(exceptionMessage);            

        }

        static IHttpHandler getCurrentHttpContextHandler()
        {
            try
            {
                var cur = HttpContext.Current;
                if (cur != null)
                {
                    if (true)
                    {
                        var h = cur.Handler;

                        if (h != null)
                        {
                            return h;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Method getCurrentHttpContextHandler() is reporting an error: " +ex.Message);
            }
            

            return null;
        }

        static Page GetCurrentPageAlternative()
        {
            try
            {
                var ph = GetPageHistoryFromSession();
                if (ph != null)
                {
                    return ph.getThisPage();
                }
                return null;
            }
            catch (Exception ex)
            {
                SysLog.SetMessage("GetCurrentPageAlternative() failed. " + ex.Message);
                return null;
            }
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClassID)
        {
            EveryPageProtocol(FriendlyPageNamePage, _thisPage, session, TemplateClassID, true, true);
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClassID, bool hasMenuBtn, bool hasLogo)
        {
            EveryPageProtocol(FriendlyPageNamePage, _thisPage, session, TemplateClassID, hasMenuBtn, hasLogo, true, true, true, true);
        }

        public static void EveryPageProtocol(string FriendlyPageNamePage, Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClass, bool hasMenuBtn, bool hasLogo, bool hasBackBtn, bool hasHomeBtn, bool hasClock, bool hasWarningIcon)
        {
            // AddTitle
            addTabTitle(_thisPage, FriendlyPageNamePage);

            // Prevent Searchengines from indexing 
            addMetaNoIndex(_thisPage);

            // Save response  
            session[SessionHelper.Response] = _thisPage.Response;

            // ForceRefresh Management
            GlobalManagement.ForceRefreshManage();

            // Reconstruct whole page if button is clicked or timer is refreshing page
            if (_thisPage.IsPostBack)
            {
                Val.guiController.Reconstruct();
            }

            // Login management
            LoginChk(_thisPage, session);


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
            ManageScripts(_thisPage, session, TemplateClass);

            // WaitServerInit
            if (!IsInitialized())
            {
                return;
            }

            // Page history for back button
            PageHistoryManage(_thisPage, session);                        

            // Please Wait banner
            PleaseWait(TemplateClass);

            ShowWarning(TemplateClass, hasWarningIcon);

            FirstRequestOver();
        }

        static void PageHistoryManage(Page _thisPage, HttpSessionState session)
        {
            // Page history for back button

            PageHistory pageHistory;
            if (session[SessionHelper.PageHistory] == null)
            {
                pageHistory = new PageHistory();
            }
            else
            {
                pageHistory = (PageHistory)session[SessionHelper.PageHistory];
            }

            pageHistory.StorePage(_thisPage);
            session[SessionHelper.PageHistory] = pageHistory;
        }

        static void ManageScripts(Page _thisPage, HttpSessionState session, HtmlGenericControl TemplateClass)
        {
            _thisPage.LoadComplete += (sender, e) => _thisPage_LoadComplete(sender, e, _thisPage, session, TemplateClass);
        }

        private static void _thisPage_LoadComplete(object sender, EventArgs e, Page thispage, HttpSessionState session, HtmlGenericControl TemplateClass)
        {
            // Register scripts added   
            ScriptLoader scriptLoader;
            scriptLoader = ScriptLoader.GetInstance(session);
            session[SessionHelper.ScriptLoader] = scriptLoader;
            scriptLoader.RegisterAllScriptsNow(thispage, TemplateClass);
        }

        static void ShowWarning(HtmlGenericControl TemplateClass, bool hasWarningIcon)
        {
            if (hasWarningIcon)
            {
                WarningManagerWebControl_ = new WarningManagerWebControl(TemplateClass, 1, 70, 5);
                TemplateClass.Controls.Add(WarningManagerWebControl_);
            }

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
            var loginBuff = session[SessionHelper.LoggedIn]; // gets login state from current session    

            if (XmlController.IsDebugEnabled())
            {
                return;
            }

            if (loginBuff != null) // if there is no login state in session state
            {
                if (loginBuff.Equals(Val.LoggedIn))
                {
                    return; // if logged in continue 
                }
                else if (loginBuff.Equals(Val.LoggingIn)) // if value from sessionState is corect you can proceed on default page - if not than loginForm must be shown
                {
                    Redirect("Default"); // value from sessionState was correct
                    session[SessionHelper.LoggedIn] = Val.LoggedIn;
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
            var cur = HttpContext.Current;

            if (cur != null)
            {
                var session = cur.Session;
                if (session != null)
                {
                    return session;
                }
            }
            return null;
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
            readonly List<string> Scripts = new List<string>();
            readonly List<string> Keys = new List<string>();

            public ScriptLoader()
            {

            }

            public void RegisterScriptOnPageLoad(string key, string script)
            { // Register scripts any time in Page Load (currently in EveryPageProtocol() )
                // after this method call RegisterAllScriptsNow() to register all scripts added.
                Scripts.Add(script);
                Keys.Add(key);
            }

            public void RegisterAllScriptsNow(Page page, HtmlGenericControl divForScript)
            { // RegisterScriptOnPageLoad must be called first to add scripts
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

            public static ScriptLoader GetInstance(HttpSessionState session_canbeNull)
            {
                try
                {
                    HttpSessionState session;
                    if (session_canbeNull == null)
                    {
                        session = GetSession();
                    }
                    else
                    {
                        session = session_canbeNull;
                    }

                    if (session != null)
                    {
                        var obj = session[SessionHelper.ScriptLoader];  

                        if (obj == null)
                        {
                            session[SessionHelper.ScriptLoader] = NewScriptLoaderInstnance();
                        }

                        obj = session[SessionHelper.ScriptLoader];     // reasign new instance                    
                        var sl = (ScriptLoader)obj;                     // cast
                        return sl;

                    }
                    else
                    {
                        throw new Exception("Session was null");
                    }
                    
                }
                catch (Exception ex)
                {
                    SysLog.SetMessage("GetScriptLoaderInstance() has not found ScriptLoader instance. Some functions might be unavailable. More info:" +ex.Message);
                }

                return null;
            }
        }

        static ScriptLoader NewScriptLoaderInstnance()
        {
            return new ScriptLoader();
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

                    foreach (var item in Helper.UserDataManager.ActiveUsers)
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
            try
            {
                if (!messagePending)
                {
                    var scriptLoader = ScriptLoader.GetInstance(null);
                    if (scriptLoader != null)
                    {
                        var script = "alert('" + message + "');";
                        scriptLoader.RegisterScriptOnPageLoad("Info", script);                       
                    }
                    messagePending = true;
                    Refresh();
                }
                else
                {
                    messagePending = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Messagebox can not be shown. This error is displayed insted. ERROR MESSAGE: " + message + ". Reason for messagebox failure:" + ex.Message);
            }  
            
        }

        static void addMetaNoIndex(Page page)
        {
            if (!Settings.PreventSearchEnginesFromIndexing)
            {
                return;
            }
                        
            page.Header.Controls.Add(new LiteralControl("<meta name=\"robots\" content=\"noindex\">"));
        }

        static void addTabTitle(Page page, string pageTtitle)
        {
            string title = pageTtitle;
            page.Header.Title = char.ToUpper(title[0]) + title.Substring(1);
        }
    }
}