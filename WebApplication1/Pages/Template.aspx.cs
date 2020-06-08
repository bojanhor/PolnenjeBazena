using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageTemplate : System.Web.UI.Page
    {
              
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Template", this, Session, TemplateClass);
            Val.guiController.PageTemplate_ = new GuiController.EmptyTemplatePage(this, Session);           


            Initialise();           
        }
                      
        private void Initialise()
        {                        
            
        }
               
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)           
        }
    }


}


