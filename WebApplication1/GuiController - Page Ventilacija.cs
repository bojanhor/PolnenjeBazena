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
        public class Ventilacija
        {
            public VentSettingsSubmenu subMenu;
            public VentSettingsContent SettingsContent = new VentSettingsContent();

            public Ventilacija()
            {
                try
                {
                    subMenu = new VentSettingsSubmenu(SettingsContent);

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Ventilacija class constructor: " + ex.Message);
                }
            }

            public class VentSettingsSubmenu : GControls.SettingsSubMenu
            {
                public VentSettingsSubmenu(VentSettingsContent SettingsContent)
                    : base(1, "Ventilacija", false, SettingsContent)
                {

                }
            }

            public class VentSettingsContent : HtmlGenericControl
            {
                // Declarations Left Groupbox
                float FontSize = 1.2F;
                int btnHeight = 12;

                int row1 = 23;
                float btnOffset = 4.5F;

                float rowbuff;
                int rowspacing = 15;

                Label TitleLeft = new Label();

                static int col1 = 8;
                static int col2 = col1 + 31;
                static int col3 = col2 + 6;
                static int col4 = col3 + 17;
                static int col5 = col4 + 4;


                Label lbl_tn1 = new Label(); // temperaturni nivo 1
                Label lbl_tn2 = new Label(); // temperaturni nivo 2
                Label lbl_tn3 = new Label(); // temperaturni nivo 3

                Label lbl_OmejevalnikObratov = new Label(); // OmejevalnikObratov 

                Label obr_vent = new Label(); // Vrednost [obratov] za omejevalec

                GControls.DropDownListForTemperatureSelect_10_30 btnNastavi1;
                GControls.DropDownListForTemperatureSelect_10_30 btnNastavi2;
                GControls.DropDownListForTemperatureSelect_10_30 btnNastavi3;
                GControls.DropDownListForDimmer btnNastaviObr;

                Label lbl_na1 = new Label(); // na
                Label lbl_na2 = new Label(); // na
                Label lbl_na3 = new Label(); // na

                GControls.DropDownListForDimmer btnNastaviObr1;
                GControls.DropDownListForDimmer btnNastaviObr2;
                GControls.DropDownListForDimmer btnNastaviObr3;

                GControls.GroupBox gb_L;
                GControls.GuiSeparator gs;



                // Declarations Right GroupBox

                Label TitleRight = new Label();
                GControls.GroupBox gb_R;

                GControls.GuiSeparator gs0;
                GControls.SuperLabel Lbl_OmObratov; // Label Omejevalnik obratov
                GControls.DropDown DropD_OmObratov;
                GControls.GuiSeparator_DottedLine gs1;

                GControls.SuperLabel Lbl_OmejiObrate;
                GControls.DropDownListForDimmer DropD_OmejiObrate;
                GControls.SuperLabel Lbl_Med;
                GControls.SuperLabel Lbl_In;
                GControls.DropDownListForHourSelect DropD_Start;
                GControls.DropDownListForHourSelect DropD_Stop;
                GControls.GuiSeparator gs2;

                GControls.SuperLabel Lbl_UpostevajZT;
                GControls.DropDown DropD_UpostevajZT;
                GControls.GuiSeparator gs3;

                GControls.SuperLabel Histereza;
                GControls.DropDown DropD_Histereza;


                public VentSettingsContent()
                {
                    // Left

                    TitleLeft.Text = "TEMPERATURNI NIVOJI";

                    // Right

                    TitleRight.Text = "OSTALE NASTAVITVE";



                    // common



                    TagName = "div";
                    SetControlAbsolutePos(this, 0, 0, 100, 100);

                    initializeValues();
                    positionControls();
                    styleControls(FontSize);
                    addControls();
                }

                // BothGroups

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

                void initializeValues()
                {
                    initializeValues_Left();
                    initializeValues_Right();
                }

                void positionControls()
                {
                    positionControls_Left();
                }

                void styleControls(float fontSize)
                {
                    styleControls_Left(FontSize);
                    styleControls_Right(FontSize);
                }

                // Left Group

                void initializeValues_Left()
                {

                    var tn = "Temperaturni nivo ";

                    lbl_tn1.Text = tn + "1:";
                    lbl_tn2.Text = tn + "2:";
                    lbl_tn3.Text = tn + "3:";
                    lbl_OmejevalnikObratov.Text = "Omejevalnik obratov";



                    lbl_na1.Text = "na";
                    lbl_na2.Text = "na";
                    lbl_na3.Text = "na";

                    var s = 3.9F;

                    btnNastavi1 = new GControls.DropDownListForTemperatureSelect_10_30("btnNastavi1", s, FontSize);
                    btnNastavi1.SaveClicked += BtnNastavi1_SaveClicked;

                    btnNastavi2 = new GControls.DropDownListForTemperatureSelect_10_30("btnNastavi2", s, FontSize);
                    btnNastavi2.SaveClicked += BtnNastavi2_SaveClicked;

                    btnNastavi3 = new GControls.DropDownListForTemperatureSelect_10_30("btnNastavi3", s, FontSize);
                    btnNastavi3.SaveClicked += BtnNastavi3_SaveClicked;

                    btnNastaviObr = new GControls.DropDownListForDimmer("btnNastaviObr", s, FontSize);
                    btnNastaviObr.SaveClicked += BtnNastaviObr_SaveClicked;

                    btnNastaviObr1 = new GControls.DropDownListForDimmer("btnNastaviObr1", s, FontSize);
                    btnNastaviObr1.SaveClicked += BtnNastaviObr1_SaveClicked;

                    btnNastaviObr2 = new GControls.DropDownListForDimmer("btnNastaviObr2", s, FontSize);
                    btnNastaviObr2.SaveClicked += BtnNastaviObr2_SaveClicked;

                    btnNastaviObr3 = new GControls.DropDownListForDimmer("btnNastaviObr3", s, FontSize);
                    btnNastaviObr3.SaveClicked += BtnNastaviObr3_SaveClicked;


                    gb_L = new GControls.GroupBox(17, 4, 45, 76);

                    Controls.Add(gb_L);
                }

                private void BtnNastaviObr3_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati3.SyncWithPC(selectedItem.Value);
                }

                private void BtnNastaviObr2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati2.SyncWithPC(selectedItem.Value);
                }

                private void BtnNastaviObr1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati1.SyncWithPC(selectedItem.Value);
                }

                private void BtnNastaviObr_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati_Omejevalnik_L.SyncWithPC(selectedItem.Value);
                }

                private void BtnNastavi3_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.TempNivo3.SyncWithPC(selectedItem.Value);
                }

                private void BtnNastavi2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.TempNivo2.SyncWithPC(selectedItem.Value);
                }

                private void BtnNastavi1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.TempNivo1.SyncWithPC(selectedItem.Value);
                }

                void positionControls_Left()
                {

                    rowbuff = row1;

                    SetControlAbsolutePos(lbl_tn1, rowbuff, col1); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_tn2, rowbuff, col1); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_tn3, rowbuff, col1); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_OmejevalnikObratov, rowbuff, col1); rowbuff = row1;


                    SetControlAbsolutePos(obr_vent, rowbuff, col3); rowbuff = row1;

                    SetControlAbsolutePos(lbl_na1, rowbuff, col4); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_na2, rowbuff, col4); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_na3, rowbuff, col4); rowbuff = row1;

                    var ratio = 2.0F;
                    rowbuff -= btnOffset;

                    SetControlAbsolutePos(btnNastavi1, rowbuff, col2, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastavi2, rowbuff, col2, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastavi3, rowbuff, col2, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastaviObr, rowbuff, col2, btnHeight * ratio, btnHeight);

                    rowbuff = row1 - btnOffset;

                    SetControlAbsolutePos(btnNastaviObr1, rowbuff, col5, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastaviObr2, rowbuff, col5, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastaviObr3, rowbuff, col5, btnHeight * ratio, btnHeight); rowbuff += rowspacing;

                    positionSeparators_Left();
                }

                void styleControls_Left(float fontSize)
                {
                    style(lbl_tn1, fontSize);
                    style(lbl_tn2, fontSize);
                    style(lbl_tn3, fontSize);
                    style(lbl_OmejevalnikObratov, fontSize);


                    style(obr_vent, fontSize);

                    style(lbl_na1, fontSize);
                    style(lbl_na2, fontSize);
                    style(lbl_na3, fontSize);

                }

                void positionSeparators_Left()
                {

                    float gs_top = row1 + 8.5F;
                    float gs_lft = col1 + 0;
                    float gs_w = col5 + 18;
                    float gs_h = 0.3F;
                    float gs_offset = rowspacing;

                    gs = new GControls.GuiSeparator(gs_top - gs_offset + 0.5F, gs_lft, gs_w, gs_h);
                    gb_L.Controls.Add(gs);


                    for (int i = 0; i < 4; i++)
                    {
                        gs = new GControls.GuiSeparator(gs_top, gs_lft, gs_w, gs_h);
                        gb_L.Controls.Add(gs);

                        gs_top += gs_offset;
                    }
                }

                // Right Group

                void initializeValues_Right()
                {

                    float leftBorder = 5;
                    float nextH = 17;
                    int spacing = 4;
                    float guiSepH = 0.3F;
                    double sizeBtn = 4F;

                    gs0 = new GControls.GuiSeparator(nextH, leftBorder, 90, guiSepH); nextH += gs0.Height + spacing - 2;

                    Lbl_OmObratov = new GControls.SuperLabel("OMEJEVALNIK OBRATOV:", nextH + 4, leftBorder, 50, 10);

                    DropD_OmObratov = new GControls.DropDownListForYesNoSelect("DD_OmObr_yesno", nextH, 50, sizeBtn, FontSize); nextH += DropD_OmObratov.Height + spacing * 3.5F;
                    DropD_OmObratov.SaveClicked += DropD_OmObratov_SaveClicked;

                    //
                    gs1 = new GControls.GuiSeparator_DottedLine(nextH, leftBorder, 90, guiSepH, 10); nextH += gs1.Height + spacing - 2;
                    //

                    Lbl_OmejiObrate = new GControls.SuperLabel("OMEJI OBRATE NA:", nextH + 4, leftBorder, 33, 5);

                    var tmp = Lbl_OmejiObrate.Left + Lbl_OmejiObrate.Width + 2;
                    var tmp1 = tmp;

                    DropD_OmejiObrate = new GControls.DropDownListForDimmer("DD_OmObr_prc", nextH, tmp, sizeBtn, FontSize); nextH += DropD_OmejiObrate.Height + spacing + 7;
                    DropD_OmejiObrate.SaveClicked += DropD_OmejiObrate_SaveClicked;

                    Lbl_Med = new GControls.SuperLabel("med", nextH + 4, leftBorder + 25, 20, 10);

                    DropD_Start = new GControls.DropDownListForHourSelect("DD_start", nextH, tmp, sizeBtn, FontSize); tmp += 24;
                    DropD_Start.SaveClicked += DropD_Start_SaveClicked;

                    Lbl_In = new GControls.SuperLabel("in", nextH + 4, tmp, 20, 10); tmp += 5;

                    DropD_Stop = new GControls.DropDownListForHourSelect("DD_stop", nextH, tmp, sizeBtn, FontSize); nextH += DropD_Stop.Height + spacing * 3.5F;
                    DropD_Stop.SaveClicked += DropD_Stop_SaveClicked;

                    //
                    gs2 = new GControls.GuiSeparator(nextH, leftBorder, 90, guiSepH); nextH += gs2.Height + spacing - 2;
                    //

                    Lbl_UpostevajZT = new GControls.SuperLabel("UPOŠTEVAJ ZUNANJO TEMPERATURO:", nextH + 2, leftBorder, 22, 10);

                    DropD_UpostevajZT = new GControls.DropDownListForYesNoSelect("DD_OutTempReg", nextH + 2, tmp1, sizeBtn, FontSize); nextH += DropD_UpostevajZT.Height + spacing * 4;
                    DropD_UpostevajZT.SaveClicked += DropD_UpostevajZT_SaveClicked;

                    //
                    gs3 = new GControls.GuiSeparator(nextH, leftBorder, 90, guiSepH); nextH += gs3.Height + spacing - 2;
                    //

                    Histereza = new GControls.SuperLabel("HISTEREZA:", nextH + 4, leftBorder, 20, 10);

                    DropD_Histereza = new GControls.DropDownListForHisteresis("DD_Hist", nextH, tmp1, sizeBtn, FontSize); nextH += DropD_Histereza.Height + spacing;
                    DropD_Histereza.SaveClicked += DropD_Histereza_SaveClicked;

                    gb_R = new GControls.GroupBox(17, 51, 45, 76);
                    Controls.Add(gb_R);
                }

                private void DropD_Histereza_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Histereza.SyncWithPC(selectedItem.Value);
                }

                private void DropD_UpostevajZT_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.UpostevajZT.SyncWithPC(selectedItem.Value);
                }

                private void DropD_Stop_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.OmObrMedB.SyncWithPC(selectedItem.Value, 1);
                }

                private void DropD_Start_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.OmObrMedA.SyncWithPC(selectedItem.Value, 1);
                }

                private void DropD_OmejiObrate_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati_Omejevalnik_R.SyncWithPC(selectedItem.Value);
                }

                private void DropD_OmObratov_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati1.SyncWithPC(selectedItem.Value);
                }

                void styleControls_Right(float FontSize)
                {
                    Lbl_OmObratov.FontSize = FontSize;
                    Lbl_OmejiObrate.FontSize = FontSize;
                    Lbl_Med.FontSize = FontSize;
                    Lbl_In.FontSize = FontSize;
                    Lbl_UpostevajZT.FontSize = FontSize;
                    Histereza.FontSize = FontSize;
                }

                // Both Groups

                void style(WebControl c, float fontSize)
                {
                    c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(fontSize, "vw"));
                    c.Style.Add(HtmlTextWriterStyle.Height, rowspacing - 1 + "%");
                }

                void addControls()
                {

                    gb_L.Controls.Add(FormatTitle(TitleLeft));

                    gb_L.Controls.Add(lbl_tn1);
                    gb_L.Controls.Add(lbl_tn2);
                    gb_L.Controls.Add(lbl_tn3);
                    gb_L.Controls.Add(lbl_OmejevalnikObratov);


                    gb_L.Controls.Add(obr_vent);

                    gb_L.Controls.Add(lbl_na1);
                    gb_L.Controls.Add(lbl_na2);
                    gb_L.Controls.Add(lbl_na3);

                    gb_L.Controls.Add(btnNastavi1);
                    gb_L.Controls.Add(btnNastavi2);
                    gb_L.Controls.Add(btnNastavi3);
                    gb_L.Controls.Add(btnNastaviObr);

                    gb_L.Controls.Add(btnNastaviObr1);
                    gb_L.Controls.Add(btnNastaviObr2);
                    gb_L.Controls.Add(btnNastaviObr3);


                    // 

                    gb_R.Controls.Add(FormatTitle(TitleRight));

                    gb_R.Controls.Add(gs0);
                    gb_R.Controls.Add(Lbl_OmObratov); // Label Omejevalnik obratov
                    gb_R.Controls.Add(DropD_OmObratov);
                    gb_R.Controls.Add(gs1);

                    gb_R.Controls.Add(Lbl_OmejiObrate);
                    gb_R.Controls.Add(DropD_OmejiObrate);
                    gb_R.Controls.Add(Lbl_Med);
                    gb_R.Controls.Add(Lbl_In);
                    gb_R.Controls.Add(DropD_Start);
                    gb_R.Controls.Add(DropD_Stop);
                    gb_R.Controls.Add(gs2);

                    gb_R.Controls.Add(Lbl_UpostevajZT);
                    gb_R.Controls.Add(DropD_UpostevajZT);
                    gb_R.Controls.Add(gs3);

                    gb_R.Controls.Add(Histereza);
                    gb_R.Controls.Add(DropD_Histereza);


                }

            }

        }
    }
}
