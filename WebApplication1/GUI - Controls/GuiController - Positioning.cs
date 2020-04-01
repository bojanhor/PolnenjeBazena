using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace WebApplication1
{
    public partial class GuiController
    {

        // SetControlAbsolutePos
        public static WebControl SetControlAbsolutePos(WebControl c, int top, int left)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left + "%");
            return c;
        }
        
        public static WebControl SetControlAbsolutePos(WebControl c, int top, int left, int width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", width + "%");
            return c;
        }

        public static WebControl SetControlAbsolutePos(WebControl c, int top, int left, int width, int height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", height + "%");
            return c;
        }

        public static WebControl SetControlAbsolutePos(WebControl c, float top, float left)
        {
            
            c.Style.Add("position", "absolute");
            c.Style.Add("top", Helper.FloatToStringWeb(top, "%"));
            c.Style.Add("left", Helper.FloatToStringWeb(left, "%"));           
            return c;
        }

        public static WebControl SetControlAbsolutePos(WebControl c, float top, float left, float width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", Helper.FloatToStringWeb(width, "%"));
            return c;
        }


        public static WebControl SetControlAbsolutePos(WebControl c, float top, float left, float width, float height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", Helper.FloatToStringWeb(height, "%"));
            return c;
        }        

        public static WebControl SetControlAbsolutePos(WebControl c, int top, string left, int width)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left);
            c.Style.Add("width", width + "%");
            return c;
        }

        public static WebControl SetControlAbsolutePos(WebControl c, int top, string left, int width, int height)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left);
            c.Style.Add("width", width + "%");
            c.Style.Add("height", height + "%");
            return c;
        }
        
        public static WebControl SetControlAbsolutePos(WebControl c, string top, string left)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top);
            c.Style.Add("left", left);
            return c;
        }

        public static WebControl SetControlAbsolutePos(WebControl c, string top, string left, string width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", width);
            return c;
        }

        public static WebControl SetControlAbsolutePos(WebControl c, string top, string left, string width, string height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", height);
            return c;
        }


        // SetFontProperties
        public static void SetFontProperties_vw(WebControl c, float size)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "vw"));
        }

        public static void SetFontProperties_percent(WebControl c, float size)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "%"));
        }

        public static void SetFontProperties_vw(WebControl c, float size, bool bold)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "vw"));
            if (bold)
            {
                c.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            }
        }

        public static void SetFontProperties_percent(WebControl c, float size, bool bold)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "%"));
            if (bold)
            {
                c.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            }
        }

        //SetControlAbsolutePos
        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, int top, int left)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left + "%");
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, int top, int left, int width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", width + "%");
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, int top, int left, int width, int height)
        {
            SetControlAbsolutePos(c, top, left, width);            
            c.Style.Add("height", height + "%");
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, float top, float left)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", Helper.FloatToStringWeb(top, "%"));
            c.Style.Add("left", Helper.FloatToStringWeb(left, "%"));           
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, float top, float left, float width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", Helper.FloatToStringWeb(width, "%"));
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, float top, float left, float width, float height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", Helper.FloatToStringWeb(height, "%"));
            return c;
        }
        
        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, int top, string left, int width)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left);
            c.Style.Add("width", width + "%");
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, int top, string left, int width, int height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", height + "%");
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, string top, string left)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top);
            c.Style.Add("left", left);
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, string top, string left, string width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", width);
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c, string top, string left, string width, string height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", height);
            return c;
        }

        //

        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, int top, int left)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left + "%");
            return c;
        }

        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, int top, int left, int width)
        {
            SetControlAbsolutePos(c, top, left);
            c.Style.Add("width", width + "%");
            return c;
        }

        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, int top, int left, int width, int height)
        {
            SetControlAbsolutePos(c, top, left, width);
            c.Style.Add("height", height + "%");
            return c;
        }

        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, float top, float left, float width, float height)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", Helper.FloatToStringWeb(top, "%"));
            c.Style.Add("left", Helper.FloatToStringWeb(left, "%"));
            c.Style.Add("width", Helper.FloatToStringWeb(width, "%"));
            c.Style.Add("height", Helper.FloatToStringWeb(height, "%"));
            return c;
        }
        
        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, int top, string left, int width)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left);
            c.Style.Add("width", width + "%");
            return c;
        }

        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, int top, string left, int width, int height)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", top + "%");
            c.Style.Add("left", left);
            c.Style.Add("width", width + "%");
            c.Style.Add("height", height + "%");
            return c;
        }

        public static HtmlInputControl SetControlAbsolutePos(HtmlInputControl c, float top, float left, float width)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add("top", Helper.FloatToStringWeb(top, "%"));
            c.Style.Add("left", Helper.FloatToStringWeb(left, "%"));
            c.Style.Add("width", Helper.FloatToStringWeb(width, "%"));
            return c;
        }

        public static void SetFontProperties_vw(HtmlGenericControl c, float size)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "vw"));
        }

        public static void SetFontProperties_percent(HtmlGenericControl c, float size)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "%"));
        }

        public static void SetFontProperties_vw(HtmlGenericControl c, float size, bool bold)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "vw"));
            if (bold)
            {
                c.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            }
        }

        public static void SetFontProperties_percent(HtmlGenericControl c, float size, bool bold)
        {
            c.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(size, "%"));
            if (bold)
            {
                c.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
            }
        }

        //

        public static WebControl SetControlAbsolutePos(WebControl c)
        {
            c.Style.Add("position", "absolute");
            return c;
        }

        public static ImageButton SetControlAbsolutePos(ImageButton c)
        {
            c.Style.Add("position", "absolute");
            return c;
        }

        public static Button SetControlAbsolutePos(Button c)
        {
            c.Style.Add("position", "absolute");
            return c;
        }

        public static HtmlGenericControl SetControlAbsolutePos(HtmlGenericControl c)
        {
            c.Style.Add("position", "absolute");
            return c;
        }

        //

        public static HtmlGenericControl EncapsulateIntoDIV_TLWH(HtmlInputControl control)
        {
            SetControlAbsolutePos(control, 0, 0, 100, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        public static HtmlGenericControl EncapsulateIntoDIV_TLWH(HtmlGenericControl control)
        {
            SetControlAbsolutePos(control, 0, 0, 100, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        public static HtmlGenericControl EncapsulateIntoDIV_TLWH(ImageButton control)
        {
            SetControlAbsolutePos(control, 0, 0, 100, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        public static HtmlGenericControl EncapsulateIntoDIV_TLW(WebControl control)
        {
            SetControlAbsolutePos(control, 0, 0, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        public static HtmlGenericControl EncapsulateIntoDIV_TLW(HtmlInputControl control)
        {
            SetControlAbsolutePos(control, 0, 0, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        public static HtmlGenericControl EncapsulateIntoDIV_TLW(HtmlGenericControl control)
        {
            SetControlAbsolutePos(control, 0, 0, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        public static HtmlGenericControl EncapsulateIntoDIV_TLW(ImageButton control)
        {
            SetControlAbsolutePos(control, 0, 0, 100);
            var div = DIV.CreateDivAbsolute();
            div.Controls.Add(control);
            return div;
        }

        // Zindex

        public static WebControl SetControlZindex(WebControl c, int Zindex)
        {
            c.Style.Add("position", "absolute");           
            c.Style.Add(HtmlTextWriterStyle.ZIndex, Zindex + "");
            return c;
        }

        public static HtmlGenericControl SetControlZindex(HtmlGenericControl c, int Zindex)
        {
            c.Style.Add("position", "absolute");
            c.Style.Add(HtmlTextWriterStyle.ZIndex, Zindex + "");
            return c;
        }


        // 

        public static Label CreateLabelTitle_OnTop(string text, HtmlGenericControl TitleAboveThisDiv, float offsetHeight, float offsetLeft, int? width, float FontMultiplyer, bool bold, bool center_h, string htmlColor)
        {
            try
            {

                Label l = new Label
                {
                    Text = text
                };
                l.Style.Add("position", "absolute");
                l.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(FontMultiplyer, "vw"));

                if (bold)
                {
                    l.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                }

                if (center_h)
                {
                    l.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                }

                if (htmlColor != null)
                {
                    l.Style.Add(HtmlTextWriterStyle.Color, htmlColor);
                }
                               
                var style = TitleAboveThisDiv.Style.Value;

                var buff = Misc.FindStringBetween(style, "top:", "%");
              
                float top;
                if (buff != "")
                {
                    top = Convert.ToSingle(buff, CultureInfo.InvariantCulture);
                }
                else
                {
                    top = 0;
                }
                var lableHeight = Convert.ToSingle((l.Height).Value, CultureInfo.InvariantCulture);

                buff = Misc.FindStringBetween(style, "left:", "%");
                float left;
                if (buff != "")
                {
                    left = Convert.ToSingle(buff, CultureInfo.InvariantCulture);
                }
                else
                {
                    left = 0;
                }
                string leftPos = Helper.FloatToStringWeb((left + offsetLeft), "%");
                string lableTop = Helper.FloatToStringWeb((top + lableHeight - offsetHeight), "%");

                l.Style.Add("top", lableTop);
                l.Style.Add("left", leftPos);

                if (width != null)
                {
                    l.Style.Add("width", width.ToString() + "%");
                }                

                return l;

            }
            catch (Exception ex)
            {

                throw new Exception("Internal error inside CreateLabelTitle_insideDiv() method. " + ex.Message);
            }
            

        }

        public static Label CreateLabelInside(string text, HtmlGenericControl TitleLeftOfThisDiv, float top, float left, float FontMultiplyer, bool bold, bool center_h, string htmlColor)
        {
            try
            {
                Label l = new Label
                {
                    Text = text
                };
                l.Style.Add("position", "absolute");
                l.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(FontMultiplyer, "vw"));
                if (center_h)
                {
                    l.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                }

                if (bold)
                {
                    l.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                }

                if (htmlColor != null)
                {
                    l.Style.Add(HtmlTextWriterStyle.Color, htmlColor);
                }
                    
                l.Style.Add("top", Helper.FloatToStringWeb(top, "%"));
                l.Style.Add("left", Helper.FloatToStringWeb(left, "%"));

                l.Style.Add("width", Helper.FloatToStringWeb((100 - left*2), "%"));
                l.Style.Add("height", Helper.FloatToStringWeb((100 - top*2), "%"));

                return l;

            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside CreateLabelTitle_insideDiv() method. " + ex.Message);
            }
        }

        public static Label CreateLabelTitle_OnLeft(string text, HtmlGenericControl TitleLeftOfThisDiv, float offsetHeight, float offsetLeft, int? width, float FontMultiplyer, bool bold, string htmlColor)
        {
            try
            {
                Label l = new Label
                {
                    Text = text
                };
                l.Style.Add("position", "absolute");
                l.Style.Add(HtmlTextWriterStyle.FontSize, Helper.FloatToStringWeb(FontMultiplyer, "vw"));

                if (bold)
                {
                    l.Style.Add(HtmlTextWriterStyle.FontWeight, "bold");
                }

                if (htmlColor != null)
                {
                    l.Style.Add(HtmlTextWriterStyle.Color, htmlColor);
                }
                                              
                var style = TitleLeftOfThisDiv.Style.Value;

                var buff = Misc.FindStringBetween(style, "top:", "%");

                float top;
                if (buff != "")
                {
                    top = Convert.ToSingle(buff, CultureInfo.InvariantCulture);
                }
                else
                {
                    top = 0;
                }
                
                var lableHeight = Convert.ToSingle((l.Height).Value, CultureInfo.InvariantCulture);

                buff = Misc.FindStringBetween(style, "left:", "%");
                float left;
                if (buff != "")
                {
                    left = Convert.ToSingle(buff, CultureInfo.InvariantCulture);
                }
                else
                {
                    left = 0;
                }


                string leftPos = Helper.FloatToStringWeb((left - offsetLeft), "%");
                string lableTop = Helper.FloatToStringWeb((top + offsetHeight), "%");

                l.Style.Add("top", lableTop);
                l.Style.Add("left", leftPos);

                if (width != null)
                {
                    l.Style.Add("width", width.ToString()+"%");
                }

                return l;

            }
            catch (Exception ex)
            {
                throw new Exception("Internal error inside CreateLabelTitle_insideDiv() method. " + ex.Message);
            }
        }        
    }
}
