using MaterialDesignWithLiveChartSample.Class;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MaterialDesignWithLiveChartSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost? host;
        public App() 
        {
            host= Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();
        }
        private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<Settings>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var main=host?.Services.GetService<MainWindow>();
            main?.Show();

            base.OnStartup(e);

        }
    }
}
