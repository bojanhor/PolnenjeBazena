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
        public class PageRazsvetljava : Dsps
        {
            public Page ThisPage;
            public System.Web.SessionState.HttpSessionState Session;
            public static string ViewStateElement_CurrentLuciSettingsShown = "CurrentLuciSettingsShown";

            public GControls.UpdatePanelFull updatePanel;

            public HtmlGenericControl divStala;
            Image Stala;           

            public HtmlGenericControl divLuciSettings;  
            
            public GControls.LucSet[] Luc;
            public Label[] AboveLucLableDimmer;

            public HtmlGenericControl divBtns;
            ImageButton ugasniVseLuci;
          
            public LuciSettingsMenu LuciSettings;
            readonly string UpdatePanelId_Luci = "LuciPanel";


            public PageRazsvetljava(Page thisPage, System.Web.SessionState.HttpSessionState session)
            {                
                try
                {
                    ThisPage = thisPage;                    
                    Session = session;

                    Luc = new GControls.LucSet[XmlController.GetHowManyLucIcons() + 1];
                    AboveLucLableDimmer = new Label[Luc.Length]; 
                    divStala = DIV.CreateDivAbsolute();

                    Stala = new Image();

                    divLuciSettings = new HtmlGenericControl("div")
                    {
                        ID = "divLuciSettings"
                    };

                    InitializeLuci();
                    AddStala();
                    InitializeLuciSettings();
                    AddBtns();

                              

                    HidePanel();


                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside Razsvetljava class constructor: " + ex.Message);
                }
            }

            int GetCurrentLuciSettingsShown()
            {
                // save value over session
                var buff = Session[ViewStateElement_CurrentLuciSettingsShown];
                if (buff == null)
                {
                    buff = 0;
                }

                return (int)buff;

            }

            void SetCurrentLuciSettingsShown(int value)
            {
                // save value over session
                Session[ViewStateElement_CurrentLuciSettingsShown] = value;
                
            }

            void IncrementCurrentLuciSettingsShown(int incrementBy)
            {
                SetCurrentLuciSettingsShown(
                    GetCurrentLuciSettingsShown() + incrementBy);
            }

            public void HidePanel()
            {
                if (GetCurrentLuciSettingsShown() == 0)
                {   
                    divLuciSettings.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");                   
                }                

            }

            void InitializeLuciSettings()
            {
                try
                {
                    updatePanel = new GControls.UpdatePanelFull("RazsvetljavaUP", Settings.UpdateValuesPCms);
                    
                    bool hasPrevious = true;
                    bool hasNext = true;
                    int lucIconsCnt = XmlController.GetHowManyLucIcons();
                    int currentShown = GetCurrentLuciSettingsShown();
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
                SetCurrentLuciSettingsShown(0); // hideDiv
                Navigator.Refresh();
            }

            void PrevButton_Click(object sender, ImageClickEventArgs e)
            {
                IncrementCurrentLuciSettingsShown(-1);
                Navigator.Refresh();
            }

            void NextButton_Click(object sender, ImageClickEventArgs e)
            {
                IncrementCurrentLuciSettingsShown(1);
                Navigator.Refresh();
            }

            private void InitializeLuci()
            {
                var up = (UpdatePanel)ThisPage.FindControl(UpdatePanelId_Luci); // finds common updatepanel for LucSet controls
                var prop =  Val.logocontroler.Prop1;
                var dimmPercent = prop.DimmerActual; // gets analog value if device supports it
                string dimmPercentVal; // buffer used to switch between "dimmPercent" (if supported) or "0%" / "100%" representation text from digital value
               
                try
                {
                    for (int i = 1; i <= XmlController.GetHowManyLucIcons(); i++)
                    {
                        Luc[i] = new GControls.LucSet(i, up);
                        AboveLucLableDimmer[i] = new Label();

                        // GET  dimmPercentVal FROM dimmer if supported   -   if not supported get digital value and convert to 0 or 100 % representation text
                        dimmPercentVal = GetRepresentationDimmerValue(dimmPercent[i], prop.LucStatus_ReadToPC[i]);

                        AboveLucLableDimmer[i] = CreateLabelTitle_OnTop(
                            dimmPercentVal, Luc[i], 2.5F, 0, Convert.ToInt32(Luc[i].Width), 1, false, true, "black"); 

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

            string GetRepresentationDimmerValue(PlcVars.Word dimmPercent, PlcVars.Bit bit)
            {
                if (dimmPercent != null)
                {
                    return dimmPercent.Value_string;
                }

                if (bit.Value == true)
                {
                    return "100%";
                }
                else if (bit.Value == false)
                {
                    return "0%";
                }

                return PropComm.NA;
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
                SetCurrentLuciSettingsShown(luc.btnID);

                var panel = (HtmlGenericControl)ThisPage.FindControl("divLuciSettings");
                panel.Style.Add(HtmlTextWriterStyle.Visibility, "visible");

                Navigator.Refresh();
            }

            private void UgasniVseLuci_Click(object sender, EventArgs e)
            {
                Val.logocontroler.Prop1.UgasniVseLuci.SendPulse();
            }
                        
            void AddBtns()
            {
                try
                {
                    divBtns = DIV.CreateDivAbsolute("10%", "80%", "14%", "83%");
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
                    :base(id, Name_, hasNext, hasPreious, hasExit, GetContent(id))
                {
                    
                }

                static LuciSubmenuContent GetContent(int id)
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
                readonly bool hasDimmer = false;


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
                public GControls.OnOffButton VklopUrnika1;
                public GControls.OnOffButton VklopUrnika2;

                public GControls.DropDownListForDimmerLUX DimmerDop;
                public GControls.DropDownListForDimmerLUX DimmerPop;
                               
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
                            DimmerDop = new GControls.DropDownListForDimmerLUX("DD_dimAm", prop.DimmerDop[id].Value_string,0,0, widthbtn, fontSize, true, false);
                            DimmerPop = new GControls.DropDownListForDimmerLUX("DD_dimPm", prop.DimmerPop[id].Value_string, 0, 0, widthbtn, fontSize, true, false);

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

                        var rce = new Datasourcer.TimeSelectorDatasource();
                        bool hasWeekTmr;

                        try
                        {
                            hasWeekTmr = XmlController.GetHasWeekTimer(id);

                            if (hasWeekTmr)
                            {
                                Vklop1 = new GControls.DropDownListForHourSelect("DD_on1", prop.VklopConadop[id].Value, widthbtn, fontSize, false)
                                {
                                    Name = "VKLOP DOPOLDNE"
                                };

                                Vklop2 = new GControls.DropDownListForHourSelect("DD_on2", prop.VklopConapop[id].Value, widthbtn, fontSize, false)
                                {
                                    Name = "VKLOP POPOLDNE"
                                };

                                Izklop1 = new GControls.DropDownListForHourSelect("DD_off1", prop.IzklopConadop[id].Value, widthbtn, fontSize, false)
                                {
                                    Name = "IZKLOP DOPOLDNE"
                                };

                                Izklop2 = new GControls.DropDownListForHourSelect("DD_off2", prop.IzklopConapop[id].Value, widthbtn, fontSize, false)
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

                            VklopUrnika1 = new GControls.OnOffButton(
                           "Vklop Urnika Dop", myID + 10, GetVklopUrnika_Dop(id), new Helper.Position(tr1 - 9, lc3, widthbtn*6), GControls.OnOffButton.Type.WithText);

                            VklopUrnika2 = new GControls.OnOffButton(
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
                    Val.logocontroler.Prop1.GetVklopUrnika(out bool buff, out bool null1, id);
                    return buff;
                }
                bool GetVklopUrnika_Pop(int id)
                {
                    Val.logocontroler.Prop1.GetVklopUrnika(out bool null1, out bool buff, id);
                    return buff;
                }
                               
                private void Izklop2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop1.IzklopConapop[myID].Value = Izklop2.GetSelectedValue();                   
                }

                private void Izklop1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop1.IzklopConadop[myID].Value = Izklop1.GetSelectedValue();                    
                }

                private void Vklop2_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop1.VklopConapop[myID].Value = Vklop2.GetSelectedValue();                    
                }

                private void Vklop1_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    Val.logocontroler.Prop1.VklopConadop[myID].Value = Vklop1.GetSelectedValue();                    
                }

                private void DimmerPop_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    var buff = Datasourcer.Datasource.GetValueFromText_short(selectedItem.Text);
                    Val.logocontroler.Prop1.DimmerPop[myID].Value = buff;
                }

                private void DimmerDop_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    var buff = Datasourcer.Datasource.GetValueFromText_short(selectedItem.Text);
                    Val.logocontroler.Prop1.DimmerDop[myID].Value = buff;
                }

                private void VklopUrnika1_Button_Click(object sender, ImageClickEventArgs e)
                {
                    var prop = Val.logocontroler.Prop1;

                    prop.GetVklopUrnika(out bool dop, out bool pop, myID);
                    prop.SetVklopUrnika(myID, !dop, pop); // reverse current value
                }

                private void VklopUrnika2Button_Click(object sender, ImageClickEventArgs e)
                {
                    var prop = Val.logocontroler.Prop1;

                    prop.GetVklopUrnika(out bool dop, out bool pop, myID);
                    prop.SetVklopUrnika(myID, dop, !pop); // reverse current value
                }
            }            
        }
    }
}
