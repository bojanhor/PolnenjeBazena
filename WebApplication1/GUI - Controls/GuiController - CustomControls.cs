using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Web.SessionState;

namespace WebApplication1
{
    public partial class GuiController
    {

        public partial class GControls
        {
            public class MasterMenuButton : ImageButton
            {
                public int btnID;

                public MasterMenuButton(int _btnID)
                {
                    btnID = _btnID;
                    ID = "Btn" + btnID;
                    this.ImageUrl = "~/Pictures/" + ID + ".png";
                    Width = Unit.Percentage(95);

                }
            }

            public class Luc : HtmlGenericControl
            {     
                              
                public LucBtn button;
                public ImageButton Zarnica1;
                public ImageButton Zarnica0;
                public Label Number1;
                public Label Number0;
                public int btnID;
                public Helper.Position position;
                public bool active = false;               
                public string address;
                public readonly string deactivatedPicture = "~\\Pictures\\Zarnica.png";
                public readonly string activatedPicture = "~\\Pictures\\Zarnica1.png";
               
                public string Top
                {
                    set
                    {
                        Style.Remove(HtmlTextWriterStyle.Top);
                        Style.Add(HtmlTextWriterStyle.Top, value + "%");
                    }
                }

                public string Left
                {
                    set
                    {
                        Style.Remove(HtmlTextWriterStyle.Left);
                        Style.Add(HtmlTextWriterStyle.Left, value + "%");
                    }
                }

                private string width;
                public string Width
                {
                    get
                    {
                        return width;
                    }
                    set
                    {
                        width = value;
                        Style.Remove(HtmlTextWriterStyle.Width);
                        Style.Add(HtmlTextWriterStyle.Width, value + "%");
                        Style.Add(HtmlTextWriterStyle.Height, Convert.ToInt32(value) * 2 + "%");
                    }
                }


                public Luc(int _btnID)
                {
                    
                    try
                    {                        
                        button = new LucBtn(this, _btnID);
                        btnID = _btnID;
                        ID = "Luc" + btnID;
                        Style.Add(HtmlTextWriterStyle.Position, "absolute");
                                                
                        position = XmlController.GetPositionLucForDefaultScreen(btnID);

                        Top = position.top.ToString().Replace(",", ".");
                        Left = position.left.ToString().Replace(",", ".");
                        Width = position.width.ToString().Replace(",", ".");

                        getZarnicaValue();
                        addImageButton();
                        AddNumber();
                        addButtonOverlay();
                                        

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    button.Click += Button_Click;

                }

                void getZarnicaValue()
                {
                   active = Val.logocontroler.Prop1.LucStatus_ReadToPC[btnID].Value;                                                                              
                }

                private void Button_Click(object sender, ImageClickEventArgs e)
                {
                    var l = Val.logocontroler.Prop1.LucStatus_ReadToPC[btnID].Value;
                    
                    var writeVal = Val.logocontroler.Prop1.LucStatus_WriteToPLC[btnID];
                    writeVal.SendPulse();


                    // temporary set values (until next refresh)
                    //if (active)
                    //{
                    //    Controls.Add(Zarnica0);
                    //    Controls.Add(Number0);
                    //}
                    //else
                    //{
                    //    Controls.Add(Zarnica1);
                    //    Controls.Add(Number1);
                    //}

                    Val.logocontroler.ForceRefreshValuesFromPLC(1);
                    Val.guiController.PageDefault_.ForceRefreshPanel();
                                                          
                }

                void AddNumber()
                {
                    Number1 = new Label();
                    Number0 = new Label();

                    var size = 100;

                    Number1.Text = btnID + "";
                    Number1.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    Number1.Style.Add(HtmlTextWriterStyle.Top, "32%");
                    Number1.Style.Add(HtmlTextWriterStyle.Left, "0%");
                    Number1.Style.Add(HtmlTextWriterStyle.Width, size + "%");
                    Number1.Style.Add(HtmlTextWriterStyle.Height, size + "%");
                    Number1.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    Number1.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");
                    Number1.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb((position.width / 3.5F), "vw")); 
                    Number1.Style.Add(HtmlTextWriterStyle.Color, "black");
                    Number1.Style.Add(HtmlTextWriterStyle.ZIndex, "9");

                    Number0.Text = btnID + "";
                    Number0.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    Number0.Style.Add(HtmlTextWriterStyle.Top, "32%");
                    Number0.Style.Add(HtmlTextWriterStyle.Left, "0%");
                    Number0.Style.Add(HtmlTextWriterStyle.Width, size + "%");
                    Number0.Style.Add(HtmlTextWriterStyle.Height, size + "%");
                    Number0.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    Number0.Style.Add(HtmlTextWriterStyle.VerticalAlign, "middle");                    
                    Number0.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb((position.width / 3.5F), "vw"));
                    Number0.Style.Add(HtmlTextWriterStyle.Color, "white");
                    Number0.Style.Add(HtmlTextWriterStyle.ZIndex, "9");

                    if (active)
                    {
                        Controls.Add(Number1);
                    }
                    else
                    {
                        Controls.Add(Number0);
                    }
                    
                }

