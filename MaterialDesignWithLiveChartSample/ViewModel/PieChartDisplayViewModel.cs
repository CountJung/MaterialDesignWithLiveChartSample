using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.Model;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class PieChartDisplayViewModel : ViewModelBase
    {
        public static PieChartDisplayViewModel? Instance { get; private set; }
        //private PieChartDisplayModel? model;
        //public PieChartDisplayModel? Model { get => model; private set => Set(ref model, value, nameof(Model)); }
        public PieChartDisplayViewModel()
        {
            Instance = this;
            //Model = new PieChartDisplayModel();
            PieChartCountSetCmd = new DelegateCommand(PieChartCountSet);
            //TestCommandBtnCmd = new DelegateCommand(TestCommandBtn);
            AddChartDataCmd = new DelegateCommand(AddChartData);
            RemoveChartDataCmd = new DelegateCommand(RemoveChartData);
            PieChartAsset = new ObservableCollection<PieChartNode>();
            for (uint i = 0; i < 5; i++)
            {
                PieChartAsset.Add(new PieChartNode(i));
            }
        }
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
        /// <summary>
        /// Creating Charts without Closing parent window causes memory leak
        /// https://blog.jetbrains.com/dotnet/2014/09/04/fighting-common-wpf-memory-leaks-with-dotmemory/
        /// </summary>
        public ICommand PieChartCountSetCmd { get; private set; }
        public void PieChartCountSet(object s)
        {
            uint chartCount = PieChartCount ?? 0;
            int currentChartCount = PieChartAsset?.Count ?? 0;
            for (uint i = (uint)currentChartCount; i < chartCount; i++)
            {
                PieChartAsset?.Add(new PieChartNode(i));
            }
            for (int i = 0; i < currentChartCount; i++)
            {
                PieChartNode pieNode = PieChartAsset?[i]!;
                pieNode.ChartVisible = i < chartCount;
            }
        }
        public ICommand AddChartDataCmd { get; private set; }
        public void AddChartData(object s)
        {
            PieChartNode pieNode = PieChartAsset?[CurrentChartNumber ?? 0]!;
            PieSeries pieSeries = new ()
            {
                Title = "AddData",
                Values = new ChartValues<ObservableValue> { new ObservableValue((double)(PieChartData ?? 1)) },
                DataLabels = true
            };
            pieNode.PieChartSeries?.Add(pieSeries);
        }
        public ICommand RemoveChartDataCmd { get; private set; }
        public void RemoveChartData(object s)
        {
            PieChartNode pieNode = PieChartAsset?[CurrentChartNumber ?? 0]!;
            pieNode.PieChartSeries?.RemoveAt(pieNode.PieChartSeries.Count - 1);
        }
        public static void RemoveAll(IList list)
        {
            while (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
        //public ICommand TestCommandBtnCmd { get; private set; }
        //public void TestCommandBtn(object s)
        //{
        //    // memory leak test
        //    BindingOperations.ClearBinding(PieChartDisplay.Instance?.PieChartCollectionDisplay, ItemsControl.ItemsSourceProperty);
        //    Binding binding = new()
        //    {
        //        Path = new PropertyPath("ItemsSource"),
        //        Source = Model?.PieChartAsset
        //    };
        //    BindingOperations.SetBinding(PieChartDisplay.Instance?.PieChartCollectionDisplay, ItemsControl.ItemsSourceProperty, binding);
        //}
    }
}
