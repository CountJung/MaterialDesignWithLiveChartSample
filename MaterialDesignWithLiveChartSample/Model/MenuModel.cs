using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class MenuModel
    {
        public string? Header { get; set; }
        public ICommand? Command { get; set; }
        public ObservableCollection<MenuModel>? SubMenu { get; set; }
    }
}
