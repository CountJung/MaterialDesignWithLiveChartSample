using MaterialDesignWithLiveChartSample.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDesignWithLiveChartSample.View
{
    /// <summary>
    /// DataBaseDisplay.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseDisplay : UserControl
    {
        public DataBaseDisplay()
        {
            InitializeComponent();
            DataContext = new DataBaseDisplayViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext!=null)
            {
                DataBaseDisplayViewModel.Instance!.Model!.SecurePassWord = ((PasswordBox)sender).SecurePassword;
                DataBaseDisplayViewModel.Instance!.Model!.PassWord = ((PasswordBox)sender).Password;
            }
        }
    }
}
