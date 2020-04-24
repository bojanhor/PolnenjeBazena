using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public class Datasourcer
    {
        public class Datasource : List<ListItem>
        {
            new public void Add(ListItem item)
            {
                base.Add(item);
            }

            public static int? GetValueFromText_int(string Text)
            {
                if (Text != null && Text != PropComm.NA)
                {
                    return Convert.ToInt32(GetNumbers(Text));
                }
                return null;
            }

            public static short? GetValueFromText_short(string Text)
            {
                if (Text != null && Text != PropComm.NA)
                {
                    return Convert.ToInt16(GetNumbers(Text));
                }
                return null;
            }

            static string GetNumbers(string input)
            {
                return new string(input.Where(c => char.IsDigit(c)).ToArray());
            }

        }

        public class TimeSelectorDatasource : Datasource
        {
            static ListItem buff;

            static string[] mins = { "00", "15", "30", "45" };
            static string[] hrs = { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };


            public TimeSelectorDatasource()
            {
                Add(new ListItem(PropComm.NA, PropComm.NA));
                foreach (var item in hrs)
                {
                    foreach (var it in mins)
                    {
                        buff = new ListItem(item + ":" + it);
                        Add(buff);
                    }
                }

                Add(new ListItem("23:59", "23:59"));
            }

        }

        public class ChartViewSelectorDatasource : Datasource
        {
            static string[] values;

            public ChartViewSelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(10);
            }

            public void GetDatasource(int increment)
            {
                values = new string[]
                {
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._Today),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._1DayAgo),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._2DaysAgo),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._3DaysAgo),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._ThisWeek),
                    ChartValues.ChartData.GetTextFromEnum(ChartValues.ChartData.ShowChartEnum._ThisMonth)

                };


                for (int i = 0; i < values.Length; i++)
                {
                    CreateRow(values[i], i + ""); // Please make sure values are synchronised With enum: ChartData.ShowChartEnum
                }

            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem
                {
                    Text = text,
                    Value = value
                };
                Add(r); // adds to base class
                return r;
            }
        }

        public class DimmerSelectorDatasource : Datasource
        {
            static string[] percents;

            public DimmerSelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(10);
            }

            public void GetDatasource(int increment)
            {

                percents = new string[100 / increment];

                CreateRow(PropComm.NA, "0");

                var buff = 0;

                for (int i = 0; i < percents.Length + 1; i++)
                {
                    CreateRow(buff + "%", buff.ToString());
                    buff += increment;
                }

            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem
                {
                    Text = text,
                    Value = value
                };
                Add(r);// adds to base class
                return r;
            }
        }

        public class TimerSelectorDatasource : Datasource
        {
            public TimerSelectorDatasource(int from_seconds, int to_seconds, float increment, string unit)
            {
                GetDatasource(from_seconds, from_seconds, increment, unit);
            }

            public void GetDatasource(int from_seconds, int to_seconds, float increment, string unit)
            {
                var range = to_seconds - from_seconds;
                float valBuff;
                string txtBuff;
                float buff;

                valBuff = from_seconds;
                txtBuff = valBuff + unit;

                CreateRow(txtBuff, valBuff.ToString());

                for (int i = 1; i < 100; i++) // protection from  to many steps
                {
                    buff = from_seconds + increment;
                    if (buff > to_seconds)
                    {
                        break;
                    }

                    valBuff = buff;
                    txtBuff = buff + unit;

                    CreateRow(txtBuff, valBuff.ToString());
                }

            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem(text, value);
                Add(r);// adds to base class
                return r;
            }
        }

        public class HisteresisSelectorDatasource : Datasource
        {
            static string[] units;

            public HisteresisSelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(1);
            }

            public void GetDatasource(int increment)
            {

                try
                {
                    units = new string[5 / increment];
                }
                catch (Exception)
                {
                    units = new string[5 / increment];
                }

                CreateRow(PropComm.NA, "0");

                var buff = 1;

                for (int i = 0; i < units.Length; i++)
                {
                    CreateRow(buff + "°C", buff.ToString());
                    buff += increment;
                }


            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem(text, value);
                Add(r);
                return r;
            }
        }

        public class Temperature_10_30_SelectorDatasource : Datasource
        {
            static string[] units;

            public Temperature_10_30_SelectorDatasource()
            {
                GetDatasource();
            }

            public void GetDatasource()
            {
                GetDatasource(2);
            }

            public void GetDatasource(int increment)
            {

                try
                {
                    units = new string[22 / increment];
                }
                catch (Exception)
                {
                    units = new string[22 / increment];
                }

                CreateRow(PropComm.NA, "0");

                var buff = 10;

                for (int i = 0; i < units.Length; i++)
                {
                    CreateRow(buff + "°C", buff.ToString());
                    buff += increment;
                }


            }

            ListItem CreateRow(string text, string value)
            {
                ListItem r = new ListItem(text, value);
                Add(r);
                return r;
            }
        }

        public class YesNoSelectorDatasource : Datasource
        {
            public YesNoSelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, false);
                CreateRow("DA", true);
                CreateRow("NE", false);

            }

            ListItem CreateRow(string text, bool value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class RocnoAvtoSelectorDatasource : Datasource
        {
            public RocnoAvtoSelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, 0);
                CreateRow("Ročno", 1);
                CreateRow("Avtomatsko", 0);

            }

            ListItem CreateRow(string text, short value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class OnOffSelectorDatasource : Datasource
        {
            public OnOffSelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, false);
                CreateRow("VKLOP", true);
                CreateRow("IZKLOP", false);

            }

            ListItem CreateRow(string text, bool value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class Rocno0Rocno1AvtoDatasource : Datasource
        {
            public Rocno0Rocno1AvtoDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, 0);
                CreateRow("Avtomatsko", 0);
                CreateRow("IZKLOP", 1);
                CreateRow("VKLOP", 2);

            }

            ListItem CreateRow(string text, ushort value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }

        public class WeekDaySelectorDatasource : Datasource
        {
            public WeekDaySelectorDatasource()
            {
                GetDataSource();
            }
            public void GetDataSource()
            {
                CreateRow(PropComm.NA, 0);
                CreateRow("IZKLOP", 0);
                CreateRow("Ponedeljek", 512);
                CreateRow("Torek", 1024);
                CreateRow("Sreda", 2048);
                CreateRow("Četrtek", 4096);
                CreateRow("Petek", 8192);
                CreateRow("Sobota", 16384);
                CreateRow("Nedelja", 256);

            }

            ListItem CreateRow(string text, ushort value)
            {
                ListItem r = new ListItem(text, value.ToString());
                Add(r);
                return r;
            }
        }
    }
}