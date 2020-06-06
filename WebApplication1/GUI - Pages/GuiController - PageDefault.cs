using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Security.Cryptography;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class PageDefault : Dsps
        {                                   
            Page thisPage;
            string Name;

            System.Web.SessionState.HttpSessionState session;
            public GControls.MasterMenuButton[] imagebuttons;
           
            public UpdatePanel UpdatePanel;
            public AsyncPostBackTrigger Ap_UpdatePanel;
            public Timer Tmr_UpdatePanel;
                       
            public HtmlGenericControl divMaster;
                                                                     
            public PageDefault(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                this.session = session;

                try
                {
                    thisPage = _thisPage;
                    Name = PageHistory.GetPageNameFromPage(_thisPage);

                    imagebuttons = new GControls.MasterMenuButton[GethowManyButtonsOnFirstPage()];
                                      
                    divMaster = DIV.CreateDivAbsolute();
                   
                    ManageUpdatePanel();
                    AddMasterDiv();
                    AddContent();

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside class constructor (Page: "+ Name + "): " + ex.Message);
                }
            }

            void ManageUpdatePanel()
            {
                try
                {
                    UpdatePanel = new UpdatePanel
                    {
                        UpdateMode = UpdatePanelUpdateMode.Conditional,
                        ID = Name + "UpdatePanel"
                    };

                    Tmr_UpdatePanel = new Timer
                    {
                        Interval = Settings.UpdateValuesPCms,
                        ID = Name + "Tmr_UpdatePanel"
                    };

                    Ap_UpdatePanel = new AsyncPostBackTrigger
                    {
                        ControlID = Name + "Tmr_UpdatePanel"
                    };

                    UpdatePanel.Triggers.Add(Ap_UpdatePanel);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error was encountered inside ManageUpdatePanel() method. Error details: " + ex.Message);
                }
                
            }
                      
           
            private static int GethowManyButtonsOnFirstPage()
            {
                return XmlController.GetHowManyMenuItems();
            }

           
            void AddMasterDiv()
            {
                try
                {
                    divMaster.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    divMaster.Style.Add(HtmlTextWriterStyle.Height, "100%");
                    divMaster.ID = "divMaster";
                }
                catch (Exception ex)
                {

                    throw new Exception("Error was encountered inside AddStala() method. Error details: " + ex.Message);
                }
                

            }

            void AddContent()
            {
                var gb1 = new GControls.GroupBox(10, 74, 24, 83);

                // Control buttons    
                Kontrola(gb1);                

                // Stanje               
                Stanje();

                Trak();


                divMaster.Controls.Add(gb1);
            }

            string imageUrl(string Name)
            {
                return "~/Pictures/" +Name + ".png";
            }

            void Kontrola(HtmlGenericControl gb1)
            {
                int top = 0; int dif = 16; int size = 50; int left = 46;
                int dottop = 6; int dotdif = 32; int dotSize = 55; int dotlft = 20;

                GControls.ImageButtonWithID btnStart = new GControls.ImageButtonWithID("Start", 1)
                { ImageUrl = imageUrl("Start1") }; SetControlAbsolutePos(btnStart, top, left, size);
                top += dif; gb1.Controls.Add(btnStart);

                GControls.ImageButtonWithID btnStop = new GControls.ImageButtonWithID("Stop", 1)
                { ImageUrl = imageUrl("Stop1") }; SetControlAbsolutePos(btnStop, top, left, size);

                top += dif + 2; gb1.Controls.Add(btnStop);

                GControls.OnOffButton btnAuto = new GControls.OnOffButton("Auto", 1, false, new Helper.Position(top, left, size), GControls.OnOffButton.Type.WithText);

                top += dif; gb1.Controls.Add(btnAuto);

                GControls.OnOffButton btnTrak = new GControls.OnOffButton("Trak", 1, false, new Helper.Position(top, left, size), GControls.OnOffButton.Type.WithText);
                top += dif; gb1.Controls.Add(btnTrak);

                GControls.ImageButtonWithID btnCirc = new GControls.ImageButtonWithID("Circ", 1)
                { ImageUrl = imageUrl("Circ1") }; SetControlAbsolutePos(btnCirc, top, left, size);
                top += dif; gb1.Controls.Add(btnCirc);

                GControls.ImageButtonWithID btnZig = new GControls.ImageButtonWithID("Zig", 1)
                { ImageUrl = imageUrl("Zig1") }; SetControlAbsolutePos(btnZig, top, left, size);
                gb1.Controls.Add(btnZig);

                // Semaphore
                var gb2 = new GControls.GroupBox(2, 4, 30, 49);

                GControls.OnOffShowRound RedDot = new GControls.OnOffShowRound("Grn", false, new Helper.Position(top, left, 100), "green");
                SetControlAbsolutePos(RedDot, dottop, dotlft, dotSize);
                dottop += dotdif;

                GControls.OnOffShowRound GnDot = new GControls.OnOffShowRound("Red", false, new Helper.Position(top, left, 100), "red");
                SetControlAbsolutePos(GnDot, dottop, dotlft, dotSize);
                dottop += dotdif;

                GControls.OnOffShowRound YelDot = new GControls.OnOffShowRound("Yel", false, new Helper.Position(top, left, 100), "yellow");
                SetControlAbsolutePos(YelDot, dottop, dotlft, dotSize);

                gb2.Controls.Add(RedDot); gb2.Controls.Add(GnDot); gb2.Controls.Add(YelDot);
                gb1.Controls.Add(gb2);

                // speedSel
                GControls.SuperLabel spdlbl = new GControls.SuperLabel("Hitrost:", 67, 15, 20, 10);
                GControls.DropDownListForDimmerLUX speed = new GControls.DropDownListForDimmerLUX("Speedsel", "Text", 70, 2, 5, 1.5F, false, false);
                gb1.Controls.Add(speed);
                gb1.Controls.Add(spdlbl);
            }
            void Stanje()
            {
                var top = 13;
                var dif = 8;

                var gb3 = new GControls.GroupBox(20, 1, 18, 70);
                GControls.SuperLabel Status = new GControls.SuperLabel("Stanje:", 3, 35, 30, 10) { FontWeightBold = true };

                var msgs = StanjeProcesa.SporocilaZaPrikaz; 
                GControls.SuperLabel lbl;

                for (int i = 0; i < msgs.Count; i++)
                {
                    lbl = new GControls.SuperLabel(msgs[i], top, 7, 70, 10);
                    gb3.Controls.Add(lbl);
                    top += dif;
                }

                gb3.Controls.Add(Status);
                divMaster.Controls.Add(gb3);

            }

            void JoyStick()
            { 
                
            }

            void Trak()
            {
                GControls.Conveyor c = new GControls.Conveyor("convey", 20,30,20, Val.logocontroler.Prop1. true);
                
                divMaster.Controls.Add(c);
                
            }



            public void RegisterOnClick()
            {

            }
                        
            
        }
    }
}