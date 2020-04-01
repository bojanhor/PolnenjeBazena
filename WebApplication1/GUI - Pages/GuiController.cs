using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


namespace WebApplication1
{
    public partial class GuiController
    {
        
        // Template class for every page
        public Template Template_ { get; set; }
        public PageDefault PageDefault_ { get; set; }
        public PageAdvanced PageAdvanced_ { get; set; }
        public PageMasterMenu PageMastermenu_ { get; set; }
        public PageRazsvetljava PageRazsvetljava_ { get; set; }
        public PageVreme PageVreme_ { get; set; }
        public PageEditor PageEditor_ { get; set; }
        public PageVentilacija PageVentilacija_ { get; set; }
        public PageVrataZavese PageVrataZavese_ { get; set; }
        public PagePadavine PagePadavine_ { get; set; }

        public GuiController()  
        {
            ConstructClass();            
        }

        void ConstructClass()
        {                        
            Template_ = new Template();          
        }

        public void Reconstruct()
        {            
            ConstructClass();           
        }
        
        public void RedirectToPageOnButtonClick(Page thisPage, int buttonId)
        {
            var link = XmlController.GetMenuItemLink(buttonId);
            Helper.Redirect(link, thisPage);
        }

                
        public static void CreateEnclosingDivForReference(HtmlGenericControl TemplateClassID)
        {
            TemplateClassID.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            TemplateClassID.Style.Add(HtmlTextWriterStyle.Top, "0px");
            TemplateClassID.Style.Add(HtmlTextWriterStyle.Left, "0px");
            TemplateClassID.Style.Add(HtmlTextWriterStyle.Width, XmlController.GetMasterWindowScaleX() + "%");
            TemplateClassID.Style.Add(HtmlTextWriterStyle.MarginLeft, "0.5%");
            TemplateClassID.Style.Add(HtmlTextWriterStyle.PaddingBottom, XmlController.GetMasterWindowScaleY() + "%");

        }

        public class DIV
        {
            public static HtmlGenericControl CreateDivRelative()
            {
                var div = new HtmlGenericControl("div");
                div.Style.Add(HtmlTextWriterStyle.Position, "relative");


                return div;
            }

            public static HtmlGenericControl CreateDivAbsolute()
            {
                var div = new HtmlGenericControl("div");
                div.Style.Add(HtmlTextWriterStyle.Position, "absolute");


                return div;
            }

            public static HtmlGenericControl CreateDivAbsolute(string top)
            {
                var div = new HtmlGenericControl("div");
                div.Style.Add(HtmlTextWriterStyle.Position, "absolute");

                if (top != null)
                {
                    div.Style.Add(HtmlTextWriterStyle.Top, (top));
                }
                return div;
            }

            public static HtmlGenericControl CreateDivAbsolute(float top, string unit)
            {
                return CreateDivAbsolute(Helper.FloatToStringWeb(top, unit));
            }

            public static HtmlGenericControl CreateDivAbsolute(float top, float left, string unit)
            {
                return CreateDivAbsolute(
                    Helper.FloatToStringWeb(top, unit),
                    Helper.FloatToStringWeb(left, unit)
                    );
            }

            public static HtmlGenericControl CreateDivAbsolute(float top, float left, float width, float height, string unit)
            {
                return CreateDivAbsolute(
                    Helper.FloatToStringWeb(top, unit),
                    Helper.FloatToStringWeb(left, unit),
                    Helper.FloatToStringWeb(width, unit),
                    Helper.FloatToStringWeb(height, unit)
                    );
            }

            public static HtmlGenericControl CreateDivAbsolute(float top, float left, float width, float height, string zindex, string unit)
            {
                return CreateDivAbsolute(
                    Helper.FloatToStringWeb(top, unit),
                    Helper.FloatToStringWeb(left, unit),
                    Helper.FloatToStringWeb(width, unit),
                    Helper.FloatToStringWeb(height, unit),
                    zindex
                    );
            }

            public static HtmlGenericControl CreateDivAbsolute(string top, string left)
            {
                var div = CreateDivAbsolute(top);

                if (left != null)
                {
                    div.Style.Add(HtmlTextWriterStyle.Left, (left));
                }
                return div;
            }

            public static HtmlGenericControl CreateDivAbsolute(string top, string left, string width, string height)
            {
                var div = CreateDivAbsolute(top, left);

                if (width != null)
                {
                    div.Style.Add(HtmlTextWriterStyle.Width, (width));
                }
                if (height != null)
                {
                    div.Style.Add(HtmlTextWriterStyle.Height, (height));
                }
                return div;
            }

            public static HtmlGenericControl CreateDivAbsolute(string top, string left, string width, string height, string zindex)
            {
                var div = CreateDivAbsolute(top, left, width, height);
                div.Style.Add(HtmlTextWriterStyle.ZIndex, zindex);
                return div;

            }



            public static HtmlGenericControl CreateAhrefInsideDiv(string id_a, string ItemName, string ItemLink, string coloronmouseover, string color)
            {
                var d = CreateDivRelative();
                var a = new HtmlGenericControl("a");
                var padding = 5; // in percent




                a.Attributes.Add("id", id_a);
                var id_d = id_a + "_d";

                d.Attributes.Add("id", id_d);

                // a style

                a.Style.Add(HtmlTextWriterStyle.Position, "absolute");
                a.Style.Add(HtmlTextWriterStyle.Padding, padding + "%");
                a.Style.Add(HtmlTextWriterStyle.Height, (100 - padding * 8.5) + "%");
                a.Style.Add(HtmlTextWriterStyle.Width, (100 - padding * 2) + "%");

                a.Attributes.Add("href", ItemLink);
                a.InnerHtml = ItemName;
                a.Style.Add("text-decoration", "none");
                a.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                a.Style.Add(HtmlTextWriterStyle.Color, "#1f1f1f");

                a.Style.Add(HtmlTextWriterStyle.TextAlign, "left");


                //div style
                d.Style.Add(HtmlTextWriterStyle.Color, color);
                d.Style.Add(HtmlTextWriterStyle.Height, "10%");
                d.Style.Add(HtmlTextWriterStyle.Width, "100%");

                a.Attributes.Add("onmouseover", "document.getElementById('" + id_d + "').style.backgroundColor = '" + coloronmouseover + "'; ");
                a.Attributes.Add("onmouseout", "document.getElementById('" + id_d + "').style.backgroundColor = '" + color + "'; ");

                d.Controls.Add(a);


                return d;
            }

        }



    }

}
