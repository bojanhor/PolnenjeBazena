using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class MasterMenu : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Meni", this, Session, TemplateClassID, false, true, true, true, true, true);
            Val.guiController.PageMastermenu_ = new GuiController.PageMasterMenu(this);
           
            Initialise();

        }

        private void Initialise()
        {

            TemplateClassID.Controls.Add(Val.guiController.PageMastermenu_.MasterbtnPannel);
            //createInitializePanel();           

            Val.guiController.PageMastermenu_.RegisterOnClick();

        }
    }
}


