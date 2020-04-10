﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.DataVisualization.Charting;
using System;
using System.Drawing;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class PagePadavine : Dsps
        {
            Page thisPage;

            public HtmlGenericControl MainDiv;

            GControls.OnOffButton EnableSvetlost; GControls.SuperLabel EnableSvetlost_lbl;
            GControls.OnOffButton EnablePadavine; GControls.SuperLabel EnablePadavine_lbl;
            GControls.OnOffButton EnableTzunanja; GControls.SuperLabel EnableTzunanja_lbl;
            GControls.OnOffButton EnableTnotranja; GControls.SuperLabel EnableTnotranja_lbl;

            GControls.DropDownListChartViewSelector chartViewSelector;

            Helper.UpdatePanelFull updatePanel = new Helper.UpdatePanelFull("chartUpdtPnl", Settings.ChartUpdateRefreshRate);


            public PagePadavine(Page _thisPage)
            {
                try
                {
                    thisPage = _thisPage;

                    Initialize();

                    AddControls();

                }
                catch (Exception ex)
                {
                    throw new Exception("Internal error inside PagePadavine class constructor: " + ex.Message);
                }


            }

            void Initialize()
            {
                MainDiv = DIV.CreateDivAbsolute(0, 0, 100, 100, "%"); MainDiv.ID = "MainDiv1";
                ChartTweaker ct1 = new ChartTweaker(thisPage, "Chart1", MainDiv, updatePanel); // Create 2 overlaying charts to prevent flicker
                ChartTweaker ct2 = new ChartTweaker(thisPage, "Chart2", MainDiv, updatePanel);
            }

            void AddControls()
            {
                var btnType = GControls.OnOffButton.Type.Padded;


                EnableSvetlost = new GControls.OnOffButton("Svetlost", 1, XmlController.GetEnableCharts_Svetlost(), btnType);
                EnablePadavine = new GControls.OnOffButton("Padavine", 2, XmlController.GetEnableCharts_Padavine(), btnType);
                EnableTzunanja = new GControls.OnOffButton("T zunanja", 3, XmlController.GetEnableCharts_Tzunanja(), btnType);
                EnableTnotranja = new GControls.OnOffButton("T notranja", 4, XmlController.GetEnableCharts_Tnotranja(), btnType);
                var chartgraphShowVal = GControls.DropDownListChartViewSelector.GetReplacementTextFromEnum(XmlController.GetShowChartMode());
                chartViewSelector = new GControls.DropDownListChartViewSelector("chartViewSelector", chartgraphShowVal, 9.2F, 78, 4, 1.2F, false);

                var topoffset = 9;                
                var leftOffsetOrg = 8;
                var leftStp = 19;
                var size = 10;

                var topoffset_lbl = topoffset + 3;
                var leftOffsetOrg_lbl = leftOffsetOrg -6F;
                var leftStp_lbl = leftStp-0.4F;
                var fontsize = 1.3F;

                EnableSvetlost_lbl = new GControls.SuperLabel("Svetlost:", topoffset_lbl, leftOffsetOrg_lbl, 10, 6) { FontSize = fontsize, FontWeightBold = true  };
                EnablePadavine_lbl = new GControls.SuperLabel("Padavine:", topoffset_lbl , leftOffsetOrg_lbl += leftStp_lbl, 10, 6) { FontSize = fontsize, FontWeightBold = true };
                EnableTzunanja_lbl = new GControls.SuperLabel("T zunanja:", topoffset_lbl , leftOffsetOrg_lbl += leftStp_lbl, 10, 6) { FontSize = fontsize, FontWeightBold = true };
                EnableTnotranja_lbl = new GControls.SuperLabel("T notranja:", topoffset_lbl, leftOffsetOrg_lbl += leftStp_lbl, 10, 6) { FontSize = fontsize, FontWeightBold = true };

                EnableSvetlost.Top = topoffset + "";
                EnablePadavine.Top = topoffset + "";
                EnableTzunanja.Top = topoffset + "";
                EnableTnotranja.Top = topoffset + "";

                EnableSvetlost.Size = size + "";
                EnablePadavine.Size = size + "";
                EnableTzunanja.Size = size + "";
                EnableTnotranja.Size = size + "";

                EnableSvetlost.Left = leftOffsetOrg + ""; leftOffsetOrg += leftStp;
                EnablePadavine.Left = leftOffsetOrg + ""; leftOffsetOrg += leftStp;
                EnableTzunanja.Left = leftOffsetOrg + ""; leftOffsetOrg += leftStp;
                EnableTnotranja.Left = leftOffsetOrg + "";

                EnableSvetlost.button.Click += EnableSvetlost_Click;
                EnablePadavine.button.Click += EnablePadavine_Click;
                EnableTzunanja.button.Click += EnableTzunanja_Click;
                EnableTnotranja.button.Click += EnableTnotranja_Click;
                                
                MainDiv.Controls.Add(EnableSvetlost);
                MainDiv.Controls.Add(EnablePadavine);
                MainDiv.Controls.Add(EnableTzunanja);
                MainDiv.Controls.Add(EnableTnotranja);
                MainDiv.Controls.Add(chartViewSelector);

                MainDiv.Controls.Add(EnableSvetlost_lbl);
                MainDiv.Controls.Add(EnablePadavine_lbl);
                MainDiv.Controls.Add(EnableTzunanja_lbl);
                MainDiv.Controls.Add(EnableTnotranja_lbl);
                MainDiv.Controls.Add(chartViewSelector);

                chartViewSelector.SaveClicked += ChartViewSelector_SaveClicked;

            }

            private void ChartViewSelector_SaveClicked(object sender, ImageClickEventArgs e, ListItem selectedItem)
            {
                var buff = chartViewSelector.GetSelectedValue();
                XmlController.SetShowChart(Convert.ToInt32(buff));
                Helper.Refresh();
            }

            private void EnableTnotranja_Click(object sender, ImageClickEventArgs e)
            {
                XmlController.SetEnableCharts_Tnotranja(!XmlController.GetEnableCharts_Tnotranja());
                Helper.Refresh();
            }

            private void EnableTzunanja_Click(object sender, ImageClickEventArgs e)
            {
                XmlController.SetEnableCharts_Tzunanja(!XmlController.GetEnableCharts_Tzunanja());
                Helper.Refresh();
            }

            private void EnablePadavine_Click(object sender, ImageClickEventArgs e)
            {
                XmlController.SetEnableCharts_Padavine(!XmlController.GetEnableCharts_Padavine());
                Helper.Refresh();
            }

            private void EnableSvetlost_Click(object sender, ImageClickEventArgs e)
            {
                XmlController.SetEnableCharts_Svetlost(!XmlController.GetEnableCharts_Svetlost());
                Helper.Refresh();
            }

            class ChartTweaker :Dsps
            {
                // Set Chart Properties
                readonly int top = 16;
                readonly int left = 0;
                readonly int width = 99;
                readonly int height = 81;

                Font ChartFont;
                readonly int leftSideChart_Steps = 10;
                string ID;

                // 

                public List<DataPoint> Points = new List<DataPoint>();

                Series series_Svetlost = new Series();
                Color color_Svetlost = Color.FromArgb(220,250, 185, 20);
                const string name_Svetlost = "Svetlost";

                Series series_Padavine = new Series();
                Color color_Padavine = Color.FromArgb(70, 5, 40, 200);
                const string name_Padavine = "Padavine";

                Series series_TZunanja = new Series();
                Color color_TZunanja = Color.FromArgb(220, 0, 0, 0);
                const string name_TZunanja = "TZunanja";

                Series series_TNotranja = new Series();
                Color color_TNotranja = Color.FromArgb(220, 90, 90, 90);
                const string name_TNotranja = "TNotranja";

                Chart ChartGraph = new Chart();
                Helper.UpdatePanelFull UpdatePanel;
                
                Page page;
                
                HtmlGenericControl MainDiv;




                public ChartTweaker(Page page, string chartID, HtmlGenericControl MainDiv, Helper.UpdatePanelFull UpdatePanel)
                {
                    this.page = page;                    
                    this.MainDiv = MainDiv;
                    this.UpdatePanel = UpdatePanel;
                    this.ID = chartID;
                    AddDataPoints();
                    Tweak();
                    Resize();
                    CreateLegend();
                    CreateUnitLabels();
                    
                }

                void CreateUnitLabels()
                {
                    var h = 18.8F;
                    var font = "1.2vw";

                    if (XmlController.GetEnableCharts_Svetlost())
                    {
                        Label Unit_Svetlost = new Label() { Text = "[%]", ForeColor = color_Svetlost};
                        Unit_Svetlost.Style.Add(HtmlTextWriterStyle.FontSize, font);
                        MainDiv.Controls.Add(Unit_Svetlost);
                        SetControlAbsolutePos(Unit_Svetlost, h, 5);
                    }

                    if (XmlController.GetEnableCharts_Padavine())
                    {
                        Label Unit_Padavine = new Label() { Text = "[mm/h]", ForeColor = color_Padavine };
                        Unit_Padavine.Style.Add(HtmlTextWriterStyle.FontSize, font);
                        MainDiv.Controls.Add(Unit_Padavine);
                        SetControlAbsolutePos(Unit_Padavine, h, 0);
                    }

                    if (XmlController.GetEnableCharts_Tnotranja() || XmlController.GetEnableCharts_Tzunanja())
                    {
                        Label Unit_T = new Label() { Text = "[°C]", ForeColor = color_TZunanja };
                        Unit_T.Style.Add(HtmlTextWriterStyle.FontSize, font);
                        MainDiv.Controls.Add(Unit_T);
                        SetControlAbsolutePos(Unit_T, h, 93);
                    }
                    
                }

                void CreateLegend()
                {
                    var legend = new Legend("Legenda");
                    LegendItem legenditem_Svetlost = new LegendItem(name_Svetlost, color_Svetlost, null);
                    LegendItem legenditem_Padavine = new LegendItem(name_Padavine, color_Padavine, null);
                    LegendItem legenditem_TZunanja = new LegendItem(name_TZunanja, color_TZunanja, null);
                    LegendItem legenditem_TNotranja = new LegendItem(name_TNotranja, color_TNotranja, null);

                    legend.Font = new Font(Settings.DefaultFont, 30);
                    legend.Position = new ElementPosition(30,2,40,8);
                    legend.BackColor = Color.FromArgb(0, Color.White);                    
                    ChartGraph.Legends.Add(legend);
                    
                }

                void Tweak()
                {
                    ChartGraph.ID = ID;

                    ChartGraph.Width = 4000;
                    ChartGraph.Height = 1500;

                    ChartGraph.AntiAliasing = AntiAliasingStyles.None;

                    ChartFont = new Font(Settings.DefaultFont, 30);                    

                    var chartarea = ChartGraph.ChartAreas.Add("ChartArea");
                    ChartGraph.ChartAreas[chartarea.Name].Position.Auto = true;
                    ChartGraph.ChartAreas[chartarea.Name].InnerPlotPosition.Auto = true;

                    ChartGraph.BackColor = Color.Transparent;

                    chartarea.Position = new ElementPosition(5, 5, 95, 90);
                    chartarea.InnerPlotPosition = new ElementPosition(3, 5, 90, 90);
                    chartarea.AxisX.LabelStyle.Font = ChartFont;


                    // svetlost
                    if (XmlController.GetEnableCharts_Svetlost())
                    {
                        ChartGraph.Series.Add(series_Svetlost);                        
                        var series = series_Svetlost;
                        series.IsXValueIndexed = true;
                        series.BorderWidth = 4;
                        series.Name = name_Svetlost; series_Svetlost.Color = color_Svetlost;
                        series.ChartType = SeriesChartType.Spline;
                        series.SetCustomProperty("LineTension", "0.1");
                        series.ChartArea = chartarea.Name;
                        series.YAxisType = AxisType.Primary;
                        chartarea.AxisY.LabelStyle.Font = ChartFont;
                        chartarea.AxisY.Minimum = 0;
                        chartarea.AxisY.Maximum = 100;
                        chartarea.AxisY.Interval = CalculateInterval(chartarea.AxisY.Minimum, chartarea.AxisY.Maximum, leftSideChart_Steps);
                        chartarea.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
                        chartarea.BackColor = Color.Transparent;
                        chartarea.AxisY.LabelStyle.ForeColor = DarkenColor(color_Svetlost, 50);


                    }
                    //Padavine
                    if (XmlController.GetEnableCharts_Padavine())
                    {
                        ChartGraph.Series.Add(series_Padavine);
                        var series = series_Padavine;
                        series.IsXValueIndexed = true;
                        series.BorderWidth = 1;
                        series.Name = name_Padavine; series_Padavine.Color = color_Padavine;
                        series.ChartType = SeriesChartType.SplineArea;
                        series.SetCustomProperty("LineTension", "0.3");
                        series.ChartArea = chartarea.Name;
                        series.YAxisType = AxisType.Primary;                      
                        chartarea.BackColor = Color.Transparent;
                        CreateYAxis(chartarea, series, 4, 0); // dodatna skala za Y na skrajni levi
                    }
                    //Temperatura zunaj
                    if (XmlController.GetEnableCharts_Tzunanja())
                    {
                        ChartGraph.Series.Add(series_TZunanja);
                        var series = series_TZunanja;
                        series.IsXValueIndexed = true;
                        series.BorderWidth = 8;
                        series.Name = name_TZunanja; series_TZunanja.Color = color_TZunanja;
                        series.ChartType = SeriesChartType.Spline;
                        series.SetCustomProperty("LineTension", "0.3");
                        series.ChartArea = chartarea.Name;
                        chartarea.AxisY2.LabelStyle.Font = ChartFont;
                        series.YAxisType = AxisType.Secondary; // skala na desni strani
                        chartarea.AxisY2.Minimum = -20;
                        chartarea.AxisY2.Maximum = 30;
                        chartarea.AxisY2.Interval = CalculateInterval(chartarea.AxisY2.Minimum, chartarea.AxisY2.Maximum, leftSideChart_Steps);
                        chartarea.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;
                        chartarea.BackColor = Color.Transparent;
                    }
                    // temperatura znotraj
                    if (XmlController.GetEnableCharts_Tnotranja())
                    {
                        ChartGraph.Series.Add(series_TNotranja);
                        var series = series_TNotranja;
                        series.IsXValueIndexed = true;
                        series.BorderWidth = 8;
                        series.Name = name_TNotranja; series_TNotranja.Color = color_TNotranja;
                        series.ChartType = SeriesChartType.Spline;
                        series.SetCustomProperty("LineTension", "0.3");
                        series.ChartArea = chartarea.Name;
                        chartarea.AxisY2.LabelStyle.Font = ChartFont;
                        series.YAxisType = AxisType.Secondary; // skala na desni strani
                        chartarea.AxisY2.Minimum = -20;
                        chartarea.AxisY2.Maximum = 30;
                        chartarea.AxisY2.Interval = CalculateInterval(chartarea.AxisY2.Minimum, chartarea.AxisY2.Maximum, leftSideChart_Steps);
                        chartarea.AxisY2.IntervalAutoMode = IntervalAutoMode.FixedCount;
                        chartarea.BackColor = Color.Transparent;

                    }

                    UpdatePanel.Controls_Add(ChartGraph);
                    MainDiv.Controls.Add(UpdatePanel);
                    
                }

                public void CreateYAxis(ChartArea area, Series series, float axisOffset, float labelsSize)
                {
                    // Create new chart area for original series
                    ChartArea areaSeries = ChartGraph.ChartAreas.Add("ChartArea_" + series.Name);
                    areaSeries.BackColor = Color.Transparent;
                    areaSeries.BorderColor = Color.Transparent;
                    areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
                    areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
                    areaSeries.AxisX.MajorGrid.Enabled = false;
                    areaSeries.AxisX.MajorTickMark.Enabled = false;
                    areaSeries.AxisX.LabelStyle.Enabled = false;
                    areaSeries.AxisY.MajorGrid.Enabled = false;
                    areaSeries.AxisY.MajorTickMark.Enabled = false;
                    areaSeries.AxisY.LabelStyle.Enabled = false;
                    
                    series.ChartArea = areaSeries.Name;

                    // Create new chart area for axis
                    ChartArea areaAxis = ChartGraph.ChartAreas.Add("AxisY_" + series.ChartArea);
                    areaAxis.BackColor = Color.Transparent;
                    areaAxis.BorderColor = Color.Transparent;
                    areaAxis.Position.FromRectangleF(ChartGraph.ChartAreas[series.ChartArea].Position.ToRectangleF());
                    areaAxis.InnerPlotPosition.FromRectangleF(ChartGraph.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

                    // Create a copy of specified series
                    Series seriesCopy = ChartGraph.Series.Add(series.Name + "_Copy");
                    seriesCopy.ChartType = series.ChartType;
                    
                    foreach (DataPoint point in series.Points)
                    {
                        seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
                    }

                    // Hide copied series
                    seriesCopy.IsVisibleInLegend = false;
                    seriesCopy.Color = Color.Transparent;
                    seriesCopy.BorderColor = Color.Transparent;
                    seriesCopy.ChartArea = areaAxis.Name;                    

                    // Disable drid lines & tickmarks
                    areaAxis.AxisX.LineWidth = 0;
                    areaAxis.AxisX.MajorGrid.Enabled = false;
                    areaAxis.AxisX.MajorTickMark.Enabled = false;
                    areaAxis.AxisX.LabelStyle.Enabled = false;
                    areaAxis.AxisY.MajorGrid.Enabled = false;
                    areaAxis.AxisY.LabelStyle.Font = ChartFont;
                    areaAxis.AxisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
                    areaAxis.AxisY.Interval = 10;
                    areaAxis.AxisY.Minimum = 0;
                    areaAxis.AxisY.Maximum = 160;
                    areaAxis.AxisY.Interval = CalculateInterval(areaAxis.AxisY.Minimum, areaAxis.AxisY.Maximum, leftSideChart_Steps);
                    areaAxis.AxisY.LabelStyle.ForeColor = DarkenColor(color_Padavine, 20);

                    // Adjust area position
                    areaAxis.Position.X -= axisOffset;
                    areaAxis.InnerPlotPosition.X += labelsSize;
                }

                int CalculateInterval(double min, double max, int steps)
                {
                    var dif = max - min;
                    var interval = Misc.ToInt( dif / steps);
                    return interval;
                }

                void AddDataPoints()
                {
                    ChartValues.ChartData buff = null;

                    if (Val.ChartValues != null)
                    {
                        buff = Val.ChartValues.GetDataForChart();
                    }
                    

                    if (buff!=null)
                    {
                        try
                        {                          
                            series_Svetlost.Points.DataBindXY(buff.datetimes, buff.Svetlost);
                            series_Padavine.Points.DataBindXY(buff.datetimes, buff.padavineH);
                            series_TZunanja.Points.DataBindXY(buff.datetimes, buff.Tzunanja);
                            series_TNotranja.Points.DataBindXY(buff.datetimes, buff.Tnotranja);
                        }
                        catch (Exception ex)
                        {
                            SysLog.Message.SetMessage("Creating points in chart failed (databind failed in method: AddPoints()). Error info: " + ex.Message);
                        }
                        
                    }
                    
                }                

                void Resize()
                {
                    SetControlAbsolutePos(ChartGraph, top, left, width, height);
                }
                                
                void SetTimersAsyncToPreventFlicker()
                {
                    // to prevent flicker charts are updated at different times. charts are overlapping

                    try
                    {
                        var page = Val.guiController.PagePadavine_.thisPage;
                        var tmr1 = page.FindControl("TimerUpdtPnl1");
                        var tmr2 = page.FindControl("TimerUpdtPnl2");

                        if (tmr1 != null)
                        {
                            if (tmr2 != null)
                            {
                                var timer1 = (Timer)tmr1;
                                var timer2 = (Timer)tmr2;

                                timer1.Interval = Settings.ChartUpdateRefreshRate;
                                timer2.Interval = Settings.ChartUpdateRefreshRate - 100; 
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Charts and/or their timers could not be found. Please use same ID trougout code." + ex.Message);
                    }
                    
                }

                Color DarkenColor(Color c, int darkenFor)
                {
                    int r, g, b;

                   r = c.R <= darkenFor ? 0: c.R - darkenFor; // darken component for specified value
                   g = c.G <= darkenFor ? 0: c.G - darkenFor;
                   b = c.B <= darkenFor ? 0: c.B - darkenFor;

                    return Color.FromArgb(255, r,g,b);
                }
            }

        }
    }
}