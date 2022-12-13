using LiveCharts;
using LiveCharts.Wpf;
using MaterialDesignWithLiveChartSample.Model;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class LineChartDisplayViewModel : ViewModelBase
    {
        public static LineChartDisplayViewModel? Instance { get; private set; }
        public LineChartDisplayModel? Model { get; private set; }
        public LineChartDisplayViewModel()
        {
            Instance = this;
            Model = new LineChartDisplayModel();
            FirstdataClickCmd = new DelegateCommand(FirstdataClick);
            ResetdataClickCmd = new DelegateCommand(ResetdataClick);
        }
        public ICommand? FirstdataClickCmd { get; private set; }
        public void FirstdataClick(object sender)
        {
            if (Model?.SeriesCollection?.Count < 2)
            {
                Model?.SeriesCollection.Add(new LineSeries { Values = new ChartValues<double> { 10, 50, 39, 50 } });
                Model?.SeriesCollection.Add(new LineSeries { Values = new ChartValues<double> { 11, 56, 42, 48 } });
            }
            Model?.SeriesCollection?[0].Values.Add(Model.FirstLineData ?? 0);
            Model?.SeriesCollection?[1].Values.Add(Model.SecondLineData ?? 0);
        }
        public ICommand? ResetdataClickCmd { get; private set; }
        public void ResetdataClick(object sender)
        {
            Model?.SeriesCollection?.Clear();
        }
    }
}
