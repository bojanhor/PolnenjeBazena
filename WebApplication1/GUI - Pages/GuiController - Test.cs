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

        public class PageTest : Dsps
        {
            HtmlGenericControl TemplateClassID;
            Page thisPage;
            System.Web.SessionState.HttpSessionState session;

            public PageTest(Page _thisPage, System.Web.SessionState.HttpSessionState session, HtmlGenericControl TemplateClass)
            {
                this.thisPage = _thisPage;
                this.session = session;
                TemplateClassID = TemplateClass;
                TestLogin();
            }

            void TestLogin()
            {
                var l = new GControls.LogMeIn(thisPage, session);
                TemplateClassID.Controls.Add(l);
            }
        }
    }
}