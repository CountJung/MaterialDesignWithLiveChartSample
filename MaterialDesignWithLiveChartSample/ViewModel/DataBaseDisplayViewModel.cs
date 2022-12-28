using MaterialDesignWithLiveChartSample.Class;
using MaterialDesignWithLiveChartSample.Model;
using MySqlConnector;
using System;
using System.Data;
using System.Windows.Input;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class DataBaseDisplayViewModel
    {
        public static DataBaseDisplayViewModel? Instance { get; private set; }
        public DataBaseDisplayModel? Model { get; private set; }
        public DataBaseDisplayViewModel()
        {
            Instance = this;
            Model = new DataBaseDisplayModel();
            //DBConnectionTest();
            DBConnectCmd = new DelegateCommand(DBConnect);
            DBDisconnectCmd = new DelegateCommand(DBDisconnect);
            SelectAllEmoticonListCmd = new DelegateCommand(SelectAllEmoticonListBtn);
            AddEmoticonCmd = new DelegateCommand(AddEmoticon);
            DeleteSelectedEmoticonCmd = new DelegateCommand(DeleteSelectedEmoticon);
        }
        private MySqlConnection? dbConnection;
        //private void DBConnectionTest()
        //{
        //    string connectionString = "Host=localhost;Port=3306;Database=testdatabase;Uid=root;Pwd=MariaDBTest;";
        //    dbConnection = new MySqlConnection(connectionString);
        //    string query = "select * from emoticontextdata";

        //    dbConnection.Open();
        //    MySqlDataAdapter adapter;
        //    DataTable dataTable = new DataTable();
        //    adapter = new MySqlDataAdapter(query, dbConnection);
        //    adapter.Fill(dataTable);
        //    foreach (DataColumn dataColumn in dataTable.Columns)
        //    {
        //        string? header = dataColumn.ColumnName;
        //        foreach (DataRow dataRow in dataTable.Rows)
        //        {
        //            string? value = dataRow[dataColumn].ToString();
        //        }
        //    }
        //    if (dbConnection.State == ConnectionState.Open)
        //        dbConnection.Close();
        //    dbConnection.Dispose();
        //}

        public ICommand DBConnectCmd { get; private set; }
        private void DBConnect(object s)
        {
            if (dbConnection?.State != ConnectionState.Open)
            {
                string? pass = DataBaseDisplayModel.SecureStringToString(Model?.SecurePassWord!);
                string connectionString = $"Host={Model?.HostIP};Port={Model?.Port};Database={Model?.DefaultDataBase};Uid={Model?.UserID};Pwd={Model?.PassWord};";
                dbConnection = new MySqlConnection(connectionString);
                dbConnection.Open();
                Model!.DBConnected = dbConnection.State == ConnectionState.Open;
                if ((bool)Model?.DBConnected!)
                    SelectAllEmoticonList();
            }
        }
        public ICommand DBDisconnectCmd { get; private set; }
        private void DBDisconnect(object sender)
        {
            DBClose();
        }
        public void DBClose()
        {
            if (dbConnection?.State == ConnectionState.Open)
            {
                dbConnection.Close();
                dbConnection.Dispose();
            }
        }
        public ICommand SelectAllEmoticonListCmd { get; private set; }
        private void SelectAllEmoticonListBtn(object s)
        {
            SelectAllEmoticonList();
        }
        private void SelectAllEmoticonList()
        {
            if (dbConnection?.State == ConnectionState.Open)
            {
                Model?.EmoticonListCollection?.Clear();
                string query = "select * from emoticontextdata";
                MySqlDataAdapter adapter;
                DataTable dataTable = new();
                adapter = new MySqlDataAdapter(query, dbConnection);
                adapter.Fill(dataTable);
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    EmoticonList emoticon = new();
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        string? header = dataColumn.ColumnName;
                        string? value = dataRow[dataColumn].ToString();
                        if (header == "ID") emoticon.ID = Convert.ToInt32(value);
                        else if (header == "Name") emoticon.Name = value;
                        else if (header == "Description") emoticon.Description = value;
                        else if (header == "Emoticon") emoticon.Emoticon = value;
                    }
                    Model?.EmoticonListCollection?.Add(emoticon);
                }
            }
        }
        public ICommand AddEmoticonCmd { get; private set; }
        private void AddEmoticon(object s)
        {
            if (dbConnection?.State == ConnectionState.Open)
            {
                EmoticonList emoticon = new();
                int listCount = (int)Model?.EmoticonListCollection?.Count!;
                int ID = (int)Model?.EmoticonListCollection[listCount - 1].ID!;
                emoticon.ID = ID + 1; emoticon.Name = "Smile"; emoticon.Emoticon = "🙂"; emoticon.Description = "Smiling face";
                MySqlCommand sqlCommand = new();
                string cmdstring = "Insert into emoticontextdata(ID, Name, Emoticon, Description) values(?ID, ?Name, ?Emoticon, ?Description)";
                sqlCommand.Connection = dbConnection;
                sqlCommand.CommandText = cmdstring;
                sqlCommand.Parameters.Add("?ID", MySqlDbType.UInt32).Value = emoticon.ID;
                sqlCommand.Parameters.Add("?Name", MySqlDbType.VarChar).Value = emoticon.Name;
                sqlCommand.Parameters.Add("?Emoticon", MySqlDbType.VarChar).Value = emoticon.Emoticon;
                sqlCommand.Parameters.Add("?Description", MySqlDbType.VarChar).Value = emoticon.Description;
                sqlCommand.ExecuteNonQuery();
                SelectAllEmoticonList();
            }
        }
        public ICommand DeleteSelectedEmoticonCmd { get; private set; }
        private void DeleteSelectedEmoticon(object s)
        {
            if (dbConnection?.State == ConnectionState.Open)
            {
                int ID = (int)Model?.SelectedEmoticonList?.ID!;
                MySqlCommand sqlCommand = new();
                string cmdstring = $"delete from emoticontextdata where ID={ID}";
                sqlCommand.Connection = dbConnection;
                sqlCommand.CommandText = cmdstring;
                sqlCommand.ExecuteNonQuery();
                SelectAllEmoticonList();
            }
        }
    }
}
