using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class EmptyTemplatePage : Dsps
        {
            readonly string Name;
            readonly Page thisPage;
            readonly System.Web.SessionState.HttpSessionState session;
                       
            public UpdatePanel UpdatePanel;
            public AsyncPostBackTrigger Ap_UpdatePanel;
            public Timer Tmr_UpdatePanel;
                       
                                               
            public EmptyTemplatePage(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                this.session = session;
                Name = PageHistory.GetPageNameFromPage(_thisPage);

                try
                {
                    thisPage = _thisPage;                    
                    ManageUpdatePanel();
                  

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside class constructor ( Pagename: "+ Name + "): " + ex.Message);
                }
            }

            void ManageUpdatePanel()
            {
                try
                {
                    UpdatePanel = new UpdatePanel
                    {
                        UpdateMode = UpdatePanelUpdateMode.Conditional,
                        ID = Name +"UpdatePanel"
                    };

                    Tmr_UpdatePanel = new Timer
                    {
                        Interval = Settings.UpdateValuesPCms,
                        ID = Name + "Tmr_UpdatePanel"
                    };

                    Ap_UpdatePanel = new AsyncPostBackTrigger
                    {
                        ControlID = Name + "UpdatePanel"
                    };

                    UpdatePanel.Triggers.Add(Ap_UpdatePanel);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error was encountered inside ManageUpdatePanel() method. page visited: "+ Name +". Error details: " + ex.Message);
                }
                
            }
            
            public void RegisterOnClick()
            {

            }
                                  
        }
    }
}