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
        public class PageXYset : Dsps
        {
            
            readonly Page thisPage;
            
            readonly Prop1 prop1 = Val.logocontroler.Prop1;
            readonly Prop2 prop2 = Val.logocontroler.Prop2;
            readonly System.Web.SessionState.HttpSessionState session;
            public GControls.UpdatePanelFull UP = new GControls.UpdatePanelFull("PageUpdatePanel", Settings.UpdateValuesPCms);


            public GControls.GroupBox gb_left, gb_right;

            GControls.ImageButtonWithID incx, incy, decx, decy;
            GControls.ImageButtonWithID incx2, incy2, decx2, decy2;

            GControls.SuperLabel x, y, x2, y2, title_l, title_r;

            int t = 25, l = 10, w = 60, h = 30, offsetBtns = 25;
            int t2;
            int spcng = 10, size = 15;

            string fontsize = "2vw";

            public PageXYset(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                this.session = session;
                this.thisPage = _thisPage;


                gb_left = new GControls.GroupBox(20, 5, 40, 70);
                gb_right = new GControls.GroupBox(20, 50, 40, 70);

                t2 = t + h + spcng;

                title_l = new GControls.SuperLabel("Pomik od: ", 5, 5, 25, 15, "1.5vw"); gb_left.Controls.Add(title_l);
                title_r = new GControls.SuperLabel("Pomik do: ", 5, 5, 25, 15, "1.5vw"); gb_right.Controls.Add(title_r);

                x = new GControls.SuperLabel("X: " + getImpulsesXTo(), t, l, w, h, fontsize); gb_right.Controls.Add(x); 
                y = new GControls.SuperLabel("Y: " + getImpulsesYTo(), t2, l, w, h, fontsize); gb_right.Controls.Add(y);
                x2 = new GControls.SuperLabel("X: " + getImpulsesXFrom(), t, l, w, h, fontsize); gb_left.Controls.Add(x2);
                y2 = new GControls.SuperLabel("Y: " + getImpulsesYFrom(), t2, l, w, h, fontsize); gb_left.Controls.Add(y2);


                incx = new GControls.ImageButtonWithID(1) { ImageUrl = Helper.ImageUrl("up_press") }; incx.Click += Incx_Click;
                incy = new GControls.ImageButtonWithID(2) { ImageUrl = Helper.ImageUrl("up_press")}; incy.Click += Incy_Click;
                decx = new GControls.ImageButtonWithID(3) { ImageUrl = Helper.ImageUrl("dwn_press") }; decx.Click += Decx_Click;
                decy = new GControls.ImageButtonWithID(4) { ImageUrl = Helper.ImageUrl("dwn_press") }; decy.Click += Decy_Click;
                incx2 = new GControls.ImageButtonWithID(5) { ImageUrl = Helper.ImageUrl("up_press") }; incx2.Click += Incx2_Click;
                 incy2 = new GControls.ImageButtonWithID(6) { ImageUrl = Helper.ImageUrl("up_press") }; incy2.Click += Incy2_Click;
                 decx2 = new GControls.ImageButtonWithID(7) { ImageUrl = Helper.ImageUrl("dwn_press") }; decx2.Click += Decx2_Click;
                 decy2 = new GControls.ImageButtonWithID(8) { ImageUrl = Helper.ImageUrl("dwn_press") }; decy2.Click += Decy2_Click;

                SetControlAbsolutePos(incx, t - spcng, l + offsetBtns, size); gb_left.Controls.Add(incx);
                SetControlAbsolutePos(incy, t2 - spcng, l + offsetBtns, size); gb_left.Controls.Add(incy);
                SetControlAbsolutePos(decx, t + spcng, l + offsetBtns, size); gb_left.Controls.Add(decx);
                SetControlAbsolutePos(decy, t2 + spcng, l + offsetBtns, size); gb_left.Controls.Add(decy);

                SetControlAbsolutePos(incx2, t - spcng, l + offsetBtns, size); gb_right.Controls.Add(incx2);
                SetControlAbsolutePos(incy2, t2 - spcng, l + offsetBtns, size); gb_right.Controls.Add(incy2);
                SetControlAbsolutePos(decx2, t + spcng, l + offsetBtns, size); gb_right.Controls.Add(decx2);
                SetControlAbsolutePos(decy2, t2 + spcng, l + offsetBtns, size); gb_right.Controls.Add(decy2);

            }

            bool bazenSelected()
            {
                if (prop1.ImpulsesDisplayVal.Value_short > 0)
                {
                    return true;
                }
                return false;
            }

            string getImpulsesXFrom()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return PropComm.NA;
                }

                return XmlController.GetBazenTypeXImpulses2(bazen).ToString();

            }

            string getImpulsesXTo()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return PropComm.NA;
                }

                return XmlController.GetBazenTypeXImpulses(bazen).ToString();

            }

            string getImpulsesYFrom()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return PropComm.NA;
                }

                return XmlController.GetBazenTypeYImpulses2(bazen).ToString();

            }

            string getImpulsesYTo()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return PropComm.NA;
                }

                return XmlController.GetBazenTypeYImpulses(bazen).ToString();

            }

            short getImpulsesXFrom_short()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return 0;
                }

                return XmlController.GetBazenTypeXImpulses2(bazen);

            }

            short getImpulsesXTo_short()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return 0;
                }

                return XmlController.GetBazenTypeXImpulses(bazen);

            }

            short getImpulsesYFrom_short()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return 0;
                }

                return XmlController.GetBazenTypeYImpulses2(bazen);

            }

            short getImpulsesYTo_short()
            {
                var bazen = prop1.ImpulsesDisplayVal.Value_short;

                if (!bazenSelected())
                {
                    return 0;
                }

                return XmlController.GetBazenTypeYImpulses(bazen);

            }

            // onclicks -------

            // Right gb - decrease Y TO
            private void Decy2_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesYTo_short();
                val--;
                XmlController.SetBazenTypeYImpulses(bazen, val);
            }

            // Right gb - decrease X  TO
            private void Decx2_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesXTo_short();
                val--;
                XmlController.SetBazenTypeXImpulses(bazen, val);
            }

            // Right gb - increase Y  TO
            private void Incy2_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesYTo_short();
                val++;
                XmlController.SetBazenTypeYImpulses(bazen, val);
            }

            // Right gb - increase X TO
            private void Incx2_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesXTo_short();
                val++;
                XmlController.SetBazenTypeXImpulses(bazen, val);

            }


            // Left gb - decrease Y FROM
            private void Decy_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesYFrom_short();
                val--;
                XmlController.SetBazenTypeYImpulses2(bazen, val);

            }

            // Left gb - decrease X FROM
            private void Decx_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesXFrom_short();
                val--;
                XmlController.SetBazenTypeXImpulses2(bazen, val);

            }

            // Left gb - increase Y FROM
            private void Incy_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesYFrom_short();
                val++;
                XmlController.SetBazenTypeYImpulses2(bazen, val);

            }

            // Left gb - increase X FROM
            private void Incx_Click(object sender, ImageClickEventArgs e)
            {
                if (!bazenSelected())
                {
                    errmsg(); return;
                }

                var bazen = prop1.ImpulsesDisplayVal.Value_short;
                short val = getImpulsesXFrom_short();
                val++;
                XmlController.SetBazenTypeXImpulses2(bazen, val);
                
            }

            void errmsg()
            {
                Navigator.messageToUser.ShowMessage("Napaka", "Prosimo poskrbite, da je naprava zagnana, da je povezava vzpostavljena, in izbran pravi bazen!");
            }


        }
    }
}