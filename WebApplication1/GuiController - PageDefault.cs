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
            Page thisPage;
            public GControls.MasterMenuButton[] imagebuttons;
            public Panel btnPannel;

            public GControls.Luc[] Luc;

            public HtmlGenericControl divWeather;
            ImageButton inTemp;
            ImageButton outTemp;
            ImageButton rainSense;
            ImageButton guiSepare1;
            ImageButton guiSepare2;
            Label inTempL;
            Label outTempL;
            Label rainSenseL;

            public HtmlGenericControl divStala;
            Image Stala;

            HtmlGenericControl divMasterButtons;

            string UpdatePanelId_Luci = "LuciPanel";


            public PageDefault(Page _thisPage)
            {
                try
                {
                    thisPage = _thisPage;
                    imagebuttons = new GControls.MasterMenuButton[GethowManyButtonsOnFirstPage()];
                    btnPannel = new Panel();

                    Luc = new GControls.Luc[XmlController.GetHowManyLucIcons() + 1];
                    divStala = DIV.CreateDivAbsolute();

                    Stala = new Image();

                    AddbtnPanel();
                    AddStala();
                    InitializeLuci();
                    AddImageButtons();
                    AddWeather();

                }
                catch (Exception ex)
                {

                    throw new Exception("Internal error inside PageDefault class constructor: " + ex.Message);
                }


            }

            void AddWeather()
            {
                divWeather = DIV.CreateDiv("77%", "70%", "25%", "16%");


                inTemp = new ImageButton()
                {
                    ImageUrl = "~/Pictures/temp-in.png",
                    Width = Unit.Percentage(20),
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

                guiSepare1 = new ImageButton()
                {
                    ImageUrl = "~/Pictures/gui_separator.png",
                    Width = Unit.Percentage(5),
                };
                guiSepare2 = new ImageButton()
                {
                    ImageUrl = "~/Pictures/gui_separator.png",
                    Width = guiSepare1.Width,
                };


                var topOffset = 75;
                
                var spacingLeft = 25;

                
                var t = inTemp.Width.ToString();
                var inTempD = DIV.CreateDiv(t);
                var outTempD = DIV.CreateDiv(t);
                var rainSenseD = DIV.CreateDiv(t);

                inTempL = new Label
                {
                    Text = "25.5°C",
                    Width = Unit.Percentage(100),
                };
                inTempD.Style.Add(HtmlTextWriterStyle.Width, inTemp.Width + "");
                inTempL.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                inTempL.Style.Add(HtmlTextWriterStyle.FontSize, "1.2vw");
                inTempD.Style.Add(HtmlTextWriterStyle.Top, topOffset + "%");
                inTempD.Style.Add(HtmlTextWriterStyle.Left, 0 + "%");
                inTempD.Controls.Add(inTempL);

                outTempL = new Label
                {
                    Text = "-16.5°C",
                    Width = Unit.Percentage(100),
                };
                outTempD.Style.Add(HtmlTextWriterStyle.Width, inTemp.Width + "");
                outTempL.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                outTempL.Style.Add(HtmlTextWriterStyle.FontSize, "1.2vw");
                outTempD.Style.Add(HtmlTextWriterStyle.Top, topOffset + "%");
                outTempD.Style.Add(HtmlTextWriterStyle.Left, spacingLeft + "%");
                outTempD.Controls.Add(outTempL);


                rainSenseL = new Label
                {
                    Text = "25L/dan",
                    Width = Unit.Percentage(100),
                };
                rainSenseD.Style.Add(HtmlTextWriterStyle.Width, inTemp.Width + "");
                rainSenseL.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                rainSenseL.Style.Add(HtmlTextWriterStyle.FontSize, "1.2vw");
                rainSenseD.Style.Add(HtmlTextWriterStyle.Top, topOffset + "%");
                rainSenseD.Style.Add(HtmlTextWriterStyle.Left, spacingLeft * 2 + "%");
                rainSenseD.Controls.Add(rainSenseL);



                divWeather.Controls.Add(inTemp);
                divWeather.Controls.Add(guiSepare1);
                divWeather.Controls.Add(outTemp);
                divWeather.Controls.Add(guiSepare2);
                divWeather.Controls.Add(rainSense);

                divWeather.Controls.Add(inTempD);
                divWeather.Controls.Add(outTempD);
                divWeather.Controls.Add(rainSenseD);

            }

            private void InitializeLuci()
            {
                
                for (int i = 1; i <= XmlController.GetHowManyLucIcons(); i++)
                {
                    Luc[i] = new GControls.Luc(i, (UpdatePanel)thisPage.FindControl(UpdatePanelId_Luci));
                }

            }

            public void RegisterOnClick()
            {
                for (int i = 0; i < imagebuttons.Length; i++)
                {
                    imagebuttons[i].Click += (sender, e)=>BtnMasterMenuClick(sender, e, thisPage);
                }

                for (int i = 1; i < Luc.Length; i++)
                {
                    Luc[i].button.Click += ZarnicaClick;
                }

                inTemp.Click += InTemp_Click;
                outTemp.Click += InTemp_Click;
                rainSense.Click += InTemp_Click;


            }

            private void InTemp_Click(object sender, ImageClickEventArgs e)
            {
                Helper.Redirect("vreme", thisPage);
            }

            private void ZarnicaClick(object sender, EventArgs e)
            {

                var luc = (GControls.LucBtn)sender;
                var plcItemWrite = Val.logocontroler.Prop1.LucStatus_WriteToPLC[luc.btnID];
                plcItemWrite.SendPulse();

                var plcItemRead = Val.logocontroler.Prop1.LucStatus_ReadToPC[luc.btnID]; // TODO odstrani


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

                Stala.ImageUrl = "~/Pictures/MasterPic.png ";
                Stala.Style.Add(HtmlTextWriterStyle.ZIndex, "1");
                Stala.Style.Add(HtmlTextWriterStyle.Width, "100%");

                divStala.Controls.Add(Stala);
                divStala.Style.Add(HtmlTextWriterStyle.Width, "100%");
                divStala.Style.Add(HtmlTextWriterStyle.Height, "100%");
                divStala.ID = "divStala";

            }

            private void AddImageButtons()
            {
                float spacing = 17.5F;
                float initialPos = 11.0F;

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


        }

    }
}