                void addImageButton()
                {
                    var size = 100;
                    Zarnica0 = new ImageButton();
                    Zarnica1 = new ImageButton();
                    Zarnica1.ImageUrl = activatedPicture;// + "?" + DateTime.Now;
                    Zarnica0.ImageUrl = deactivatedPicture;// + "?" + DateTime.Now;

                    Zarnica0.ID = "ZarnicaOff" + btnID;
                    Zarnica0.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    Zarnica0.Style.Add(HtmlTextWriterStyle.Top, "0%");
                    Zarnica0.Style.Add(HtmlTextWriterStyle.Left, "0%");
                    Zarnica0.Style.Add(HtmlTextWriterStyle.Width, size + "%");
                    Zarnica0.Style.Add(HtmlTextWriterStyle.Height, size + "%");
                    Zarnica0.Style.Add(HtmlTextWriterStyle.ZIndex, "8");

                    Zarnica1.ID = "ZarnicaOn" + btnID;
                    Zarnica1.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    Zarnica1.Style.Add(HtmlTextWriterStyle.Top, "0%");
                    Zarnica1.Style.Add(HtmlTextWriterStyle.Left, "0%");
                    Zarnica1.Style.Add(HtmlTextWriterStyle.Width, size + "%");
                    Zarnica1.Style.Add(HtmlTextWriterStyle.Height, size + "%");
                    Zarnica1.Style.Add(HtmlTextWriterStyle.ZIndex, "8");

                    if (active)
                    {
                        Controls.Add(Zarnica1);
                    }
                    else
                    {
                        Controls.Add(Zarnica0);
                    }                    
                }

                void addButtonOverlay()
                {
                    var size = 100;

                    button.ID = "Zarnica_Btn" + btnID;
                    button.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    button.Style.Add(HtmlTextWriterStyle.Top, "0%");
                    button.Style.Add(HtmlTextWriterStyle.Left, "0%");
                    button.Style.Add(HtmlTextWriterStyle.Width, size + "%");
                    button.Style.Add(HtmlTextWriterStyle.Height, size + "%");
                    button.Style.Add(HtmlTextWriterStyle.ZIndex, "10");
                    button.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");
                    Controls.Add(button);
                }               
            }
                        
            public class LucSet : Luc
            {   
                              
                public LucSet(int _btnID, UpdatePanel Parent) 
                    : base(_btnID)
                {                      
                        
                }                       
            }

            public class LucBtn : TransparentButton
            {
                public HtmlGenericControl ParentControl;
                public int btnID;

                public LucBtn(HtmlGenericControl ParentControl_, int btnID_)
                {
                    ParentControl = ParentControl_;
                    btnID = btnID_;
                }
            }

            public class TransparentButton : ImageButton
            {
                public TransparentButton()
                {
                    this.ImageUrl = "~/Pictures/TransparentPixel.png";
                   
                }
            }               
            
            public class ImageButtonWithID : ImageButton
            {
                public int btnID;

                public ImageButtonWithID(int btnID_)
                {
                    btnID = btnID_;
                }
            }

            public class PaddedOnOffButton : OnOffButton
            {

                public PaddedOnOffButton(string description, int _btnID, bool status, Helper.Position position, Type type)
                    : base(description, _btnID, status, position, type)
                {

                }

                public PaddedOnOffButton(string description, int _btnID, bool status, Helper.Position position)
                    : base(description, _btnID, status, position, Type.Padded)
                {

                }

                public PaddedOnOffButton(string description, int _btnID, bool status, Type type)
                    : base(description, _btnID, status, type)
                {

                }


            }

            public class ShadowedOnOffButton : OnOffButton
            {
                public ShadowedOnOffButton(string description, int _btnID, bool status, Helper.Position position)
                    : base(description, _btnID, status, position, Type.Shadowed)
                {

                }

                public ShadowedOnOffButton(string description, int _btnID, bool status, Type type)
                    : base(description, _btnID, status, type)
                {

                }

            }

            public class NotShadowedOnOffButton : OnOffButton
            {
                public NotShadowedOnOffButton(string description, int _btnID, bool status, Helper.Position position)
                    : base(description, _btnID, status, position, Type.NotShadowed)
                {

                }
                public NotShadowedOnOffButton(string description, int _btnID, bool status, Type type)
                    : base(description, _btnID, status, type)
                {

                }

            }

            public class OnOffButton : HtmlGenericControl
            {

                public int btnID;
                public Helper.Position position;
                public ImageButtonWithID button;
                public Label TextLabel;
                public bool active;
                public string address;
                readonly string deactivatedPicture;
                readonly string activatedPicture;

                public Label LabelText; // only for padded type with text

                HtmlGenericControl div;

                public enum Type
                {
                    Shadowed,
                    NotShadowed,
                    Padded,
                    WithText
                }

                public string Top
                {
                    set
                    {
                        Style.Remove(HtmlTextWriterStyle.Top);
                        Style.Add(HtmlTextWriterStyle.Top, value + "%");
                    }
                }

                public string Left
                {
                    set
                    {
                        Style.Remove(HtmlTextWriterStyle.Left);
                        Style.Add(HtmlTextWriterStyle.Left, value + "%");
                    }
                }

                private string width;
                public string Width
                {
                    get
                    {
                        return width;
                    }
                    set
                    {
                        width = value;
                        Style.Remove(HtmlTextWriterStyle.Width);
                        Style.Add(HtmlTextWriterStyle.Width, value + "%");
                    }
                }

