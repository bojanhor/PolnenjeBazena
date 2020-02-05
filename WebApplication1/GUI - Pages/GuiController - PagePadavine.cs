using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.DataVisualization.Charting;
using System;

namespace WebApplication1
{
    public partial class GuiController
    {
        public class PagePadavine
        {
            Page thisPage;

            public HtmlGenericControl MainDiv;
                        
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
                MainDiv = DIV.CreateDivAbsolute(0, 0, 1, 1, "%");

                ChartTweaker ct = new ChartTweaker(thisPage, "Chart1");

                
               
            }

            void AddControls()
            {
                
            }

            class ChartTweaker
            {
                // Set Chart Properties
                const int top = 20;
                const int left = 5;
                const int width = 90;
                const int height = 70;

                // 

                public List<DataPoint> Points = new List<DataPoint>();
                Series series = new Series();


                Control Chart_control;
                public Chart ChartGraph;
                Page page;
                string chartID;

                public ChartTweaker(Page page, string chartID)
                {
                    this.page = page;
                    this.chartID = chartID;

                    FindChart();
                    Tweak();
                    Resize();
                                        
                }

                void Tweak()
                {
                    Points.Add(new DataPoint(1,5)); Points.Add(new DataPoint(2, 6)); Points.Add(new DataPoint(3, 7)); Points.Add(new DataPoint(4, 8)); Points.Add(new DataPoint(5, 6));
                    Points.Add(new DataPoint(6, 4)); Points.Add(new DataPoint(7, 1)); Points.Add(new DataPoint(8, 3)); Points.Add(new DataPoint(9, 10)); Points.Add(new DataPoint(10, 8));

                    series.ChartType = SeriesChartType.Line;
                    series.AxisLabel = "axisLable1";
                    series.Name = "Series1";
                    AddPoints();
                    ChartGraph.Series.Add(series);
                }

                void AddPoints()
                {
                    foreach (var item in Points)
                    {
                        series.Points.Add(item);
                    }                                      
                }

                void FindChart()
                {
                    try
                    {
                        Chart_control = page.FindControl(chartID);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ChartTweaker could not find chart with ID: " + chartID + " on your page. Please provide valid ID. Error Info: " + ex.Message);
                    }

                    try
                    {
                        ChartGraph = (Chart)Chart_control;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Control with ID: " + chartID + " was found, but is not of type Chart. Please provide valid Control of type Chart. Error Info: " + ex.Message);
                    }
                }

                void Resize()
                {
                    SetControlAbsolutePos(ChartGraph, top, left, width, height);
                }
            }

        }
    }
}