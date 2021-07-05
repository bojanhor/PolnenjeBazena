using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            try
            {
                Navigator.EveryPageProtocol("Dobrodošli", this, Session, TemplateClassID, true, Navigator.PageLogoType.WithLogo, false, true, true, true);

                Val.guiController.PageDefault_ = new GuiController.PageDefault(this, Session);
                Initialise();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }

        private void Initialise()
        {
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.ConvUP);            
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.divMaster);                       
            
            Val.guiController.PageDefault_.ConvUP.Controls_Add(Val.guiController.PageDefault_.divConveyor);
            
        }
               
        
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)           
        }
    }


}


