using MaterialDesignWithLiveChartSample.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Windows;

namespace MaterialDesignWithLiveChartSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ILogger<MainWindow> logger, IServiceProvider serviceProvider)
        {
            MaterialDesignWindowCustom.RegisterCommands(this);
            InitializeComponent();
            DataContext = new MainWindowViewModel(/*logger, serviceProvider*/);
            ((MainWindowViewModel)DataContext).InitializeCustomControls(logger, serviceProvider);
            Closing += ((MainWindowViewModel)DataContext).OnClosingMainWindow;
        }
    }
}
