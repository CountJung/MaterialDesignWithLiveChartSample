using MaterialDesignWithLiveChartSample.Model;
using MaterialDesignWithLiveChartSample.View;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
            TestCommandBtnCmd = new DelegateCommand(TestCommandBtn);
        }
        public ICommand PieChartCountSetCmd { get; private set; }
        public void PieChartCountSet(object s)
        {
            //find chart memory leak or allocate chart with settled count 
            foreach (PieChartNode pies in Model?.PieChartData ?? Enumerable.Empty<PieChartNode>())
            {
                pies.PieChartSeries?.Chart.View.Series.Clear();
                pies.PieChartSeries?.Clear();
                pies.PieChartSeries = null;
            }
            Model?.PieChartData?.Clear();
            Model!.PieChartData = null;
            Model.PieChartData = new ObservableCollection<PieChartNode>();
            uint chartCount = Model?.PiechartCount ?? 0;
            for (uint i = 0; i < chartCount; i++)
            {
                string nodeName = string.Format("Pie Chart {0}", i);
                Model?.PieChartData?.Add(new PieChartNode(Model.GetPieChartSeriesCollection(), nodeName));
            }
        }
        public ICommand TestCommandBtnCmd { get; private set; }
        public void TestCommandBtn(object s)
        {
            // memory leak test
            BindingOperations.ClearBinding(PieChartDisplay.Instance?.PieChartCollectionDisplay, ItemsControl.ItemsSourceProperty);
            Binding binding = new()
            {
                Path = new PropertyPath("ItemsSource"),
                Source = Model?.PieChartData
            };
            BindingOperations.SetBinding(PieChartDisplay.Instance?.PieChartCollectionDisplay, ItemsControl.ItemsSourceProperty, binding);
        }
    }
}