                public OnOffButton(string description, int _btnID, bool status, Type type) :
                    this(description, _btnID, status, new Helper.Position(0, 0, 100), type)
                {

                }

                public OnOffButton(string description, int _btnID, bool status, Helper.Position position_, Type type)
                {
                    try
                    {
                        button = new ImageButtonWithID(_btnID);

                        TagName = "div";
                        button.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                        button.Style.Add(HtmlTextWriterStyle.Top, "0%");
                        button.Style.Add(HtmlTextWriterStyle.Left, "0%");
                        button.Width = Unit.Percentage(100);


                        position = position_;

                        btnID = _btnID;
                        ID = description.Replace(" ", "") + btnID;

                        Top = position.top.ToString().Replace(",", ".");
                        Left = position.left.ToString().Replace(",", ".");
                        Width = position.width.ToString().Replace(",", ".");
                        Style.Add(HtmlTextWriterStyle.Height, Width+"%");

                        Style.Add(HtmlTextWriterStyle.Position, "absolute");



                        if (type == Type.Shadowed)
                        {
                            div = DIV.CreateDivAbsolute("0%", "0%", "100%", "100%");
                            deactivatedPicture = "~\\Pictures\\gumb-off.png";
                            activatedPicture = "~\\Pictures\\gumb-on.png";
                        }

                        else if (type == Type.NotShadowed)
                        {
                            div = DIV.CreateDivAbsolute("0%", "0%", "100%", "100%");
                            deactivatedPicture = "~\\Pictures\\gumb-off-noshadow.png";
                            activatedPicture = "~\\Pictures\\gumb-on-noshadow.png";
                        }

                        else if (type == Type.Padded)
                        {
                            div = DIV.CreateDivAbsolute("0%", "0%", "100%", "100%");
                            deactivatedPicture = "~\\Pictures\\gumb-off-padded.png";
                            activatedPicture = "~\\Pictures\\gumb-on-padded.png";

                        }
                        else if (type == Type.WithText)
                        {
                            div = DIV.CreateDivAbsolute("0%", "0%", "13.8vw", "13.8vw");
                            deactivatedPicture = "~\\Pictures\\gumb-off-padded-text.png";
                            activatedPicture = "~\\Pictures\\gumb-on-padded-text.png";

                            TextLabel = new Label()
                            {
                                Text = description.ToUpper(),
                            };

                            TextLabel.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                            TextLabel.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                            TextLabel.Style.Add(HtmlTextWriterStyle.FontSize, "1.1vw");
                            TextLabel.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                            TextLabel.Style.Add(HtmlTextWriterStyle.ZIndex, "20");

                            TextLabel.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            TextLabel.Style.Add(HtmlTextWriterStyle.Top, "9%");

                            div.Controls.Add(TextLabel);

                        }
                        else
                        {
                            throw new Exception("Type of OnOffButton is not recognised.");
                        }

                        div.Style.Add("position", "absolute");
                        div.Style.Add(HtmlTextWriterStyle.ZIndex, "10");

                        div.Controls.Add(button);

                        if (status)
                        {
                            Activate();
                        }
                        else
                        {
                            Deactivate();
                        }

                        Controls.Add(div);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }


                }


                void Activate()
                {
                    button.ImageUrl = activatedPicture; //+ "?" + DateTime.Now;                    
                    active = true;
                }

                void Deactivate()
                {
                    button.ImageUrl = deactivatedPicture; //+ "?" + DateTime.Now;                
                    active = false;
                }

            }

            public class ButtonWithLabel : HtmlGenericControl
            {
                public TransparentButton button = new TransparentButton();
                public Image Image = new Image();
                public Label l = new Label();
                string _text;

                public ButtonWithLabel(string text, float size, float FontSize)
                {
                    _text = text;
                    Image.ImageUrl = "~/Pictures/EmptyBtn.png";
                    SetControlAbsolutePos(Image, 0, 0, 100, 100);
                    SetControlAbsolutePos(button, 0, 0, 100, 100);
                    Style.Add(HtmlTextWriterStyle.Width, size + "%");
                    Style.Add(HtmlTextWriterStyle.Height, size + "%");
                    button.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");

                    Controls.Add(Image);
                    Controls.Add(CreateLabelInside(text, this, 30, 10, FontSize, true, true, Settings.LightBlackColor));
                    Controls.Add(button);
                }

            }

            public class ButtonWithLabel_SelectMenu : HtmlGenericControl
            {
                DropDown.UpdateEventHandler _SaveClicked;
                public DropDown.UpdateEventHandler SaveClicked
                {
                    get
                    {
                        return _SaveClicked;
                    }
                    set
                    {
                        _SaveClicked = value;
                        submenu.SaveClicked = value;
                    }
                }

                public TransparentButton button = new TransparentButton();
                SubMenuSelect submenu;
                public Image Image = new Image();
                public Label l = new Label();
                string _text;
                string menuID; 


