using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.ViewModel;
using System.Collections.ObjectModel;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class PieChartNode : ViewModelBase
    {
        private ObservableCollection<ChartValues<ObservableValue>>? controlValue;
        public ObservableCollection<ChartValues<ObservableValue>>? ControlValue { get => controlValue; set => Set(ref controlValue, value, nameof(ControlValue)); }
        private SeriesCollection? pieChartSeries;
        public SeriesCollection? PieChartSeries { get => pieChartSeries; set => Set(ref pieChartSeries, value, nameof(PieChartSeries)); }
        private string? pieChartName;
        public string? PieChartName { get => pieChartName; set => Set(ref pieChartName, value, nameof(PieChartName)); }
        private bool? chartVisible;
        public bool? ChartVisible { get => chartVisible; set=>Set(ref chartVisible, value, nameof(ChartVisible)); }
        public PieChartNode(uint index)
        {
            //PieChartSeries = pieChartSeries;
            //PieChartName = pieChartName;
            InitSamplePieChart(index);
        }
        private void InitSamplePieChart(uint index)
        {
            ChartVisible = true;
            string nodeName = string.Format("Pie Chart {0}", index);
            PieChartName = nodeName;
            ControlValue = new ObservableCollection<ChartValues<ObservableValue>>
            {
                new ChartValues<ObservableValue> { new ObservableValue(8) },
                new ChartValues<ObservableValue> { new ObservableValue(6) },
                new ChartValues<ObservableValue> { new ObservableValue(10) },
                new ChartValues<ObservableValue> { new ObservableValue(4) }
            };
            PieChartSeries = new SeriesCollection
            {
                new PieSeries
                {
                     Title = "Chrome",
                     Values = ControlValue[0],
                     DataLabels = true
                },
                new PieSeries
                {
                    Title = "Mozilla",
                    Values = ControlValue[1],
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Opera",
                    Values = ControlValue[2],
                    DataLabels = true
                },
                new PieSeries
                {
                    Title = "Explorer",
                    Values = ControlValue[3],
                    DataLabels = true
                }
            };
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
            for (uint i = 0; i < 5; i++)
            {
                //string nodeName = string.Format("Pie Chart {0}", i);
                PieChartData.Add(new PieChartNode(i));
            }
        }
        //public SeriesCollection GetPieChartSeriesCollection() => new()
        //{
        //    new PieSeries
        //    {
        //        Title = "Chrome",
        //        Values = new ChartValues<ObservableValue> { new ObservableValue(8) },
        //        DataLabels = true
        //    },
        //    new PieSeries
        //    {
        //        Title = "Mozilla",
        //        Values = new ChartValues<ObservableValue> { new ObservableValue(6) },
        //        DataLabels = true
        //    },
        //    new PieSeries
        //    {
        //        Title = "Opera",
        //        Values = new ChartValues<ObservableValue> { new ObservableValue(10) },
        //        DataLabels = true
        //    },
        //    new PieSeries
        //    {
        //        Title = "Explorer",
        //        Values = new ChartValues<ObservableValue> { new ObservableValue(4) },
        //        DataLabels = true
        //    }
        //};
    }
}
