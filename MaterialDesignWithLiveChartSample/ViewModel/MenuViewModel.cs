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
            MenuDataBaseCmd = new DelegateCommand(MenuDataBase);
            MenuItem = new ObservableCollection<MenuModel>(new List<MenuModel>
            {
                //Only SubMenu Command can be executable
                new MenuModel
                {
                    Header="File", SubMenu=new ObservableCollection<MenuModel>(new List<MenuModel>
                    {
                        new MenuModel{Header="Open", Command=MenuFileOpenCmd},
                        new MenuModel{Header="Save", Command=MenuFileCloseCmd}
                    })
                },
                new MenuModel
                {
                    Header="Chart", SubMenu=new ObservableCollection<MenuModel>(new List<MenuModel>
                    {
                        new MenuModel{Header="LineChart", Command=MenuLineChartCmd},
                        new MenuModel{Header="PieChart", Command=MenuPieChartCmd}
                    })
                },
                new MenuModel
                {
                    Header="Library", SubMenu=new ObservableCollection<MenuModel>(new List<MenuModel>
                    {
                        new MenuModel{Header="DataBaseTest", Command=MenuDataBaseCmd}
                    })
                }
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
        /// <summary>
        /// Charts
        /// </summary>
        public ICommand? MenuLineChartCmd { get; private set; }
        private void MenuLineChart(object s) => MainWindowViewModel.Instance!.SelectedControlIndex = (int)MainWindowViewModel.DisplayNumber.LineChartNumber;
        public ICommand? MenuPieChartCmd { get; private set; }
        private void MenuPieChart(object s) => MainWindowViewModel.Instance!.SelectedControlIndex = (int)MainWindowViewModel.DisplayNumber.PieChartNumber;
        public ICommand? MenuDataBaseCmd { get; private set; }
        private void MenuDataBase(object s) => MainWindowViewModel.Instance!.SelectedControlIndex = (int)MainWindowViewModel.DisplayNumber.DataBaseNumber;
    }
}
