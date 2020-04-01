using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace WebApplication1
{
    public partial class GuiController
    {

        public class PageLogView : Dsps
        {   
            public GControls.ButtonWithLabel refresh = new GControls.ButtonWithLabel("Refresh", 7, 1.2F);

            public PageLogView()
            {
                CreateControlBtns();
            }

            void CreateControlBtns()
            {
                var size = 7;                
                SetControlAbsolutePos(refresh, 0, 45, size, size);                

            }
        }

    }
}