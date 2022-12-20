using MaterialDesignWithLiveChartSample.Model;
using MySqlConnector;
using System.Data;

namespace MaterialDesignWithLiveChartSample.ViewModel
{
    public class DataBaseDisplayViewModel
    {
        public DataBaseDisplayModel? Model { get; private set; }
        public DataBaseDisplayViewModel()
        {
            Model = new DataBaseDisplayModel();
            DBConnectionTest();
        }
        private MySqlConnection? mySqlConnection;
        private void DBConnectionTest()
        {
            string connectionString = "Host=localhost;Port=3306;Database=testdatabase;Uid=root;Pwd=MariaDBTest;";
            mySqlConnection = new MySqlConnection(connectionString);
            string query = "select * from emoticontextdata";
            //DataTable? dataTable;
            //MySqlDataReader reader;
            //mySqlConnection.Open();

            //using (MySqlCommand sqlCommand =new MySqlCommand(query,mySqlConnection))
            //{
            //    reader = sqlCommand.ExecuteReader(/*CommandBehavior.SchemaOnly*/);
            //    dataTable = reader.GetSchemaTable();
            //}
            //if (dataTable?.Rows is not null)
            //{
            //    foreach (DataRow data in dataTable?.Rows!)
            //    {
            //        string? colName = data?.Field<string>("ColumnName");
            //        //string? value = (string ?)data?[colName!];
            //    }
            //}
            //while (reader.Read())
            //{
            //    string? value = reader.ToString();
            //}
            //reader.Close();
            //mySqlConnection.Close();
            //DataSet dataSet = new DataSet();

            mySqlConnection.Open();
            MySqlDataAdapter adapter;
            DataTable dataTable = new DataTable();
            adapter = new MySqlDataAdapter(query, mySqlConnection);
            adapter.Fill(dataTable);
            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                string? header = dataColumn.ColumnName;
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string? value = dataRow[dataColumn].ToString();
                }
            }
            if (mySqlConnection.State == ConnectionState.Open)
                mySqlConnection.Close();
            mySqlConnection.Dispose();
        }
    }
}
