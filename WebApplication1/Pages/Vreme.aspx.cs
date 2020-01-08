using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageVreme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PageVreme_ = new GuiController.PageVreme();

            Helper.EveryPageProtocol("Vreme", this, Session, TemplateClassID);
            Set();
            Initialise();
            
        }

        private void Initialise()
        {
            
        }

        void Set()
        {

            string link = "https://vreme.arso.gov.si/napoved/Letali%C5%A1%C4%8De%20Jo%C5%BEeta%20Pu%C4%8Dnika%20Ljubljana/graf/0";
            vreme.Attributes.Add("Src", link);

            vreme.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            vreme.Style.Add(HtmlTextWriterStyle.Width, "50%");
            vreme.Style.Add(HtmlTextWriterStyle.Height, "84%");
            vreme.Style.Add(HtmlTextWriterStyle.Top, "10%");
            vreme.Style.Add(HtmlTextWriterStyle.Left, "28%");

                        

            


        }
        
    }
}
