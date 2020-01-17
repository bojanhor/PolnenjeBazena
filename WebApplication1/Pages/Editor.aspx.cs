using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageEditor : System.Web.UI.Page
    {        
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PageEditor_ = new GuiController.PageEditor();
            Helper.EveryPageProtocol("Zahtevne Nastavitve", this, Session, TemplateClassID, true, false);
                       
            Initialise();
            RegisterOnClick();
        }
                
        void Initialise()
        {            
           
            
            if (!IsPostBack)
            {
                Editor.InnerText = XmlController.GetXMLTextAndStopRefreshing();
            }

            TemplateClassID.Controls.Add(Val.guiController.PageEditor_.save);
            TemplateClassID.Controls.Add(Val.guiController.PageEditor_.refresh);

        }

        public void RegisterOnClick()
        {
            Val.guiController.PageEditor_.save.button.Click += Save_Click;
            Val.guiController.PageEditor_.refresh.button.Click += Refresh_Click1;
        }

        private void Refresh_Click1(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.RefreshFile_readAgain();
            Helper.Refresh();
        }

        private void Save_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.SaveCurrentTB(Editor.InnerText);
            Refresh_Click1(null, null);
        }

    }
}