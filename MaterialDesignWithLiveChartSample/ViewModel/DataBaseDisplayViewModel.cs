using MaterialDesignWithLiveChartSample.Class;
using MaterialDesignWithLiveChartSample.Model;
using MySqlConnector;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;

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
            ModifySelectedEmoticonCmd = new DelegateCommand(ModifySelectedEmoticon);
        }
        private MySqlConnection? dbConnection;

        public ICommand DBConnectCmd { get; private set; }
        private void DBConnect(object s)
        {
            if (dbConnection?.State != ConnectionState.Open)
            {
                string? pass = DataBaseDisplayModel.SecureStringToString(Model?.SecurePassWord!);
                string connectionString = $"Host={Model?.HostIP};Port={Model?.Port};Database={Model?.DefaultDataBase};Uid={Model?.UserID};Pwd={Model?.PassWord};";
                dbConnection = new MySqlConnection(connectionString);
                dbConnection.Open();
                Model!.DBConnected = dbConnection?.State == ConnectionState.Open;
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
            Model!.DBConnected = dbConnection?.State == ConnectionState.Open;
            Model?.EmoticonListCollection?.Clear();
        }
        public ICommand SelectAllEmoticonListCmd { get; private set; }
        private void SelectAllEmoticonListBtn(object s)
        {
            SelectAllEmoticonList();
        }
        private void SelectAllEmoticonList()
        {
            try
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
            catch (MySqlException sqlEx)
            {
                Trace.WriteLine(sqlEx.Message);
                if (sqlEx.Number == 1146)
                    CreateEmoticonTable();
            }
        }
        public ICommand AddEmoticonCmd { get; private set; }
        private void AddEmoticon(object s)
        {
            try
            {
                if (dbConnection?.State == ConnectionState.Open)
                {
                    EmoticonList emoticon = new();
                    int listCount = (int)Model?.EmoticonListCollection?.Count!;
                    int ID = listCount == 0 ? 0 : (int)Model?.EmoticonListCollection[listCount - 1].ID!;
                    emoticon.ID = ID + 1; emoticon.Name = "Slightly Smiling Face"; emoticon.Emoticon = "🙂"; 
                    emoticon.Description = "A yellow face with simple, open eyes and a thin, closed smile. Conveys a wide range of positive, happy, and friendly sentiments. Its tone can also be patronizing, passive-aggressive, or ironic, as if saying This is fine when it’s really not.";
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
            catch (MySqlException mySqlEx) 
            {
                Trace.WriteLine(mySqlEx.Message);
            }
        }
        public ICommand DeleteSelectedEmoticonCmd { get; private set; }
        private void DeleteSelectedEmoticon(object s)
        {
            try
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
            catch (MySqlException mySqlEx) 
            {
                Trace.WriteLine(mySqlEx.Message);
            }
        }
        public ICommand ModifySelectedEmoticonCmd { get; private set; }
        public void ModifySelectedEmoticon(object sender)
        {
            try
            {
                if (dbConnection?.State == ConnectionState.Open)
                {
                    int ID = (int)Model?.SelectedEmoticonList?.ID!;
                    if (Model?.SelectedEmoticonList?.Name == null) return;
                    MySqlCommand sqlCommand = new();
                    string cmdstring = $"update emoticontextdata set" +
                        $" Name='{Model?.SelectedEmoticonList?.Name}', Emoticon='{Model?.SelectedEmoticonList?.Emoticon}'," +
                        $" Description='{Model?.SelectedEmoticonList?.Description}'" +
                        $" where ID={ID}";
                    sqlCommand.Connection = dbConnection;
                    sqlCommand.CommandText = cmdstring;
                    sqlCommand.ExecuteNonQuery();
                    SelectAllEmoticonList();
                }
            }
            catch (MySqlException mySqlEx) 
            {
                Trace.WriteLine(mySqlEx.Message);
            }
        }
        private void CreateEmoticonTable()
        {
            if (dbConnection?.State == ConnectionState.Open)
            {
                MySqlCommand sqlCommand = new();
                string cmdstring = "CREATE TABLE IF NOT EXISTS `emoticontextdata` (" +
                    "  `ID` int(10) unsigned NOT NULL AUTO_INCREMENT, " +
                    "  `Name` varchar(100) DEFAULT NULL, " +
                    "  `Emoticon` varchar(10) DEFAULT NULL, " +
                    "  `Description` varchar(1000) DEFAULT NULL, " +
                    "  PRIMARY KEY (`ID`) " +
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;"; //AUTO_INCREMENT=5 COLLATE=utf8mb4_general_ci
                sqlCommand.Connection = dbConnection;
                sqlCommand.CommandText = cmdstring;
                sqlCommand.ExecuteNonQuery();
                AddEmoticon(null!);
            }
        }
    }
}
