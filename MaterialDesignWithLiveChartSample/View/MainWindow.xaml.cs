using MaterialDesignWithLiveChartSample.ViewModel;
using System.Windows;

namespace MaterialDesignWithLiveChartSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MaterialDesignWindowCustom.RegisterCommands(this);
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            Closing += ((MainWindowViewModel)DataContext).OnClosingMainWindow;
        }
    }
}
