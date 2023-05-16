using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.Model;
using System;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class LineChartDisplayViewModel : ViewModelBase
    {
        public static LineChartDisplayViewModel? Instance { get; private set; }
        //public LineChartDisplayModel? Model { get; private set; }
        public LineChartDisplayViewModel()
        {
            Instance = this;
            //Model = new LineChartDisplayModel();
            FirstdataClickCmd = new DelegateCommand(FirstdataClick);
            ResetdataClickCmd = new DelegateCommand(ResetdataClick);

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
            FormatterAxisY = value => value.ToString("N");
            FormatterAxisX = value => DateTime.Now.ToString();
        }
        public SeriesCollection? SeriesCollection { get; set; }
        public string[]? Labels { get; set; }
        public Func<double, string>? FormatterAxisY { get; set; }
        public Func<double, string>? FormatterAxisX { get; set; }
        private double? firstLineData;
        public double? FirstLineData { get => firstLineData; set => Set(ref firstLineData, value, nameof(FirstLineData)); }
        private double? secondLineData;
        public double? SecondLineData { get => secondLineData; set => Set(ref secondLineData, value, nameof(SecondLineData)); }

        public ICommand? FirstdataClickCmd { get; private set; }
        public void FirstdataClick(object sender)
        {
            if (SeriesCollection?.Count < 2)
            {
                SeriesCollection.Add(new LineSeries { Values = new ChartValues<double> { 10, 50, 39, 50 } });
                SeriesCollection.Add(new LineSeries { Values = new ChartValues<double> { 11, 56, 42, 48 } });
            }
            SeriesCollection?[0].Values.Add(FirstLineData ?? 0);
            SeriesCollection?[1].Values.Add(SecondLineData ?? 0);
        }
        public ICommand? ResetdataClickCmd { get; private set; }
        public void ResetdataClick(object sender)
        {
            SeriesCollection?.Clear();
        }
    }
}
