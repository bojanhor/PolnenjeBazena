﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace WebApplication1
{

    public partial class GuiController
    {
        public class PageVrataZavese : Dsps
        {

            public static string ViewStateElement_MenuShown = "MenuShown";
            Page thisPage;
            public static System.Web.SessionState.HttpSessionState Session;

            public VrataSettingsSubmenu subMenu;
            public VrazaZaveseContent SettingsContent = new VrazaZaveseContent();

            public PageVrataZavese(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                thisPage = _thisPage;
                Session = session;

                try
                {
                    subMenu = new VrataSettingsSubmenu(SettingsContent);

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Vrata_Zavese class constructor: " + ex.Message);
                }
            }

            public class VrataSettingsSubmenu : GControls.SettingsSubMenu
            {
                public VrataSettingsSubmenu(VrazaZaveseContent SettingsContent)
                    : base(1, "Vrata", false, SettingsContent)
                {

                }
            }

            public class ZaveseSettingsSubmenu : GControls.SettingsSubMenu
            {
                public ZaveseSettingsSubmenu(VrazaZaveseContent SettingsContent)
                    : base(1, "Zavese", false, SettingsContent)
                {

                }
            }

            public class VrazaZaveseContent : HtmlGenericControl
            {
                // left gb
                GControls.ImageButtonWithID sett = new GControls.ImageButtonWithID("L_", 0);
                GControls.ImageButtonWithID gor;
                GControls.ImageButtonWithID dol;
                GControls.ImageButtonWithID gorarw;
                GControls.ImageButtonWithID dolarw;
                GControls.ImageButtonWithID stop;
                GControls.ImageButtonWithID stoparw;
                GControls.GroupBox gb;
              
                GControls.DropDownListForTimer_1_30s timerUpDn;
                Image positionUp;
                Image positionDn;


                UpdatePanel updtPnl = new UpdatePanel();
                Timer tmrPnl = new Timer();
                AsyncPostBackTrigger apbt = new AsyncPostBackTrigger();
                
                // right gb
                GControls.ImageButtonWithID sett2 = new GControls.ImageButtonWithID("R_", 0);
                GControls.ImageButtonWithID gor2;
                GControls.ImageButtonWithID dol2;
                GControls.ImageButtonWithID gorarw2;
                GControls.ImageButtonWithID dolarw2;
                GControls.ImageButtonWithID stop2;
                GControls.ImageButtonWithID stoparw2;
                GControls.GroupBox gb2;

                GControls.DropDownListForTimer_1_30s timerUpDn2;
                Image positionUp2;
                Image positionDn2;

                UpdatePanel updtPnl2 = new UpdatePanel();
                Timer tmrPnl2 = new Timer();
                AsyncPostBackTrigger apbt2 = new AsyncPostBackTrigger();

                void ManageSettingsContent()
                {
                    
                    if (GetMenuShown_Left())
                    {
                        Ventilacija_ZaveseSettingsContent settings = new Ventilacija_ZaveseSettingsContent();
                        settings.exitButton.Click += (sender, e) => { SetMenuShown_None(); Navigator.Refresh(); }; // closes menu and refresh starts updatepanel
                        Controls.Add(settings);
                    }
                    else if (GetMenuShown_Right())
                    {
                        Ventilacija_ZaveseSettingsContent settings = new Ventilacija_ZaveseSettingsContent();
                        Controls.Add(settings);
                    }

                }

                public VrazaZaveseContent()
                {
                    // menu
                    ManageSettingsContent();                    
                    
                    // left gb
                    UpdtPnlCtrl();
                    InitializeValues();
                    PositionControls();
                    StyleControls();

                    gb.Controls.Add(updtPnl);
                    Controls.Add(tmrPnl);
                    
                    gb.Controls.Add(gor);
                    gb.Controls.Add(dol);
                    gb.Controls.Add(gorarw);
                    gb.Controls.Add(dolarw);
                    gb.Controls.Add(stop);
                    gb.Controls.Add(stoparw);

                    updtPnl.ContentTemplateContainer.Controls.Add(positionUp);
                    updtPnl.ContentTemplateContainer.Controls.Add(positionDn);                    

                    Controls.Add(gb);
                    gb.Controls.Add(sett);

                    // right gb
                    gb2.Controls.Add(updtPnl2);
                    Controls.Add(tmrPnl2);

                    gb2.Controls.Add(gor2);
                    gb2.Controls.Add(dol2);
                    gb2.Controls.Add(gorarw2);
                    gb2.Controls.Add(dolarw2);
                    gb2.Controls.Add(stop2);
                    gb2.Controls.Add(stoparw2);

                    updtPnl2.ContentTemplateContainer.Controls.Add(positionUp2);
                    updtPnl2.ContentTemplateContainer.Controls.Add(positionDn2);

                    Controls.Add(gb2);
                    gb2.Controls.Add(sett2);
                }

                void UpdtPnlCtrl()
                {
                    // left
                    updtPnl.ID = "updtPnl_Vrata_Zavese";

                    tmrPnl.ID = "tmrPnl_Vrata_Zavese";
                    tmrPnl.Interval = Settings.UpdateValuesPCms;

                    apbt.ControlID = tmrPnl.ID;

                    updtPnl.Triggers.Add(apbt);

                    // right
                    updtPnl2.ID = "updtPnl_Vrata_Zavese2";

                    tmrPnl2.ID = "tmrPnl_Vrata_Zavese2";
                    tmrPnl2.Interval = Settings.UpdateValuesPCms;

                    apbt2.ControlID = tmrPnl2.ID;

                    updtPnl2.Triggers.Add(apbt2);
                }

                HtmlGenericControl FormatTitle(Label l)
                {
                    l.Style.Add(HtmlTextWriterStyle.FontSize, "2vw");
                    l.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    l.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    l.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    l.Style.Add(HtmlTextWriterStyle.Color, Settings.LightBlackColor);
                    l.Style.Add("top", "3%");

                    return SetControlAbsolutePos(EncapsulateIntoDIV_TLW(l), 2, 0, 100, 10);
                }

                void InitializeValues()
                {
                    var fontSize = 5;
                    var size = 10;


                    var prop = Val.logocontroler.Prop3;

                    // left gb
                    gor = new GControls.ImageButtonWithID(11);
                    dol = new GControls.ImageButtonWithID(12);
                    gorarw = new GControls.ImageButtonWithID(111);
                    dolarw = new GControls.ImageButtonWithID(112);
                    stop = new GControls.ImageButtonWithID(13);
                    stoparw = new GControls.ImageButtonWithID(113);



                    gb = new GControls.GroupBox(19, 5, 43, 70);
                                        
                    timerUpDn = new GControls.DropDownListForTimer_1_30s("timeToUpPos",prop.CasPotovanja1.Value_string, 20,10,size, fontSize, true, false);
                    positionUp = new Image();
                    positionDn = new Image();

                    positionUp.Visible = Val.logocontroler.Prop3.KoncnoStikaloGor1.Value_bool; // visible if true
                    positionDn.Visible = Val.logocontroler.Prop3.KoncnoStikaloGor1.Value_bool;



                    // right gb
                    gor2 = new GControls.ImageButtonWithID(21);
                    dol2 = new GControls.ImageButtonWithID(22);
                    gorarw2 = new GControls.ImageButtonWithID(211);
                    dolarw2 = new GControls.ImageButtonWithID(212);
                    stop2 = new GControls.ImageButtonWithID(23);
                    stoparw2 = new GControls.ImageButtonWithID(213);

                    gb2 = new GControls.GroupBox(19, 51, 43, 70);

                   
                    timerUpDn2 = new GControls.DropDownListForTimer_1_30s("timeToUpPos", prop.CasPotovanja2.Value_string, 20, 10, size, fontSize, true, false);
                    positionUp2 = new Image();
                    positionDn2 = new Image();

                    positionUp2.Visible = Val.logocontroler.Prop3.KoncnoStikaloGor2.Value_bool; // visible if true
                    positionDn2.Visible = Val.logocontroler.Prop3.KoncnoStikaloGor2.Value_bool;

                    // events
                    RegisterEvents();                    
                }

                void RegisterEvents()
                {
                    gor.Click += Gor_Click;
                    dol.Click += Dol_Click;
                    stop.Click += Stop_Click;
                    gorarw.Click += Gor_Click;
                    dolarw.Click += Dol_Click;
                    stoparw.Click += Stop_Click;
                    sett.Click += Sett_Click; ;

                    gor2.Click += Gor_Click2;
                    dol2.Click += Dol_Click2;
                    stop2.Click += Stop_Click2;
                    gorarw2.Click += Gor_Click2;
                    dolarw2.Click += Dol_Click2;
                    stoparw2.Click += Stop_Click2;
                    sett2.Click += Sett2_Click;
                }

                void PositionControls()
                {

                    var x = 5;
                    var y1 = 5;
                    var y2 = 35;
                    var y3 =  y2 + y2 - y1;

                    var yoff = 11;
                    var xoff = 29;

                    var offArw = xoff + 17;

                    // left
                    SetControlAbsolutePos(gor, y1, x, 45);
                    SetControlAbsolutePos(dol, y2, x, 45);
                    SetControlAbsolutePos(gorarw, y1 + yoff, x+ xoff, 10);
                    SetControlAbsolutePos(dolarw, y2 + yoff +2, x+ xoff, 10);

                    SetControlAbsolutePos(stop, y3, x, 45);
                    SetControlAbsolutePos(stoparw, y3 + yoff, x + xoff +2, 8);

                    SetControlAbsolutePos(positionUp, y1-2 + yoff, x + offArw, 15);
                    SetControlAbsolutePos(positionDn, y2-2 + yoff + 2, x + offArw, 15);
                    SetControlAbsolutePos(sett, 5, 85, 12);

                    // right
                    SetControlAbsolutePos(gor2, y1, x, 45);
                    SetControlAbsolutePos(dol2, y2, x, 45);
                    SetControlAbsolutePos(gorarw2, y1 + yoff, x + xoff, 10);
                    SetControlAbsolutePos(dolarw2, y2 + yoff + 2, x + xoff, 10);

                    SetControlAbsolutePos(stop2, y3, x, 45);
                    SetControlAbsolutePos(stoparw2, y3 + yoff, x + xoff + 2, 8);

                    SetControlAbsolutePos(positionUp2, y1 - 2 + yoff, x + offArw, 15);
                    SetControlAbsolutePos(positionDn2, y2 - 2 + yoff + 2, x + offArw, 15);
                    SetControlAbsolutePos(sett2, 5, 85, 12);

                }

                void StyleControls()
                {
                    

                    // left gb
                    gor.ImageUrl = "~/Pictures/vrata.png";
                    dol.ImageUrl = "~/Pictures/vrata.png";

                    gorarw.ImageUrl = "~/Pictures/up.png";
                    dolarw.ImageUrl = "~/Pictures/dwn.png";

                    positionUp.ImageUrl = "~/Pictures/up.png";
                    positionDn.ImageUrl = "~/Pictures/dwn.png";

                    stop.ImageUrl = "~/Pictures/vrata.png";
                    stoparw.ImageUrl = "~/Pictures/stop.png";

                    sett.ImageUrl = "~/Pictures/VentSettings.png";


                    // right gb

                    gor2.ImageUrl = "~/Pictures/vrata.png";
                    dol2.ImageUrl = "~/Pictures/vrata.png";

                    gorarw2.ImageUrl = "~/Pictures/up.png";
                    dolarw2.ImageUrl = "~/Pictures/dwn.png";

                    positionUp2.ImageUrl = "~/Pictures/up.png";
                    positionDn2.ImageUrl = "~/Pictures/dwn.png";

                    stop2.ImageUrl = "~/Pictures/vrata.png";
                    stoparw2.ImageUrl = "~/Pictures/stop.png";

                    sett2.ImageUrl = "~/Pictures/VentSettings.png";
                }

                class Ventilacija_ZaveseSettingsContent : GControls.SettingsSubMenu
                {
                    public Ventilacija_ZaveseSettingsContent()
                        : base(0, "Nastavitve", true, Content())
                    {
                        SetControlAbsolutePos(this, 15, 15, 70, 70);
                    }

                    // Sub-Menu Vrata
                    static HtmlGenericControl Content()
                    {
                        HtmlGenericControl div = new HtmlGenericControl("div");
                        GControls.GroupBox gbL = new GControls.GroupBox(20,5,30,65);
                        GControls.OnOffButton UporabljaKS = new GControls.OnOffButton("Končna stikala", 0, GetStatusUporabljaKS(), new Helper.Position(35,7), GControls.OnOffButton.Type.WithText);
                        GControls.SuperLabel lblUporabljaKS = new GControls.SuperLabel("Oprema podpira končna stikala:", 25, 10, 20, 15)
                        {
                            FontSize = 1.2F
                        };

                        SetControlAbsolutePos(lblUporabljaKS, 15, 10);

                        div.Controls.Add(gbL);
                        div.Controls.Add(UporabljaKS);
                        div.Controls.Add(lblUporabljaKS);

                        UporabljaKS.button.Click += (sender, e) => // OnClick Event
                        {
                            var val = Val.logocontroler.Prop3.UporabljaKoncnaStikala1;
                            val.Value_bool = !val.Value_bool; // toggle state                            
                        };

                        return div;
                    }

                    static bool GetStatusUporabljaKS()
                    {
                        return Val.logocontroler.Prop3.UporabljaKoncnaStikala1.Value_bool;
                    }
                }

                // OnClick Events
                #region OnClick Events

                private void Dol_Click(object sender, ImageClickEventArgs e)
                {
                    Val.logocontroler.Prop3.VrataDolPulse1.SendPulse();
                }

                private void Gor_Click(object sender, ImageClickEventArgs e)
                {
                    Val.logocontroler.Prop3.VrataGorPulse1.SendPulse();
                }

                private void Stop_Click(object sender, ImageClickEventArgs e)
                {
                    Val.logocontroler.Prop3.VrataStopPulse1.SendPulse();
                }

                private void Sett_Click(object sender, ImageClickEventArgs e)
                {
                    SetMenuShown_Left();
                    Navigator.Refresh();
                }


                //
                private void Dol_Click2(object sender, ImageClickEventArgs e)
                {
                    Val.logocontroler.Prop3.VrataDolPulse2.SendPulse();
                }

                private void Gor_Click2(object sender, ImageClickEventArgs e)
                {
                    Val.logocontroler.Prop3.VrataGorPulse2.SendPulse();
                }

                private void Stop_Click2(object sender, ImageClickEventArgs e)
                {
                    Val.logocontroler.Prop3.VrataStopPulse2.SendPulse();
                }

               
                private void Sett2_Click(object sender, ImageClickEventArgs e)
                {
                    SetMenuShown_Right();
                    Navigator.Refresh();
                }

                #endregion
            }

            // session specific
            #region session specific

            static int GetMenuShown() // 0 - no menu is shown  |  1 - left menu is shown  |  2 - right menu is shown
            {
                try
                {
                    if (Session[ViewStateElement_MenuShown] != null)
                    {
                        return (int)Session[ViewStateElement_MenuShown];
                    }
                    return 0;
                }
                catch (Exception)
                {
                    return 0;
                }
                
            }

            static bool GetMenuShown_Left()
            {
                if (GetMenuShown() == 1) 
                {
                    return true;
                }
                return false;
            }

            static bool GetMenuShown_Right()
            {
                if (GetMenuShown() == 2)
                {
                    return true;
                }
                return false;
            }

            static void SetMenuShown_Left()
            {
                Session[ViewStateElement_MenuShown] = 1;
            }

            static void SetMenuShown_Right()
            {
                Session[ViewStateElement_MenuShown] = 2;
            }

            static void SetMenuShown_None()
            {
                Session[ViewStateElement_MenuShown] = 0;
            }
            #endregion 
        }

    }
}

