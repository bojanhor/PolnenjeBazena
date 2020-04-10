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

        public class PageEditor : Dsps
        {           
            public GControls.ButtonWithLabel save = new GControls.ButtonWithLabel("Save", 7, 1.2F);
            public GControls.ButtonWithLabel refresh = new GControls.ButtonWithLabel("Refresh", 7, 1.2F);
            public GControls.ButtonWithLabel downloadConfig = new GControls.ButtonWithLabel("Download", 7, 1.1F);

            public PageEditor()
            {
                CreateControlBtns();
            }

            void CreateControlBtns()
            {
                var size = 8;
                SetControlAbsolutePos(save, 0, 35, size, size);
                SetControlAbsolutePos(refresh, 0, 45, size, size);
                SetControlAbsolutePos(downloadConfig, 0, 55, size, size);

            }
        }

    }
}