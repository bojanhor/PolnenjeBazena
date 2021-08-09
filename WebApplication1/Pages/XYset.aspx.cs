using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageXYset : System.Web.UI.Page
    {
              
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("XYset", this, Session, TemplateClass, true, Navigator.PageLogoType.WithEmtySpaceForLogo, true, true, true, true);
            Val.guiController.PageXYset_ = new GuiController.PageXYset(this, Session);         


            Initialise();           
        }
                      
        private void Initialise()
        {
            Val.guiController.PageXYset_.UP.Controls_Add(Val.guiController.PageXYset_.gb_main);
            TemplateClass.Controls.Add(Val.guiController.PageXYset_.UP);
        }

        
               
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)           
        }
    }


}


