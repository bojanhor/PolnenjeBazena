﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1.Pages
{
    public partial class PageLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Val.guiController.PageLogin_ = new GuiController.PageLogin(this, Session, TemplateClassID);
            Navigator.EveryPageProtocol("Vpis", this, Session, TemplateClassID, false, true, false, false, true);
            
        }
    }
}