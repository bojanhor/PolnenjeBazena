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
        public class PageMesalaPehala : Dsps
        {
            public Submenu subMenu;
            public MesalaPehalaContent SettingsContent = new MesalaPehalaContent();

            public PageMesalaPehala()
            {
                try
                {
                    subMenu = new Submenu(SettingsContent);

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Ventilacija class constructor: " + ex.Message);
                }
            }

            public class Submenu : GControls.SettingsSubMenu
            {
                public Submenu(MesalaPehalaContent SettingsContent)
                    : base(1, "Mešalo", false, SettingsContent)
                {

                }
            }

            public class MesalaPehalaContent : HtmlGenericControl
            {

                GControls.UpdatePanelFull up = new GControls.UpdatePanelFull("MesalaPehalaUpdatePanel", Settings.UpdateValuesPCms);
                GControls.GroupBox gb;

                int top = 8;

                static readonly int rows = 3; // LOGO supporst only 3
                int topOffset = 30;
                int spacingT = 25;
                int lblxtra = 5;

                int leftOffset = 10;
               
                int lblH = 6;
                int ctrlH = 5;
                float fontSize = 1.2F;

                GControls.SuperLabel[] lbl_weekdayToStart = new GControls.SuperLabel[rows];
                GControls.DropDownListForWeekDaySelect[] weekdayToStart = new GControls.DropDownListForWeekDaySelect[rows];
                GControls.SuperLabel[] lbl_med = new GControls.SuperLabel[rows];
                GControls.DropDownListForHourSelect[] med = new GControls.DropDownListForHourSelect[rows];
                GControls.SuperLabel[] lbl_in = new GControls.SuperLabel[rows];
                GControls.DropDownListForHourSelect[] in_ = new GControls.DropDownListForHourSelect[rows];

                // Režim

                GControls.GuiSeparator gs; GControls.GuiSeparator_DottedLine gs1; GControls.GuiSeparator_DottedLine gs2;
                GControls.SuperLabel lbl_rezim;
                GControls.DropDownListForRocno0Rocno1AvtoSelect Rezim;
                GControls.SuperLabel lbl_Status;
                GControls.SuperLabel Status;

                public MesalaPehalaContent()
                {
                    InitializeControls();
                    AddControls();
                }


                void InitializeControls()
                {
                    gb = new GControls.GroupBox("gb1", 17, 4, 83, 76);
                    gs = new GControls.GuiSeparator(topOffset - 5, 9, 82, 0.5F);

                    var prop = Val.logocontroler.Prop4;

                    lbl_rezim = new GControls.SuperLabel("Režim:", top + lblxtra, 10, 15, lblH ) { FontWeightBold = true };
                    Rezim = new GControls.DropDownListForRocno0Rocno1AvtoSelect("rezimSel", prop.ShowVal_Rezim.Value, top, 22, ctrlH, fontSize, false, false);

                    lbl_Status = new GControls.SuperLabel("Status Motorja:", top + 5, 40, 35, lblH);
                    Status = new GControls.SuperLabel(StatusLabelShow(), top + 5, 54, 35, lblH) { FontWeightBold = true };
                    
                    for (int i = 0; i < rows; i++)
                    {
                        lbl_weekdayToStart[i] = new GControls.SuperLabel("Dan:", topOffset + lblxtra, leftOffset, 15, lblH) { FontWeightBold = true};
                        weekdayToStart[i] = new GControls.DropDownListForWeekDaySelect("wkdySel" + i, prop.weekday[i].Value_string, topOffset, leftOffset + 5, ctrlH, fontSize, false, true);
                        lbl_med[i] = new GControls.SuperLabel("med", topOffset + lblxtra, leftOffset + 29, 10, lblH);
                        med[i] = new GControls.DropDownListForHourSelect("med" + i, prop.start[i].Value, topOffset, leftOffset + 34, ctrlH,  fontSize, false, false);
                        lbl_in[i] = new GControls.SuperLabel("in", topOffset + lblxtra, leftOffset + 51, 10, lblH);
                        in_[i] = new GControls.DropDownListForHourSelect("in" + i, prop.stop[i].Value, topOffset, leftOffset + 54, ctrlH, fontSize, false, false);

                        if (i == 1)
                        {
                            gs1 = new GControls.GuiSeparator_DottedLine(topOffset - 6, 9, 85, 0.5F, 9);
                        }

                        if (i == 2)
                        {
                            gs2 = new GControls.GuiSeparator_DottedLine(topOffset - 6, 9, 85, 0.5F, 9);
                        }

                        topOffset += spacingT;

                        weekdayToStart[i].SaveClicked += MesalaPehalaWeekDay_SaveClicked;
                        med[i].SaveClicked += MesalaPehalaStartTime_SaveClicked;
                        in_[i].SaveClicked += MesalaPehalaStopTime_SaveClicked;
                    }

                    Rezim.SaveClicked += Rezim_SaveClicked;


                }

                private void Rezim_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    if (Rezim.IsSelectedValueAuto())
                    {
                        Val.logocontroler.Prop4.SetAuto.SendPulse();
                    }
                    else if (Rezim.IsSelectedValueMan0())
                    {
                        Val.logocontroler.Prop4.SetMan0.SendPulse();
                    }
                    else if (Rezim.IsSelectedValueMan1())
                    {
                        Val.logocontroler.Prop4.SetMan1.SendPulse();
                    }
                }

                private void MesalaPehalaStopTime_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    try
                    {
                        var ID = Helper.GetID_FromBtnObject(sender);
                        Val.logocontroler.Prop4.stop[ID].Value = in_[ID].GetSelectedValue();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Internal Error inside event MesalaPehalaStopTime_SaveClicked(,,).");
                    }
                }

                private void MesalaPehalaStartTime_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    try
                    {
                        var ID = Helper.GetID_FromBtnObject(sender);
                        Val.logocontroler.Prop4.start[ID].Value = med[ID].GetSelectedValue();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Internal Error inside event MesalaPehalaStartTime_SaveClicked(,,).");
                    }

                }

                private void MesalaPehalaWeekDay_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    try
                    {
                        var ID = Helper.GetID_FromBtnObject(sender);
                        Val.logocontroler.Prop4.weekday[ID].Value = weekdayToStart[ID].GetSelectedValue();
                    }
                    catch (Exception)
                    {
                        throw new Exception("Internal Error inside event MesalaPehalaWeekDay_SaveClicked(,,).");
                    }


                }

                void AddControls()
                {
                    
                    gb.Controls.Add(gs);
                    gb.Controls.Add(gs1);
                    gb.Controls.Add(gs2);

                    gb.Controls.Add(lbl_rezim);
                    gb.Controls.Add(Rezim);
                    gb.Controls.Add(lbl_Status);
                    gb.Controls.Add(Status);


                    for (int i = 0; i < rows; i++)
                    {
                        gb.Controls.Add(lbl_weekdayToStart[i]);
                        gb.Controls.Add(weekdayToStart[i]);
                        gb.Controls.Add(lbl_med[i]);
                        gb.Controls.Add(med[i]);
                        gb.Controls.Add(lbl_in[i]);
                        gb.Controls.Add(in_[i]);
                    }

                    up.Controls_Add(gb);
                    Controls.Add(up);
                }

                string StatusLabelShow()
                {
                    var motor = Val.logocontroler.Prop4.aktivenMotor;

                    if (motor.Value_bool)
                    {
                        return "Aktiven";
                    }
                    return "Neaktiven";
                }
            }

        }
    }
}
