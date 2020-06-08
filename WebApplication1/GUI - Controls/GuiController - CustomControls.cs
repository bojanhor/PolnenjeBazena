using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Web.SessionState;
using Microsoft.Ajax.Utilities;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.DataVisualization.Charting;

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
                    this.Click += (sender, e) => MasterMenuButton_Click(sender, e, getLink(btnID-1));

                }

                private void MasterMenuButton_Click(object sender, ImageClickEventArgs e, string Link)
                {
                    Navigator.Redirect(Link);
                }

                string getLink(int ID)
                {
                    return XmlController.GetMenuDDItemLink((short)ID);
                }
            }
                      
            public class LucBtn : TransparentButton
            {
                public HtmlGenericControl ParentControl;
                public int btnID;

                public LucBtn(HtmlGenericControl ParentControl_, int btnID_)
                    : base("btnt_" + btnID_)
                {
                    ParentControl = ParentControl_;
                    btnID = btnID_;
                }
            }

            public class TransparentButton : ImageButton
            {
                public TransparentButton(string ID)
                {
                    this.ImageUrl = "~/Pictures/TransparentPixel.png";
                    this.ID = ID;

                }
            }

            public class ImageButtonWithID : ImageButton
            {
                public int btnID;

                public ImageButtonWithID(int btnID_)
                {
                    ctor("", btnID_);
                }

                public ImageButtonWithID(string ID_prefix, int btnID_)
                {
                    ctor(ID_prefix, btnID_);
                }

                void ctor(string ID_prefix, int btnID_)
                {
                    btnID = btnID_;
                    this.ID = ID_prefix + btnID;
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

                private string size;
                public string Size
                {
                    get
                    {
                        return size;
                    }
                    set
                    {
                        size = value;
                        Style.Remove(HtmlTextWriterStyle.Width);
                        Style.Add(HtmlTextWriterStyle.Width, value + "%");

                        Style.Remove(HtmlTextWriterStyle.Height);
                        Style.Add(HtmlTextWriterStyle.Height, value + "%");
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
                        button = new ImageButtonWithID(description.Replace(" ", ""), _btnID);

                        TagName = "div";
                        button.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                        button.Style.Add(HtmlTextWriterStyle.Top, "0%");
                        button.Style.Add(HtmlTextWriterStyle.Left, "0%");
                        button.Width = Unit.Percentage(100);


                        position = position_;

                        btnID = _btnID;
                        this.ID = description.Replace(" ", "") + "_btn" + btnID;

                        Top = position.top.ToString().Replace(",", ".");
                        Left = position.left.ToString().Replace(",", ".");
                        Size = position.width.ToString().Replace(",", ".");
                        Style.Add(HtmlTextWriterStyle.Height, Size + "%");

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
                            div = DIV.CreateDivAbsolute("0%", "0%", "100%", "100%");
                            deactivatedPicture = "~\\Pictures\\gumb-off-padded-text.png";
                            activatedPicture = "~\\Pictures\\gumb-on-padded-text.png";

                            TextLabel = new Label()
                            {
                                Text = description.ToUpper(),
                            };

                            TextLabel.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                            TextLabel.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                            TextLabel.Style.Add(HtmlTextWriterStyle.FontSize, "1.0vw");
                            TextLabel.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                            TextLabel.Style.Add(HtmlTextWriterStyle.ZIndex, "20");

                            TextLabel.Style.Add(HtmlTextWriterStyle.Width, "100%");
                            TextLabel.Style.Add(HtmlTextWriterStyle.Top, "5%");

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
                    catch (Exception)
                    {
                        throw;
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
                public TransparentButton button;
                public Image Image = new Image();
                public Label l = new Label();
                string _text;

                public ButtonWithLabel(string ID_prefix, string text, float size, float FontSize)
                {
                    ctor(ID_prefix, text, size, FontSize);
                }

                public ButtonWithLabel(string text, float size, float FontSize)
                {
                    ctor("", text, size, FontSize);
                }

                void ctor(string ID_prefix, string text, float size, float FontSize)
                {
                    button = new TransparentButton(ID_prefix + text.Replace(" ", ""));
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

                public TransparentButton button;
                SubMenuSelect submenu;
                public Image Image = new Image();
                public Label l = new Label();
                string _text;
                string menuID;


                public ButtonWithLabel_SelectMenu(string Name, Datasourcer.Datasource dataSource, string ID, string text, float FontSize, Timer updateTimer, bool wideMode)
                {
                    button = new TransparentButton("menu_" + Name.Replace(" ", "") + "_" + ID);
                    Ctor(Name, dataSource, ID, text, FontSize, updateTimer, wideMode);
                }


                void Ctor(string Name, Datasourcer.Datasource dataSource, string ID, string text, float FontSize, Timer updateTimer, bool wideMode)
                {

                    _text = text;
                    menuID = ID;


                    Image.ImageUrl = wideMode ? "~/Pictures/EmptyBtnWide.png" : "~/Pictures/EmptyBtn.png";
                    SetControlAbsolutePos(Image, 0, 0, 100, 100);
                    SetControlAbsolutePos(button, 0, 0, 100, 100);

                    button.Style.Add(HtmlTextWriterStyle.BackgroundColor, "transparent");
                    button.ID = menuID + "_btn";

                    if (Name != null)
                    {
                        submenu = new SubMenuSelect(menuID + "_submenu", Name, dataSource, text, updateTimer, wideMode);
                    }
                    else
                    {
                        submenu = new SubMenuSelect(menuID + "_submenu", dataSource, text, updateTimer, wideMode);
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
                                                           
                    public ImageButton exitButton;

                    ButtonWithLabel saveBtn;

                    public DropDownList DropDown;

                    Label TitleNameLabel;

                    void Ctor(string ID, List<ListItem> DataSource, string text, Timer updateTimer, bool wideMode)
                    {

                        this.ID = ID;
                        Style.Add("display", "none");

                        // Exit button

                        exitButton = new ImageButton()
                        {
                            ID = "btnExit_s_" + ID,
                            ImageUrl = "~/Pictures/exit.png",
                            Width = Unit.Percentage(5)
                        };

                        SetControlAbsolutePos(exitButton, 5, 87, 10);
                        exitButton.Click += ExitButton_Click;

                        Controls.Add(exitButton);

                        // Save btn
                        saveBtn = new ButtonWithLabel("btnSave" + ID, "Shrani", 30, 1.5F);
                        SetControlAbsolutePos(saveBtn, 65, 36);

                        Controls.Add(saveBtn);

                        // dropdown list
                        DropDown = new DropDownList
                        {
                            DataSource = DataSource
                        };
                        DropDown.DataBind();

                        DropDown.Style.Add(HtmlTextWriterStyle.FontSize, "1.5vw");
                        DropDown.Style.Add("box-shadow", "0.5vw 0.5vw 0.5vw #BBB");
                        DropDown.Style.Add("border-radius", "0.5" + "vw");
                        DropDown.Style.Add("background-color", "#f7f7f7");
                        DropDown.Style.Add("border-style", "solid");
                        DropDown.Style.Add("border-width", "0.1vw");
                        DropDown.Style.Add("border-color", "#ededed");
                        if (wideMode)
                        {
                            SetControlAbsolutePos(DropDown, 30, 30, 42, 23);
                        }
                        else
                        {
                            SetControlAbsolutePos(DropDown, 30, 39, 23, 23);
                        }


                        ManageSelectedItem(text, DataSource);

                        saveBtn.button.Click += (sender, e) => SaveBtn_Click1(sender, e, updateTimer);

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

                    private void SaveBtn_Click1(object sender, ImageClickEventArgs e, Timer updateTimer)
                    {
                        if (updateTimer != null)
                        {
                            updateTimer.Enabled = true;
                        }
                    }

                    private void ExitButton_Click(object sender, ImageClickEventArgs e)
                    {
                        Navigator.Refresh(); // loads inital page (with closed/invisible menu and restarts updatepanel timer)
                    }

                    public SubMenuSelect(string ID, List<ListItem> list, string text, Timer updateTimer, bool wideMode)                        
                    {
                        Ctor(ID, list, text, updateTimer, wideMode);
                    }

                    public SubMenuSelect(string ID, string NameLable, List<ListItem> list, string text, Timer updateTimer, bool wideMode)                        
                    {
                        Ctor(ID, list, text, updateTimer, wideMode);

                        AddLabel(NameLable);
                    }

                    void AddLabel(string NameLable)
                    {
                        TitleNameLabel = new Label()
                        {
                            Text = NameLable
                        };

                        TitleNameLabel.Style.Add(HtmlTextWriterStyle.FontSize, "1vw");
                        TitleNameLabel.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

                        SetControlAbsolutePos(TitleNameLabel, 7, 5, 50, 15);
                        Controls.Add(TitleNameLabel);
                    }

                }
            }

            public class OnOffShow : HtmlGenericControl
            {
                public int btnID;
                public Helper.Position position;
                public Image Image = new Image();

                public string activatedPicture;
                public string deactivatedPicture;


                public OnOffShow(string id, bool activated, Helper.Position position, string activatedPicture, string deactivatedPicture)
                {
                    ID = id;
                                        
                    try
                    {
                        if (activated)
                        {
                            if (activatedPicture != null)
                            {
                                Image.ImageUrl = activatedPicture;
                            }

                        }
                        else
                        {
                            if (deactivatedPicture != null)
                            {
                                Image.ImageUrl = deactivatedPicture;
                            }

                        }

                        Image.BackColor = System.Drawing.Color.Transparent;
                        Image.ForeColor = System.Drawing.Color.Transparent;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error inside OnOffShow class constructor: " + ex.Message);
                    }

                                        
                    SetControlAbsolutePos(Image, 0, 0, position.width);
                    Controls.Add(Image);
                                        
                }
                                               
            }


            public class OnOffShowRound : OnOffShow
            {                 
                public OnOffShowRound(string id, bool activated, Helper.Position position, string color) 
                    :base(id, activated, position, getActivatedPicture(color), getDeactivatedPicture(color))
                {                    
                                                    
                }
                static string getActivatedPicture(string color)
                {                    
                    color = color.ToLower();
                    if (color == "red")
                    {
                        return GetPicture("DotRed");
                    }
                    else if (color == "yellow")
                    {
                        return GetPicture("DotYe");
                    }
                    else if (color == "green")
                    {
                        return GetPicture("DotGn");
                    }

                    else
                    {
                        throw new Exception("Unsupported color wanted. (OnOffShowRound)");
                    }                    
                }
                static string getDeactivatedPicture(string color)
                {
                    color = color.ToLower();
                    if (color == "red")
                    {
                        return GetPicture("DotRedX");
                    }
                    else if (color == "yellow")
                    {
                        return GetPicture("DotYeX");
                    }
                    else if (color == "green")
                    {
                        return GetPicture("DotGnX");
                    }

                    else
                    {
                        throw new Exception("Unsupported color wanted. (OnOffShowRound)");
                    }                                       
                }
                static string GetPicture(string Color)
                {
                    return "~/Pictures/" + Color + ".png";
                }
            }

            public class Conveyor : HtmlGenericControl
            {
                public Conveyor(string idprefix, float top, float left, float size, bool moving, bool full)
                {
                    Image trak = new Image();
                   
                    
                    if (moving)
                    {
                        if (full)
                        {
                            trak.ImageUrl = GetGif("ConveyOnFull");
                            SetControlAbsolutePos(trak, 0, 0, 100);
                        }
                        else
                        {
                            trak.ImageUrl = GetGif("ConveyOn");
                            SetControlAbsolutePos(trak, 45, 18.4F, 81.7F); // perfect overlay
                        }                        
                    }
                    else
                    {
                        if (full)
                        {
                            trak.ImageUrl = GetGif("ConveyOffFull");
                            SetControlAbsolutePos(trak, 0, 0, 100);
                        }
                        else
                        {
                            trak.ImageUrl = GetGif("ConveyOff");
                            SetControlAbsolutePos(trak, 45, 18.4F, 81.7F); // perfect overlay
                        }                        
                    }                    

                    SetControlAbsolutePos(this, top, left, size,size);
                    Controls.Add(trak);                    
                }

                static string GetGif(string Pic)
                {
                    return "~/Pictures/" + Pic + ".gif";
                }
                static string GetPicture(string Pic)
                {
                    return "~/Pictures/" + Pic + ".png";
                }
            }

            public class DirectionButton : ImageButtonWithID
            {               
                public Direction direction { get; private set; }
                public DirectionButton(string ID_prefix, int btnID_, bool pressed, Direction direction) :base(ID_prefix, btnID_)
                {
                    this.direction = direction;

                    if (direction == Direction.Up)
                    {
                        if (pressed)
                        {
                            this.ImageUrl = "~/Pictures/up_press.png";
                        }
                        else
                        {
                            this.ImageUrl = "~/Pictures/up.png";
                        }                        
                    }
                    else if (direction == Direction.Down)
                    {
                        if (pressed)
                        {
                            this.ImageUrl = "~/Pictures/dwn_press.png";
                        }
                        else
                        {
                            this.ImageUrl = "~/Pictures/dwn.png";
                        }                        
                    }
                    else if (direction == Direction.Left)
                    {
                        if (pressed)
                        {
                            this.ImageUrl = "~/Pictures/prv_press.png";
                        }
                        else
                        {
                            this.ImageUrl = "~/Pictures/prv.png";
                        }                        
                    }
                    else if (direction == Direction.Right)
                    {
                        if (pressed)
                        {
                            this.ImageUrl = "~/Pictures/nxt_press.png";
                        }
                        else
                        {
                            this.ImageUrl = "~/Pictures/nxt.png";
                        }                        
                    }

                }
                public enum Direction
                {
                    Up,Down,Left,Right
                }
            }

            public class JoystickDirection : HtmlGenericControl
            {
                public DirectionButton btn_up { get; private set; }
                public DirectionButton btn_dn { get; private set; }
                public DirectionButton btn_l { get; private set; }
                public DirectionButton btn_r { get; private set; }
                GroupBox g;
                public JoystickDirection(int top, int left, int size, bool upPressed, bool dwnPressed, bool lftPressed, bool rghtPressen)
                {                    
                    var sizeDwn = 20;
                    var widthMltply = size / 2;
                    var cntrIcon = 33;
                
                    btn_up = new DirectionButton("Joy", 1, upPressed, DirectionButton.Direction.Up); SetControlAbsolutePos(btn_up, 5, cntrIcon, sizeDwn*1.9F);
                    btn_dn = new DirectionButton("Joy", 2, dwnPressed, DirectionButton.Direction.Down); SetControlAbsolutePos(btn_dn, 75, cntrIcon, sizeDwn*1.9F);
                    btn_l = new DirectionButton("Joy", 3, lftPressed, DirectionButton.Direction.Left); SetControlAbsolutePos(btn_l, 32, 5, sizeDwn);
                    btn_r = new DirectionButton("Joy", 4, rghtPressen, DirectionButton.Direction.Right); SetControlAbsolutePos(btn_r, 32, cntrIcon + 43, sizeDwn);
                    g = new GroupBox(0, 0, 100, 100);

                    SetControlAbsolutePos(this, top, left, widthMltply, size);

                    g.Controls.Add(btn_up); g.Controls.Add(btn_dn); g.Controls.Add(btn_l); g.Controls.Add(btn_r);
                    Controls.Add(g);
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
                    Ctor_(id, Name_, false, false, hasExit, false, content);
                }

                public SettingsSubMenu(int id, string Name_, bool hasNext, bool hasPreious, bool hasExit, HtmlGenericControl content)
                {
                    Ctor_(id, Name_, hasNext, hasPreious, hasExit, false, content);
                }


                public void Ctor_(int id, string Name_, bool hasNext, bool hasPreious, bool hasExit, bool hasPLCTimeShow, HtmlGenericControl content)
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

                        Controls.Add(Clock);
                        Controls.Add(content);


                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Error constructing object SettingsSubMenu. Error message: " + ex.Message);
                    }

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
                public GroupBox(string ID, int top, int left, int width, int height)
                {
                    this.ID = ID;
                    SetControlAbsolutePos(this, top, left, width, height);
                    BasicStyle();
                }

                public GroupBox(int top, int left, int width, int height)
                {
                    SetControlAbsolutePos(this, top, left, width, height);
                    BasicStyle();
                }

                public GroupBox()
                {
                    Style.Add("position", "fixed");
                    Style.Add(HtmlTextWriterStyle.Top, "15vw");
                    Style.Add(HtmlTextWriterStyle.Left, "30vw");
                    Style.Add(HtmlTextWriterStyle.Width, XmlController.GetMasterWindowScaleX() / 3 + "%");
                    Style.Add(HtmlTextWriterStyle.PaddingBottom, XmlController.GetMasterWindowScaleY() / 3 + "%");
                    Style.Add(HtmlTextWriterStyle.ZIndex, "99");

                    BasicStyle();

                }

                public GroupBox(int top, int left, int width, int height, bool stylized)
                {
                    SetControlAbsolutePos(this, top, left, width, height);

                    if (stylized)
                    {
                        AdvancedStyle();
                    }
                    else
                    {
                        BasicStyle();
                    }
                }                

                void BasicStyle()
                {
                    Style.Add("border-radius", 1 + "vw");
                    Style.Add("background-color", "#f7f7f7");
                    Style.Add("border-style", "solid");
                    Style.Add("border-width", "0.1vw");
                    Style.Add("border-color", "#ededed");
                    Style.Add("box-shadow", "0.5vw 0.5vw 0.5vw #BBB");
                }

                void AdvancedStyle()
                {
                    BasicStyle();
                    var header = DIV.CreateDivAbsolute(-0.3F, -0.1F, 100,15,"%");

                    header.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#a3a3a3");
                    header.Style.Add("border-radius", 1 + "vw" + " " + 1 + "vw" + " " + 0 + "vw" + " " + 0 + "vw");                    
                    header.Style.Add("border-style", "solid");
                    header.Style.Add("border-width", "0.1vw");
                    header.Style.Add("border-color", "#a3a3a3");
                    //header.Style.Add("box-shadow", "0.5vw 0.5vw 0.5vw #BBB");

                    Controls.Add(header);

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
                        seps = (howMayDots * 2) - 1;
                        w = (width / seps);
                        w = w + (w / seps) * 2;
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
                        tmpl += w * 2;
                        Controls.Add(subdiv[i]);
                    }

                    SubPanelFit(this, top, left, width, height * 1.0F);
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
                public Label label = new Label();

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
                        label.Style.Remove(HtmlTextWriterStyle.FontSize);
                        label.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(value, "vw"));
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
                            label.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                        }
                        else
                        {
                            label.Style.Remove(HtmlTextWriterStyle.FontWeight);
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

                public SuperLabel(string text, float top, float left, float width, float height)
                {

                    label.Text = text;
                    Top = top;
                    Left = left;
                    Width = width;
                    Height = height;

                    label.Style.Add("top", "0");
                    label.Style.Add("left", "0");
                    label.Width = Unit.Percentage(100);
                    label.Height = Unit.Percentage(100);
                    label.Style.Add(HtmlTextWriterStyle.Position, "absolute");

                    Style.Add(HtmlTextWriterStyle.Position, "absolute");
                    Style.Add(HtmlTextWriterStyle.Top, Helper.FloatToStringWeb(top, "%"));
                    Style.Add(HtmlTextWriterStyle.Left, Helper.FloatToStringWeb(left, "%"));
                    Style.Add(HtmlTextWriterStyle.Width, Helper.FloatToStringWeb(width, "%"));
                    Style.Add(HtmlTextWriterStyle.Height, Helper.FloatToStringWeb(height, "%"));

                    Controls.Add(label);
                }

                public void CenterLabel()
                {
                    label.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
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

                public ButtonWithLabel_SelectMenu Button_Outside;  // button 


                public delegate void UpdateEventHandler(object sender, ImageClickEventArgs e, ListItem selectedItem);     // used to get selected value in dropdown control inside menu    

                public event UpdateEventHandler SaveClicked
                {
                    add
                    {
                        Button_Outside.SaveClicked += value;
                    }

                    remove { Button_Outside.SaveClicked -= value; }
                }


                public ListItem selectedItem;
                public Datasourcer.Datasource DataSource = new Datasourcer.Datasource();

                // updatable
                public UpdatePanel updatePanel = new UpdatePanel();
                Control ctc;
                UpdatePanelTriggerCollection triggers;
                AsyncPostBackTrigger trigger;
                Timer UpdateTimer;

                public DropDown(string LableTitle, Datasourcer.Datasource dataSource, string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool wideMode)
                {
                    SetDropdown(LableTitle, dataSource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, wideMode);
                }

                public DropDown(string LableTitle, Datasourcer.Datasource dataSource, string ID, bool? PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool wideMode)
                {
                    string buff = null;
                    if (PlcTextValue != null)
                    {
                        buff = PlcTextValue.ToString();
                    }
                    SetDropdown(LableTitle, dataSource, ID, buff, top, left, size, fontSize, selfUpdatable, wideMode);
                }

                void SetDropdown(string LableTitle, Datasourcer.Datasource dataSource, string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool wideMode)
                {

                    this.ID = ID;

                    DataSource = dataSource;
                    Style.Add("top", Helper.FloatToStringWeb(top, "%"));
                    Style.Add("left", Helper.FloatToStringWeb(left, "%"));
                    Style.Add("position", "absolute");

                    if (selfUpdatable)
                    {
                        UpdateTimer = new Timer
                        {
                            Interval = Settings.UpdateValuesPCms,
                            ID = ID + "_tmr"
                        };
                    }

                    try
                    {
                        Button_Outside = new ButtonWithLabel_SelectMenu(LableTitle, DataSource, ID + "_s", ManageSelectedItem(PlcTextValue), fontSize, UpdateTimer, wideMode)
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
                    ctc.Controls.Add(SetControlAbsolutePos(Button_Outside, 0, 0, 100, 100));

                    updatePanel.UpdateMode = UpdatePanelUpdateMode.Conditional;
                    updatePanel.ID = ID + "_up";

                    if (selfUpdatable)
                    {
                        triggers = updatePanel.Triggers;
                        updatePanel.ContentTemplateContainer.Controls.Add(UpdateTimer);

                        trigger = new AsyncPostBackTrigger
                        {
                            ControlID = UpdateTimer.ID
                        };
                        triggers.Add(trigger);

                    }

                    Controls.Add(updatePanel);

                    var sizeW = wideMode ? size * 3 : size * 2;
                    Style.Add(HtmlTextWriterStyle.Width, Helper.FloatToStringWeb(sizeW, "vw"));
                    Style.Add(HtmlTextWriterStyle.Height, Helper.FloatToStringWeb(size, "vw"));

                }

                string ManageSelectedItem(string PlcTextValue)
                {
                    string buffSelectedItem = PlcTextValue ?? PropComm.NA;
                    foreach (var item in DataSource)
                    {
                        if (item != null)
                        {
                            if (item.Text == buffSelectedItem)
                            {
                                selectedItem = item;
                                return selectedItem.Text;
                            }
                        }
                    }

                    for (int i = DataSource.Count - 1; i > 0; i--) // order is returned so the N/A or null is the last possible outcom
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

                private void DropDown_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
                {
                    this.selectedItem = selectedItem;
                    Navigator.Refresh();
                }


                private void UpdateTimer_Tick1(object sender, EventArgs e)
                {
                    // Implicit update
                }


                void SetUpdateInterval()
                {
                    //triggers.Add(trigger); // TODO delete
                }

                public string GetSelectedValue()
                {
                    foreach (var item in DataSource)
                    {
                        if (item.Text == selectedItem.Text)
                        {
                            return item.Value;
                        }
                    }

                    return null;
                }

                public string GetSelectedText()
                {
                    return selectedItem.Text;
                }

            }
            //

            public class DropDownListChartViewSelector : DropDown
            {
                static Datasourcer.ChartViewSelectorDatasource datasource = new Datasourcer.ChartViewSelectorDatasource();

                public DropDownListChartViewSelector(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable)
                    : base("Izbor pogleda:", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, true)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                public static string GetReplacementTextFromEnum(int enum_)
                {
                    try
                    {
                        return datasource[enum_].Text;
                    }
                    catch (Exception)
                    {
                        return datasource[0].Value;
                    }

                }
            }

            public class DropDownListForDimmerLUX : DropDown
            {
                static Datasourcer.DimmerSelectorDatasource datasource = new Datasourcer.DimmerSelectorDatasource();


                public DropDownListForDimmerLUX(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool wideMode)
                    : base("Izberite Svetilnost:", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, wideMode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }
            }

            public class DropDownListForDimmerRPM : DropDown
            {
                static Datasourcer.DimmerSelectorDatasource datasource = new Datasourcer.DimmerSelectorDatasource();


                public DropDownListForDimmerRPM(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool wideMode)
                    : base("Izberite obrate:", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, wideMode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                new public short GetSelectedValue()
                {
                    return Convert.ToInt16(base.GetSelectedValue());
                }
            }

            public class DropDownListForHisteresis : DropDown
            {
                static Datasourcer.HisteresisSelectorDatasource datasource;

                public DropDownListForHisteresis(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode, bool hasZero)
                    : base("Izberite histerezo:", GetDatasource(hasZero), ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }

                static Datasourcer.HisteresisSelectorDatasource GetDatasource(bool hasZero)
                {
                    datasource = new Datasourcer.HisteresisSelectorDatasource(hasZero);
                    return datasource;
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                new public short GetSelectedValue()
                {
                    return Convert.ToInt16(base.GetSelectedValue());
                }
            }

            public class DropDownListForTimer_1_30s : DropDown
            {
                static Datasourcer.TimerSelectorDatasource datasource = new Datasourcer.TimerSelectorDatasource(1, 30, 1, "s");

                public DropDownListForTimer_1_30s(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }


                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }
            }

            public class DropDownListForTemperatureSelect_10_30 : DropDown
            {
                static Datasourcer.Temperature_10_30_SelectorDatasource datasource = new Datasourcer.Temperature_10_30_SelectorDatasource();

                public DropDownListForTemperatureSelect_10_30(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                    : base("Izberite željeno temperaturo:", datasource, ID, PlcTextValue, 0, 0, size, fontSize, selfUpdatable, false)
                {
                    Ctor();
                }

                public DropDownListForTemperatureSelect_10_30(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("Izberite željeno temperaturo:", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }


                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                new public short GetSelectedValue()
                {
                    return Convert.ToInt16(base.GetSelectedValue());
                }
            }

            public class DropDownListForWeekDaySelect : DropDown
            {
                static Datasourcer.WeekDaySelectorDatasource datasource = new Datasourcer.WeekDaySelectorDatasource();

                public DropDownListForWeekDaySelect(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("Izberite željeni dan:", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }


                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                new public byte GetSelectedValue()
                {
                    return Convert.ToByte(base.GetSelectedValue());
                }
            }

            public class DropDownListForHourSelect : DropDown
            {
                static Datasourcer.TimeSelectorDatasource datasource = new Datasourcer.TimeSelectorDatasource();

                public DropDownListForHourSelect(string ID, string PlcTextValue, float size, float fontSize, bool selfUpdatable)
                   : base("Izberite čas:", datasource, ID, PlcTextValue, 0, 0, size, fontSize, selfUpdatable, false)
                {
                    Ctor();
                }

                public DropDownListForHourSelect(string ID, string PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                   : base("Izberite čas:", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }
            }

            public class DropDownListForYesNoSelect : DropDown
            {
                static Datasourcer.YesNoSelectorDatasource datasource = new Datasourcer.YesNoSelectorDatasource();

                public DropDownListForYesNoSelect(string ID, short? PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("", datasource, ID, selectVal(PlcTextValue), top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                static string selectVal(short? val)
                {
                    if (val != null)
                    {
                        if (val == 0)
                        {
                            return datasource[2].Text; // Avtom.
                        }
                        return datasource[1].Text; // Rocno
                    }
                    return null;
                }

                new public short GetSelectedValue()
                {
                    var buff = base.GetSelectedValue();
                    if (buff == true.ToString())
                    {
                        return 1;
                    }
                    return 0;
                }
            }

            public class DropDownListForRocnoAvtoSelect : DropDown
            {
                static Datasourcer.RocnoAvtoSelectorDatasource datasource = new Datasourcer.RocnoAvtoSelectorDatasource();

                public DropDownListForRocnoAvtoSelect(string ID, short? PlcValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("Izberite režim delovanja:", datasource, ID, selectVal(PlcValue), top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                static string selectVal(short? val)
                {
                    if (val != null)
                    {
                        if (val == 0)
                        {
                            return datasource[2].Text; // Avtom.
                        }
                        return datasource[1].Text; // Rocno
                    }
                    return null;
                }

                new public short GetSelectedValue()
                {
                    return Convert.ToInt16(base.GetSelectedValue());
                }
            }

            public class DropDownListForRocno0Rocno1AvtoSelect : DropDown
            {
                static Datasourcer.Rocno0Rocno1AvtoDatasource datasource = new Datasourcer.Rocno0Rocno1AvtoDatasource();

                public DropDownListForRocno0Rocno1AvtoSelect(string ID, short? PlcValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("Izberite režim delovanja:", datasource, ID, selectVal(PlcValue), top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }

                static string selectVal(short? val)
                {
                    if (val != null)
                    {
                        if (val == 1)
                        {
                            return datasource[2].Text; // Rocno izklop
                        }
                        if (val == 2)
                        {
                            return datasource[3].Text; // Rocno vklop
                        }

                        return datasource[1].Text; // Avtom.

                    }
                    return null;
                }

                new public short GetSelectedValue()
                {
                    return Convert.ToInt16(base.GetSelectedValue());
                }

                public bool IsSelectedValueAuto()
                {
                    if (GetSelectedText() != PropComm.NA)
                    {
                        if (GetSelectedValue() == 0)
                        {
                            return true;
                        }
                    }
                    return false;
                }

                public bool IsSelectedValueMan1()
                {
                    if (GetSelectedValue() == 2)
                    {
                        return true;
                    }
                    return false;
                }

                public bool IsSelectedValueMan0()
                {
                    if (GetSelectedValue() == 1)
                    {
                        return true;
                    }
                    return false;
                }
            }

            public class DropDownListForOnOffSelect : DropDown
            {
                static Datasourcer.OnOffSelectorDatasource datasource = new Datasourcer.OnOffSelectorDatasource();

                public DropDownListForOnOffSelect(string ID, bool? PlcTextValue, float top, float left, float size, float fontSize, bool selfUpdatable, bool widemode)
                    : base("", datasource, ID, PlcTextValue, top, left, size, fontSize, selfUpdatable, widemode)
                {
                    Ctor();
                }

                void Ctor()
                {
                    DataSource = datasource;
                    Button_Outside.DataBind();
                }
            }

            public class LogMeIn : HtmlGenericControl
            {

                Label Title;
                Label Usrnmlbl;
                Label pwdlbl;

                Label UsrnmlblERR;
                Label pwdlblERR;

                TextBox username;
                TextBox password;
                ButtonWithLabel OK;

                Page page;
                HttpSessionState session;

                float fontSize;

                GroupBox g = new GroupBox(0, 0, 100, 100);

                List<WebControl> l = new List<WebControl>();

                float top, left, width, height;

                public LogMeIn(Page _thisPage, HttpSessionState session)
                {
                    top = 20;
                    left = 27;

                    this.session = session;
                    this.page = _thisPage;

                    fontSize = (top + left) / 35F;

                    float lblLeft = 5;
                    float lbltopoff = 12;

                    float tbLeft = 18;
                    float tbw = 40;
                    float tbh = 8;

                    float lblrow1 = 14;
                    float lblrow2 = 22;
                    float lbloffset = 2;


                    this.ID = "LogInForm";

                    width = 100 - (left * 2) - 10;
                    height = 100 - top * 3;

                    lblrow1 += lbltopoff;
                    lblrow2 += tbh + lbltopoff;
                    tbLeft += fontSize * 10 + lblLeft;

                    SetControlAbsolutePos(this, top, left, width, height);

                    //
                    Title = new Label()
                    {
                        Text = "Za nadaljevanje se vpišite:"

                    };
                    l.Add(Title);
                    SetControlAbsolutePos(l.Last(), lbltopoff, tbLeft - 2);
                    l.Last().Style.Add(HtmlTextWriterStyle.FontWeight, "bold");

                    //
                    Usrnmlbl = new Label()
                    {
                        Text = "Uporabniško ime:"
                    };
                    l.Add(Usrnmlbl);
                    SetControlAbsolutePos(l.Last(), lblrow1 + lbloffset, lblLeft);
                    l.Last().Attributes.Add("autocomplete", "on");

                    //
                    pwdlbl = new Label()
                    {
                        Text = "Geslo:"
                    };
                    l.Add(pwdlbl);
                    SetControlAbsolutePos(l.Last(), lblrow2 + lbloffset, lblLeft + fontSize * 10 + 2);

                    //
                    username = new TextBox()
                    {
                        ID = "usrField",
                        TextMode = TextBoxMode.SingleLine
                    };
                    l.Add(username);
                    SetControlAbsolutePos(l.Last(), lblrow1, tbLeft, tbw, tbh);

                    //
                    password = new TextBox()
                    {
                        ID = "pwdField",
                        TextMode = TextBoxMode.Password
                    };
                    l.Add(password);
                    SetControlAbsolutePos(l.Last(), lblrow2, tbLeft, tbw, tbh);

                    UsrnmlblERR = new Label(); ErrLblFormat(UsrnmlblERR); UsrnmlblERR.Style.Add("top", "27%");
                    pwdlblERR = new Label(); ErrLblFormat(pwdlblERR); pwdlblERR.Style.Add("top", "43%");

                    //
                    addToPage();
                    Controls.Add(g);

                    // scriptloader
                    var sl = Navigator.ScriptLoader.GetInstance(session);
                    sl.RegisterScriptOnPageLoad("FocusNextIfEnterKeyPressed", Val.FocusNextIfEnterKeyPressedScript); // MoveNext('TextBox1',event.keyCode) is defined here

                    // javascript on keypress - enter
                    username.Attributes.Add("onkeydown", "return MoveNext('pwdField',event.keyCode);"); // method defined in FocusNextIfEnterKeyPressed.js file

                    ErrorLoginManage();
                }

                void ErrLblFormat(Label lbl)
                {
                    lbl.Style.Add("position", "absolute");
                    lbl.Text = "X"; lbl.ForeColor = System.Drawing.Color.Red;
                    lbl.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                    lbl.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                    lbl.Style.Add(HtmlTextWriterStyle.Left, "78%");
                    l.Add(lbl);
                }

                // just shows error on page
                public void ErrorLoginManage()
                {
                    var sess = Navigator.GetSession(); // get session

                    if (sess != null)
                    {
                        var lauth = sess[SessionHelper.LoginAuth]; // get auth info - just for graphics

                        if (lauth != null)
                        {
                            var ucs = (Helper.UserDataManager.UserCheckStatus)lauth; // cast type


                            if (ucs == Helper.UserDataManager.UserCheckStatus.InvalidUserName) // invalid username flag
                            {
                                UsrnmlblERR.Style.Remove(HtmlTextWriterStyle.Visibility); // removes hidden atribute
                                return;
                            }
                            else
                            {
                                UsrnmlblERR.Style.Remove(HtmlTextWriterStyle.Visibility);
                                UsrnmlblERR.Style.Add(HtmlTextWriterStyle.Visibility, "hidden"); // adds hidden atribute
                            }


                            if (ucs == Helper.UserDataManager.UserCheckStatus.InvalidPassword) // invalid password flag
                            {
                                pwdlblERR.Style.Remove(HtmlTextWriterStyle.Visibility); // removes hidden atribute
                            }

                        }
                    }
                }

                void addToPage()
                {
                    foreach (var item in l)
                    {
                        item.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(fontSize, "vw"));
                        g.Controls.Add(item);
                    }


                    // OK button
                    OK = new ButtonWithLabel("Vpis", 20, fontSize);

                    OK.button.Click += Button_Click;
                    SetControlAbsolutePos(OK, 60, 40);
                    g.Controls.Add(OK);
                }

                private void Button_Click(object sender, ImageClickEventArgs e)
                {
                    var ip = Helper.GetClientIP();

                    SysLog.SetMessage("Login try: " + ip);

                    try
                    {
                        Helper.UserDataManager.UserCheckStatus valid;

                        if (!Navigator.LoginTryData.LoginTryIP(ip)) // checks if too many retries to login were performed
                        {
                            var ok = Helper.UserDataManager.ConfirmUsername(
                            out valid,
                            ((TextBox)FindControl("usrField")).Text,
                            ((TextBox)FindControl("pwdField")).Text,
                            Navigator.GetSession());

                            if (ok)
                            {
                                session[SessionHelper.LoggedIn] = Val.LoggingIn;
                                SysLog.SetMessage("Login Success: " + ip + " - User:  " + username.Text);
                            }
                            else
                            {
                                session[SessionHelper.LoggedIn] = "AUTH_False";
                                SysLog.SetMessage("Login Denied: " + ip + " - User:  " + username.Text);
                            }

                            session[SessionHelper.LoginAuth] = valid;

                            Navigator.Redirect("Default");
                        }
                    }
                    catch (Exception ex)
                    {
                        SysLog.SetMessage("Authentication error - General. " + ex.Message);
                    }
                }
            }

            public class UpdatePanelFull : UpdatePanel
            {
                public Timer Timer;
                AsyncPostBackTrigger ap;
                
                public UpdatePanelFull(string ID, int intervalUpdate)
                {
                    Timer = new Timer();
                    ap = new AsyncPostBackTrigger();
                    this.ID = ID;
                    Timer.Interval = intervalUpdate;
                    Timer.ID = ID + "tmr";
                    ap.ControlID = Timer.ID;
                    ContentTemplateContainer.Controls.Add(Timer);
                    Triggers.Add(ap);
                    this.UpdateMode = UpdatePanelUpdateMode.Conditional;
                }

                public void Controls_Add(Control c)
                {
                    this.ContentTemplateContainer.Controls.Add(c);
                }

                public bool IsUpdating(Page page)
                {
                    string controlName = page.Request.Form["__EVENTTARGET"];
                    if (!string.IsNullOrEmpty(controlName))
                    {
                        var control = page.FindControl(controlName);
                        if (control != null)
                        {
                            if (control.ID == Timer.ID)
                            {
                                return true;
                            }
                        }
                    }
                    return false;



                }

            }

            public class PleaseWaitBanner : GroupBox
            {
                SuperLabel plsWait = new SuperLabel("Prosim počakajte...", 20, 20, 50, 15);

                public PleaseWaitBanner():base(top(), left(), width(), height())
                {
                    ID = "PlsWaitBanner_hidden";
                    Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
                    this.Style.Add(HtmlTextWriterStyle.ZIndex, "99");

                    plsWait.FontWeightBold = true;
                    Controls.Add(plsWait);
                }

                public void ShowBanner()
                {
                    Style.Remove(HtmlTextWriterStyle.Visibility);
                    Style.Add(HtmlTextWriterStyle.Visibility, "visible");
                }

                static int top()
                {
                    return 30;
                }

                static int left()
                {
                    return 30;
                }

                static int width()
                {
                    return 40;
                }

                static int height()
                {
                    return 30;
                }
            }
            
        }

    }
}