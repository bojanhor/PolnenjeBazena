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
                GControls.SuperLabel rezim;
                GControls.DropDownListForRocnoAvtoSelect Rezim;
                GControls.DropDownListForOnOffSelect rocnaNastavitev;

                public MesalaPehalaContent()
                {
                    InitializeControls();
                    AddControls();
                }


                void InitializeControls()
                {
                    gb = new GControls.GroupBox("gb1", 17, 4, 83, 76);
                    gs = new GControls.GuiSeparator(topOffset - 5, 9, 82, 0.5F);

                    var prop = Val.logocontroler.Prop5;

                    rezim = new GControls.SuperLabel("Režim:", top + lblxtra, 10, 15, lblH ) { FontWeightBold = true };
                     Rezim = new GControls.DropDownListForRocnoAvtoSelect("rezimSel", prop.rocno.Value, top, 22, ctrlH, fontSize, false, false);
                    rocnaNastavitev = new GControls.DropDownListForOnOffSelect("rnval", prop.rocno.Value, top, 44, ctrlH, fontSize, false, false);

                    for (int i = 0; i < rows; i++)
                    {
                        lbl_weekdayToStart[i] = new GControls.SuperLabel("Dan:", topOffset + lblxtra, leftOffset, 15, lblH) { FontWeightBold = true};
                        weekdayToStart[i] = new GControls.DropDownListForWeekDaySelect("wkdySel" + i, prop.weekday[i].Value_string, topOffset, leftOffset + 5, ctrlH, fontSize, false, true);
                        lbl_med[i] = new GControls.SuperLabel("med", topOffset + lblxtra, leftOffset + 29, 10, lblH);
                        med[i] = new GControls.DropDownListForHourSelect("med" + i, prop.start[i].Value_string, topOffset, leftOffset + 34, ctrlH,  fontSize, false, false);
                        lbl_in[i] = new GControls.SuperLabel("in", topOffset + lblxtra, leftOffset + 51, 10, lblH);
                        in_[i] = new GControls.DropDownListForHourSelect("in" + i, prop.stop[i].Value_string, topOffset, leftOffset + 54, ctrlH, fontSize, false, false);

                        if (i == 1)
                        {
                            gs1 = new GControls.GuiSeparator_DottedLine(topOffset - 6, 9, 85, 0.5F, 9);
                        }

                        if (i == 2)
                        {
                            gs2 = new GControls.GuiSeparator_DottedLine(topOffset - 6, 9, 85, 0.5F, 9);
                        }

                        topOffset += spacingT;
                    }

                }

                void AddControls()
                {
                    Controls.Add(gb);
                    gb.Controls.Add(gs);
                    gb.Controls.Add(gs1);
                    gb.Controls.Add(gs2);

                    gb.Controls.Add(rezim);
                    gb.Controls.Add(Rezim);
                    gb.Controls.Add(rocnaNastavitev);

                    var up = gb;

                    for (int i = 0; i < rows; i++)
                    {
                        up.Controls.Add(lbl_weekdayToStart[i]);
                        up.Controls.Add(weekdayToStart[i]);
                        up.Controls.Add(lbl_med[i]);
                        up.Controls.Add(med[i]);
                        up.Controls.Add(lbl_in[i]);
                        up.Controls.Add(in_[i]);
                    }
                }
            }

        }
    }
}
