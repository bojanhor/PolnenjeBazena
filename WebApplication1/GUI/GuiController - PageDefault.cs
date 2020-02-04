using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class PageDefault
        {
            public static string ViewStateElement_LucActiveOnGui = "LucActiveOnGui";
                        
            Page thisPage;
            System.Web.SessionState.HttpSessionState session;
            public GControls.MasterMenuButton[] imagebuttons;
            public Panel btnPannel;

            public UpdatePanel LuciUpdatePanel;
            public AsyncPostBackTrigger Ap_LuciUpdatePanel;
            public Timer Tmr_LuciUpdatePanel;

            public GControls.Luc[] Luc;

            public HtmlGenericControl divWeather;
            ImageButton inTemp;
            ImageButton outTemp;
            ImageButton rainSense;
            ImageButton dayNight;
            ImageButton SunRise;
            ImageButton SunSet;

            ImageButton[] guiSepare = new ImageButton[5];           
            
            public HtmlGenericControl divStala;
            Image Stala;

            HtmlGenericControl divMasterButtons;
                                   
            public PageDefault(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {               
                this.session = session;                

                try
                {
                    thisPage = _thisPage;
                    imagebuttons = new GControls.MasterMenuButton[GethowManyButtonsOnFirstPage()];
                    btnPannel = new Panel();

                    Luc = new GControls.Luc[XmlController.GetHowManyLucIcons() + 1];
                    divStala = DIV.CreateDivAbsolute();

                    Stala = new Image();

                    ManageUpdatePanelLuci();
                    AddStala();
                    InitializeLuci();
                    AddImageButtons_Menu();
                    AddWeather();
                    AddbtnPanel();

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside PageDefault class constructor: " + ex.Message);
                }


            }

            public void ForceRefreshPanel()
            {
                Tmr_LuciUpdatePanel.Interval = 300;
            }

            void ManageUpdatePanelLuci()
            {
                try
                {
                    LuciUpdatePanel = new UpdatePanel();
                    LuciUpdatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                    LuciUpdatePanel.ID = "LuciUpdatePanel";

                    Tmr_LuciUpdatePanel = new Timer();
                    Tmr_LuciUpdatePanel.Interval = Settings.UpdateValuesPCms;
                    Tmr_LuciUpdatePanel.ID = "Tmr_LuciUpdatePanel";

                    Ap_LuciUpdatePanel = new AsyncPostBackTrigger();
                    Ap_LuciUpdatePanel.ControlID = "Tmr_LuciUpdatePanel";

                    LuciUpdatePanel.Triggers.Add(Ap_LuciUpdatePanel);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error was encountered inside ManageUpdatePanelLuci() method. Error details: " + ex.Message);
                }
                
            }

            void AddWeather()
            {
                try
                {
                    divWeather = DIV.CreateDiv("80%", "51%", "40%", "12%");
                    divWeather.Style.Add(HtmlTextWriterStyle.ZIndex, "10");

                    inTemp = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/temp-in.png",
                        Width = Unit.Percentage(11),
                    };

                    outTemp = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/temp-out.png",
                        Width = inTemp.Width,
                    };

                    rainSense = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/vlaznost.png",
                        Width = inTemp.Width,
                    };

                    dayNight = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/sun.png",
                        Width = inTemp.Width,
                    };

                    SunRise = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/sunRise.png",
                        Width = inTemp.Width,
                    };

                    SunSet = new ImageButton()
                    {
                        ImageUrl = "~/Pictures/sunSet.png",
                        Width = inTemp.Width,
                    };


                    for (int i = 0; i < guiSepare.Length; i++)
                    {
                        guiSepare[i] = new ImageButton()
                        {
                            ImageUrl = "~/Pictures/gui_separator.png",
                            Width = Unit.Percentage(3.5F)

                        };
                    }
                   
                    var topOffset = 90;
                    var spacingLeft = 14.5F;

                    var t = inTemp.Width.ToString();
                    var inTempD = DIV.CreateDiv(t);
                    var outTempD = DIV.CreateDiv(t);
                    var rainSenseD = DIV.CreateDiv(t);

                    var prop = Val.logocontroler.Prop2; // Change temperature source here

                    WeatherLabelFormater("Dan/Noč", inTemp.Width.Value, topOffset, 0);
                    WeatherLabelFormater("Vzhod", inTemp.Width.Value, topOffset, spacingLeft);
                    WeatherLabelFormater("Zahod", inTemp.Width.Value, topOffset, spacingLeft * 2);
                    WeatherLabelFormater(prop.TempZnotraj.Value, inTemp.Width.Value, topOffset, spacingLeft * 3);
                    WeatherLabelFormater(prop.TempZunaj.Value, inTemp.Width.Value, topOffset, spacingLeft * 4);
                    WeatherLabelFormater("25L/dan", inTemp.Width.Value, topOffset, spacingLeft * 5);

                    divWeather.Controls.Add(dayNight);
                    divWeather.Controls.Add(guiSepare[0]);
                    divWeather.Controls.Add(SunRise);
                    divWeather.Controls.Add(guiSepare[1]);
                    divWeather.Controls.Add(SunSet);
                    divWeather.Controls.Add(guiSepare[2]);

                    divWeather.Controls.Add(inTemp);
                    divWeather.Controls.Add(guiSepare[3]);
                    divWeather.Controls.Add(outTemp);
                    divWeather.Controls.Add(guiSepare[4]);
                    divWeather.Controls.Add(rainSense);
                  
                }
                catch (Exception ex)
                {
                    throw new Exception("Error iside AddWeather() method. Error info: "+ex.Message);
                }
                

            }

            void WeatherLabelFormater(string LableText, double width, float topOffset, float spacingLeft)
            {               
                var div = DIV.CreateDiv(Helper.FloatToStringWeb(width, "%"));
                var l = new Label();
                div.Style.Add(HtmlTextWriterStyle.Width, width + "%");
                l.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                l.Style.Add(HtmlTextWriterStyle.FontSize, "1.2vw");
                div.Style.Add(HtmlTextWriterStyle.Top, Helper.FloatToStringWeb(topOffset, "%"));
                div.Style.Add(HtmlTextWriterStyle.Left, Helper.FloatToStringWeb(spacingLeft, "%"));
                l.Width = Unit.Percentage(100);
                l.Text = LableText;
                div.Controls.Add(l);

                divWeather.Controls.Add(div);

            }

            private void InitializeLuci()
            {
                try
                {
                    for (int i = 1; i <= XmlController.GetHowManyLucIcons(); i++)
                    {
                        Luc[i] = new GControls.Luc(i);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error initialising GControls.Luc() class instances. Error info: " + ex.Message);
                }

                try
                {
                    foreach (var item in Luc)
                    {
                        if (item != null)
                        {
                            divStala.Controls.Add(item);
                        }
                    }

                    LuciUpdatePanel.ContentTemplateContainer.Controls.Add(divStala);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error adding controls to panel (LuciUpdatePanel). Error info: " + ex.Message);
                }
               

            }

            public void RegisterOnClick()
            {
                for (int i = 0; i < imagebuttons.Length; i++)
                {
                    imagebuttons[i].Click += (sender, e)=>BtnMasterMenuClick(sender, e, thisPage);
                }          

                inTemp.Click += InTemp_Click;
                outTemp.Click += InTemp_Click;
                rainSense.Click += InTemp_Click;

            }

            private void InTemp_Click(object sender, ImageClickEventArgs e)
            {
                Helper.Redirect("vreme", thisPage);
            }                        

            private void BtnMasterMenuClick(object sender, ImageClickEventArgs e, Page thisPage)
            {
                var me = (GControls.MasterMenuButton)sender;
                Val.guiController.RedirectToPageOnButtonClick(thisPage, me.btnID);
            }

            private static int GethowManyButtonsOnFirstPage()
            {
                return XmlController.GetHowManyMenuItems();
            }

            void AddbtnPanel()
            {
                btnPannel.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                btnPannel.Style.Add(HtmlTextWriterStyle.Top, "0%");
                btnPannel.Style.Add(HtmlTextWriterStyle.Left, "90%");
                btnPannel.Style.Add(HtmlTextWriterStyle.Width, "8%");
                btnPannel.Style.Add(HtmlTextWriterStyle.Height, "95%");
                btnPannel.ID = "btnPannel";
            }

            void AddStala()
            {
                try
                {
                    Stala.ImageUrl = "~/Pictures/MasterPic.png ";
                    Stala.Style.Add(HtmlTextWriterStyle.Width, "100%");

                    divStala.Controls.Add(Stala);
                    divStala.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    divStala.Style.Add(HtmlTextWriterStyle.Height, "100%");
                    divStala.ID = "divStala";
                }
                catch (Exception ex)
                {

                    throw new Exception("Error was encountered inside AddStala() method. Error details: " + ex.Message);
                }
                

            }

            private void AddImageButtons_Menu()
            {
                float spacing = 17.5F;
                float initialPos = 11.0F;

                try
                {
                    for (int i = 0; i < imagebuttons.Length; i++)
                    {
                        divMasterButtons = DIV.CreateDiv(Helper.FloatToStringWeb(initialPos, "%"));
                        initialPos += spacing;
                        imagebuttons[i] = new GControls.MasterMenuButton(i + 1);
                        divMasterButtons.ID = imagebuttons[i].ID + "_div";
                        divMasterButtons.Controls.Add(imagebuttons[i]);
                        btnPannel.Controls.Add(divMasterButtons);

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error inside AddImageButtons_Menu() method. Error info: " + ex.Message);
                }

                
            }
                                  
        }
    }
}