using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Deployment.Internal;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class PageXYset : Dsps
        {
            
            readonly Page thisPage;
            
            readonly Prop1 prop1 = Val.logocontroler.Prop1;
            readonly Prop2 prop2 = Val.logocontroler.Prop2;
            readonly System.Web.SessionState.HttpSessionState session;
            public GControls.UpdatePanelFull UP = new GControls.UpdatePanelFull("PageUpdatePanel", Settings.UpdateValuesPCms);


            public GControls.GroupBox gb_main;

            GControls.ImageButtonWithID incX, incy, decx, decy;

            GControls.SuperLabel x, y;

            int t = 15, l = 10, w = 60, h = 30;

            string fontsize = "2vw";
           
            public PageXYset(Page _thisPage, System.Web.SessionState.HttpSessionState session)
            {
                this.session = session;
                this.thisPage = _thisPage;


                gb_main = new GControls.GroupBox(20, 5, 40, 70);

                x = new GControls.SuperLabel("X: " + prop1.XPos.Value_string,t,l,w,h,fontsize ); gb_main.Controls.Add(x);
                y = new GControls.SuperLabel("Y: " + prop1.YPos.Value_string, t+h+5, l, w, h, fontsize); gb_main.Controls.Add(y);



            }


        }
    }
}