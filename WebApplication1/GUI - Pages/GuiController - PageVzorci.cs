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
            
            readonly Prop1 prop1 = Val.logocontroler.Prop1;
            readonly Prop2 prop2 = Val.logocontroler.Prop2;
            readonly System.Web.SessionState.HttpSessionState session;
            public GControls.UpdatePanelFull UP = new GControls.UpdatePanelFull("PageUpdatePanel", Settings.UpdateValuesPCms);


            public GControls.GroupBox gb_Rob; public GControls.GroupBox gb_ZigZag; public HtmlGenericControl GlobalControls;
            List<GControls.ImageButtonWithID> RobList = new List<GControls.ImageButtonWithID>();
            GControls.ImageButtonWithID RobX1, RobY1, RobX2, RobY2;
            GControls.ImageButtonWithID ZigZag_zRobom;
            GControls.ImageButtonWithID ZigZag;
            GControls.ImageButtonWithID Krozenje;
            GControls.ImageButtonWithID Krog;

            GControls.SuperLabel lbl_PolnenjeRobu; GControls.SuperLabel lbl_PolnenjeZigZag;
            public GControls.SuperLabel Opozorilo;



            public PageVzorci(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                this.session = session;
                this.thisPage = _thisPage;
                RobX1 = new GControls.ImageButtonWithID("X1", 0); if (prop1.RobX1_read.Value_bool) { RobX1.ImageUrl = "~/Pictures/CircX1.png"; } else { RobX1.ImageUrl = "~/Pictures/CircX1_off.png"; };
                RobY1 = new GControls.ImageButtonWithID("Y1", 0); if (prop1.RobY1_read.Value_bool) { RobY1.ImageUrl = "~/Pictures/CircY1.png"; } else { RobY1.ImageUrl = "~/Pictures/CircY1_off.png"; };
                RobX2 = new GControls.ImageButtonWithID("X2", 0); if (prop1.RobX2_read.Value_bool) { RobX2.ImageUrl = "~/Pictures/CircX2.png"; } else { RobX2.ImageUrl = "~/Pictures/CircX2_off.png"; };
                RobY2 = new GControls.ImageButtonWithID("Y2", 0); if (prop1.RobY2_read.Value_bool) { RobY2.ImageUrl = "~/Pictures/CircY2.png"; } else { RobY2.ImageUrl = "~/Pictures/CircY2_off.png"; };

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
                ZigZag = new GControls.ImageButtonWithID("Zig", 0); if (prop1.ZigZag_read.Value_bool) { ZigZag.ImageUrl = "~/Pictures/Zig1_on.png"; } else { ZigZag.ImageUrl = "~/Pictures/Zig1.png"; };
                gb_ZigZag.Controls.Add(ZigZag); SetControlAbsolutePos(ZigZag, top, left, size);

                ZigZag.Click += ZigZag_Click;                
               
                ZigZag_zRobom = new GControls.ImageButtonWithID("ZigzRobom", 0); if (prop1.ZigZagzRobom_read.Value_bool) { ZigZag_zRobom.ImageUrl = "~/Pictures/Zig1zRobom_on.png"; } else { ZigZag_zRobom.ImageUrl = "~/Pictures/Zig1zRobom.png"; };
                gb_ZigZag.Controls.Add(ZigZag_zRobom); SetControlAbsolutePos(ZigZag_zRobom, top+ offset, left, size);

                ZigZag_zRobom.Click += ZigZag_zRobom_Click;
                              
                                
                Opozorilo = new GControls.SuperLabel("Pred začetkom dela preverite, da je izbran pravi bazen!", 13, 30, 60, 10) { FontSize = 1.5F };
                Opozorilo.Style.Add(HtmlTextWriterStyle.Color, Settings.RedColorHtmlHumar);
                Opozorilo.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

                CreateGlobalControls();

            }

            public void CreateGlobalControls()
            {
                var prop = Val.logocontroler.Prop1;

                GControls.UpdatePanelFull up = new GControls.UpdatePanelFull("up_startPause", Settings.UpdateValuesPCms);
                GControls.StartPauseButton Start = new GControls.StartPauseButton("Start", 0, prop.Man_AutoReadState.Value_bool, prop.Start, prop.Pause);
                SetControlAbsolutePos(Start, 2, 2, 50);

                GlobalControls = DIV.CreateDivAbsolute(1,1,20,20,"%");
                up.Controls_Add(Start);
                GlobalControls.Controls.Add(up);
            }

            private void ZigZag_zRobom_Click(object sender, ImageClickEventArgs e)
            {
                Val.Kontrola.StopPatern();
                prop1.ZigZagzRobom.SendPulse();
                if (Val.Kontrola.StartZigZagzRobom())
                {
                    //Navigator.Redirect("Default");
                }
                else
                {
                    //Navigator.MessageBox(general_err_start_msg);
                }
            }

            private void ZigZag_Click(object sender, ImageClickEventArgs e)
            {
                
                Val.Kontrola.StopPatern();
                prop1.ZigZag.SendPulse();
                if (Val.Kontrola.StartZigZag())
                {                    
                    //Navigator.Redirect("Default");
                }
                else
                {
                    //Navigator.MessageBox(general_err_start_msg);
                }
            }

            private void RobY2_Click(object sender, ImageClickEventArgs e)
            {
                
                Val.Kontrola.StopPatern();
                prop1.RobY2.SendPulse();
                if (Val.Kontrola.StartRobY2())
                {
                    
                    //Navigator.Redirect("Default");
                }
                else
                {
                    //Navigator.MessageBox(general_err_start_msg);
                }
            }

            private void RobX2_Click(object sender, ImageClickEventArgs e)
            {
                
                Val.Kontrola.StopPatern();
                prop1.RobX2.SendPulse();
                if (Val.Kontrola.StartRobX2())
                {
                    
                    //Navigator.Redirect("Default");
                }
                else
                {
                    //Navigator.MessageBox(general_err_start_msg);
                }
            }

            private void RobY1_Click(object sender, ImageClickEventArgs e)
            {
                
                Val.Kontrola.StopPatern();
                prop1.RobY1.SendPulse();
                if (Val.Kontrola.StartRobY1())
                {
                    
                    //Navigator.Redirect("Default");
                }
                else
                {
                    //Navigator.MessageBox(general_err_start_msg);
                }
            }

            private void RobX1_Click(object sender, ImageClickEventArgs e)
            {
                
                Val.Kontrola.StopPatern();
                prop1.RobX1.SendPulse();
                if (Val.Kontrola.StartRobX1())
                {
                    
                    //Navigator.Redirect("Default");
                }
                else
                {
                    //Navigator.MessageBox(general_err_start_msg);
                }
            }
        }
    }
}