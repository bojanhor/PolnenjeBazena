using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageDefault : System.Web.UI.Page
    {
        
        int SteviloLuci = XmlController.GetHowManyLucIcons();

        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Dobrodošli", this, Session, TemplateClassID);
            Val.guiController.PageDefault_ = new GuiController.PageDefault(this, Session);           


            Initialise();           
        }
                      
        private void Initialise()
        {
            CreateInitializePanel();
            
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.Tmr_UpdatePanel);
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.UpdatePanel);
            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.btnPannel);

            Val.guiController.PageDefault_.RegisterOnClick();
        }
               
        void CreateInitializePanel()
        {
            // vreme
            Timer2.Interval = Settings.UpdateValuesPCms * 5;
            TemperaturePanel.ContentTemplateContainer.Controls.Add(
            Val.guiController.PageDefault_.divWeather);

            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.divVremeLevo);
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)           
        }
    }


}


