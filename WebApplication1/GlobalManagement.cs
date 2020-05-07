using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WebApplication1
{
    public static class GlobalManagement
    {
        static int ForceRefreshState = 0;
        static int FastTimerInterval = 300;
        static int normalTimerrInterval = Settings.UpdateValuesPCms;

        public static IEnumerable<T> GetAllControlsOfType<T>(this Control parent) where T : Control
        {
            var result = new List<T>();
            foreach (Control control in parent.Controls)
            {
                if (control is T)
                {
                    result.Add((T)control);
                }
                if (control.HasControls())
                {
                    result.AddRange(control.GetAllControlsOfType<T>());
                }
            }
            return result;
        }

        public static List<Timer> GetTimers()
        {
            return (List<Timer>)GetAllControlsOfType<Timer>(Navigator.GetCurrentPage().Form);
        }

        public static void DisableAllTimersOnPage()
        {
            var page = Navigator.GetCurrentPage();
            var timers = GetTimers();

            foreach (var item in timers)
            {
                item.Enabled = false;
            }
        }

        public static void ForceRefreshValues(int device)
        {
            SetTimerIntrvalFast();

            // set faster logo sync interval
            Val.logocontroler.ForceRefreshValuesFromPLC(device);
            ForceRefreshState = 5;
        }

        static void SetTimerIntrvalFast()
        {
            // set shorter timer interval
            var timers = GetTimers();
            foreach (var item in timers)
            {
                if (item.Interval > FastTimerInterval)
                {
                    item.Interval = FastTimerInterval;                    
                }
            } // do not refresh here - it will reset timer interval           
        }

        static void SetTimerIntrvalNormal()
        {            
            Navigator.Refresh(); // full refresh causes timer intervals to be reset
        }

        public static void ForceRefreshManage()
        {
            // Used for faster loading times of update panel changes (most used after onClick event)
            if (ForceRefreshState > 0)
            {
                if (ForceRefreshState == 1)
                {
                    SetTimerIntrvalNormal();
                }
                ForceRefreshState -= 1;
            }
            
        }
    }
}