                public ButtonWithLabel_SelectMenu(string Name, Helper.Datasource dataSource, string ID, string text, float FontSize, Timer updateTimer)
                {
                    ctor(Name, dataSource, ID, text, FontSize, updateTimer);
                }

                
                void ctor(string Name, Helper.Datasource dataSource, string ID, string text, float FontSize, Timer updateTimer)
                {

                    _text = text;
                    menuID = ID;
                    Image.ImageUrl = "~/Pictures/EmptyBtn.png";
                    SetControlAbsolutePos(Image, 0, 0, 100, 100);
                    SetControlAbsolutePos(button, 0, 0, 100, 100);

                    button.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");
                    button.ID = menuID + "_btn";

                    if (Name != null)
                    {
                        submenu = new SubMenuSelect(menuID + "_submenu", Name, dataSource, text, updateTimer);
                    }
                    else
                    {
                        submenu = new SubMenuSelect(menuID + "_submenu", dataSource, text, updateTimer);
                    }


                    Controls.Add(Image);
                    Controls.Add(CreateLabelInside(text, this, 30, 10, FontSize, true, true, Settings.LightBlackColor));
                    Controls.Add(button);

                    Controls.Add(submenu);

                    button.Click += (sender, e) => Button_Click(sender, e, updateTimer); // OnClick Event

                }

                private void Button_Click(object sender, ImageClickEventArgs e, Timer updateTimer)
                {                  
                    submenu.Style.Remove(HtmlTextWriterStyle.Display);
                    submenu.Style.Add(HtmlTextWriterStyle.Display, "block");

                    if (updateTimer != null)
                    {
                        updateTimer.Enabled = false;
                        
                    }

                    GlobalManagement.DisableAllTimersOnPage();
                    
                }

                class SubMenuSelect : GroupBox
                {
                    DropDown.UpdateEventHandler _SaveClicked;
                    public DropDown.UpdateEventHandler SaveClicked
                    {
                        get
                        {
                            return _SaveClicked;
                        }
                        set
                        {
                            _SaveClicked = value;
                            saveBtn.button.Click += (sender, e) => SaveClicked(sender, e, DropDown.SelectedItem);
                        }
                    }

                    static string top = "0%";
                    static string left = "0%";
                    static string width = "100%";
                    static string height = "100%";

                    public ImageButton exitButton;

                    ButtonWithLabel saveBtn;

                    public DropDownList DropDown;                    

                    Label TitleNameLabel;

                    void ctor(string ID, List<ListItem> DataSource, string text, Timer updateTimer)
                    {
                                               
                        this.ID = ID;
                        Style.Add("display", "none");

                        // Exit button

                        exitButton = new ImageButton()
                        {
                            ImageUrl = "~/Pictures/exit.png",
                            Width = Unit.Percentage(5)
                        };

                        SetControlAbsolutePos(exitButton, 5, 87, 10);                        
                        exitButton.Click += ExitButton_Click;

                        Controls.Add(exitButton);

                        // Save btn
                        saveBtn = new ButtonWithLabel("Shrani", 30, 1.5F);
                        SetControlAbsolutePos(saveBtn, 65, 36);                       

                        Controls.Add(saveBtn);

                        // dropdown list
                        DropDown = new DropDownList();
                        DropDown.DataSource = DataSource;
                        DropDown.DataBind();

                        DropDown.Style.Add(HtmlTextWriterStyle.FontSize, "1.5vw");
                        DropDown.Style.Add("box-shadow", "0.5vw 0.5vw 0.5vw #BBB");
                        DropDown.Style.Add("border-radius", "0.5" + "vw");
                        DropDown.Style.Add("background-color", "#f7f7f7");
                        DropDown.Style.Add("border-style", "solid");
                        DropDown.Style.Add("border-width", "0.1vw");
                        DropDown.Style.Add("border-color", "#ededed");
                        SetControlAbsolutePos(DropDown, 30, 39, 23, 23);

                        ManageSelectedItem(text, DataSource);

                        saveBtn.button.Click += (sender, e) => saveBtn_Click1(sender, e, updateTimer);

                        Controls.Add(DropDown);
                                              
                    }

                    void ManageSelectedItem(string PlcTextValue, List<ListItem> DataSource)
                    {                       
                        string buffSelectedItem = PlcTextValue ?? PropComm.NA;

                        for (int i = 0; i < DataSource.Count; i++)
                        {
                            var item = DataSource[i];

                            if (item != null)
                            {
                                if (item.Text == PlcTextValue || item.Value == PlcTextValue)
                                {
                                    this.DropDown.SelectedIndex = i;
                                    return;
                                }
                            }
                        }
                        DropDown.SelectedIndex = 0; //return PropComm.NA;


                    }

                    private void saveBtn_Click1(object sender, ImageClickEventArgs e, Timer updateTimer)
                    {
                        if (updateTimer != null)
                        {
                            updateTimer.Enabled = true;
                        }                        
                    }

                    private void ExitButton_Click(object sender, ImageClickEventArgs e)
                    {
                        Helper.Refresh(); // loads inital page (with closed/invisible menu and restarts updatepanel timer)
                    }

                    public SubMenuSelect(string ID, List<ListItem> list, string text, Timer updateTimer) 
                        : base(top, left, width, height)
                    {
                        ctor(ID, list, text, updateTimer);
                    }

                    public SubMenuSelect(string ID, string NameLable, List<ListItem> list, string text, Timer updateTimer)
                        : base(top, left, width, height)
                    {
                        ctor(ID, list, text, updateTimer);

                        addLabel(NameLable);
                    }

                    void addLabel(string NameLable)
                    {
                        TitleNameLabel = new Label()
                        {
                            Text = NameLable
                        };

                        SetControlAbsolutePos(TitleNameLabel, 5, 3, 50, 15);
                        Controls.Add(TitleNameLabel);
                    }                                        
                }
            }

