using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Padavine : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PagePadavine_ = new GuiController.PagePadavine(this);

            Helper.EveryPageProtocol("Padavine", this, Session, TemplateClassID,true, false);
            Initialise();

            
        }

        private void Initialise()
        {
            TemplateClassID.Controls.Add(Val.guiController.PagePadavine_.MainDiv);

        }
                
    }
}


