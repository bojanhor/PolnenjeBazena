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
                const int top = 10;
                const int left = 10;
                const int width = 80;
                const int height = 70;

                // 

                Control Chart_control;
                Chart ChartGraph;
                Page page;
                string chartID;

                public ChartTweaker(Page page, string chartID)
                {
                    this.page = page;
                    this.chartID = chartID;

                    FindChart();

                    Resize();
                                        
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