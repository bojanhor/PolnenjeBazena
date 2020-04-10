using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class MesalaPehala : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PageMesalaPehala_ = new GuiController.PageMesalaPehala();

            Helper.EveryPageProtocol("Mesala / Pehala", this, Session, TemplateClassID);
            Initialise();
           
        }

        
        void Initialise()
        {
            TemplateClassID.Controls.Add(Val.guiController.PageMesalaPehala_.subMenu);            
        }
    }
}