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

        public class Template
        {
            List<GControls.MenuItem> DDMenuItems = new List<GControls.MenuItem>();              // dropdown items

            static Image TemplateImage = new Image();
            public static ImageButton HomeBtn = new ImageButton();
            public static ImageButton MenuBtn = new ImageButton();
            ImageButton PrevButton;
            public Label PageName = new Label();
            readonly short howManyDDItems;

            public Template()
            {
                try
                {
                    if (Settings.EnableHoveronMenu)
                    {
                        howManyDDItems = XmlController.GetHowManyMenuDDItems();
                        GetDDMenuItemsFromXML();
                    }

                }
                catch (Exception ex)
                {

                    throw new Exception("Error ocurded inside Template class constructor: " + ex.Message);
                }

            }

            
            private void GetDDMenuItemsFromXML()
            {

                for (short i = 1; i <= howManyDDItems; i++)
                {
                    GControls.MenuItem buff = new GControls.MenuItem(XmlController.GetMenuDDItemName(i), XmlController.GetMenuDDItemLink(i));
                    DDMenuItems.Add(buff);
                }

            }
           
            public string GetInnerHtmlForDDMenu()
            {
                var buff = Environment.NewLine + " <div> ";

                for (int i = 0; i < howManyDDItems; i++)
                {
                    buff += "<a href= \"" + DDMenuItems[i].ItemName + "\" >" + DDMenuItems[i].ItemLink + "</a>" +
                        Environment.NewLine;
                }

                return buff +
                    " </div> " +
                    Environment.NewLine;
            }

            public static void CreateBackground(Page thisPage, HtmlGenericControl TemplateClassID, bool hasLogo)
            {
                try
                {
                    var div = DIV.CreateDiv("0.5%", "75%", "3.5%", "auto", "10");
                    HomeBtn.Attributes.Add("id", "HomeBtnID");
                    HomeBtn.ImageUrl = "~/Pictures/domov.png";
                    HomeBtn.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    HomeBtn.Style.Add(HtmlTextWriterStyle.Width, "100%");
                    HomeBtn.Style.Add(HtmlTextWriterStyle.Height, "auto");
                    HomeBtn.Click += (sender, e) => HomeBtn_Click(sender, e, thisPage);

                    div.Controls.Add(HomeBtn);
                    TemplateClassID.Controls.Add(div);
                    TemplateImage.Attributes.Add("id", "BackGroundPic");

                    MenuBtn.Click += (sender, e) => MenuBtn_Click(sender, e, thisPage);
                    

                    if (hasLogo)
                    {
                        TemplateImage.ImageUrl = "~/Pictures/TemplateImage.png";
                    }
                    else
                    {
                        TemplateImage.ImageUrl = "~/Pictures/TemplateImageNoLogo.png";
                    }


                    TemplateImage.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    TemplateImage.Style.Add(HtmlTextWriterStyle.Top, "0px");
                    TemplateImage.Style.Add(HtmlTextWriterStyle.Left, "0px");
                    TemplateImage.Style.Add(HtmlTextWriterStyle.Width, "99%");
                    TemplateImage.Style.Add(HtmlTextWriterStyle.Height, "100%");
                    TemplateImage.Style.Add(HtmlTextWriterStyle.ZIndex, "-1");

                    TemplateClassID.Controls.Add(TemplateImage);
                }
                catch (Exception ex )
                {
                    throw ex;
                }
                

            }

            public void createTitleLabel(Page page, HtmlGenericControl TemplateClassID, bool hasLogo, string FriendlyPageName)
            {
                var LabelPagename = Val.guiController.Template_.PageName;                
                SetControlAbsolutePos(LabelPagename, 0, 0, 100, 100);
                LabelPagename.Text = FriendlyPageName;

                HtmlGenericControl divPagename;
                if (hasLogo)
                {
                    divPagename = DIV.CreateDiv("1.2%", "33%", "30%", "5%");
                }
                else
                {
                    divPagename = DIV.CreateDiv("1.2%", "7%", "30%", "5%");
                }
                

                LabelPagename.Style.Add(HtmlTextWriterStyle.Color, Settings.LightBlackColor);
                LabelPagename.Style.Add(HtmlTextWriterStyle.FontSize, "1.9vw");
                LabelPagename.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

                divPagename.Controls.Add(LabelPagename);
                page.Controls.Add(divPagename);
            }

            public void createBackButton(Page page, System.Web.SessionState.HttpSessionState session, HtmlGenericControl TemplateClassID, bool hasLogo)
            {
                PrevButton = new ImageButton()
                {
                    ImageUrl = "~/Pictures/prv.png",
                    Width = Unit.Percentage(1.6)
                };
                if (hasLogo)
                {
                    SetControlAbsolutePos(PrevButton, 0.8F, 30);
                }
                else
                {
                    SetControlAbsolutePos(PrevButton, 0.8F, 3);
                }
                

                PrevButton.ID = "BackBtn";

                TemplateClassID.Controls.Add(PrevButton);

                PrevButton.Click += (sender, e) => PrevButton_Click(sender, e, session, page);
            }

            private void PrevButton_Click(object sender, ImageClickEventArgs e, System.Web.SessionState.HttpSessionState session, Page thisPage)
            {                
                Helper.RedirectBack(session, thisPage);
            }

            public void CreateMenu(Page _thispage, HtmlGenericControl TemplateClassID)
            {

                string menuBackColorOnHover = "#999999";
                string menuBackColor = "#b0b0b0";
                string MenuTextColor = "#3b3b3b";
                string MenuTextSizeMultiply = "1.2";

                //

                var MenuBtnMaster = DIV.CreateDiv("0.5%", "80%", "9%", "6%", "10");
                MenuBtnMaster.ID = "MenuBtnMaster";

                MenuBtnMaster.Style.Add(HtmlTextWriterStyle.Position, "absolute");

                if (Settings.EnableHoveronMenu)
                {
                    MenuBtnMaster.Attributes.Add("onmouseover", "document.getElementById('MenuContent').style.display = 'block'; ");
                    MenuBtnMaster.Attributes.Add("onmouseout", "document.getElementById('MenuContent').style.display = 'none'; ");
                }
                //

                var MenuContent = DIV.CreateDiv("6.5%", "79%", "18%", "70%", "11");
                MenuContent.ID = "MenuContent";

                if (Settings.EnableHoveronMenu)
                {
                    MenuContent.Attributes.Add("onmouseover", "document.getElementById('MenuContent').style.display = 'block'; ");
                    MenuContent.Attributes.Add("onmouseout", "document.getElementById('MenuContent').style.display = 'none'; ");
                }


                MenuContent.Style.Add("border", "none");
                MenuContent.Style.Add("background-color", "#b1b1b1");
                MenuContent.Style.Add("box-shadow", "0px 8px 16px 0px rgba(0,0,0,0.2)");
                MenuContent.Style.Add("display", "none");

                //

                MenuBtn.ID = "MenuBtn";
                MenuBtn.CssClass = "MenuBtnCss";
                if (Settings.EnableHoveronMenu)
                {
                    MenuBtn.OnClientClick = "return false"; // prevents onclick action postback
                }


                MenuBtn.ImageUrl = "~/Pictures/meni.png";

                MenuBtn.Style.Add("border", "none");
                MenuBtn.Style.Add("position", "absolute");
                MenuBtn.Style.Add("width", "100%");
                MenuBtn.Style.Add("height", "100%");

                //



                TemplateClassID.Controls.Add(MenuBtnMaster);
                MenuBtnMaster.Controls.Add(MenuBtn);


                TemplateClassID.Controls.Add(MenuContent);


                if (Settings.EnableHoveronMenu)
                {
                    for (int i = 0; i < DDMenuItems.Count; i++)
                    {

                        var item = AddToMenu(DDMenuItems[i].ItemLink, DDMenuItems[i].ItemName, "menuItem" + i, menuBackColorOnHover, menuBackColor, MenuTextColor, MenuTextSizeMultiply);
                        DDMenuItems[i].ItemId = item.ID;
                        MenuContent.Controls.Add(item);

                    }
                }


            }
          
            public HtmlGenericControl AddToMenu(string pagename, string text, string id, string menuBackColorOnHover, string menuBackColor, string textColor, string sizeMultiplier)
            {

                var baseUrl = Helper.GetBaseUrl();
                var MenuContent_a = DIV.CreateAhrefInsideDiv(id, text.ToUpper(), baseUrl + pagename, menuBackColorOnHover, menuBackColor);
                MenuContent_a.Style.Add(HtmlTextWriterStyle.Color, textColor);

                MenuContent_a.Style.Add("font-size", sizeMultiplier + "vw");


                return MenuContent_a;
            }

            public static void HomeBtn_Click(object sender, ImageClickEventArgs e, Page thisPage)
            {
                Helper.Redirect(Settings.DefaultPage, thisPage);                

            }

            private static void MenuBtn_Click(object sender, ImageClickEventArgs e, Page thisPage)
            {
                Helper.Redirect("MasterMenu", thisPage);
            }

        }



    }
}