using MaterialDesignWithLiveChartSample.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class PieChartDisplayViewModel : ViewModelBase
    {
        public static PieChartDisplayViewModel? Instance { get; private set; }
        public PieChartDisplayModel? Model { get; private set; }
        public PieChartDisplayViewModel()
        {
            Instance = this;
            Model = new PieChartDisplayModel();
        }
    }
}
