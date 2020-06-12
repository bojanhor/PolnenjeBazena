using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.IO;

namespace WebApplication1
{
    
    public partial class GuiController
    {
        public class PageAdvanced : Dsps
        {
            readonly Page thisPage;
            public System.Web.SessionState.HttpSessionState Session;
            
            public HtmlGenericControl divDebug;
            HtmlGenericControl divDebug_1;
            HtmlGenericControl divDebug_2;
            public GControls.ButtonWithLabel BtnEditor = new GControls.ButtonWithLabel("XMLSettings", 10, 1.2F);
            public GControls.ButtonWithLabel BtnLogView = new GControls.ButtonWithLabel("LogView", 10, 1.2F);
            public GControls.ButtonWithLabel BtnRestartServer = new GControls.ButtonWithLabel("RestatrtServer", 10, 1.2F);

            public GControls.NotShadowedOnOffButton Spremljaj;
            readonly GControls.ImageButtonWithID EditBtn = new GControls.ImageButtonWithID("Edit", 0);

            
            public GControls.NotShadowedOnOffButton[] connectSwitch = new GControls.NotShadowedOnOffButton[Settings.Devices];

            public Label LabelDebug = new Label();
            public HtmlGenericControl LabelDebugDiv;
            public Label SpremljajLable = new Label();
            public TextBox TextBoxDebug = new TextBox();


            public PageAdvanced(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                thisPage = _thisPage;
                Session = session;

                Spremljaj = new GControls.NotShadowedOnOffButton("Spremljaj", 1, GetSpremljajChecked(), new Helper.Position(0, 0, 15));

                AppDomainInitializerTables();
                AddLabelDebug();                
                PositionEdit();
                PositionLogView();
                PositionRestartServerBttn();
                AddSpremljajFunctionality();
            }

            void AddSpremljajFunctionality()
            {
                var session = Session[SessionHelper.ScriptLoader];

                if (session != null)
                {
                    var scriptloader = (Navigator.ScriptLoader)session;

                    if (!Spremljaj.active)
                    {
                        scriptloader.RegisterScriptOnPageLoad("scrollTo", Val.ScrolToBottomTextboxScript);
                    }
                    else
                    {
                        scriptloader.RegisterScriptOnPageLoad("scrollTo", Val.RetainPositionTextboxScript);
                    }
                }
                
            }

            bool GetSpremljajChecked()
            {
                // get value over session
                var buff = Session[SessionHelper.SpremljajChecked];
                if (buff == null)
                {
                    buff = false;
                }

                return (bool)buff;

            }

            void SetSpremljajChecked(bool value)
            {
                // set value over session
                Session[SessionHelper.SpremljajChecked] = value;                
            }

            void PositionEdit()
            {                
                SetControlAbsolutePos(BtnEditor,15, 86);
                BtnEditor.button.Click += Button_Click1;
                
            }

            void PositionLogView()
            {
                SetControlAbsolutePos(BtnLogView, 25, 86);
                BtnLogView.button.Click += Button_Click2; ;

            }

            void PositionRestartServerBttn()
            {
                SetControlAbsolutePos(BtnRestartServer, 35, 86);
                BtnRestartServer.button.Click += Button_Click;

            }

            private void Button_Click(object sender, ImageClickEventArgs e)
            {
                Navigator.Redirect("Default" + "");
                HttpRuntime.UnloadAppDomain();
            }

            private void Button_Click2(object sender, ImageClickEventArgs e)
            {
                Navigator.Redirect("LogView");
            }

            private void Button_Click1(object sender, ImageClickEventArgs e)
            {
                Navigator.Redirect("Editor");
            }
                        

            void AppDomainInitializerTables()
            {                

                for (int i = 0; i < Settings.Devices; i++)
                {
                                        
                    connectSwitch[i] = new GControls.NotShadowedOnOffButton(
                        "ConnectButton",
                        (i+1), 
                        Val.logocontroler.LOGOConnection[i+1].IsLogoConnected, 
                        new Helper.Position(0,0,100));

                    connectSwitch[i].Style.Add(HtmlTextWriterStyle.Position, "relative");
                }
            }