            public class SettingsSubMenu : HtmlGenericControl
            {
                public ImageButton exitButton;
                public ImageButton nextButton;
                public ImageButton PrevButton;
                HtmlGenericControl NameDiv = DIV.CreateDivAbsolute("5.2%", "5%", "30%", "8%");
                Label Name = new Label();

                Label Clock = new Label();

                public SettingsSubMenu(int id, string Name_, bool hasExit, HtmlGenericControl content)
                {
                    ctor_(id, Name_, false, false, hasExit, false, content);
                }

                public SettingsSubMenu(int id, string Name_, bool hasNext, bool hasPreious, bool hasExit, HtmlGenericControl content)
                {
                    ctor_(id, Name_, hasNext, hasPreious, hasExit, false, content);
                }

                public SettingsSubMenu(int id, string Name_, bool hasNext, bool hasPreious, bool hasExit, bool hasPLCTimeShow, HtmlGenericControl content)
                {
                    ctor_(id, Name_, hasNext, hasPreious, hasExit, hasPLCTimeShow, content);
                }

                public void ctor_(int id, string Name_, bool hasNext, bool hasPreious, bool hasExit, bool hasPLCTimeShow, HtmlGenericControl content)
                {
                    try
                    {
                        ID = Name_ + id;
                        Name.Text = Name_.ToUpper();
                        AddSettingsPanelContent();


                        if (hasNext)
                        {
                            nextButton = new ImageButton()
                            {
                                ImageUrl = "~/Pictures/nxt.png",
                                Width = Unit.Percentage(2.6)
                            };

                            SetControlAbsolutePos(nextButton, 5, 37);

                            nextButton.ID = "UpdatePanelSettings_nxt";

                            Controls.Add(nextButton);
                        }

                        if (hasPreious)
                        {
                            PrevButton = new ImageButton()
                            {
                                ImageUrl = "~/Pictures/prv.png",
                                Width = Unit.Percentage(2.6)
                            };
                            SetControlAbsolutePos(PrevButton, 5, 3);

                            PrevButton.ID = "UpdatePanelSettings_prv";

                            Controls.Add(PrevButton);

                        }

                        if (hasExit)
                        {
                            Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Pictures/alert.png");

                            exitButton = new ImageButton()
                            {
                                ImageUrl = "~/Pictures/exit.png",
                                Width = Unit.Percentage(5)
                            };

                            SetControlAbsolutePos(exitButton, 5, 92);

                            Controls.Add(exitButton);

                        }
                        else
                        {
                            Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Pictures/alertNoExitBtn.png");
                        }

                        if (hasPLCTimeShow)
                        {
                            AddClock();
                        }

                        Controls.Add(Clock);
                        Controls.Add(content);


                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error constructing object SettingsSubMenu. Error message: " +ex.Message);
                    }
                   
                }

                void AddClock()
                {
                    Clock.Text = "TRENUTEN ČAS: " + Val.logocontroler.Prop1.LogoClock.Value_ClockForSiemensLogoFormat;
                    SetControlAbsolutePos(Clock, 10, 45, 30,5);
                    Clock.Style.Add(HtmlTextWriterStyle.FontSize, "1.4vw");
                    Clock.Style.Add(HtmlTextWriterStyle.Color, Settings.LightBlackColor);

                }


                void AddSettingsPanelContent()
                {
                    Style.Add(HtmlTextWriterStyle.Position, "absolute");

                    SetControlAbsolutePos(this, 5 + "%", 8 + "%", 79 + "vw", 43 + "vw");
                    Style.Add(HtmlTextWriterStyle.ZIndex, "20");

                    Style.Add("background-size", "contain");
                    Style.Add("background-size", "cover");
                    var top = 18;

                    SetControlAbsolutePos(Name, top, "center", 100, 100 - top);
                    Name.Style.Add("font-size", "1.8vw");
                    Name.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                    Name.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

                    NameDiv.Controls.Add(Name);
                    Controls.Add(NameDiv);

                }

            }

            public class MenuItem
            {
                public string ItemName = PropComm.NA;
                public string ItemLink = PropComm.NA;

                public string ItemId = PropComm.NA;


                public MenuItem(string _ItemName, string _ItemLink)
                {
                    ItemName = _ItemName;
                    ItemLink = _ItemLink;
                }

                public MenuItem()
                {

                }
            }

            public class GroupBox : HtmlGenericControl
            {

                public GroupBox(int top, int left, int width, int height)
                {
                    SubPanelFitGroupbox(this, top, left, width, height);
                    
                }

