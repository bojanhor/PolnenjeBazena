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
        readonly int maxEntries = 10;
        readonly List<string> PageNames = new List<string>();
        readonly List<Page> Pages = new List<Page>();

        public void StorePage(Page Page)
        {
            var PageName = GetPageNameFromPage(Page);

            if (PageNames.Count > 0)
            {
                if (PageNames[PageNames.Count - 1] != PageName) // prevents duplicates
                {                    
                    PageNames.Add(PageName);
                    Pages.Add(Page);
                }
            }
            else
            {               
                PageNames.Add(PageName);
                Pages.Add(Page);
            }

            if (PageNames.Count > maxEntries)
            {                
                PageNames.RemoveAt(PageNames.Count - 1);
                Pages.RemoveAt(Pages.Count - 1);
            }

            
        }

        public static string GetPageNameFromPage(Page p)
        {
            return new FileInfo(p.Request.Url.AbsolutePath).Name;
        }


        public string getPreviousPageName()
        {
            if (PageNames.Count < 2)
            {
                return Settings.DefaultPage;
            }
            PageNames.RemoveAt(PageNames.Count - 1);
            return PageNames[PageNames.Count - 1];
        }

        public string getThisPageName()
        {
            if (PageNames.Count < 1)
            {
                return Settings.DefaultPage;
            }
            return PageNames[PageNames.Count - 1];
        }

        public Page getThisPage()
        {
            if (Pages.Count < 1)
            {
                return null;
            }
            return Pages[PageNames.Count - 1];
        }

    }
}