            void AddLabelDebug()
            {
                divDebug = DIV.CreateDivAbsolute();
                divDebug_1 = DIV.CreateDivRelative();
                divDebug_2 = DIV.CreateDivRelative();


                divDebug.Style.Add(HtmlTextWriterStyle.Top, "10%");
                divDebug.Style.Add(HtmlTextWriterStyle.Left, "40%");
                divDebug.Style.Add(HtmlTextWriterStyle.Width, "45%");
                divDebug.Style.Add(HtmlTextWriterStyle.Height, "78%");

                LabelDebug.ID = "LabelDebug";
                LabelDebug.Text = "Dnevnik:";

                LabelDebugDiv = DIV.CreateDivAbsolute(0, 0, 100, 100, "%");
                LabelDebugDiv.Style.Add(HtmlTextWriterStyle.BackgroundColor, Settings.RedColorHtmlHumar);
                             
                divDebug_1.Style.Add(HtmlTextWriterStyle.Width, "100%");
                divDebug_1.Style.Add(HtmlTextWriterStyle.Height, "8%");

                LabelDebug.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                LabelDebug.Style.Add(HtmlTextWriterStyle.Width, "50%");
                LabelDebug.Style.Add(HtmlTextWriterStyle.Height, "50%");
                LabelDebug.Style.Add(HtmlTextWriterStyle.PaddingTop, "2%");     
                LabelDebug.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                LabelDebug.Style.Add(HtmlTextWriterStyle.PaddingLeft, "3%");
                LabelDebug.Style.Add(HtmlTextWriterStyle.Color, "white");
                LabelDebug.Style.Add(HtmlTextWriterStyle.FontSize, "1.0vw");
                LabelDebug.Style.Add(HtmlTextWriterStyle.BorderStyle, "none");                
                LabelDebug.Style.Add("border-top", "0.0vw");
                LabelDebug.Style.Add("border-bottom", "0.0vw");


                //
                TextBoxDebug.TextMode = TextBoxMode.MultiLine;
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.Width, "100%");
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.Height, "100%");
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.Top, "0%");
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.FontSize, "0.8vw");
                TextBoxDebug.Style.Add(HtmlTextWriterStyle.Padding, "0% 0%");
                TextBoxDebug.Style.Add("border-width", "0.0vw");
                TextBoxDebug.Style.Add("border-top-width", "0px");
                TextBoxDebug.Style.Add("border-color", "lightgrey");
                TextBoxDebug.Style.Add("background", "rgba(255,255,255,0.6)");
                TextBoxDebug.ID = "TextBoxDebugID";

                
                Spremljaj.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                Spremljaj.Style.Add(HtmlTextWriterStyle.Height, "100%");
                Spremljaj.Style.Add(HtmlTextWriterStyle.Top, "10%");
                Spremljaj.Style.Add(HtmlTextWriterStyle.Left, "75%");
                
                SpremljajLable.Style.Add(HtmlTextWriterStyle.Position, "absolute");                
                SpremljajLable.Style.Add(HtmlTextWriterStyle.Height, "50%");
                SpremljajLable.Style.Add(HtmlTextWriterStyle.Top, "30%");
                SpremljajLable.Style.Add(HtmlTextWriterStyle.Left, "62%");
                SpremljajLable.Style.Add(HtmlTextWriterStyle.FontSize, "0.9vw");
                SpremljajLable.Style.Add(HtmlTextWriterStyle.Padding, "0% 0%");
                SpremljajLable.Style.Add(HtmlTextWriterStyle.Color, "white");
                SpremljajLable.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                SpremljajLable.Text = "Spremljaj:";

                TextBoxDebug.Text = SysLog.GetMessagesTB_min();

                Spremljaj.button.Click += Spremljaj_Button_Click;                
                
                divDebug_2.Style.Add("width", "100%");
                divDebug_2.Style.Add("height", "100%");

                LabelDebugDiv.Controls.Add(LabelDebug);
                divDebug_1.Controls.Add(LabelDebugDiv);
                divDebug_1.Controls.Add(Spremljaj);
                divDebug_1.Controls.Add(SpremljajLable);
                divDebug_2.Controls.Add(TextBoxDebug);

                divDebug.Controls.Add(divDebug_1);
                divDebug.Controls.Add(divDebug_2);
                
            }

            private void Spremljaj_Button_Click(object sender, ImageClickEventArgs e)
            {
                // button value changes from Val.SpremljajChecked on postback

                SetSpremljajChecked(!GetSpremljajChecked());
                Navigator.Refresh();

            }
            
        }
    }
}