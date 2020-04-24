using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace WebApplication1
{
    public static class GlobalManagement
    {
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
    }
}