                public GroupBox(string top, string left, string width, string height)
                {
                    // TODO tweak popups
                    Style.Add("position","fixed");
                    Style.Add(HtmlTextWriterStyle.Top, "15vw");
                    Style.Add(HtmlTextWriterStyle.Left, "30vw");
                    Style.Add(HtmlTextWriterStyle.Width, XmlController.GetMasterWindowScaleX()/3 + "%");                    
                    Style.Add(HtmlTextWriterStyle.PaddingBottom, XmlController.GetMasterWindowScaleY()/3 + "%");
                    Style.Add(HtmlTextWriterStyle.ZIndex, "99");
                                       

                    Style.Add("border-radius", 1 + "vw");
                    Style.Add("background-color", "#f7f7f7");
                    Style.Add("border-style", "solid");
                    Style.Add("border-width", "0.1vw");
                    Style.Add("border-color", "#ededed");
                    Style.Add("box-shadow", "0.5vw 0.5vw 0.5vw #BBB");
                   
                }

               
                private void SubPanelFitGroupbox(HtmlGenericControl Groupbox, int top, int left, int width, int height)
                {
                    SetControlAbsolutePos(Groupbox, top, left, width, height);
                    Groupbox.Style.Add("border-radius", 1 + "vw");
                    Groupbox.Style.Add("background-color", "#f7f7f7");
                    Groupbox.Style.Add("border-style", "solid");
                    Groupbox.Style.Add("border-width", "0.1vw");
                    Groupbox.Style.Add("border-color", "#ededed");
                    Groupbox.Style.Add("box-shadow", "0.5vw 0.5vw 0.5vw #BBB");


                }
            }

            public class GuiSeparator_DottedLine : HtmlGenericControl
            {
                public float Top { get; private set; }
                public float Left { get; private set; }
                public float Width { get; private set; }
                public float Height { get; private set; }
                private int _ZIndex;
                public int ZIndex
                {
                    get
                    {
                        return _ZIndex;
                    }
                    set
                    {
                        Style.Add(HtmlTextWriterStyle.ZIndex, value.ToString());
                        _ZIndex = value;
                    }
                }

                HtmlGenericControl[] subdiv;

                public GuiSeparator_DottedLine(float top, float left, float width, float height, int howMayDots)
                {
                    float w = 0;
                    float seps = 0;
                    try
                    {
                        seps = (howMayDots * 2)-1;
                        w = (width / seps) ;
                        w = w + (w / seps)*2;
                    }
                    catch (Exception)
                    {
                        seps = 1;
                        w = width;
                    }
                                         

                    if (howMayDots < 2)
                    {
                        howMayDots = 2;
                    }
                    subdiv = new HtmlGenericControl[howMayDots];

                    float tmpl = 0;

                    for (int i = 0; i < subdiv.Length; i++)
                    {
                        subdiv[i] = new HtmlGenericControl("div");
                        SubPanelFitGroupbox(subdiv[i], top, tmpl, w, height);
                        tmpl += w * 2 ;
                        Controls.Add(subdiv[i]);
                    }
                    
                    SubPanelFit(this, top, left, width, height*1.0F);
                }

                private void SubPanelFitGroupbox(HtmlGenericControl Groupbox, float top, float left, float width, float height)
                {

                    SetControlAbsolutePos(Groupbox, top, left, width, height);


                    Groupbox.Style.Add("border-radius", 2 + "vw");
                    Groupbox.Style.Add("background-color", "#e0e0e0");
                    Groupbox.Style.Add("border-style", "solid");
                    Groupbox.Style.Add("border-width", "0.05vw");
                    Groupbox.Style.Add("border-color", "#dbdbdb");
                    Groupbox.Style.Add("box-shadow", "0.15vw 0.15vw 0.15vw #BBB");


                }

                private void SubPanelFit(HtmlGenericControl Groupbox, float top, float left, float width, float height)
                {

                    Top = top;
                    Left = left;
                    Height = height;
                    Width = width;

                    SetControlAbsolutePos(Groupbox, top, left, width, height);


                }
            }

            public class GuiSeparator : HtmlGenericControl
            {
                public float Top { get; private set; }
                public float Left { get; private set; }
                public float Width { get; private set; }
                public float Height { get; private set; }
                private int _ZIndex;
                public int ZIndex
                {
                    get
                    {
                        return _ZIndex;
                    }
                    set
                    {
                        Style.Add(HtmlTextWriterStyle.ZIndex, value.ToString());
                        _ZIndex = value;
                    }
                }

                public GuiSeparator(float top, float left, float width, float height)
                {
                    SubPanelFitGroupbox(this, top, left, width, height);
                }

                private void SubPanelFitGroupbox(HtmlGenericControl Groupbox, float top, float left, float width, float height)
                {

                    Top = top;
                    Left = left;
                    Height = height;
                    Width = width;

                    SetControlAbsolutePos(Groupbox, top, left, width, height);
                    Groupbox.Style.Add("border-radius", 2 + "vw");
                    Groupbox.Style.Add("background-color", "#e0e0e0");
                    Groupbox.Style.Add("border-style", "solid");
                    Groupbox.Style.Add("border-width", "0.05vw");
                    Groupbox.Style.Add("border-color", "#dbdbdb");
                    Groupbox.Style.Add("box-shadow", "0.15vw 0.15vw 0.15vw #BBB");


                }
            }

            public class SuperLabel : HtmlGenericControl
            {
                public Label lable = new Label();

                public float Top { get; private set; }
                public float Left { get; private set; }
                public float Width { get; private set; }
                public float Height { get; private set; }

                private float fontSize;
                public float FontSize
                {
                    get
                    {
                        return fontSize;
                    }

                    set
                    {
                        lable.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(value, "vw"));
                        fontSize = value;
                    }

                }

