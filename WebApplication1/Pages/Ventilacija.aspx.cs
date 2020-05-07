using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class Ventilacija : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Ventilacija", this, Session, TemplateClassID);
            Val.guiController.PageVentilacija_ = new GuiController.PageVentilacija();
            
            Initialise();
           
        }

        
        void Initialise()
        {
            TemplateClassID.Controls.Add(Val.guiController.PageVentilacija_.subMenu);            
        }
    }
}