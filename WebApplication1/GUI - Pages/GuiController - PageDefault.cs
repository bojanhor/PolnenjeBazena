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
        public class PageDefault : Dsps
        {
            public static string ViewStateElement_LucActiveOnGui = "LucActiveOnGui";
                        
            Page thisPage;
            System.Web.SessionState.HttpSessionState session;
            public GControls.MasterMenuButton[] imagebuttons;
            public Panel btnPannel;

            public UpdatePanel UpdatePanel;
            public AsyncPostBackTrigger Ap_UpdatePanel;
            public Timer Tmr_UpdatePanel;
                       
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
                  
                    divStala = DIV.CreateDivAbsolute();
                    Stala = new Image();

                    ManageUpdatePanel();
                    AddStala();                   
                    AddImageButtons_Menu();                   
                    AddbtnPanel();

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside PageDefault class constructor: " + ex.Message);
                }
            }

            void ManageUpdatePanel()
            {
                try
                {
                    UpdatePanel = new UpdatePanel
                    {
                        UpdateMode = UpdatePanelUpdateMode.Conditional,
                        ID = "LuciUpdatePanel"
                    };

                    Tmr_UpdatePanel = new Timer
                    {
                        Interval = Settings.UpdateValuesPCms,
                        ID = "Tmr_LuciUpdatePanel"
                    };

                    Ap_UpdatePanel = new AsyncPostBackTrigger
                    {
                        ControlID = "Tmr_LuciUpdatePanel"
                    };

                    UpdatePanel.Triggers.Add(Ap_UpdatePanel);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error was encountered inside ManageUpdatePanelLuci() method. Error details: " + ex.Message);
                }
                
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
                        divMasterButtons = DIV.CreateDivAbsolute(Helper.FloatToStringWeb(initialPos, "%"));
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

            public void RegisterOnClick()
            {

            }
                                  
        }
    }
}