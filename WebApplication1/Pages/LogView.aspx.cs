using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageLogView : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PageLogView_ = new GuiController.PageLogView();
            Helper.EveryPageProtocol("Pogled dnevnika", this, Session, TemplateClassID, true, false);
                       
            Initialise();
            RegisterOnClick();
        }
                
        void Initialise()
        {            
           
            
            if (!IsPostBack)
            {
                Editor.InnerText = SysLog.MessageManager.GetLogFileContent();
            }
            
            TemplateClassID.Controls.Add(Val.guiController.PageLogView_.refresh);

        }

        public void RegisterOnClick()
        {            
            Val.guiController.PageLogView_.refresh.button.Click += Refresh_Click1;
        }

        private void Refresh_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.RefreshFile_readAgain();
            Helper.Refresh();
        }
                

    }
}