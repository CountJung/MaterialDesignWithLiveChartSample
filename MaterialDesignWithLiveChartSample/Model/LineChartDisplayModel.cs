using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class LineChartDisplayModel
    {
        public SeriesCollection? SeriesCollection { get; set; }
        public string[]? Labels { get; set; }
        public Func<double, string>? Formatter { get; set; }
        public LineChartDisplayModel()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "2015", /*LineSmoothness=0, PointGeometrySize=0, Fill=Brushes.Transparent,*/
                    Values = new ChartValues<double> { 10, 50, 39, 50 }
                },
                //adding series will update and animate the chart automatically
                new LineSeries
                {
                    Title = "2016", /*LineSmoothness=0, PointGeometrySize=0, Fill=Brushes.Transparent,*/
                    Values = new ChartValues<double> { 11, 56, 42 }
                }
            };

            //also adding values updates and animates the chart automatically
            SeriesCollection[1].Values.Add(48d);

            Labels = new[] { "Maria", "Susan", "Charles", "Frida" };
            //value format
            //https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings?redirectedfrom=MSDN
            Formatter = value => value.ToString("N");
        }
    }
}
