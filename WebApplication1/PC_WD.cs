using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebApplication1
{
    public class PC_WD
    {
        public bool WatchdogCounter = false;

        public PC_WD()
        {
            Misc.SmartThread PCWD = new Misc.SmartThread(() => PCWD_m());
            PCWD.Start("PCWD", ApartmentState.MTA, true);
        }

        void PCWD_m()
        {
            var wdtimer = XmlController.GetReadWriteCycle(1);


            try
            {
                while (true)
                {
                    WatchdogCounter = !WatchdogCounter;
                    Thread.Sleep(wdtimer);

                    if (Val.logocontroler != null && Val.logocontroler.Prop1 != null && Val.logocontroler.Prop1.PCWD != null)
                    {
                        if (Val.logocontroler.Prop1.PCWD.Value_short <= 0)
                        {
                            Val.logocontroler.Prop1.PCWD.Value_short = 1;
                        }
                        else
                        {
                            Val.logocontroler.Prop1.PCWD.Value_short = 0;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("PC Watchdog has stopped unexpectedly. " + ex.Message);
            }


        }

    }
}