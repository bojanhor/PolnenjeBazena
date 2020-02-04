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
        public class Razsvetljava
        {
            public Page ThisPage;
            public System.Web.SessionState.HttpSessionState Session;
            public static string ViewStateElement_CurrentLuciSettingsShown = "CurrentLuciSettingsShown";

            public Helper.UpdatePanelFull updatePanel;

            public HtmlGenericControl divStala;
            Image Stala;           

            public HtmlGenericControl divLuciSettings;  
            
            public GControls.LucSet[] Luc;
            public Label[] AboveLucLableDimmer;

            public HtmlGenericControl divBtns;
            ImageButton ugasniVseLuci;
          
            public LuciSettingsMenu LuciSettings;

            string UpdatePanelId_Luci = "LuciPanel";


            public Razsvetljava(Page thisPage, System.Web.SessionState.HttpSessionState session)
            {                
                try
                {
                    ThisPage = thisPage;                    
                    Session = session;

                    Luc = new GControls.LucSet[XmlController.GetHowManyLucIcons() + 1];
                    AboveLucLableDimmer = new Label[Luc.Length]; 
                    divStala = DIV.CreateDivAbsolute();

                    Stala = new Image();

                    divLuciSettings = new HtmlGenericControl("div");
                    divLuciSettings.ID = "divLuciSettings";

                    InitializeLuci();
                    AddStala();
                    initializeLuciSettings();
                    AddBtns();

                              

                    HidePanel();


                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Razsvetljava class constructor: " + ex.Message);
                }
            }

            int getCurrentLuciSettingsShown()
            {
                // save value over session
                var buff = Session[ViewStateElement_CurrentLuciSettingsShown];
                if (buff == null)
                {
                    buff = 0;
                }

                return (int)buff;

            }

            void setCurrentLuciSettingsShown(int value)
            {
                // save value over session
                Session[ViewStateElement_CurrentLuciSettingsShown] = value;
                
            }

            void incrementCurrentLuciSettingsShown(int incrementBy)
            {
                setCurrentLuciSettingsShown(
                    getCurrentLuciSettingsShown() + incrementBy);
            }

            public void HidePanel()
            {
                if (getCurrentLuciSettingsShown() == 0)
                {   
                    divLuciSettings.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");                   
                }                

            }

            void initializeLuciSettings()
            {
                try
                {
                    updatePanel = new Helper.UpdatePanelFull("RazsvetljavaUP", Settings.UpdateValuesPCms);
                    
                    bool hasPrevious = true;
                    bool hasNext = true;
                    int lucIconsCnt = XmlController.GetHowManyLucIcons();
                    int currentShown = getCurrentLuciSettingsShown();
                    string Showname = "Cona" + currentShown + "/" + lucIconsCnt; // Custom name for user

                    if (currentShown <= 1)
                    {
                        hasPrevious = false;
                    }
                    else if (currentShown >= lucIconsCnt)
                    {
                        hasNext = false;
                    }

                    if (currentShown > 0)
                    {
                        LuciSettings = new LuciSettingsMenu(currentShown, Showname, hasNext, hasPrevious, true, updatePanel.Timer);

                        LuciSettings.exitButton.Click += ExitButton_Click;
                        if (LuciSettings.PrevButton != null)
                        {
                            LuciSettings.PrevButton.Click += PrevButton_Click;
                        }
                        if (LuciSettings.nextButton != null)
                        {
                            LuciSettings.nextButton.Click += NextButton_Click;
                        }
                      
                        updatePanel.Controls_Add(LuciSettings);
                        divLuciSettings.Controls.Add(updatePanel);

                    }
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inside initializeLuciSettings() method. Error info: " +ex.Message);
                }
               
            }

            private void ExitButton_Click(object sender, ImageClickEventArgs e)
            {               
                setCurrentLuciSettingsShown(0); // hideDiv
                Helper.Refresh();
            }

            void PrevButton_Click(object sender, ImageClickEventArgs e)
            {
                incrementCurrentLuciSettingsShown(-1);
                Helper.Refresh();
            }

            void NextButton_Click(object sender, ImageClickEventArgs e)
            {
                incrementCurrentLuciSettingsShown(1);
                Helper.Refresh();
            }

            private void InitializeLuci()
            {
                try
                {
                    for (int i = 1; i <= XmlController.GetHowManyLucIcons(); i++)
                    {
                        Luc[i] = new GControls.LucSet(i, (UpdatePanel)ThisPage.FindControl(UpdatePanelId_Luci));
                        AboveLucLableDimmer[i] = new Label();

                        AboveLucLableDimmer[i] = CreateLabelTitle_OnTop(
                            "100%", Luc[i], 2.5F, 0, Convert.ToInt32(Luc[i].Width), 1, false, true, "black"); // TODO PLC Value 100%

                        AboveLucLableDimmer[i].Style.Add(HtmlTextWriterStyle.BackgroundColor, "white");

                        AboveLucLableDimmer[i].Style.Add("border-radius", "10%");
                        AboveLucLableDimmer[i].Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
                        AboveLucLableDimmer[i].Style.Add(HtmlTextWriterStyle.BorderWidth, "0.1vw");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inside InitializeLuci() method. Error info: " +ex.Message);
                }
               


            }

            public void RegisterOnClick()
            {                
                for (int i = 1; i < Luc.Length; i++)
                {
                    Luc[i].button.Click += Zarnica_Click;    
                }

                ugasniVseLuci.Click += UgasniVseLuci_Click;

            }

            private void Zarnica_Click(object sender, EventArgs e)
            {
                var luc = (GControls.LucBtn)sender;                
                setCurrentLuciSettingsShown(luc.btnID);

                var panel = (HtmlGenericControl)ThisPage.FindControl("divLuciSettings");
                panel.Style.Add(HtmlTextWriterStyle.Visibility, "visible");

                Helper.Refresh();
            }

            private void UgasniVseLuci_Click(object sender, EventArgs e)
            {
                Val.logocontroler.Prop1.UgasniVseLuci.SendPulse();
            }
                        
            void AddBtns()
            {
                try
                {
                    divBtns = DIV.CreateDiv("10%", "80%", "14%", "83%");
                    ugasniVseLuci = new ImageButton
                    {
                        ImageUrl = "~/Pictures/UgasniVseLuci.png",
                        Width = Unit.Percentage(100)
                    };

                    divBtns.Controls.Add(ugasniVseLuci);
                                                        
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inside AddBtns() method. Error info: " + ex.Message);
                }
                
            }

            void AddStala()
            {
                try
                {
                    Stala.ImageUrl = "~/Pictures/MasterPic.png";
                    Stala.Style.Add(HtmlTextWriterStyle.ZIndex, "1");
                    Stala.Style.Add(HtmlTextWriterStyle.Width, "100%");

                    divStala.Controls.Add(Stala);
                    divStala.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    divStala.Style.Add(HtmlTextWriterStyle.Height, "100%");
                    divStala.ID = "divStalaR";

                }
                catch (Exception ex)
                {
                    throw new Exception("Error inside AddStala() method. Error Info: " + ex.Message); ;
                }
               
            }

            public class LuciSettingsMenu : GControls.SettingsSubMenu
            {
                public static LuciSubmenuContent content;

                public LuciSettingsMenu(int id, string Name_, bool hasNext, bool hasPreious, bool hasExit, Timer UpdateTimer) 
                    :base(id, Name_, hasNext, hasPreious, hasExit, getContent(id))
                {
                    
                }

                static LuciSubmenuContent getContent(int id)
                {
                    content = new LuciSubmenuContent(id);                    
                    return content;
                }
               
            }
            
            public class LuciSubmenuContent : HtmlGenericControl
            {

                readonly int tr1 = 25; // top attribute row 1
                readonly int tr2 = 65;
                readonly int lc1 = 15; // left attribute column 1
                readonly int lc2 = 40;
                readonly int lc3 = 60;
                               

                bool hasDimmer = false;


                readonly int widthbtn = 4;
                readonly int widthdd = 15;

                readonly float fontSize = 1.5F;

                GControls.GroupBox Groupbox1;
                GControls.GroupBox Groupbox2;

                GControls.OnOffButton IzklopKoJeDan;

                readonly int myID;

                Label Cona = new Label();
                Label Dopoldne = new Label();
                Label Popoldne = new Label();

                public GControls.DropDownListForHourSelect Vklop1;
                public GControls.DropDownListForHourSelect Vklop2;
                public GControls.DropDownListForHourSelect Izklop1;
                public GControls.DropDownListForHourSelect Izklop2;
                public GControls.PaddedOnOffButton VklopUrnika1;
                public GControls.PaddedOnOffButton VklopUrnika2;

                public GControls.DropDownListForDimmer DimmerDop;
                public GControls.DropDownListForDimmer DimmerPop;
                               
                public LuciSubmenuContent(int id)
                {
                    Groupbox1 = new GControls.GroupBox(19, 10, 67, 35);
                    Groupbox2 = new GControls.GroupBox(57, 10, 67, 35);

                    var prop = Val.logocontroler.Prop1;

                    IzklopKoJeDan = new GControls.OnOffButton("Izklop Podnevi", id, prop.GetIzklopiKoJeDan(id), new Helper.Position(18, 78, 18), GControls.OnOffButton.Type.WithText);

                    var topOffsetLabel = 10;

                    try
                    {
                        hasDimmer = XmlController.GetHasDimmer(id);
                        

                        if (hasDimmer && prop.DimmerDop[id] != null)
                        {
                            DimmerDop = new GControls.DropDownListForDimmer("DD_dimAm", prop.DimmerDop[id].Value_string, widthbtn, fontSize, false);
                            DimmerPop = new GControls.DropDownListForDimmer("DD_dimPm", prop.DimmerPop[id].Value_string, widthbtn, fontSize, false);

                            SetControlAbsolutePos(DimmerDop, tr2, lc1, widthbtn);
                            SetControlAbsolutePos(DimmerPop, tr2, lc1, widthbtn);

                            Groupbox1.Controls.Add(DimmerDop);
                            Groupbox1.Controls.Add(CreateLabelTitle_OnLeft("Svetilnost:", DimmerDop, topOffsetLabel, 9.5F, 10, 1.1F, true, Settings.LightBlackColor));

                            Groupbox2.Controls.Add(DimmerPop);
                            Groupbox2.Controls.Add(CreateLabelTitle_OnLeft("Svetilnost:", DimmerPop, topOffsetLabel, 9.5F, 10, 1.1F, true, Settings.LightBlackColor));

                            DimmerDop.SaveClicked += DimmerDop_SaveClicked;
                            DimmerPop.SaveClicked += DimmerPop_SaveClicked;

                        }

                        Dopoldne.Text = "Dopoldne:";
                        Popoldne.Text = "Popoldne:";

                        SetFontProperties_vw(Dopoldne, 1.5F, true);
                        SetFontProperties_vw(Popoldne, 1.5F, true);

                        SetControlAbsolutePos(Dopoldne, 3, 2);
                        SetControlAbsolutePos(Popoldne, 3, 2);

                        Dopoldne.Style.Add(HtmlTextWriterStyle.Color, Settings.LightBlackColor);
                        Popoldne.Style.Add(HtmlTextWriterStyle.Color, Settings.LightBlackColor);

                        myID = id;                        

                        var rce = new Helper.TimeSelectorDatasource();
                        bool hasWeekTmr;

                        try
                        {
                            hasWeekTmr = XmlController.GetHasWeekTimer(id);

                            if (hasWeekTmr)
                            {
                                Vklop1 = new GControls.DropDownListForHourSelect("DD_on1", prop.VklopConadop[id].Value_WeektimerForSiemensLogoFormat, widthbtn, fontSize, false)
                                {
                                    Name = "VKLOP DOPOLDNE"
                                };

                                Vklop2 = new GControls.DropDownListForHourSelect("DD_on2", prop.VklopConapop[id].Value_WeektimerForSiemensLogoFormat, widthbtn, fontSize, false)
                                {
                                    Name = "VKLOP POPOLDNE"
                                };

                                Izklop1 = new GControls.DropDownListForHourSelect("DD_off1", prop.IzklopConadop[id].Value_WeektimerForSiemensLogoFormat, widthbtn, fontSize, false)
                                {
                                    Name = "IZKLOP DOPOLDNE"
                                };

                                Izklop2 = new GControls.DropDownListForHourSelect("DD_off2", prop.IzklopConapop[id].Value_WeektimerForSiemensLogoFormat, widthbtn, fontSize, false)
                                {
                                    Name = "IZKLOP POPOLDNE"
                                };
                            }                           

                        }
                        catch (Exception ex)
                        {
                            throw new Exception ("Error initialising dropdowns for hour select. Error info: " + ex.Message);
                        }
                                                
                        if (hasWeekTmr)
                        {

                            VklopUrnika1 = new GControls.PaddedOnOffButton(
                           "Vklop Urnika Dop", myID + 10, GetVklopUrnika_Dop(id), new Helper.Position(tr1 - 9, lc3, widthbtn*6), GControls.OnOffButton.Type.WithText);

                            VklopUrnika2 = new GControls.PaddedOnOffButton(
                           "Vklop Urnika Pop", myID + 10, GetVklopUrnika_Pop(id), new Helper.Position(tr1 - 9, lc3, widthbtn * 6), GControls.OnOffButton.Type.WithText);

                            SetControlAbsolutePos(Vklop1, tr1, lc1, widthdd);
                            SetControlAbsolutePos(Vklop2, tr1, lc1, widthdd);
                            SetControlAbsolutePos(Izklop1, tr1, lc2, widthdd);
                            SetControlAbsolutePos(Izklop2, tr1, lc2, widthdd);

                            VklopUrnika1.button.Click += VklopUrnika1_Button_Click;
                            VklopUrnika2.button.Click += VklopUrnika2Button_Click;
                            Groupbox1.Controls.Add(Vklop1);
                            Groupbox1.Controls.Add(CreateLabelTitle_OnLeft("Start:", Vklop1, topOffsetLabel, 5F, 10, 1.1F, true, Settings.LightBlackColor));
                            Groupbox1.Controls.Add(Izklop1);
                            Groupbox1.Controls.Add(CreateLabelTitle_OnLeft("Stop:", Izklop1, topOffsetLabel, 5F, 10, 1.1F, true, Settings.LightBlackColor));
                            Groupbox1.Controls.Add(VklopUrnika1);                            

                            Groupbox2.Controls.Add(Vklop2);
                            Groupbox2.Controls.Add(CreateLabelTitle_OnLeft("Start:", Vklop2, topOffsetLabel, 5F, 10, 1.1F, true, Settings.LightBlackColor));
                            Groupbox2.Controls.Add(Izklop2);
                            Groupbox2.Controls.Add(CreateLabelTitle_OnLeft("Stop:", Izklop2, topOffsetLabel, 5F, 10, 1.1F, true, Settings.LightBlackColor));
                            Groupbox2.Controls.Add(VklopUrnika2);                    

                            SetControlAbsolutePos(VklopUrnika1);
                            SetControlAbsolutePos(VklopUrnika2);

                            Vklop1.SaveClicked += Vklop1_SaveClicked;
                            Vklop2.SaveClicked += Vklop2_SaveClicked;
                            Izklop1.SaveClicked += Izklop1_SaveClicked;
                            Izklop2.SaveClicked += Izklop2_SaveClicked;
                        }

                        
                        Controls.Add(IzklopKoJeDan);
                        IzklopKoJeDan.button.Click += (sender, e) => IzklopKoJeDan_Click(sender, e, myID);   

                        TagName = "div";

                        SetControlAbsolutePos(Cona, tr1, tr1, widthdd);                        
                       
                        Controls.Add(Cona);
                        
                        Controls.Add(Groupbox1);
                        Controls.Add(Groupbox2);

                        Groupbox1.Controls.Add(Dopoldne);
                        Groupbox2.Controls.Add(Popoldne);

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error inside LuciSubmenuContent(int id) construstor. Error info:" + ex.Message);
                    }

                }

                private void IzklopKoJeDan_Click(object sender, ImageClickEventArgs e, int id)
                {
                    var prop = Val.logocontroler.Prop1;
                    prop.SetIzklopKoJeDan(id, !prop.GetIzklopiKoJeDan(id)); // toggle value
                }

                bool GetIzklopiKoJeDan(int id)
                {
                    return Val.logocontroler.Prop1.GetIzklopiKoJeDan(id);
                }

                bool GetVklopUrnika_Dop(int id)
                {
                    bool buff;
                    bool null1;
                    Val.logocontroler.Prop1.GetVklopUrnika(out buff, out null1, id);
                    return buff;
                }
                bool GetVklopUrnika_Pop(int id)
                {
                    bool buff;
                    bool null1;
                    Val.logocontroler.Prop1.GetVklopUrnika(out null1, out buff, id);
                    return buff;
                }

                    void saveClickedTmr(ListItem selectedItem)
                {
                    var buff = selectedItem.Text;
                    Val.logocontroler.Prop1.IzklopConapop[myID].Value_WeektimerForSiemensLogoFormat = buff;
                }

                private void Izklop2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    saveClickedTmr(selectedItem);
                }

                private void Izklop1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    saveClickedTmr(selectedItem);
                }

                private void Vklop2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    saveClickedTmr(selectedItem);
                }

                private void Vklop1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    saveClickedTmr(selectedItem);
                }

                private void DimmerPop_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    var buff = Helper.Datasource.GetValueFromText_short(selectedItem.Text);
                    Val.logocontroler.Prop1.DimmerPop[myID].Value = buff;
                }

                private void DimmerDop_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    var buff = Helper.Datasource.GetValueFromText_short(selectedItem.Text);
                    Val.logocontroler.Prop1.DimmerDop[myID].Value = buff;
                }

                private void VklopUrnika1_Button_Click(object sender, ImageClickEventArgs e)
                {
                    var prop = Val.logocontroler.Prop1;
                    bool dop;
                    bool pop;

                    prop.GetVklopUrnika(out dop, out pop, myID);
                    prop.SetVklopUrnika(myID, !dop, pop); // reverse current value
                }

                private void VklopUrnika2Button_Click(object sender, ImageClickEventArgs e)
                {
                    var prop = Val.logocontroler.Prop1;
                    bool dop;
                    bool pop;

                    prop.GetVklopUrnika(out dop, out pop, myID);
                    prop.SetVklopUrnika(myID, dop, !pop); // reverse current value
                }
            }
        }
    }
}
