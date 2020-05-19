using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageBlocked : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Navigator.EveryPageProtocol("Blocked", this, Session, TemplateClassID,false, true,false, false, false, false);

            var top = 20;
            var left = 27;
                        
            var fontSize = (top + left) / 20F;

            float lblLeft = 5;
            float lbltopoff = 12;

            float tbLeft = 3;           
            
            ID = "BlockedForm";

            var width = 100 - (left * 2) - 10;
            var height = 100 - top * 3;

            var gb = new GuiController.GControls.GroupBox(top,left,width, height);
            
            tbLeft += fontSize * 10 + lblLeft;
            
            //
            var Title = new Label()
            {
                Text = "Blokirani ste s sistema!"                
            };

            Title.Style.Add("position", "absolute");
            Title.Style.Add(HtmlTextWriterStyle.FontSize, fontSize + "vw");
            Title.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

            GuiController.SetControlAbsolutePos(Title, lbltopoff, tbLeft - 2);
            gb.Controls.Add(Title);
            TemplateClassID.Controls.Add(gb);

        }
    }
}