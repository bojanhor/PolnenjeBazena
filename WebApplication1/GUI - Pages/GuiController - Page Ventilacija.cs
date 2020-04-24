using System;
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
        public class PageVentilacija : Dsps
        {
            public VentSettingsSubmenu subMenu;
            public VentSettingsContent SettingsContent = new VentSettingsContent();

            public PageVentilacija()
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
                { }
            }

            public class VentSettingsContent : HtmlGenericControl
            {
                // Declarations Left Groupbox
                readonly float FontSize = 1.2F;
                readonly int btnHeight = 12;
                readonly int row1 = 23;
                readonly float btnOffset = 4.5F;

                float rowbuff;
                readonly int rowspacing = 15;

                Label TitleLeft = new Label();

                static readonly int col1 = 8;
                static readonly int col2 = col1 + 31;
                static readonly int col3 = col2 + 6;
                static readonly int col4 = col3 + 17;
                static readonly int col5 = col4 + 4;


                Label lbl_tn1 = new Label(); // temperaturni nivo 1
                Label lbl_tn2 = new Label(); // temperaturni nivo 2
                Label lbl_tn3 = new Label(); // temperaturni nivo 3
                                
                Label obr_vent = new Label(); // Vrednost [obratov] za omejevalec

                GControls.DropDownListForTemperatureSelect_10_30 btnNastavi1;
                GControls.DropDownListForTemperatureSelect_10_30 btnNastavi2;
                GControls.DropDownListForTemperatureSelect_10_30 btnNastavi3;                

                Label lbl_na1 = new Label(); // na
                Label lbl_na2 = new Label(); // na
                Label lbl_na3 = new Label(); // na

                GControls.DropDownListForDimmerRPM btnNastaviObr1;
                GControls.DropDownListForDimmerRPM btnNastaviObr2;
                GControls.DropDownListForDimmerRPM btnNastaviObr3;

                GControls.GroupBox gb_L;
                GControls.GuiSeparator gs;

                ImageButton inTemp;
                Image guiSepare;
                ImageButton outTemp;
                HtmlGenericControl weather;

                // Declarations Right GroupBox

                Label TitleRight = new Label();
                GControls.GroupBox gb_R;

                GControls.GuiSeparator gs0;
                GControls.SuperLabel Lbl_OmObratov; // Label Omejevalnik obratov
                GControls.DropDownListForYesNoSelect DropD_OmObratov;
                GControls.GuiSeparator_DottedLine gs1;

                GControls.SuperLabel Lbl_OmejiObrate;
                GControls.DropDownListForDimmerRPM DropD_OmejiObrate;
                GControls.SuperLabel Lbl_Med;
                GControls.SuperLabel Lbl_In;
                GControls.DropDownListForHourSelect DropD_Start;
                GControls.DropDownListForHourSelect DropD_Stop;
                GControls.GuiSeparator gs2;

                GControls.SuperLabel Lbl_UpostevajZT;
                GControls.DropDownListForYesNoSelect DropD_UpostevajZT;
                GControls.GuiSeparator gs3;

                // Zgoraj
                GControls.SuperLabel Rezimprikaz_lbl;
                GControls.DropDownListForRocno0Rocno1AvtoSelect RezimPrikaz_dd;

                GControls.SuperLabel VrednostRocniNacin;
                GControls.DropDownListForDimmerRPM VrednostRocniNacin_DD;

                GControls.SuperLabel DejanskiVrtljaji_lbl;
                GControls.SuperLabel DejanskiVrtljaji_val;

                GControls.UpdatePanelFull up = new GControls.UpdatePanelFull("VentilacijaUPanel", Settings.UpdateValuesPCms);
                                             

                public VentSettingsContent()
                {
                    // Top


                    // Left

                    TitleLeft.Text = "TEMPERATURNI NIVOJI";

                    // Right

                    TitleRight.Text = "OSTALE NASTAVITVE";



                    // common



                    TagName = "div";
                    SetControlAbsolutePos(this, 0, 0, 100, 100);

                    InitializeValues();
                    PositionControls();
                    StyleControls(FontSize);
                    AddControls();
                    Controls.Add(up);
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

                void InitializeValues()
                {
                    InitializeValues_Top();
                    InitializeValues_Left();
                    InitializeValues_Right();
                }

                void PositionControls()
                {
                    PositionControls_Left();
                }

                void StyleControls(float fontSize)
                {
                    StyleControls_Left(FontSize);
                    StyleControls_Right(FontSize);
                }

                // Top group
                void InitializeValues_Top()
                {
                    var prop = Val.logocontroler.Prop2;
                    var sizeBtn = 4;
                    var top = 7.1F;
                    var fs = 1; // fontsize

                    Rezimprikaz_lbl= new GControls.SuperLabel("Ročni Način:", top+2, 45, 7, 5);
                    RezimPrikaz_dd = new GControls.DropDownListForRocno0Rocno1AvtoSelect("VklopRocniNacin", prop.Rezim_Prikaz.Value_short, top, 49, sizeBtn, 1, false, false);

                    VrednostRocniNacin = new GControls.SuperLabel("Nastavi obrate na:", top + 2, 61, 7, 5);
                    VrednostRocniNacin_DD = new GControls.DropDownListForDimmerRPM("VrednostRocniNacin", prop.Obrati_RocniNacin.Value_string, top, 67, sizeBtn, 1, false, false);

                    DejanskiVrtljaji_lbl = new GControls.SuperLabel("Dejanski vrtljaji:", top + 2, 82, 7, 5);
                    DejanskiVrtljaji_val = new GControls.SuperLabel(prop.DejanskiRPM.Value_string, top + 3, 89, 7, 5);

                    Rezimprikaz_lbl.FontSize = fs; DejanskiVrtljaji_lbl.FontSize = fs; DejanskiVrtljaji_val.FontSize = fs*1.2F;

                    RezimPrikaz_dd.SaveClicked += RezimPrikaz_dd_SaveClicked;
                    VrednostRocniNacin_DD.SaveClicked += VrednostRocniNacin_DD_SaveClicked;

                }

                private void VrednostRocniNacin_DD_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Obrati_RocniNacin.Value = VrednostRocniNacin_DD.GetSelectedValue();
                }

                private void RezimPrikaz_dd_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    var prop = Val.logocontroler.Prop2;
                    var buff = RezimPrikaz_dd.GetSelectedValue();

                    if (buff == 0) // AUTO
                     prop.Rezim_Set_Auto.SendPulse(); 

                    else if (buff == 1) // Man0
                     prop.Rezim_Set_Man0.SendPulse(); 

                    else if (buff == 2) // Man1
                     prop.Rezim_Set_Man1.SendPulse(); 
                }

                void AddAndPositionControlsTop()
                {
                    up.Controls_Add(VrednostRocniNacin);
                    up.Controls_Add(VrednostRocniNacin_DD);
                    up.Controls_Add(Rezimprikaz_lbl);
                    up.Controls_Add(RezimPrikaz_dd);       
                    up.Controls_Add(DejanskiVrtljaji_lbl);
                    up.Controls_Add(DejanskiVrtljaji_val);
                }

                // Left Group

                void InitializeValues_Left()
                {
                    var tn = "Temperaturni nivo ";

                    lbl_tn1.Text = tn + "1:";
                    lbl_tn2.Text = tn + "2:";
                    lbl_tn3.Text = tn + "3:";
                    
                    lbl_na1.Text = "na";
                    lbl_na2.Text = "na";
                    lbl_na3.Text = "na";

                    var s = 3.9F;

                    var prop = Val.logocontroler.Prop2;

                    btnNastavi1 = new GControls.DropDownListForTemperatureSelect_10_30("btnNastavi1", prop.TempNivo1.Value_string, s, FontSize, false);
                    btnNastavi1.SaveClicked += BtnNastavi1_SaveClicked;

                    btnNastavi2 = new GControls.DropDownListForTemperatureSelect_10_30("btnNastavi2", prop.TempNivo2.Value_string, s, FontSize, false);
                    btnNastavi2.SaveClicked += BtnNastavi2_SaveClicked;

                    btnNastavi3 = new GControls.DropDownListForTemperatureSelect_10_30("btnNastavi3", prop.TempNivo3.Value_string, s, FontSize, false);
                    btnNastavi3.SaveClicked += BtnNastavi3_SaveClicked;
                                       
                    btnNastaviObr1 = new GControls.DropDownListForDimmerRPM("btnNastaviObr1", prop.ObratiTemperatura1.Value_string, 0, 0, s, FontSize, true, false);
                    btnNastaviObr1.SaveClicked += BtnNastaviObr1_SaveClicked;

                    btnNastaviObr2 = new GControls.DropDownListForDimmerRPM("btnNastaviObr2", prop.ObratiTemperatura2.Value_string, 0, 0, s, FontSize, true, false);
                    btnNastaviObr2.SaveClicked += BtnNastaviObr2_SaveClicked;

                    btnNastaviObr3 = new GControls.DropDownListForDimmerRPM("btnNastaviObr3", prop.ObratiTemperatura3.Value_string, 0, 0, s, FontSize, true, false);
                    btnNastaviObr3.SaveClicked += BtnNastaviObr3_SaveClicked;

                    inTemp = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/temp-in.png",
                        Width = Unit.Percentage(21),
                    };

                    guiSepare = new Image()
                    {
                        ImageUrl = "~/Pictures/gui_separator.png",
                        Width = Unit.Percentage(5.5F)
                    };

                    outTemp = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/temp-out.png",
                        Width = inTemp.Width,
                    };

                    weather = DIV.CreateDivAbsolute();
                    weather.Controls.Add(inTemp);
                    weather.Controls.Add(guiSepare);
                    weather.Controls.Add(outTemp);

                    int topOffWthr = 80, lftOffWthr = 15;
                    SetControlAbsolutePos(weather, topOffWthr, lftOffWthr, 45, 15);

                    gb_L = new GControls.GroupBox("gb_L", 17, 4, 45, 76);
                    gb_L.Controls.Add(weather);
                    
                    up.Controls_Add(gb_L);

                    WeatherLabelFormater(prop.TempZnotraj.Value_string_formatted, inTemp.Width.Value, topOffWthr +5, lftOffWthr -23);
                    WeatherLabelFormater(prop.TempZunaj.Value_string_formatted, inTemp.Width.Value, topOffWthr + 5, lftOffWthr + 8);
                }

                private void BtnNastaviObr3_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.ObratiTemperatura3.Value = btnNastaviObr3.GetSelectedValue();
                }

                private void BtnNastaviObr2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.ObratiTemperatura2.Value = btnNastaviObr2.GetSelectedValue();
                }

                private void BtnNastaviObr1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.ObratiTemperatura1.Value = btnNastaviObr1.GetSelectedValue();
                }
                                
                private void BtnNastavi3_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.TempNivo3.Value = btnNastavi3.GetSelectedValue();
                }

                private void BtnNastavi2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.TempNivo2.Value = btnNastavi2.GetSelectedValue();
                }

                private void BtnNastavi1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.TempNivo1.Value = btnNastavi1.GetSelectedValue();
                }

                void PositionControls_Left()
                {
                    
                    rowbuff = row1;

                    SetControlAbsolutePos(lbl_tn1, rowbuff, col1); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_tn2, rowbuff, col1); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_tn3, rowbuff, col1); rowbuff += rowspacing;
                   
                    SetControlAbsolutePos(obr_vent, rowbuff, col3); rowbuff = row1;

                    SetControlAbsolutePos(lbl_na1, rowbuff, col4); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_na2, rowbuff, col4); rowbuff += rowspacing;
                    SetControlAbsolutePos(lbl_na3, rowbuff, col4); rowbuff = row1;

                    var ratio = 2.0F;
                    rowbuff -= btnOffset;

                    SetControlAbsolutePos(btnNastavi1, rowbuff, col2, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastavi2, rowbuff, col2, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastavi3, rowbuff, col2, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                                                           
                    rowbuff = row1 - btnOffset;

                    SetControlAbsolutePos(btnNastaviObr1, rowbuff, col5, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastaviObr2, rowbuff, col5, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                    SetControlAbsolutePos(btnNastaviObr3, rowbuff, col5, btnHeight * ratio, btnHeight); rowbuff += rowspacing;
                                       

                    PositionSeparators_Left();
                }

                void StyleControls_Left(float fontSize)
                {
                    Stylize(lbl_tn1, fontSize);
                    Stylize(lbl_tn2, fontSize);
                    Stylize(lbl_tn3, fontSize);
                   
                    Stylize(obr_vent, fontSize);

                    Stylize(lbl_na1, fontSize);
                    Stylize(lbl_na2, fontSize);
                    Stylize(lbl_na3, fontSize);

                }

                void PositionSeparators_Left()
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

                void InitializeValues_Right()
                {

                    float leftBorder = 5;
                    float nextH = 17;
                    int spacing = 4;
                    float guiSepH = 0.3F;
                    float sizeBtn = 4F;

                    gs0 = new GControls.GuiSeparator(nextH, leftBorder, 90, guiSepH); nextH += gs0.Height + spacing - 2;

                    Lbl_OmObratov = new GControls.SuperLabel("OMEJEVALNIK OBRATOV:", nextH + 4, leftBorder, 50, 10);

                    var prop = Val.logocontroler.Prop2;

                    DropD_OmObratov = new GControls.DropDownListForYesNoSelect("DD_OmObr_yesno", prop.Nocn_Nacin.Value, nextH, 50, sizeBtn, FontSize, false, false); nextH += DropD_OmObratov.Height + spacing * 3.5F;
                    DropD_OmObratov.SaveClicked += DropD_OmObratov_SaveClicked;

                    //
                    gs1 = new GControls.GuiSeparator_DottedLine(nextH, leftBorder, 90, guiSepH, 10); nextH += gs1.Height + spacing - 2;
                    //

                    Lbl_OmejiObrate = new GControls.SuperLabel("OMEJI OBRATE NA:", nextH + 4, leftBorder, 33, 5);

                    var tmp = Lbl_OmejiObrate.Left + Lbl_OmejiObrate.Width + 2;
                    var tmp1 = tmp;

                    DropD_OmejiObrate = new GControls.DropDownListForDimmerRPM("DD_OmObr_prc", prop.OmejiObrateNa.Value_string, nextH, tmp, sizeBtn, FontSize, false, false); nextH += DropD_OmejiObrate.Height + spacing + 7;
                    DropD_OmejiObrate.SaveClicked += DropD_OmejiObrate_SaveClicked;

                    Lbl_Med = new GControls.SuperLabel("med", nextH + 4, leftBorder + 25, 20, 10);

                    DropD_Start = new GControls.DropDownListForHourSelect("DD_start", prop.OmObrMedA.Value, nextH, tmp, sizeBtn, FontSize, false, false); tmp += 24;
                    DropD_Start.SaveClicked += DropD_Start_SaveClicked;

                    Lbl_In = new GControls.SuperLabel("in", nextH + 4, tmp, 20, 10); tmp += 5;

                    DropD_Stop = new GControls.DropDownListForHourSelect("DD_stop", prop.OmObrMedB.Value, nextH, tmp, sizeBtn, FontSize, false, false); nextH += DropD_Stop.Height + spacing * 3.5F;
                    DropD_Stop.SaveClicked += DropD_Stop_SaveClicked;

                    //
                    gs2 = new GControls.GuiSeparator(nextH, leftBorder, 90, guiSepH); nextH += gs2.Height + spacing - 2;
                    //

                    Lbl_UpostevajZT = new GControls.SuperLabel("UPOŠTEVAJ ZUNANJO TEMPERATURO:", nextH + 2, leftBorder, 22, 10);

                    DropD_UpostevajZT = new GControls.DropDownListForYesNoSelect("DD_OutTempReg", prop.UpostevajZT.Value, nextH + 2, tmp1, sizeBtn, FontSize, false, false); nextH += DropD_UpostevajZT.Height + spacing * 4;
                    DropD_UpostevajZT.SaveClicked += DropD_UpostevajZT_SaveClicked;

                    //
                    gs3 = new GControls.GuiSeparator(nextH, leftBorder, 90, guiSepH); nextH += gs3.Height + spacing - 2;
                    //
                    
                    gb_R = new GControls.GroupBox("gb_R", 17, 51, 45, 76);
                    up.Controls_Add(gb_R);
                }
                                
                private void DropD_UpostevajZT_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.UpostevajZT.Value = DropD_UpostevajZT.GetSelectedValue();
                }

                private void DropD_Stop_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.OmObrMedB.Value = DropD_Stop.GetSelectedValue();
                }

                private void DropD_Start_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.OmObrMedA.Value = DropD_Start.GetSelectedValue();
                }

                private void DropD_OmejiObrate_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.OmejiObrateNa.Value = DropD_OmejiObrate.GetSelectedValue();
                }

                private void DropD_OmObratov_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop2.Nocn_Nacin.Value = DropD_OmObratov.GetSelectedValue();
                }

                void StyleControls_Right(float FontSize)
                {
                    Lbl_OmObratov.FontSize = FontSize;
                    Lbl_OmejiObrate.FontSize = FontSize;
                    Lbl_Med.FontSize = FontSize;
                    Lbl_In.FontSize = FontSize;
                    Lbl_UpostevajZT.FontSize = FontSize;                    
                }

                // Both Groups

                void Stylize(WebControl c, float fontSize)
                {
                    c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(fontSize, "vw"));
                    c.Style.Add(HtmlTextWriterStyle.Height, rowspacing - 1 + "%");
                }

                void AddControls()
                {                  
                    AddAndPositionControlsTop();
                   
                    //
                    gb_L.Controls.Add(FormatTitle(TitleLeft));
                  
                    gb_L.Controls.Add(lbl_tn1);
                    gb_L.Controls.Add(lbl_tn2);
                    gb_L.Controls.Add(lbl_tn3); 
                                       
                    gb_L.Controls.Add(obr_vent);
                   
                    gb_L.Controls.Add(lbl_na1);
                    gb_L.Controls.Add(lbl_na2);
                    gb_L.Controls.Add(lbl_na3);
                    

                    gb_L.Controls.Add(btnNastavi1);
                    gb_L.Controls.Add(btnNastavi2);
                    gb_L.Controls.Add(btnNastavi3);                   

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
                                                           
                }

                void WeatherLabelFormater(string LableText, double width, float topOffset, float spacingLeft)
                {
                    var div = DIV.CreateDivAbsolute(Helper.FloatToStringWeb(width, "%"));
                    var l = new Label();
                    div.Style.Add(HtmlTextWriterStyle.Width, width + "%");
                    l.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    l.Style.Add(HtmlTextWriterStyle.FontSize, "1.2vw");
                    div.Style.Add(HtmlTextWriterStyle.Top, Helper.FloatToStringWeb(topOffset, "%"));
                    div.Style.Add(HtmlTextWriterStyle.Left, Helper.FloatToStringWeb(spacingLeft + 3, "%"));
                    l.Width = Unit.Percentage(100);
                    l.Text = LableText;
                    div.Controls.Add(l);

                    weather.Controls.Add(div);

                }

            }

        }
    }
}
