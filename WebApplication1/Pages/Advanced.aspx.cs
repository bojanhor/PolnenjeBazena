using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;

namespace WebApplication1.Pages
{
    public partial class PageAdvanced : System.Web.UI.Page
    {
        
        
        protected void Page_Load(object sender, EventArgs e)
        {           
            Val.guiController.PageAdvanced_ = new GuiController.PageAdvanced(this, Session);
            Navigator.EveryPageProtocol("Nastavitve", this, Session, TemplateClassID);

            AddConnectSwitch();
            AddDebug();
            AddTable();
            AddTimer();     
            AddButtonsAux();          
                                    
        }
        
        void AddButtonsAux()
        {
            TemplateClassID.Controls.Add(Val.guiController.PageAdvanced_.BtnEditor);
            TemplateClassID.Controls.Add(Val.guiController.PageAdvanced_.BtnLogView);
            TemplateClassID.Controls.Add(Val.guiController.PageAdvanced_.BtnRestartServer);
        }

        void AddConnectSwitch()
        {
            
            for (int i = 0; i < Val.guiController.PageAdvanced_.connectSwitch.Length; i++)
            {
                Val.guiController.PageAdvanced_.connectSwitch[i].button.Click += ConnectSwitch1_Click;
            }
            
        }

        private void ConnectSwitch1_Click(object sender, ImageClickEventArgs e)
        {
            var sw = (GuiController.GControls.ImageButtonWithID)sender;
            var id = Convert.ToInt16(sw.btnID);

            if (Val.logocontroler.LOGOConnection[id].IsLogoConnected)
            {
                Val.logocontroler.DisconnectAsync(id);
            }
            else
            {
                Val.logocontroler.ConnectAsync(id);
            }
        }

        void AddDebug()
        {
            TemplateClassID.Controls.Add(Val.guiController.PageAdvanced_.divDebug);
        }

        void AddTable()
        {
            TableCell0_1.Text = "Watchdog Value";
            TableCell0.Text = "Device";

            for (int i = 1; i <= Settings.Devices; i++)
            {
                
                switch (i) // Table values
                {
                    case 1:
                        cellLogo1.Text = XmlController.GetDeviceName(i);
                        cellLogo_watchdogVal1.Text = PropComm.NA;
                        TableCellDevName1.Text = XmlController.GetShowName(i);
                        TableCell11.Controls.Add(Val.guiController.PageAdvanced_.connectSwitch[i-1]);
                        break;

                    case 2:
                        cellLogo2.Text = XmlController.GetDeviceName(i);
                        cellLogo_watchdogVal2.Text = PropComm.NA;
                        TableCellDevName2.Text = XmlController.GetShowName(i);
                        TableCell12.Controls.Add(Val.guiController.PageAdvanced_.connectSwitch[i-1]);
                        break;

                    case 3:
                        cellLogo3.Text = XmlController.GetDeviceName(i);
                        cellLogo_watchdogVal3.Text = PropComm.NA;
                        TableCellDevName3.Text = XmlController.GetShowName(i);
                        TableCell13.Controls.Add(Val.guiController.PageAdvanced_.connectSwitch[i-1]);
                        break;

                    case 4:
                        cellLogo4.Text = XmlController.GetDeviceName(i);
                        cellLogo_watchdogVal4.Text = PropComm.NA;
                        TableCellDevName4.Text = XmlController.GetShowName(i);
                        TableCell14.Controls.Add(Val.guiController.PageAdvanced_.connectSwitch[i-1]);
                        break;

                    case 5:
                        cellLogo5.Text = XmlController.GetDeviceName(i);
                        cellLogo_watchdogVal5.Text = PropComm.NA;
                        TableCellDevName5.Text = XmlController.GetShowName(i);
                        TableCell15.Controls.Add(Val.guiController.PageAdvanced_.connectSwitch[i-1]);
                        break;

                    default:break;
                }                
            }
            
        }

        void AddTimer()
        {            
            Timer.Tick += Timer_Tick_UpdatePanel;
            Timer.Interval = Settings.UpdateValuesPCms/2;
            Timer.Enabled = true;
        }
        

        
        public void Timer_Tick_UpdatePanel(object sender, EventArgs e)
        {
                        
            for (int i = 1; i < Val.watchdog.Length; i++)
            {
                if (Val.watchdog[i] != null)
                {
                    switch (i)
                    {
                        case 1: cellLogo_watchdogVal1.Text = Val.watchdog[i].ToString(); break;
                        case 2: cellLogo_watchdogVal2.Text = Val.watchdog[i].ToString(); break;
                        case 3: cellLogo_watchdogVal3.Text = Val.watchdog[i].ToString(); break;
                        case 4: cellLogo_watchdogVal4.Text = Val.watchdog[i].ToString(); break;
                        case 5: cellLogo_watchdogVal5.Text = Val.watchdog[i].ToString(); break;
                        default:
                            break;
                    }
                }                
            }

            WritelineDebug();
                        
        }

        

        private void WritelineDebug()
        {            
            Val.guiController.PageAdvanced_.TextBoxDebug.Text = SysLog.Message.GetMessageForTB_min();
        }
               
    }
}