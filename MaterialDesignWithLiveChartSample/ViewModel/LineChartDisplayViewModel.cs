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
        public LineChartDisplayModel? Model { get; private set; }
        public LineChartDisplayViewModel()
        {
            Model = new LineChartDisplayModel();
        }
    }
}
