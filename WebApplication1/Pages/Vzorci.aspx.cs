using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageVzorci : System.Web.UI.Page
    {
              
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Vzorci", this, Session, TemplateClass, true, Navigator.PageLogoType.WithEmtySpaceForLogo, true, true, true, true);
            Val.guiController.PageVzorci_ = new GuiController.PageVzorci(this, Session);         


            Initialise();           
        }
                      
        private void Initialise()
        {
            Val.guiController.PageVzorci_.UP.Controls_Add(Val.guiController.PageVzorci_.gb_Rob);
            Val.guiController.PageVzorci_.UP.Controls_Add(Val.guiController.PageVzorci_.gb_ZigZag);
            TemplateClass.Controls.Add(Val.guiController.PageVzorci_.Opozorilo);
            TemplateClass.Controls.Add(Val.guiController.PageVzorci_.UP);
            TemplateClass.Controls.Add(Val.guiController.PageVzorci_.GlobalControls);
        }

        
               
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)           
        }
    }


}


