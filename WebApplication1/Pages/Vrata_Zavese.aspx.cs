using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class Vrata_Zavese : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PageVrataZavese_ = new GuiController.PageVrataZavese(this, Session);

            Helper.EveryPageProtocol("Vrata", this, Session, TemplateClassID);
            Initialise();
           
        }

        
        void Initialise()
        {
            TemplateClassID.Controls.Add(Val.guiController.PageVrataZavese_.subMenu);            
        }
    }
}