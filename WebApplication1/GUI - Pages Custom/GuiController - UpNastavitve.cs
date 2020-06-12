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
        public class PageUpNastavitve : Dsps
        {
            private readonly string Name;
            private readonly Page thisPage;
            private readonly System.Web.SessionState.HttpSessionState session;
                       
            public GControls.UpdatePanelFull up;
                       
                                               
            public PageUpNastavitve(Page _thisPage, System.Web.SessionState.HttpSessionState session)
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

            private void ManageUpdatePanel()
            {
                try
                {
                    up = new GControls.UpdatePanelFull("upNastavitve_panel", Settings.UpdateValuesPCms);
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