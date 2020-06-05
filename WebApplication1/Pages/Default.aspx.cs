using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageDefault : System.Web.UI.Page
    {
              
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Dobrodošli", this, Session, TemplateClassID);
            Val.guiController.PageDefault_ = new GuiController.PageDefault(this, Session);           


            Initialise();           
        }
                      
        private void Initialise()
        {                        
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.Tmr_UpdatePanel);
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.UpdatePanel);
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.divMaster);
 
             Val.guiController.PageDefault_.RegisterOnClick();
        }
               
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)           
        }
    }


}


