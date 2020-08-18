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
            Navigator.EveryPageProtocol("Pogled dnevnika", this, Session, TemplateClassID, true, Navigator.PageLogoType.NoLogo);
            Val.guiController.PageLogView_ = new GuiController.PageLogView();            
                       
            Initialise();
            RegisterOnClick();
        }
                
        void Initialise()
        {                 
            if (!IsPostBack)
            {
                Editor.InnerText = SysLog.GetMessagesTB_large();
            }
            
            TemplateClassID.Controls.Add(Val.guiController.PageLogView_.refresh);
            TemplateClassID.Controls.Add(Val.guiController.PageLogView_.download);
        }

        public void RegisterOnClick()
        {            
            Val.guiController.PageLogView_.refresh.button.Click += Refresh_Click1;
            Val.guiController.PageLogView_.download.button.Click += Button_Click;

        }

        private void Button_Click(object sender, ImageClickEventArgs e)
        {
            var content = XmlController.DownloadLogFile();

            if (content != null)
            {
                Helper.DownloadFile(this, "txt", content);
            }
        }

        private void Refresh_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.RefreshFile_readAgain();
            Navigator.Refresh();
        }
                

    }
}