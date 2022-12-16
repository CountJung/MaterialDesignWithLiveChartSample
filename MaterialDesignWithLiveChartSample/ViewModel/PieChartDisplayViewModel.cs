using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.Model;
using System.Collections;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class PieChartDisplayViewModel : ViewModelBase
    {
        public static PieChartDisplayViewModel? Instance { get; private set; }
        private PieChartDisplayModel? model;
        public PieChartDisplayModel? Model { get => model; private set => Set(ref model, value, nameof(Model)); }
        public PieChartDisplayViewModel()
        {
            Instance = this;
            Model = new PieChartDisplayModel();
            PieChartCountSetCmd = new DelegateCommand(PieChartCountSet);
            //TestCommandBtnCmd = new DelegateCommand(TestCommandBtn);
            AddChartDataCmd = new DelegateCommand(AddChartData);
            RemoveChartDataCmd = new DelegateCommand(RemoveChartData);
        }
        /// <summary>
        /// Creating Charts without Closing parent window causes memory leak
        /// https://blog.jetbrains.com/dotnet/2014/09/04/fighting-common-wpf-memory-leaks-with-dotmemory/
        /// </summary>
        public ICommand PieChartCountSetCmd { get; private set; }
        public void PieChartCountSet(object s)
        {
            //foreach (PieChartNode pies in Model?.PieChartData ?? Enumerable.Empty<PieChartNode>())
            //{
            //    for(int i=0;i< pies.ControlValue?.Count; i++)
            //    {
            //        pies.ControlValue[i].Clear();
            //    }
            //    pies.ControlValue?.Clear();
            //    pies.PieChartSeries?.Clear();
            //}
            //Model?.PieChartData?.Clear();

            uint chartCount = Model?.PieChartCount ?? 0;
            int currentChartCount = model?.PieChartAsset?.Count ?? 0;
            for (uint i = (uint)currentChartCount; i < chartCount; i++)
            {
                Model?.PieChartAsset?.Add(new PieChartNode(i));
            }
            for (int i = 0; i < currentChartCount; i++)
            {
                PieChartNode pieNode = Model?.PieChartAsset?[i]!;
                pieNode.ChartVisible = i < chartCount;
            }
        }
        public ICommand AddChartDataCmd { get; private set; }
        public void AddChartData(object s)
        {
            PieChartNode pieNode = Model?.PieChartAsset?[Model?.CurrentChartNumber ?? 0]!;
            PieSeries pieSeries = new ()
            {
                Title = "AddData",
                Values = new ChartValues<ObservableValue> { new ObservableValue((double)(Model?.PieChartData!)) },
                DataLabels = true
            };
            pieNode.PieChartSeries?.Add(pieSeries);
        }
        public ICommand RemoveChartDataCmd { get; private set; }
        public void RemoveChartData(object s)
        {
            PieChartNode pieNode = Model?.PieChartAsset?[Model.CurrentChartNumber ?? 0]!;
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
