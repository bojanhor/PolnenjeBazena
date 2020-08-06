using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Web.SessionState;
using static WebApplication1.GuiController;
using static WebApplication1.GuiController.GControls;
using System.Threading;

namespace WebApplication1
{
    public class WarningManagerWebControl : HtmlGenericControl
    {
        readonly HtmlGenericControl TemplateClass;
        readonly UpdatePanelFull up = new UpdatePanelFull("WarningShowUpdatePanel", Settings.UpdateValuesPCms);
        readonly GroupBox WarningMenuShow = new GroupBox(20, 20, 60, 60, true);
        ImageButton exitButton;
        readonly SuperLabel TiltleLbl = new SuperLabel("OPOZORILA: ", 4, 2, 30, 6);
        readonly SuperLabel[] messages = new SuperLabel[5];
        readonly Image WarningSymbol = new Image();
        readonly TransparentButton btn = new TransparentButton("btnWarningSymbol");
        
        public WarningManagerWebControl(HtmlGenericControl TemplateClass, float top, float left, float size)
        {
            this.TemplateClass = TemplateClass;

            ID = "Warnings";
            Style.Add(HtmlTextWriterStyle.ZIndex, "10");
            SetControlAbsolutePos(this, top, left, size, size);

            var pos_top = 0;
            var pos_left = 0;
            var pos_wid = 2.5F;

            WarningSymbol.ImageUrl = "~/Pictures/Warning.png";
            SetControlAbsolutePos_vw(WarningSymbol, pos_top, pos_left, pos_wid, pos_wid);

            SetControlAbsolutePos_vw(btn, pos_top, pos_left, pos_wid, pos_wid);
            WarningMenuShow.ID = "WarningMenuShow";
            WarningMenuShow.Style.Add(HtmlTextWriterStyle.ZIndex, "90");

            TiltleLbl.FontSize = 2;
            exitBtn();

            DisplayMessages();

            btn.Click += Btn_Click;
            exitButton.Click += ExitButton_Click;

            ShowWarningSymbol();

            AddControls();
           
        }

        void exitBtn()
        {
            exitButton = new ImageButton()
            {
                ID = "btnExit_s_" + ID,
                ImageUrl = "~/Pictures/exit.png",
                Width = Unit.Percentage(5)
            };
            SetControlAbsolutePos(exitButton, 3, 93);

            WarningMenuShow.Controls.Add(exitButton);
        }

        private void ExitButton_Click(object sender, ImageClickEventArgs e)
        {
            Navigator.GetSession()[SessionHelper.WarningShowVisible] = "false";
            Navigator.Refresh();
        }

        void DisplayMessages()
        {
            var top = 20;
            var dist = 16;                        
            int from = 0;
            

            if (WarningManager.Warnings.Count >= 5)
            {
                from = WarningManager.Warnings.Count - 5;
                WarningManager.WarningsShowList = WarningManager.Warnings.GetRange(from, 5);
            }
            else
            {
                if (WarningManager.Warnings.Count < 1)
                {
                    return;
                }

                WarningManager.WarningsShowList = WarningManager.Warnings.GetRange(0, (WarningManager.Warnings.Count));

            }

            for (int i = 0; i < WarningManager.WarningsShowList.Count; i++)
            {
                if (WarningManager.WarningsShowList[i] != null)
                {
                    var newTop = top + dist * i;
                    messages[i] = new SuperLabel("", newTop, 5, 85, 6) { FontWeightBold = true, FontSize = 1.4F };
                    CancelMessageBtn(i, newTop);
                    Separe(newTop);

                    messages[i].label.Text = WarningManager.WarningsShowList[i].GetMessage();
                    WarningMenuShow.Controls.Add(messages[i]);
                }
            }
        }

        private void Btn_Click(object sender, ImageClickEventArgs e)
        {
            Navigator.GetSession()[SessionHelper.WarningShowVisible] = "true";
            Navigator.Refresh();
        }

        void AddControls()
        {
            if (WarningShowEnabled())
            {
                up.Controls_Add(WarningMenuShow);
                TemplateClass.Controls.Add(up);
            }

            WarningMenuShow.Controls.Add(TiltleLbl);
            Controls.Add(WarningSymbol);
            Controls.Add(btn);
        }

        void ShowIcon(WebControl c, bool show)
        {
            c.Style.Remove(HtmlTextWriterStyle.Visibility);
            if (show)
            {
                c.Style.Add(HtmlTextWriterStyle.Visibility, "visible");
            }
            else
            {
                c.Style.Add(HtmlTextWriterStyle.Visibility, "hidden");
            }
        }

