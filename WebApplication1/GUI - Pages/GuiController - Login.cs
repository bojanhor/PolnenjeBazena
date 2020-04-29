using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class GuiController
    {

        public class PageLogin : Dsps
        {
            HtmlGenericControl TemplateClassID;
            Page thisPage;
            System.Web.SessionState.HttpSessionState session;

            public PageLogin(Page _thisPage, System.Web.SessionState.HttpSessionState session, HtmlGenericControl TemplateClass)
            {
                this.thisPage = _thisPage;
                this.session = session;
                TemplateClassID = TemplateClass;
                Login();
            }

            void Login()
            {
                var l = new GControls.LogMeIn(thisPage, session);                
                TemplateClassID.Controls.Add(l);
            }
        }
    }
}