using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApp1
{
    public partial class DataGridExample : Form
    {

        SQLiteConnection m_dbConnection;

        public void createNewDatabase()
        {
            if (!System.IO.File.Exists("MyDatabase.sqlite"))
            {
                SQLiteConnection.CreateFile("MyDatabase.sqlite");
            }
        }

        // Creates a connection with our database file.
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3");
            m_dbConnection.Open();
        }

        void createTable()
        {
            String sql = "CREATE TABLE IF NOT EXISTS timesheet (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , name TEXT NOT NULL, date TEXT NOT NULL, time_in TEXT NOT NULL, time_out TEXT NOT NULL)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public void fillTable()
        {
            String sql = "SELECT * FROM timesheet ORDER BY name";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            String output = "";

            table.Columns.Add(new DataColumn("check", typeof(bool)));
            table.Columns.Add(new DataColumn("id", typeof(String)));
            table.Columns.Add(new DataColumn("Name", typeof(String)));
            table.Columns.Add(new DataColumn("Date", typeof(String)));
            table.Columns.Add(new DataColumn("Time in", typeof(String)));
            table.Columns.Add(new DataColumn("Time out", typeof(String)));
            dataGridView1.DataSource = table.DefaultView;
            // new Checkbox
            CheckBox box;

            while (reader.Read())
            {
                table.Rows.Add(new object[] {0, reader["id"].ToString(), reader["name"].ToString(), reader["date"].ToString(),
                    reader["time_in"].ToString(), reader["time_out"].ToString() });

            }

           
        }

        DataTable table = new DataTable();

        public DataGridExample()
        {
            InitializeComponent();
            createNewDatabase();
            connectToDatabase();
            createTable();
            fillTable();
        }
    }
}
