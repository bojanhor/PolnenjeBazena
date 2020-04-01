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
        public class PageMasterMenu : Dsps
        {
            Page thisPage;
            public Panel MasterbtnPannel;
            GControls.MasterMenuButton[] imageButtons = new GControls.MasterMenuButton[XmlController.GetHowManyMenuDDItems() + 1];
            Label[] TitleUnder = new Label[XmlController.GetHowManyMenuDDItems() + 1];
            HtmlGenericControl divMasterButtons;
           
            public PageMasterMenu(Page _thisPage)
            {
                try
                {
                    thisPage = _thisPage;
                    InitialisePanel();
                    InitializeTables();
                    AddBtns();




                }
                catch (Exception ex)
                {

                    throw new Exception("Internal error inside PageMasterMenu class constructor: " + ex.Message);
                }


            }

            void InitialisePanel()
            {
                MasterbtnPannel = new Panel();

                MasterbtnPannel.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                MasterbtnPannel.Style.Add(HtmlTextWriterStyle.Top, "22%");
                MasterbtnPannel.Style.Add(HtmlTextWriterStyle.Left, "4%");
                MasterbtnPannel.Style.Add(HtmlTextWriterStyle.Width, "90%");
                MasterbtnPannel.Style.Add(HtmlTextWriterStyle.Height, "70%");
                MasterbtnPannel.Style.Add(HtmlTextWriterStyle.ZIndex, "1");

            }

            void AddBtns()
            {
                int elements = XmlController.GetHowManyMenuDDItems();

                // limit elements min max (1-10)
                if (elements < 1) { throw new Exception("Not enough MasterMenu Items. Value must be more than 0"); }
                if (elements > 10) { throw new Exception("Too many MasterMenu Items. Value must be less than or equal to 10"); }

                int firstRowElements = 0;
                int secondRowElements = 0;

                // if 6 or under
                if (elements <= 6)
                {
                    firstRowElements = elements;
                }
                else
                {
                    // if 6 or above
                    switch (elements)
                    {
                        case 7: firstRowElements = 4; secondRowElements = elements - firstRowElements; break;
                        case 8: firstRowElements = 4; secondRowElements = elements - firstRowElements; break;
                        case 9: firstRowElements = 5; secondRowElements = elements - firstRowElements; break;
                        case 10: firstRowElements = 5; secondRowElements = elements - firstRowElements; break;
                        default:
                            break;
                    }
                }


                var divfortitles = DIV.CreateDivAbsolute("0%","0%","100%","100%");


                //horizontal positioning
                int initialPos = 0;
                int currentPos = initialPos;
                int spacing = 1;


                switch (elements)
                {
                    case 1: initialPos = 40; break;
                    case 2: initialPos = 35; spacing = 20; break;    // 1 line, 1 element
                    case 3: initialPos = 20; spacing = 20; break;    // 1 line, 2 element
                    case 4: initialPos = 10; spacing = 20; break;    // 1 line, 3 element
                    case 5: initialPos = 5; spacing = 18; break;     // 1 line, 4 element
                    case 6: initialPos = 2; spacing = 16; break;     // 1 line, 5 element
                                                                     //
                    case 7: initialPos = 10; spacing = 20; break;    // 2 line, 4 element in first line, 3 element in second line
                    case 8: initialPos = 10; spacing = 20; break;    // 2 line, 4 element in first line, 4 element in second line
                    case 9: initialPos = 5; spacing = 18; break;     // 2 line, 5 element in first line, 4 element in second line
                    case 10: initialPos = 5; spacing = 18; break;    // 2 line, 5 element in first line, 5 element in second line

                    default:
                        break;
                }

                currentPos = initialPos;

                // 1st line div
                divMasterButtons = DIV.CreateDivAbsolute("0%");
                divMasterButtons.ID = imageButtons[1].ID + "_div";
                PrepareDivForRow(divMasterButtons);


                // 1st line elements
                // positioning
                for (int i = 1; i < imageButtons.Length; i++)
                {
                    if (i > firstRowElements)
                    {
                        break;
                    }

                    imageButtons[i] = new GControls.MasterMenuButton(i);
                    imageButtons[i].Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    if (secondRowElements > 0)
                    {
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Top, "0%"); // if two line - shift up
                    }
                    else
                    {
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Top, "25%"); // if just one line - center
                    }

                    imageButtons[i].Style.Add(HtmlTextWriterStyle.Width, "15%");
                    imageButtons[i].Style.Add(HtmlTextWriterStyle.Height, "auto");
                    imageButtons[i].Style.Add(HtmlTextWriterStyle.Left, currentPos + "%");
                    imageButtons[i].Style.Add(HtmlTextWriterStyle.ZIndex, "10");

                    currentPos += spacing;
                    divMasterButtons.Controls.Add(imageButtons[i]);
                    divfortitles.Controls.Add(AddTitles(imageButtons[i]));
                }


                // 2nd line elements
                if (secondRowElements > 0)
                {
                    currentPos = initialPos;
                    
                    // 2nd line div                                        
                    PrepareDivForRow(divMasterButtons);


                    // positioning
                    for (int i = firstRowElements + 1; i < imageButtons.Length; i++)
                    {
                        if (i > elements)
                        {
                            break;
                        }

                        imageButtons[i] = new GControls.MasterMenuButton(i);
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Position, "absolute");
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Top, "50%");
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Width, "15%");
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Height, "auto");
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.Left, currentPos + "%");
                        imageButtons[i].Style.Add(HtmlTextWriterStyle.ZIndex, "10");

                        currentPos += spacing;
                        divMasterButtons.Controls.Add(imageButtons[i]);
                        divfortitles.Controls.Add(AddTitles(imageButtons[i]));
                    }                    
                }

                MasterbtnPannel.Controls.Add(divfortitles);
                MasterbtnPannel.Controls.Add(divMasterButtons);

                
            }

            void PrepareDivForRow(HtmlGenericControl div)
            {
                div.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                div.Style.Add(HtmlTextWriterStyle.Top, "0%");
                div.Style.Add(HtmlTextWriterStyle.Left, "0%");
                div.Style.Add(HtmlTextWriterStyle.Width, "100%");
                div.Style.Add(HtmlTextWriterStyle.Height, "100%");
                div.Style.Add(HtmlTextWriterStyle.ZIndex, "5");
            }

            private HtmlGenericControl AddTitles(GControls.MasterMenuButton imageBtn)
            {                
                var title = TitleUnder[imageBtn.btnID];
                title.Text = XmlController.GetMenuDDItemName((short)imageBtn.btnID).ToUpper();
                title.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                title.Style.Add(HtmlTextWriterStyle.FontSize, "1.3vw");

                title.ID = imageBtn.ID + "_title";
                
                var div = Helper.PositionUnderTitle(imageBtn, title);

                return div;
            }

            void InitializeTables()
            {
                for (int i = 1; i < imageButtons.Length; i++)
                {
                    imageButtons[i] = new GControls.MasterMenuButton(i);
                    TitleUnder[i] = new Label();
                }
            }



            public void RegisterOnClick()
            {
                for (int i = 1; i < imageButtons.Length; i++)
                {
                    imageButtons[i].Click += PageMasterMenu_Click;
                }
            }

            private void PageMasterMenu_Click(object sender, ImageClickEventArgs e)
            {
                try
                {
                    var btn = (GControls.MasterMenuButton)sender;
                    var id = btn.btnID;

                    var link = XmlController.GetMenuDDItemLink((short)id);

                    Helper.Redirect(link, thisPage);

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error - PageMasterMenu_Click(). Error message: " + ex.Message);
                }
                
            }
        }

    }
}