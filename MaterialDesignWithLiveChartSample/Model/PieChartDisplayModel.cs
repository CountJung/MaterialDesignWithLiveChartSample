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
        public bool? ChartVisible { get => chartVisible; set => Set(ref chartVisible, value, nameof(ChartVisible)); }
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
        /// <summary>
        /// To avoid Binding memoryleaks by fault.
        /// 1. apply INotifyPropertyChanged
        /// 2. OneTime or OneWayToSource binding on XAML
        /// 3. Use System.Windows.Data.BindingOperations.ClearBinding explicitly -> testing
        /// </summary>
        private ObservableCollection<PieChartNode>? pieChartAsset;
        public ObservableCollection<PieChartNode>? PieChartAsset
        { get => pieChartAsset; set => Set(ref pieChartAsset, value, nameof(PieChartAsset)); }

        private uint? pieChartCount;
        public uint? PieChartCount
        {
            get => pieChartCount;
            set => Set(ref pieChartCount, value > 50 ? 50 : value, nameof(PieChartCount));
        }
        private int? currentChartNumber;
        public int? CurrentChartNumber
        {
            get => currentChartNumber;
            set
            {
                if (value < pieChartAsset?.Count)
                {
                    Set(ref currentChartNumber, value, nameof(CurrentChartNumber));
                    CurrentChartVisibility = PieChartAsset?[CurrentChartNumber ?? 0].ChartVisible;
                }
            }
        }
        private bool? currentChartVisibility;
        public bool? CurrentChartVisibility
        {
            get
            {
                currentChartVisibility = PieChartAsset?[CurrentChartNumber ?? 0].ChartVisible;
                return currentChartVisibility;
            }
            set
            {
                PieChartNode node = PieChartAsset?[CurrentChartNumber ?? 0]!;
                node.ChartVisible = value;
                Set(ref currentChartVisibility, value, nameof(CurrentChartVisibility));
            }
        }
        private double? pieChartData;
        public double? PieChartData { get => pieChartData; set => Set(ref pieChartData, value, nameof(PieChartData)); }
        public PieChartDisplayModel()
        {
            PieChartAsset = new ObservableCollection<PieChartNode>();
            for (uint i = 0; i < 5; i++)
            {
                //string nodeName = string.Format("Pie Chart {0}", i);
                PieChartAsset.Add(new PieChartNode(i));
            }
        }

    }
}
