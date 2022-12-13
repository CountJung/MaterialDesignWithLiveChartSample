using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.ViewModel;
using System.Collections.ObjectModel;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class PieChartNode : ViewModelBase
    {
        private SeriesCollection? pieChartSeries;
        public SeriesCollection? PieChartSeries { get => pieChartSeries; set => Set(ref pieChartSeries, value, nameof(PieChartSeries)); }
        private string? pieChartName;
        public string? PieChartName { get => pieChartName; set => Set(ref pieChartName, value, nameof(PieChartName)); }
        public PieChartNode(SeriesCollection? pieChartSeries, string? pieChartName)
        {
            PieChartSeries = pieChartSeries;
            PieChartName = pieChartName;
        }
    }
    public class PieChartDisplayModel : ViewModelBase
    {
        private uint? piechartCount;
        public uint? PiechartCount
        {
            get => piechartCount;
            set => Set(ref piechartCount, value > 50 ? 50 : value, nameof(PiechartCount));
        }
        /// <summary>
        /// To avoid Binding memoryleaks by fault.
        /// 1. apply INotifyPropertyChanged
        /// 2. OneTime or OneWayToSource binding on XAML
        /// 3. Use System.Windows.Data.BindingOperations.ClearBinding explicitly -> testing
        /// </summary>
        private ObservableCollection<PieChartNode>? pieChartData;
        public ObservableCollection<PieChartNode>? PieChartData
        { get => pieChartData; set => Set(ref pieChartData, value, nameof(PieChartData)); /* pieChartData = value; */}

        public PieChartDisplayModel()
        {
            PieChartData = new ObservableCollection<PieChartNode>();
            for (int i = 0; i < 5; i++)
            {
                string nodeName = string.Format("Pie Chart {0}", i);
                PieChartData.Add(new PieChartNode(GetPieChartSeriesCollection(), nodeName));
            }
        }
        public SeriesCollection GetPieChartSeriesCollection() => new()
        {
            new PieSeries
            {
                Title = "Chrome",
                Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Mozilla",
                Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Opera",
                Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
                DataLabels = true
            },
            new PieSeries
            {
                Title = "Explorer",
                Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
                DataLabels = true
            }
        };
    }
}
