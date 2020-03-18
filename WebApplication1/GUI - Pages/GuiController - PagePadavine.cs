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

                double[] ypnts;
                string[] adt;

                private int fontSize;
                public int FontSize
                {
                    get { return fontSize; }
                    set {
                        fontSize = value;
                        SetFont(value);
                    }
                }

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
                    var dtn = DateTime.Now;
                    var dt1 = dtn.AddMinutes(1);
                    var dt2 = dt1.AddMinutes(1);
                    var dt3 = dt2.AddMinutes(1);
                    var dt4 = dt3.AddMinutes(1);
                    var dt5 = dt4.AddMinutes(1);
                    var dt6 = dt5.AddMinutes(1);
                    var dt7 = dt6.AddMinutes(1);
                    var dt8 = dt7.AddMinutes(1);
                    var dt9 = dt8.AddMinutes(1);
                    var dt10 = dt9.AddMinutes(1);

                    adt = new string[11];
                    adt[0] = dtn.ToString("dd.MM  hh:mm");
                    adt[1] = dt1.ToString("dd.MM  hh:mm");
                    adt[2] = dt2.ToString("dd.MM  hh:mm");
                    adt[3] = dt3.ToString("dd.MM  hh:mm");
                    adt[4] = dt4.ToString("dd.MM  hh:mm");
                    adt[5] = dt5.ToString("dd.MM  hh:mm");
                    adt[6] = dt6.ToString("dd.MM  hh:mm");
                    adt[7] = dt7.ToString("dd.MM  hh:mm");
                    adt[8] = dt8.ToString("dd.MM  hh:mm");
                    adt[9] = dt9.ToString("dd.MM  hh:mm");
                    adt[10] = dt10.ToString("dd.MM  hh:mm");

                    ypnts = new double[11];
                    ypnts[0] = 3;
                    ypnts[1] = 4;
                    ypnts[2] = 5;
                    ypnts[3] = 8;
                    ypnts[4] = 8;
                    ypnts[5] = 4;
                    ypnts[6] = 5;
                    ypnts[7] = 3;
                    ypnts[8] = 1;
                    ypnts[9] = 2;
                    ypnts[10] = 3;



                    series.ChartType = SeriesChartType.Line;
             
                    series.Name = "Series1";
                    series.BorderWidth = 8;
                    
                    series.IsXValueIndexed = true;
                    FontSize = 20;
                                        
                    AddPoints();
                    ChartGraph.Series.Add(series);
                    
                }

               
                void AddPoints()
                {
                    series.Points.DataBindXY(adt, ypnts);                                   
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

                void SetFont(float _fontsize)
                {
                    if (ChartGraph != null)
                    {
                        if (ChartGraph.ChartAreas[0] != null)
                        {
                            ChartGraph.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font(Settings.DefaultFont, _fontsize);
                            ChartGraph.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font(Settings.DefaultFont, _fontsize);
                        }

                    }
                }
            }

        }
    }
}