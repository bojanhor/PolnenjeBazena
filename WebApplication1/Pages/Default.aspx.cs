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
            Val.guiController.PageDefault_ = new GuiController.PageDefault(this);

            Helper.EveryPageProtocol("Dobrodošli", this, Session, TemplateClassID);
            Initialise();

            
        }

        private void Initialise()
        {

            TemplateClassID.Controls.Add(Val.guiController.PageDefault_.btnPannel);
            CreateInitializePanel();

            Val.guiController.PageDefault_.RegisterOnClick();

        }

        private void AddLuci()
        {
            foreach (var item in Val.guiController.PageDefault_.Luc)
            {
                if (item != null)
                {
                    if (item.Width != "0")
                    {
                        LuciPanel.ContentTemplateContainer.Controls.Add(item);

                    }
                }
            }
        }

        void CreateInitializePanel()
        {
            Timer1.Interval = Settings.UpdateValuesPCms;
            LuciPanel.ContentTemplateContainer.Controls.Add(
                Val.guiController.PageDefault_.divStala);
            AddLuci();

            Timer2.Interval = Settings.UpdateValuesPCms * 5;
            TemperaturePanel.ContentTemplateContainer.Controls.Add(
                Val.guiController.PageDefault_.divWeather);


        }


        protected void Timer1_Tick(object sender, EventArgs e)
        {
            // Updates panel implicitly (with postback whole class is recreated)

        }
    }


}