                private bool fontWeightBold;
                public bool FontWeightBold
                {
                    get
                    {
                        return fontWeightBold;
                    }

                    set
                    {
                        fontWeightBold = value;
                        if (value)
                        {
                            lable.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        else
                        {
                            lable.Style.Remove(HtmlTextWriterStyle.FontWeight);
                        }
                    }

                }

                private int _ZIndex;
                public int ZIndex
                {
                    get
                    {
                        return _ZIndex;
                    }
                    set
                    {
                        Style.Add(HtmlTextWriterStyle.ZIndex, value.ToString());
                        _ZIndex = value;
                    }
                }

                public SuperLabel(float top, float left)
                {
                    ctor_S(top, left);
                }

                public SuperLabel(float top, float left, float width, float height)
                {
                    ctor(top, left, width, height);
                }

                public SuperLabel(string text, float top, float left)
                {
                    lable.Text = text;
                    ctor_S(top, left);
                }

                public SuperLabel(string text, float top, float left, float width, float height)
                {
                    lable.Text = text;
                    ctor(top, left, width, height);
                }

                void ctor_S(float top, float left)
                {
                    Controls.Add(lable);
                    lable.Style.Add("top", "0");
                    lable.Style.Add("left", "0");
                    lable.Width = Unit.Percentage(100);
                    lable.Height = Unit.Percentage(100);
                    lable.Style.Add(HtmlTextWriterStyle.Position, "absolute");

                    Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    Style.Add(HtmlTextWriterStyle.Top, Helper.FloatToStringWeb(top, "%"));
                    Style.Add(HtmlTextWriterStyle.Left, Helper.FloatToStringWeb(left, "%"));

                    Top = top;
                    Left = left;
                }

                void ctor(float top, float left, float width, float height)
                {
                    Width = width;
                    Height = height;
                    ctor_S(top, left);
                    Style.Add(HtmlTextWriterStyle.Width, Helper.FloatToStringWeb(width, "%"));
                    Style.Add(HtmlTextWriterStyle.Height, Helper.FloatToStringWeb(height, "%"));
                }

                public void CenterLabel()
                {
                    lable.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                }

            }

            public class DropDown : HtmlGenericControl
            {

                public string Name { get; set; }
                public float Top { get; private set; }
                public float Left { get; private set; }
                public float Width { get; private set; }
                public float Height { get; private set; }

                private int _ZIndex;
                public int ZIndex
                {
                    get
                    {
                        return _ZIndex;
                    }
                    set
                    {
                        Style.Add(HtmlTextWriterStyle.ZIndex, value.ToString());
                        _ZIndex = value;
                    }
                }

                public ButtonWithLabel_SelectMenu dropdown;  // button 


                public delegate void UpdateEventHandler(object sender, ImageClickEventArgs e, ListItem selectedItem);     // used to get selected value in dropdown control inside menu    

                public event UpdateEventHandler SaveClicked
                {
                    add
                    {
                        dropdown.SaveClicked += value;
                    }

                    remove { dropdown.SaveClicked -= value; }
                }


                public ListItem selectedItem;
                public Helper.Datasource DataSource = new Helper.Datasource();

                // updatable
                public UpdatePanel updatePanel = new UpdatePanel();
                Control ctc;
                UpdatePanelTriggerCollection triggers;
                AsyncPostBackTrigger trigger;
                Timer UpdateTimer;


