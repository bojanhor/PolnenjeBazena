using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Deployment.Internal;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class PageVzorci : Dsps
        {
            readonly Page thisPage;
            readonly string Name;
            readonly Prop1 prop1 = Val.logocontroler.Prop1;
            readonly Prop2 prop2 = Val.logocontroler.Prop2;
            readonly System.Web.SessionState.HttpSessionState session;
            public GControls.UpdatePanelFull UP = new GControls.UpdatePanelFull("PageUpdatePanel", Settings.UpdateValuesPCms);


            public GControls.GroupBox gb_Rob; public GControls.GroupBox gb_ZigZag;
            List<GControls.ImageButtonWithID> RobList = new List<GControls.ImageButtonWithID>();
            GControls.ImageButtonWithID RobX1, RobY1, RobX2, RobY2;
            GControls.ImageButtonWithID ZigZag;
            GControls.SuperLabel lbl_PolnenjeRobu; GControls.SuperLabel lbl_PolnenjeZigZag;
            GControls.SuperLabel stplbl;
            GControls.DropDownListForTimer_1_30s TimeZigY;


            public PageVzorci(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                this.session = session;               
                RobX1 = new GControls.ImageButtonWithID("X1", 0) { ImageUrl = "~/Pictures/CircX1.png" };
                RobY1 = new GControls.ImageButtonWithID("Y1", 0) { ImageUrl = "~/Pictures/CircY1.png" };
                RobX2 = new GControls.ImageButtonWithID("X2", 0) { ImageUrl = "~/Pictures/CircX2.png" };
                RobY2 = new GControls.ImageButtonWithID("Y2", 0) { ImageUrl = "~/Pictures/CircY2.png" };
                RobList.Add(RobX1); RobList.Add(RobY1); RobList.Add(RobX2); RobList.Add(RobY2);
                gb_Rob = new GControls.GroupBox(20, 5, 40, 70);
                
                lbl_PolnenjeRobu = new GControls.SuperLabel("POLNENJE ROBU", 5, 20, 50, 5) { FontSize = 1.5F }; gb_Rob.Controls.Add(lbl_PolnenjeRobu);

                var top = 13; var offset = 20; var left = 20; var buff = 0; var size = 30;
                foreach (var item in RobList)
                {
                    gb_Rob.Controls.Add(item); SetControlAbsolutePos(item, top + buff, left, size); buff += offset;                    
                }

                RobX1.Click += RobX1_Click; RobY1.Click += RobY1_Click; RobX2.Click += RobX2_Click; RobY2.Click += RobY2_Click;


                gb_ZigZag = new GControls.GroupBox(20, 50, 40, 70);
                lbl_PolnenjeZigZag = new GControls.SuperLabel("POLNENJE ZigZag", 5, 20, 50, 5) { FontSize = 1.5F };
                gb_ZigZag.Controls.Add(lbl_PolnenjeZigZag);
                ZigZag = new GControls.ImageButtonWithID("Zig", 0) {ImageUrl = "~/Pictures/Zig1_on.png" };
                gb_ZigZag.Controls.Add(ZigZag); SetControlAbsolutePos(ZigZag, top, left, size);

                ZigZag.Click += ZigZag_Click;

                // Y step Time
                stplbl = new GControls.SuperLabel("Korak:", top, left + 46, 20, 10) { FontSize = 1.2F };
                TimeZigY = new GControls.DropDownListForTimer_1_30s("Stepsel", prop1.TimeZigY.Value_string, top + 4, left + 38, 5, 1.5F, true, false);
                TimeZigY.SaveClicked += TimeZigY_SaveClicked;
                gb_ZigZag.Controls.Add(TimeZigY);
                gb_ZigZag.Controls.Add(stplbl);

            }

            private void TimeZigY_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
            {
                Val.logocontroler.Prop1.TimeZigY.Value_short = TimeZigY.GetSelectedValue();                
            }

            private void ZigZag_Click(object sender, ImageClickEventArgs e)
            {
                prop1.ZigZag.SendPulse();
                Navigator.Redirect("Default");
            }

            private void RobY2_Click(object sender, ImageClickEventArgs e)
            {
                prop1.RobY2.SendPulse();
                Navigator.Redirect("Default");
            }

            private void RobX2_Click(object sender, ImageClickEventArgs e)
            {
                prop1.RobX2.SendPulse();
                Navigator.Redirect("Default");
            }

            private void RobY1_Click(object sender, ImageClickEventArgs e)
            {
                prop1.RobY1.SendPulse();
                Navigator.Redirect("Default");
            }

            private void RobX1_Click(object sender, ImageClickEventArgs e)
            {
                prop1.RobX1.SendPulse();
                Navigator.Redirect("Default");
            }
        }
    }
}