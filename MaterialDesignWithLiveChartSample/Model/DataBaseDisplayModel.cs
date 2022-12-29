using MaterialDesignWithLiveChartSample.Class;
using MaterialDesignWithLiveChartSample.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDesignWithLiveChartSample.Model
{
    public class DataBaseDisplayModel:ViewModelBase
    {
        public string? HostIP { get; set; }
        public int Port { get; set; }
        public string? DefaultDataBase { get; set; }
        public string? UserID { get; set; }
        public string? PassWord { get; set; }
        public SecureString? SecurePassWord { get; set; }
        private bool dbConnected;
        public bool DBConnected { get => dbConnected; set => Set(ref dbConnected, value, nameof(DBConnected)); }
        public ObservableCollection<EmoticonList>? EmoticonListCollection { get; }
        private EmoticonList? selectedEmoticonList;
        public EmoticonList? SelectedEmoticonList 
        {
            get => selectedEmoticonList ??= new EmoticonList();
            set 
            { 
                Set(ref selectedEmoticonList, value, nameof(SelectedEmoticonList));
                //DataBaseDisplayViewModel.Instance?.ModifySelectedEmoticon();
            }
        }
        public DataBaseDisplayModel()
        {
            EmoticonListCollection= new ObservableCollection<EmoticonList>();
            HostIP = "localhost";
            Port = 3306;
            DefaultDataBase = "testdatabase";
            UserID = "root";
            PassWord = "MariaDBTest";
        }
        public static string? SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