        bool WarningShowEnabled()
        {
            var ws_obj = Navigator.GetSession()[SessionHelper.WarningShowVisible];
            if (ws_obj != null)
            {
                var ws = (string)ws_obj;
                if (ws != null)
                {
                    if (ws == "true")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void CancelMessageBtn(int id, int top)
        {
            ImageButton cancelBtn = new ImageButton()
            {
                ID = "btncancel_" + id,
                ImageUrl = "~/Pictures/exit.png",
                Width = Unit.Percentage(4)
            };

            SetControlAbsolutePos(cancelBtn, top - 1, 92);

            WarningMenuShow.Controls.Add(cancelBtn);

            cancelBtn.Click += CancelBtn_Click;
        }

        private void CancelBtn_Click(object sender, ImageClickEventArgs e)
        {
            var btn = (ImageButton)sender;
            var btnId = Convert.ToInt32(Helper.GetNumbersOnlyFromString(btn.ID));
            var warningToRemove = FindMessage(btnId);
            WarningManager.RemoveMessageForUser_Warning(warningToRemove);
            SysLog.Message.SetMessage("User acknowledged message: " + warningToRemove.GetMessage());
        }

        void RemoveMessage(string warningToRemove)
        {
            WarningManager.RemoveMessageForUser_Warning(warningToRemove);
        }

        void ShowWarningSymbol()
        {
            var show = false;
            if (WarningManager.Warnings.Count > 0)
            {
                show = true;
            }
            ShowIcon(WarningSymbol, show);
            ShowIcon(btn, show);

        }

        string FindLableText(int btnIndex)
        {
            return messages[btnIndex].label.Text;
        }

        void Separe(int top)
        {
            GuiSeparator_DottedLine gs = new GuiSeparator_DottedLine(top + 10, 5, 93, 0.5F, 20);
            WarningMenuShow.Controls.Add(gs);
        }

        WarningManager.Warning FindMessage(int btnId)
        {
            foreach (var item in WarningManager.WarningsShowList)
            {
                if (item != null)
                {
                    if (item.GetMessage() == FindLableText(btnId))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

    }

    public class WarningManager
    {
        public static List<Warning> Warnings = new List<Warning>();

        public WarningManager()
        {
            StartWarningTrackerThread();
        }

        static Misc.SmartThread WarningTrackerThread;

        public static List<Warning> WarningsShowList;
        static readonly List<Tracker> MessageTrackerList = new List<Tracker>();

        public static void AddMessageForUser_Warning(string message)
        {
            if (!PreventThisMessage_IsDuplicacate(message))
            {
                Warnings.Add(new Warning(message, CreateId()));
                SysLog.Message.SetMessage("Message shown to user: " + message);
            }
        }

        void WarningTrackerThreadMethod()
        {
            bool Alarm = false;
            WarningManager.Warnings = new List<Warning>();

            try
            {
                foreach (var item in MessageTrackerList)
                {
                    Alarm = item.UpdateValue_TriggerAlarm();

                    if (Alarm)
                    {
                        AddMessageForUser_Warning(item.WarningMessage);
                    }
                }

                Helper.WarningManagerInitialized = true;                
            }
            catch (Exception ex)
            {
                throw new Exception("WarningTrackerThread encountered an error and was terminated: " + ex.Message);
            }
        }

        public static void AddWarningTrackerFromPLCVar(PlcVars.PlcType PlcVar, object valueToTrigerWarning, WarningTriggerCondition Condition, string WarningMessage)
        {
            Tracker t = new Tracker(PlcVar, valueToTrigerWarning, Condition, WarningMessage);
            MessageTrackerList.Add(t);
                        
        }        
       
        public static void RemoveMessageForUser_Warning(Warning warning)
        {
            WarningManager.Warnings.Remove(warning);
        }
        public static void RemoveMessageForUser_Warning(string warning)
        {
            if (WarningsShowList != null)
            {
                WarningsShowList.Find(item => item.GetMessage() == warning);
                foreach (var item in WarningsShowList)
                {
                    if (warning == item.GetMessage())
                    {
                        RemoveMessageForUser_Warning(item);
                    }
                }
            }
            
        }

        public enum WarningTriggerCondition
        {
            EqualTo,
            NotEqualTo,
            GreaterThan,
            LessThan,
            GreaterThanOrEqualTo,
            LessThanOrEqualTo
        }

        static int CreateId()
        {
            int id = 0;
            bool again = false;

            while (true)
            {
                foreach (var item in WarningManager.Warnings)
                {
                    if (item != null)
                    {
                        if (item.GetID() == id)
                        {
                            id++;
                            again = true;
                            break;
                        }

                    }
                }

                if (!again)
                {
                    return id;
                }
                else
                {
                    again = false;
                }
            }
        }

        static bool PreventThisMessage_IsDuplicacate(string message)
        {
            if (Warnings != null)
            {
                foreach (var item in WarningManager.Warnings)
                {
                    if (message == item.GetMessage())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void StartWarningTrackerThread()
        {
            WarningTrackerThread = new Misc.SmartThread(() => WarningTrackerThreadMethod());
            WarningTrackerThread.Start("WarningTrackerThread", ApartmentState.MTA, true);
        }

        class Tracker
        {
            public PlcVars.PlcType PlcVar;
            public object valueToTrigerWarning;
            public WarningTriggerCondition Condition;
            public string WarningMessage;

            public Tracker(PlcVars.PlcType PlcVar, object valueToTrigerWarning, WarningTriggerCondition Condition, string WarningMessage)
            {
                this.WarningMessage = WarningMessage;

                // Type checks only
                var typ = PlcVar.GetType();
                PlcVars.PlcType buff;

                var typ1 = valueToTrigerWarning.GetType();
                object buff1;

                if (typ == typeof(PlcVars.Bit))
                {
                    buff = (PlcVars.Bit)PlcVar;
                }

                else if (typ == typeof(PlcVars.Byte))
                {
                    buff = (PlcVars.Byte)PlcVar;
                }

                else if (typ == typeof(PlcVars.Word))
                {
                    buff = (PlcVars.Word)PlcVar;
                }

                else if (typ == typeof(PlcVars.DWord))
                {
                    buff = (PlcVars.DWord)PlcVar;
                }

                else
                {
                    throw new Exception("Error: Unsupported type was passed (PlcVars.PlcType PlcVar). Supported types are: PlcVars.Bit, PlcVars.Byte, PlcVars.Word, PlcVars.DWord. Parent Class of this exception is WarningManager");
                }

                this.PlcVar = buff;

                //


                string messageErrorTypeConflict = "Error: Type conflict. If PlcVars.Bit was passed, valueToTrigerWarning should be of type bool, and vice versa.";

                if (typ1 == typeof(bool))
                {
                    if (
                        typ != typeof(PlcVars.Bit) ||
                        (Condition != WarningTriggerCondition.EqualTo && Condition != WarningTriggerCondition.NotEqualTo)
                        )
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }

                    buff1 = (bool)valueToTrigerWarning;
                }

                else if (typ1 == typeof(short))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (short)valueToTrigerWarning;
                }

                else if (typ1 == typeof(int))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (int)valueToTrigerWarning;
                }

                else if (typ1 == typeof(bool?))
                {
                    if (typ != typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (bool?)valueToTrigerWarning;
                }

                else if (typ1 == typeof(short?))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (short?)valueToTrigerWarning;
                }

                else if (typ1 == typeof(int?))
                {
                    if (typ == typeof(PlcVars.Bit))
                    {
                        throw new Exception(messageErrorTypeConflict);
                    }
                    buff1 = (int?)valueToTrigerWarning;
                }

                else
                {
                    throw new Exception("Error: Unsupported type was passed (object valueToTrigerWarning). Supported types are: bool, short, int, bool?, short?, int? . Parent Class of this exception is WarningManager");
                }

                this.valueToTrigerWarning = buff1;


                this.Condition = Condition;
            }

            public bool UpdateValue_TriggerAlarm()
            {
                try
                {
                    // Type checks
                    var typ = PlcVar.GetType();


                    if (typ == typeof(PlcVars.Bit))
                    {
                        var buff = (PlcVars.Bit)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareBool_Alarm((bool)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (typ == typeof(PlcVars.Byte))
                    {
                        var buff = (PlcVars.Byte)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareOthers_Alarm((short)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (typ == typeof(PlcVars.Word))
                    {
                        var buff = (PlcVars.Word)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareOthers_Alarm((short)val);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (typ == typeof(PlcVars.DWord))
                    {
                        var buff = (PlcVars.DWord)PlcVar;
                        var val = buff.Value;

                        if (val != null)
                        {
                            return CompareOthers_Alarm((short)val);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    throw new Exception("Wrong type was stored in collection.");
                }
                catch (Exception ex)
                {

                    throw new Exception("UpdateValue() reportred an Error (parent class: WarningManager): " + ex.Message);
                }

            }

            bool CompareBool_Alarm(bool val)
            {

                if (Condition == WarningTriggerCondition.EqualTo)
                {
                    if (val == (bool)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (Condition == WarningTriggerCondition.NotEqualTo)
                {
                    if (val == (bool)valueToTrigerWarning)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                throw new Exception("Condition parameter was invalid.");
            }

            bool CompareOthers_Alarm(short val)
            {
                if (Condition == WarningTriggerCondition.EqualTo)
                {
                    if (val == (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.GreaterThan)
                {
                    if (val > (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.GreaterThanOrEqualTo)
                {
                    if (val >= (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.LessThan)
                {
                    if (val < (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.LessThanOrEqualTo)
                {
                    if (val <= (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                if (Condition == WarningTriggerCondition.NotEqualTo)
                {
                    if (val != (int)valueToTrigerWarning)
                    {
                        return true;
                    }
                    else { return false; }
                }

                throw new Exception("Condition parameter was invalid.");
            }
        }

        public class Warning
        {
            readonly int id;
            readonly string message = "";

            public Warning(string Message, int id)
            {
                this.id = id;
                message = Message;
            }

            public string GetMessage()
            {
                return message;
            }

            public int GetID()
            {
                return id;
            }
        }
    }
}