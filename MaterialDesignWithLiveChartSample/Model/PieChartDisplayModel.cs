using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.ViewModel;
using System.Collections.ObjectModel;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class PieChartNode
    {
        public SeriesCollection? PieChartSeries { get; set; }
        public string? PieChartName { get; set; }
        public PieChartNode(SeriesCollection? pieChartSeries, string? pieChartName)
        {
            PieChartSeries = pieChartSeries;
            PieChartName = pieChartName;
        }
    }
    public class PieChartDisplayModel : ViewModelBase
    {
        private SeriesCollection? pieChartSeriesCollection;
        public SeriesCollection? PieChartSeriesCollection
        { get => pieChartSeriesCollection; set => pieChartSeriesCollection = value; }

        private ObservableCollection<PieChartNode>? pieChartNode;
        public ObservableCollection<PieChartNode>? PieChartnode
        { get => pieChartNode; set => pieChartNode = value; }
        public PieChartDisplayModel()
        {
            PieChartnode = new ObservableCollection<PieChartNode>();
            for (int i = 0; i < 5; i++)
            {
                string nodeName = string.Format("Pie Chart {0}", i);
                PieChartnode.Add(new PieChartNode(GetPieChartSeriesCollection(), nodeName));
            }

        }
        private SeriesCollection GetPieChartSeriesCollection() => new()
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
