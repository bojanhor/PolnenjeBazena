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
                       
            InitialiseEditor();
            RegisterOnClickEditor();
        }
                
        void InitialiseEditor()
        {            
           
            
            if (!IsPostBack)
            {
                Editor.InnerText = XmlController.GetXMLTextAndStopRefreshing();
            }

            TemplateClassID.Controls.Add(Val.guiController.PageEditor_.save);
            TemplateClassID.Controls.Add(Val.guiController.PageEditor_.refresh);

        }

        public void RegisterOnClickEditor()
        {
            Val.guiController.PageEditor_.save.button.Click += Save_ClickEditor;
            Val.guiController.PageEditor_.refresh.button.Click += Refresh_ClickEditor;
        }

        private void Refresh_ClickEditor(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.RefreshFile_readAgain();
            Helper.Refresh();
        }

        private void Save_ClickEditor(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.SaveCurrentTB(Editor.InnerText);
            Refresh_ClickEditor(null, null);
        }

    }
}