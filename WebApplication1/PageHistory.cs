using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WebApplication1
{
    [Serializable]
    class PageHistory
    {
        int maxEntries = 10;
        List<string> pages = new List<string>();

        public void StorePage(string pageName)
        {
            if (pages.Count > 0)
            {
                if (pages[pages.Count - 1] != pageName) // prevents duplicates
                {
                    pages.Add(pageName);
                }
            }
            else
            {
                pages.Add(pageName);
            }

            if (pages.Count > maxEntries)
            {
                pages.RemoveAt(pages.Count - 1);
            }
        }

        public string getPreviousPage()
        {
            if (pages.Count < 2)
            {
                return Settings.DefaultPage;
            }
            pages.RemoveAt(pages.Count - 1);
            return pages[pages.Count - 1];
        }

        public string getLastPage()
        {
            if (pages.Count < 1)
            {
                return Settings.DefaultPage;
            }
            return pages[pages.Count - 1];
        }

    }
}