                public DropDown(Helper.Datasource dataSource, string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                {
                    DataSource = dataSource;
                    setDropdown(ID, PlcTextValue, size, 0.3F, 1, fontSize, selfUpdatable);
                }

                public DropDown(Helper.Datasource dataSource, string ID, string PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                {
                    DataSource = dataSource;
                    Style.Add("top", Helper.FloatToStringWeb(top, "%"));
                    Style.Add("left", Helper.FloatToStringWeb(left, "%"));
                    setDropdown(ID, PlcTextValue, (float)size, 0.3F, 1, fontSize, selfUpdatable);
                }
                              

                string ManageSelectedItem(string PlcTextValue)
                {
                    string buffSelectedItem = PlcTextValue ?? PropComm.NA;
                    foreach (var item in DataSource)
                    {
                        if (item != null)
                        {
                            if (item.Text == PlcTextValue)
                            {
                                selectedItem = item;
                                return selectedItem.Text;
                            }
                        }
                    }
                    
                    for (int i = DataSource.Count-1; i > 0; i--) // order is returned so the N/A or null is the last possible outcom
                    {
                        if (DataSource[i] != null)
                        {
                            if (DataSource[i].Value == PlcTextValue)
                            {
                                selectedItem = DataSource[i];
                                return selectedItem.Text;
                            }
                        }
                    }
                    
                    return PlcTextValue;
                }

                void setDropdown(string ID, string PlcTextValue, float size, float shadowsize, float borderradius, float fontSize, bool selfUpdatable)
                {

                    this.ID = ID;
                    Style.Add("position", "absolute");

                    if (selfUpdatable)
                    {
                        UpdateTimer = new Timer();
                        UpdateTimer.Interval = Settings.UpdateValuesPCms;
                        UpdateTimer.ID = ID + "_tmr";
                    }
                    
                    try
                    {
                        dropdown = new ButtonWithLabel_SelectMenu(Name, DataSource, ID + "_s", ManageSelectedItem(PlcTextValue), 1.5F, UpdateTimer)
                        {
                            ID = ID + "_dd"
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error initialising Submenu. Error info: " + ex.Message);
                    }
                    

                    SaveClicked += DropDown_SaveClicked;

                    ctc = updatePanel.ContentTemplateContainer;
                    ctc.Controls.Add(SetControlAbsolutePos(dropdown, 0, 0, 100, 100));

                    updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                    updatePanel.ID = ID + "_up";

                    if (selfUpdatable)
                    {
                        triggers = updatePanel.Triggers;
                        updatePanel.ContentTemplateContainer.Controls.Add(UpdateTimer);

                        trigger = new AsyncPostBackTrigger();
                        trigger.ControlID = UpdateTimer.ID;
                        triggers.Add(trigger);
                        
                    }
                                      
                    Controls.Add(updatePanel);


                    Style.Add(HtmlTextWriterStyle.Width, Helper.FloatToStringWeb(size * 2, "vw"));
                    Style.Add(HtmlTextWriterStyle.Height, Helper.FloatToStringWeb(size, "vw"));

                }


                private void DropDown_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    this.selectedItem = selectedItem;               
                    Helper.Refresh();
                }

               
                private void UpdateTimer_Tick1(object sender, EventArgs e)
                {
                    // Implicit update
                }


                void SetUpdateInterval()
                {                    
                    //triggers.Add(trigger); // TODO delete
                }
                
            }
                //
               

            public class DropDownListForDimmer : DropDown
            {
                static Helper.DimmerSelectorDatasource datasource = new Helper.DimmerSelectorDatasource();

                public DropDownListForDimmer(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForDimmer(string ID, string PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForDimmer(string ID, string PlcTextValue, float size, float shadowsize, float bordreradius, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, shadowsize, bordreradius, fontSize, selfUpdatable)
                {
                    ctor();
                }



                public string GetSelectedValue()
                {
                    return selectedItem.Value;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

                void ctor()
                {
                    DataSource = datasource;
                    dropdown.DataBind();
                }
            }

            public class DropDownListForHisteresis : DropDown
            {
                static Helper.HisteresisSelectorDatasource datasource = new Helper.HisteresisSelectorDatasource();

                public DropDownListForHisteresis(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForHisteresis(string ID, string PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForHisteresis(string ID, string PlcTextValue, float size, float shadowsize, float bordreradius, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, shadowsize, bordreradius, fontSize, selfUpdatable)
                {
                    ctor();
                }



                public string GetSelectedValue()
                {
                    return selectedItem.Value;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

                void ctor()
                {
                    DataSource = datasource;
                    dropdown.DataBind();
                }
            }

            public class DropDownListForTimer_1_30s : DropDown
            {
                static Helper.TimerSelectorDatasource datasource = new Helper.TimerSelectorDatasource(1, 30, 1, "s");

                public DropDownListForTimer_1_30s(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForTimer_1_30s(string ID, string PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForTimer_1_30s(string ID, string PlcTextValue, float size, float shadowsize, float bordreradius, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, shadowsize, bordreradius, fontSize, selfUpdatable)
                {
                    ctor();
                }



                public string GetSelectedValue()
                {
                    return selectedItem.Value;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

                void ctor()
                {
                    DataSource = datasource;
                    dropdown.DataBind();
                }
            }

            public class DropDownListForTemperatureSelect_10_30 : DropDown
            {
                static Helper.Temperature_10_30_SelectorDatasource datasource = new Helper.Temperature_10_30_SelectorDatasource();

                public DropDownListForTemperatureSelect_10_30(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForTemperatureSelect_10_30(string ID, string PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForTemperatureSelect_10_30(string ID, string PlcTextValue, float size, float shadowsize, float bordreradius, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, shadowsize, bordreradius, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public string GetSelectedValue()
                {
                    return selectedItem.Value;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

                void ctor()
                {
                    DataSource = datasource;
                    dropdown.DataBind();
                }
            }

            public class DropDownListForHourSelect : DropDown
            {
                static Helper.TimeSelectorDatasource datasource = new Helper.TimeSelectorDatasource();                

                public DropDownListForHourSelect(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                   : base(datasource, ID, PlcTextValue, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForHourSelect(string ID, string PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                   : base(datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForHourSelect(string ID, string PlcTextValue, float size, float shadowsize, float bordreradius, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue, size, shadowsize, bordreradius, fontSize, selfUpdatable)
                {
                    ctor();
                }


                public string GetSelectedValue()
                {
                    return selectedItem.Value;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

                void ctor()
                {                   
                    DataSource = datasource;
                    dropdown.DataBind();
                }
            }

            public class DropDownListForYesNoSelect : DropDown
            {
                static Helper.YesNoSelectorDatasource datasource = new Helper.YesNoSelectorDatasource();

                public DropDownListForYesNoSelect(string ID, bool PlcTextValue, float size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue.ToString(), size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForYesNoSelect(string ID, bool PlcTextValue, float top, float left, double size, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue.ToString(), top, left, size, fontSize, selfUpdatable)
                {
                    ctor();
                }

                public DropDownListForYesNoSelect(string ID, bool PlcTextValue, float size, float shadowsize, float bordreradius, float fontSize, bool selfUpdatable)
                    : base(datasource, ID, PlcTextValue.ToString(), size, shadowsize, bordreradius, fontSize, selfUpdatable)
                {
                    ctor();
                }


                public string GetSelectedValue()
                {
                    return selectedItem.Value;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

                void ctor()
                {
                    DataSource = datasource;
                    dropdown.DataBind();
                }
            }

           
        }
    }
}