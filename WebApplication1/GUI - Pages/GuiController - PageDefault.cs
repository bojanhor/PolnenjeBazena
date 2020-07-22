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
        public class PageDefault : Dsps
        {
            readonly Page thisPage;
            readonly string Name;
            readonly Prop1 prop1 = Val.logocontroler.Prop1; 
            readonly Prop2 prop2 = Val.logocontroler.Prop2;
            readonly System.Web.SessionState.HttpSessionState session;
            public GControls.MasterMenuButton[] imagebuttons;

            public GControls.UpdatePanelFull UP = new GControls.UpdatePanelFull("PageUpdatePanel", Settings.UpdateValuesPCms);
            public GControls.UpdatePanelFull ConvUP;

            public HtmlGenericControl divMaster;
            public HtmlGenericControl divConveyor;
            GControls.DropDownListForDimmerRPM speed;
            GControls.DropDownListForTimer_1_30s TimeZigY;
            GControls.GroupBox gb1;
            GControls.ImageButtonWithID btnStart;
            GControls.ImageButtonWithID btnStop;
            GControls.OnOffButton btnAuto;
            GControls.OnOffButton btnTrak;
            GControls.ImageButtonWithID btnCirc;
            GControls.ImageButtonWithID btnZig;

            // Semaphore
            GControls.GroupBox gb2;
            GControls.OnOffShowRound RedDot;
            GControls.OnOffShowRound GnDot;
            GControls.OnOffShowRound YelDot;

            //
            GControls.SuperLabel spdlbl;
            GControls.SuperLabel stplbl;

            // stanje
            GControls.SuperLabel Status;
            GControls.GroupBox gb3;
            GControls.SuperLabel lbl_msgs;

            GControls.JoystickDirection joystick;

            GControls.Conveyor conveyor;
            GControls.GroupBox gb_Conveyor;

            GControls.OnOffButton SimMaterial;
            GControls.SuperLabel SimMat;

            // koncne pozicije
            GControls.ImageButtonWithID KoncnaPozX1;
            GControls.ImageButtonWithID KoncnaPozX2;
            GControls.ImageButtonWithID KoncnaPozY1;
            GControls.ImageButtonWithID KoncnaPozY2;


            public PageDefault(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {                
                this.session = session;

                if (SessionHelper.GetCurrentUser() == "Local") // prvi username v config
                {
                    ConvUP = new GControls.UpdatePanelFull("ConveyorUpdatePanel", Settings.Updateanimations);
                }
                else
                {
                    ConvUP = new GControls.UpdatePanelFull("ConveyorUpdatePanel", Settings.UpdateValuesPCms);
                }
                

                try
                {
                    thisPage = _thisPage;
                    Name = PageHistory.GetPageNameFromPage(_thisPage);

                    imagebuttons = new GControls.MasterMenuButton[GethowManyButtonsOnFirstPage()];

                    divMaster = DIV.CreateDivAbsolute();
                    divConveyor = DIV.CreateDivAbsolute();

                    AddDivs();
                    AddContent();
                    SimulirajMaterial();
                    JoyStick();
                    KoncnePozicije();
                                        
                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside class constructor (Page: " + Name + "): " + ex.Message);
                }
            }



            private static int GethowManyButtonsOnFirstPage()
            {
                return XmlController.GetHowManyMenuItems();
            }


            void AddDivs()
            {
                try
                {
                    divMaster.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    divMaster.Style.Add(HtmlTextWriterStyle.Height, "100%");
                    divMaster.ID = "divMaster";

                    SetControlAbsolutePos(divConveyor, 20, 29, 35, 50);
                    divConveyor.ID = "divConveyor";
                }
                catch (Exception ex) {

                    throw new Exception("Error was encountered inside AddDivs() method. Error details: " + ex.Message);
                }


            }

            void AddContent()
            {
                gb1 = new GControls.GroupBox(10, 74, 24, 83);

                // Control buttons    
                Kontrola(gb1);

                // Stanje               
                Stanje();

                Trak();


                divMaster.Controls.Add(gb1);
            }

            string ImageUrl(string Name)
            {
                return "~/Pictures/" + Name + ".png";
            }

            void Kontrola(HtmlGenericControl gb1)
            {                

                int top = 0; int dif = 16; int size = 50; int left = 46;
                int dottop = 6; int dotdif = 32; int dotSize = 55; int dotlft = 20;

                try
                {
                    btnStart = new GControls.ImageButtonWithID("Start", 1)
                    { ImageUrl = ImageUrl("Start1") }; SetControlAbsolutePos(btnStart, top, left, size); btnStart.Click += BtnStart_Click;
                    top += dif; gb1.Controls.Add(btnStart);

                    btnStop = new GControls.ImageButtonWithID("Stop", 1)
                    { ImageUrl = ImageUrl("Stop1") }; SetControlAbsolutePos(btnStop, top, left, size); btnStop.Click += BtnStop_Click;

                    top += dif + 2; gb1.Controls.Add(btnStop);

                    btnAuto = new GControls.OnOffButton("Auto", 1, prop1.Man_AutoReadState.Value_bool, new Helper.Position(top, left, size), GControls.OnOffButton.Type.WithText);
                    btnAuto.button.Click += BtnAuto_Click1;
                    top += dif; gb1.Controls.Add(btnAuto);

                    btnTrak = new GControls.OnOffButton("Trak", 1, prop1.TrakRead.Value_bool, new Helper.Position(top, left, size), GControls.OnOffButton.Type.WithText);
                    btnTrak.button.Click += BtnTrak_Click;
                    top += dif; gb1.Controls.Add(btnTrak);

                    btnCirc = new GControls.ImageButtonWithID("Circ", 1)
                    { ImageUrl = ImageUrl(CircleInProgress() ? "Circ1_on" : "Circ1") }; 
                    SetControlAbsolutePos(btnCirc, top, left, size); btnCirc.Click += BtnCirc_Click;
                    top += dif; gb1.Controls.Add(btnCirc); btnCirc.Style.Add(HtmlTextWriterStyle.ZIndex, "10");
                                        

                    btnZig = new GControls.ImageButtonWithID("Zig", 1)
                    { ImageUrl = ImageUrl(ZigZagInProgress() ? "Zig1_on" : "Zig1") }; 
                    SetControlAbsolutePos(btnZig, top, left, size); btnZig.Click += BtnZig_Click;
                    gb1.Controls.Add(btnZig); btnZig.Style.Add(HtmlTextWriterStyle.ZIndex, "10");

                    // Semaphore
                    gb2 = new GControls.GroupBox(2, 4, 30, 49);

                    RedDot = new GControls.OnOffShowRound("Red", prop1.SemaforRd.Value_bool, new Helper.Position(top, left, 100), "red");
                    SetControlAbsolutePos(RedDot, dottop, dotlft, dotSize);
                    dottop += dotdif;

                    GnDot = new GControls.OnOffShowRound("Grn", prop1.SemaforGn.Value_bool, new Helper.Position(top, left, 100), "green");
                    SetControlAbsolutePos(GnDot, dottop, dotlft, dotSize);
                    dottop += dotdif;

                    YelDot = new GControls.OnOffShowRound("Yel", prop1.SemaforYe.Value_bool, new Helper.Position(top, left, 100), "yellow");
                    SetControlAbsolutePos(YelDot, dottop, dotlft, dotSize);

                    gb2.Controls.Add(RedDot); gb2.Controls.Add(GnDot); gb2.Controls.Add(YelDot);
                    gb1.Controls.Add(gb2);

                    // speedSel
                    spdlbl = new GControls.SuperLabel("Hitrost:", 62, 15, 20, 10) { FontSize = 1.2F }; ;
                    speed = new GControls.DropDownListForDimmerRPM("Speedsel", prop1.SpeedRead.Value_string, 65, 2, 5, 1.5F, false, false);
                    speed.SaveClicked += Speed_SaveClicked;
                    gb1.Controls.Add(speed);
                    gb1.Controls.Add(spdlbl);

                    // Y step Time
                    stplbl = new GControls.SuperLabel("Korak:", 77, 15, 20, 10) {FontSize = 1.2F };
                    TimeZigY = new GControls.DropDownListForTimer_1_30s("Stepsel", prop1.TimeZigY.Value_string, 80, 2, 5, 1.5F, false, false);
                    TimeZigY.SaveClicked += TimeZigY_SaveClicked;
                    gb1.Controls.Add(TimeZigY);
                    gb1.Controls.Add(stplbl);

                }
                catch (Exception ex)
                {

                    throw new Exception("Internal error inside Kontrola(HtmlGenericControl) method. "+ ex.Message);
                }
                
            }
                        

            void Stanje()
            {
                var top = 13;
                var dif = 8;
                try
                {
                    gb3 = new GControls.GroupBox(20, 1, 20, 70);
                    Status = new GControls.SuperLabel("Stanje:", 3, 35, 30, 10) { FontWeightBold = true };

                    var msgs = StanjeProcesa.SporocilaZaPrikaz;
                    var cstmMsgs = StanjeProcesa.SporocilaZaPrikaz_custom;

                    for (int i = 0; i < cstmMsgs.Count; i++)
                    {
                        lbl_msgs = new GControls.SuperLabel("- " + cstmMsgs[i], top, 7, 90, 10) { FontSize = 1 };

                        lbl_msgs.FontWeightBold = true;
                        lbl_msgs.Style.Add(HtmlTextWriterStyle.Color, Settings.RedColorHtmlHumar);

                        gb3.Controls.Add(lbl_msgs);
                        top += dif;
                    }

                    for (int i = 0; i < msgs.Count; i++)
                    {
                        lbl_msgs = new GControls.SuperLabel("- " + msgs[i].Message, top, 7, 90, 10) { FontSize = 1 };
                        if (msgs[i].Emergency)
                        {
                            lbl_msgs.FontWeightBold = true;
                            lbl_msgs.Style.Add(HtmlTextWriterStyle.Color, Settings.RedColorHtmlHumar);
                        }
                        gb3.Controls.Add(lbl_msgs);
                        top += dif;
                    }

                    gb3.Controls.Add(Status);
                    divMaster.Controls.Add(gb3);

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Stanje() method. " + ex.Message);
                }


            }

            void JoyStick()
            {
                try
                {
                    joystick = new GControls.JoystickDirection(74, 29, 15,
                    prop1.JoyStickCommandY2.Value_bool, prop1.JoyStickCommandY1.Value_bool, prop1.JoyStickCommandX1.Value_bool, prop1.JoyStickCommandX2.Value_bool);
                    divMaster.Controls.Add(joystick);

                    joystick.btn_up.Click += Btn_up_Click;
                    joystick.btn_dn.Click += Btn_dn_Click;
                    joystick.btn_l.Click += Btn_l_Click;
                    joystick.btn_r.Click += Btn_r_Click;

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside JoyStick() method. " + ex.Message);
                }
                
            }

            void Trak()
            {
                var cor_YHigher = 48; var cor_Yrange = 0.70F; var cor_Xrange = 0.6F;

                float posx = prop2.PosX.Value_short; posx *= cor_Xrange; // start position of conveyor simulation
                float posy = prop2.PosY.Value_short; posy = (posy + cor_YHigher) * cor_Yrange; // start position of conveyor simulation

                try
                {
                    conveyor = new GControls.Conveyor("convey",
                    (100 - posy), (posx),
                    40,
                    prop1.TrakRead.Value_bool, prop1.ReadPrisotMat.Value_bool);

                    gb_Conveyor = new GControls.GroupBox(0, 0, 100, 100);

                    //gc.Controls.Add(new Label() { Text = "X:" +posx.ToString() +" Y:" + (100 - posy).ToString() });
                    gb_Conveyor.Controls.Add(conveyor);
                    divConveyor.Controls.Add(gb_Conveyor);
                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Trak() method. " + ex.Message);
                }
                

                

            }

            void SimulirajMaterial()
            {
                try
                {
                    var top = 89; var left = 56; var size = 6;
                    SimMaterial = new GControls.OnOffButton("SimulirajMaterial", 1, Val.logocontroler.Prop1.ReadPrisotMat.Value_bool, new Helper.Position(top, left, size), GControls.OnOffButton.Type.Shadowed);
                    SimMaterial.button.Click += Button_Click;
                    SimMat = new GControls.SuperLabel("Simuliraj Material:", top + 1.7F, left - 8, size + 3, size - 3) { FontSize = 1.0F };
                    divMaster.Controls.Add(SimMaterial);
                    divMaster.Controls.Add(SimMat);
                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside SimulirajMaterial() method. " + ex.Message);
                }

            }

            void KoncnePozicije()
            {

                try
                {
                    if (prop1.ReadKSX1.Value_bool)
                    {
                        KoncnaPozX1 = new GControls.ImageButtonWithID(10) { ImageUrl = "~/Pictures/gui_separator.png" };
                        gb_Conveyor.Controls.Add(KoncnaPozX1); SetControlAbsolutePos(KoncnaPozX1, 7, -3, 7, 86);
                        prop1.JoyStickCommandX1.Value_bool = false;
                    }
                    if (prop1.ReadKSX2.Value_bool)
                    {
                        KoncnaPozX2 = new GControls.ImageButtonWithID(11) { ImageUrl = "~/Pictures/gui_separator.png" };
                        gb_Conveyor.Controls.Add(KoncnaPozX2); SetControlAbsolutePos(KoncnaPozX2, 7, 96.5F, 7, 86);
                        prop1.JoyStickCommandX2.Value_bool = false;
                    }
                    if (prop1.ReadKSY1.Value_bool)
                    {
                        KoncnaPozY1 = new GControls.ImageButtonWithID(12) { ImageUrl = "~/Pictures/gui_separator2.png" };
                        gb_Conveyor.Controls.Add(KoncnaPozY1); SetControlAbsolutePos(KoncnaPozY1, 95, 5, 90, 10);
                        prop1.JoyStickCommandY1.Value_bool = false;
                    }
                    if (prop1.ReadKSY2.Value_bool)
                    {
                        KoncnaPozY2 = new GControls.ImageButtonWithID(13) { ImageUrl = "~/Pictures/gui_separator2.png" };
                        gb_Conveyor.Controls.Add(KoncnaPozY2); SetControlAbsolutePos(KoncnaPozY2, -5, 5, 90, 10);
                        prop1.JoyStickCommandY2.Value_bool = false;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside KoncnePozicije() method. " + ex.Message);
                }


            }

            // helpers
            bool CircleInProgress()
            {
                var a = Val.logocontroler.Prop1;

                if (a.InitCircle.Value_bool || a.KorakCircle1.Value_bool || a.KorakCircle2.Value_bool || a.KorakCircle3.Value_bool || a.KorakCircle4.Value_bool)
                {
                    return true;
                }
                return false;
            }

            bool ZigZagInProgress()
            {
                var a = Val.logocontroler.Prop1;

                if (a.Inicializacija.Value_bool || a.KorakZigZag1.Value_bool || a.KorakZigZag2.Value_bool)
                {
                    return true;
                }
                return false;
            }

            // ONCLICKS

            private void Btn_r_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.JoyStickCommandX2.ToggleValue();
                Val.logocontroler.Prop1.JoyStickCommandX1.Value_bool = false;
            }

            private void Btn_l_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.JoyStickCommandX1.ToggleValue();
                Val.logocontroler.Prop1.JoyStickCommandX2.Value_bool = false;
            }

            private void Btn_dn_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.JoyStickCommandY1.ToggleValue();
                Val.logocontroler.Prop1.JoyStickCommandY2.Value_bool = false;
            }

            private void Btn_up_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.JoyStickCommandY2.ToggleValue();
                Val.logocontroler.Prop1.JoyStickCommandY1.Value_bool = false;
            }
                        
            private void Button_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.SymPrisotMat.ToggleValue();
            }

            private void Speed_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
            {
                Val.logocontroler.Prop1.SpeedSet.Value_short = speed.GetSelectedValue();
            }

            private void TimeZigY_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
            {
                Val.logocontroler.Prop1.TimeZigY.Value_short = TimeZigY.GetSelectedValue();
            }

            private void BtnZig_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.ZigZag.SendPulse();
            }

            private void BtnCirc_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.Circle.SendPulse();
            }

            private void BtnAuto_Click1(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.Auto.SendPulse();
            }

            private void BtnTrak_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.Trak_muss.ToggleValue();
            }

            private void BtnStop_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.Stop.SendPulse();
            }

            private void BtnStart_Click(object sender, ImageClickEventArgs e)
            {
                Val.logocontroler.Prop1.Start.SendPulse();
            }


        }
    }
}