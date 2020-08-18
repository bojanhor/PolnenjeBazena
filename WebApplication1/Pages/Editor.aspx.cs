using System;
using System.Collections.Generic;
using System.IO;
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
            Navigator.EveryPageProtocol("Zahtevne Nastavitve", this, Session, TemplateClassID, true, Navigator.PageLogoType.NoLogo, true, true, true, true);
            Val.guiController.PageEditor_ = new GuiController.PageEditor();            
                       
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
            TemplateClassID.Controls.Add(Val.guiController.PageEditor_.downloadConfig);

        }

        public void RegisterOnClickEditor()
        {
            Val.guiController.PageEditor_.save.button.Click += Save_ClickEditor;
            Val.guiController.PageEditor_.refresh.button.Click += Refresh_ClickEditor;
            Val.guiController.PageEditor_.downloadConfig.button.Click += Download_Click;
        }

        private void Download_Click(object sender, ImageClickEventArgs e)
        {
            var content = XmlController.DownloadConfigFile();

            if (content != null)
            {
                Helper.DownloadFile(this, "xml", content);                
            }
        }

        private void Refresh_ClickEditor(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.RefreshFile_readAgain();
            Navigator.Refresh();
        }

        private void Save_ClickEditor(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            XmlController.SaveCurrentTB(Editor.InnerText);
            Refresh_ClickEditor(null, null);
        }

    }
}