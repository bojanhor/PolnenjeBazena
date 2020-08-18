using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class UpNastavitve : System.Web.UI.Page
    {
              
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Uporabniske Nastavitve", this, Session, TemplateClassID, true, Navigator.PageLogoType.NoLogo, true, true, true, true);
            Val.guiController.UpNastavitve_ = new GuiController.PageUpNastavitve(this, Session);           


            Initialise();           
        }
                      
        private void Initialise()
        {                        
            
        }
                
    }


}


