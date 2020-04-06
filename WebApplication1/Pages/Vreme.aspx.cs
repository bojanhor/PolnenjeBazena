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

            Helper.EveryPageProtocol("Vreme", this, Session, TemplateClassID, true, false);
            Set();
            Initialise();
            
        }

        private void Initialise()
        {
            
        }

        void Set()
        {

            string link = XmlController.GetPageVremeLink();

            vreme.Attributes.Add("Src", link);

            var vremewidth = XmlController.GetPageVremeWidth();

            vreme.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            vreme.Style.Add(HtmlTextWriterStyle.Width, vremewidth + "%");
            vreme.Style.Add(HtmlTextWriterStyle.Height, "88%");
            vreme.Style.Add(HtmlTextWriterStyle.Top, "8%");
            vreme.Style.Add(HtmlTextWriterStyle.Left, 100 - vremewidth + "%");


        }
        
    }
}
