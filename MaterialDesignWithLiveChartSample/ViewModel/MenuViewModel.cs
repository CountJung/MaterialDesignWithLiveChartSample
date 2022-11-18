using MaterialDesignWithLiveChartSample.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class MenuViewModel : ViewModelBase
    {
        public ObservableCollection<MenuModel>? MenuItem { get; private set; }
        //public ObservableCollection<ControlItems>? Controls { get; private set; }  
        public MenuViewModel()
        {
            MenuFileOpenCmd = new DelegateCommand(MenuFileOpen);
            MenuFileCloseCmd = new DelegateCommand(MenuFileClose);
            MenuLineChartCmd = new DelegateCommand(MenuLineChart);
            MenuPieChartCmd = new DelegateCommand(MenuPieChart);
            MenuItem = new ObservableCollection<MenuModel>(new List<MenuModel>
            {
                //Only SubMenu Command can be executable
                new MenuModel{Header="File", SubMenu=new ObservableCollection<MenuModel>(new List<MenuModel>
                {
                    new MenuModel{Header="Open", Command=MenuFileOpenCmd},
                    new MenuModel{Header="Save", Command=MenuFileCloseCmd}
                })},
                new MenuModel{Header="View", SubMenu=new ObservableCollection<MenuModel>(new List<MenuModel>
                {
                    new MenuModel{Header="LineChart", Command=MenuLineChartCmd},
                    new MenuModel{Header="PieChart", Command=MenuPieChartCmd}
                })}
            });
            //CustomPages
            //Controls = new ObservableCollection<ControlItems>();
        }
        public ICommand? MenuFileOpenCmd { get; private set; }
        private void MenuFileOpen(object s)
        {
        }
        public ICommand? MenuFileCloseCmd { get; private set; }
        private void MenuFileClose(object s)
        {
        }
        public ICommand? MenuLineChartCmd { get; private set; }
        private void MenuLineChart(object s) => MainWindowViewModel.Instance!.SelectedControlIndex = 0;
        public ICommand? MenuPieChartCmd { get; private set; }
        private void MenuPieChart(object s) => MainWindowViewModel.Instance!.SelectedControlIndex = 1;

    }
}
