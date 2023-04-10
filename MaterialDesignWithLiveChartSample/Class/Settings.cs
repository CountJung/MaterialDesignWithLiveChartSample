using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.Class
{
    public class Settings
    {
        private readonly IServiceProvider serviceProvider;
        public Settings(IServiceProvider serviceProvider) 
        {
            this.serviceProvider = serviceProvider;
        }

        public string? DefaultSetting { get; set; } = "Default";
    }
}
