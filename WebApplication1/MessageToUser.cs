using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace WebApplication1
{
    public class MessageToUser : System.Web.UI.WebControls.WebControl
    {
        public bool Show { get; private set; } = false;

      
        private GuiController.GControls.GroupBox gb = new GuiController.GControls.GroupBox(0, 0, 60, 60);
        private GuiController.GControls.SuperLabel lab, txt;
        public GuiController.GControls.ButtonWithLabel Okbtn = new GuiController.GControls.ButtonWithLabel("OK", 25.0F, 2.0F);

        public MessageToUser()
        {
           
       
        }

        public void Button_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {// triggered from navigator
            Show = false;
            Navigator.Refresh();
        }

        public void ShowMessage(string title, string text)
        {
            Show = true;
            GuiController.SetControlAbsolutePos_vw(this, 10, 30, 60, 40);
            this.ID = "messageToUser";
            gb.ID = "messageToUserGB";
            this.Style.Add(System.Web.UI.HtmlTextWriterStyle.ZIndex, "99");
            gb.Style.Add(System.Web.UI.HtmlTextWriterStyle.BackgroundColor, "orange");
            GuiController.SetControlAbsolutePos(Okbtn, 70, 37);
            
            txt = new GuiController.GControls.SuperLabel(text, 30, 6, 90, 15);
            lab = new GuiController.GControls.SuperLabel(title, 5, 5, 90, 20);
            lab.FontWeightBold = true;
            lab.FontSize = 1.5F;
            txt.FontSize = 1.0F;

            this.Controls.Add(gb);
            gb.Controls.Add(lab);
            gb.Controls.Add(txt);
            gb.Controls.Add(Okbtn);

          
            Navigator.Refresh();
        }

    }
}