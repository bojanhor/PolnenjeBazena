using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
   
    public partial class Razsvetljava : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Razsvetljava", this, Session, TemplateClassID);
            Val.guiController.PageRazsvetljava_ = new GuiController.PageRazsvetljava(this, Session);            
            
            Initialise();            
           
        }   

        private void Initialise()
        {

            TemplateClassID.Controls.Add(Val.guiController.PageRazsvetljava_.divStala);
            CreateInitializePanel();
      
            Val.guiController.PageRazsvetljava_.RegisterOnClick();

        }
        private void AddLuci()
        {
            foreach (var item in Val.guiController.PageRazsvetljava_.Luc)
            {
                if (item != null)
                {
                    if (item.Width != "0")
                    {
                        LuciPanel.ContentTemplateContainer.Controls.Add(item);     
                        
                    }
                }
            }

            foreach (var item in Val.guiController.PageRazsvetljava_.AboveLucLableDimmer)
            {
                if (item != null)
                {
                    LuciPanel.ContentTemplateContainer.Controls.Add(item);
                }
                
            }
        }
        void CreateInitializePanel()
        {

            Timer1.Interval = Settings.UpdateValuesPCms;
            
            LuciPanel.ContentTemplateContainer.Controls.Add(
                Val.guiController.PageRazsvetljava_.divStala);
            AddLuci();

            TemplateClassID.Controls.Add(Val.guiController.PageRazsvetljava_.divBtns);
                  
            UpdatePanelSettings.ContentTemplateContainer.Controls.Add(Val.guiController.PageRazsvetljava_.divLuciSettings);
                                               
        }

    
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)

        }

    }
}