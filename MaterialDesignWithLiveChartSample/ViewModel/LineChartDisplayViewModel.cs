using MaterialDesignThemes.Wpf;
using MaterialDesignWithLiveChartSample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class LineChartDisplayViewModel
    {
        public static LineChartDisplayViewModel? Instance { get; private set; }
        public LineChartDisplayModel? Model { get; private set; }
        public LineChartDisplayViewModel()
        {
            Instance = this;
            Model = new LineChartDisplayModel();
        }
    }
}
