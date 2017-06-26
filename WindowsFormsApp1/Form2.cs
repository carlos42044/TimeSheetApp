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
    public partial class Form2 : Form
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
        public Int32 rowCount = 0;

        // adds a label, paramaters: string, and column index
        // colors odd rows SlateGray, even rows Linen
        public void addLabel(String str, int colIndex)
        {
            Label label = new Label();
            label.Dock = DockStyle.Fill;
            label.Text = str;
            label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            tableLayoutPanel1.Controls.Add(label, colIndex, rowCount);
            //rowCount % 2 == 1 ? label.ForeColor = Color.Gray : label.ForeColor = Color.White;
            //rowCount++;
            if (rowCount % 2 == 1)
            {
                label.BackColor = Color.SlateGray;
            } else
            {
                label.BackColor = Color.Linen;
            }
        }

        public void fillTable()
        {
            String sql = "SELECT * FROM timesheet ORDER BY name";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            String output = "";

            // new Checkbox
            CheckBox box;

            while (reader.Read())
            {
                box = new CheckBox();
                box.AutoSize = true;
                tableLayoutPanel1.Controls.Add(box, 0, rowCount);

                if (rowCount % 2 == 1)
                {
                    box.BackColor = Color.SlateGray;
                } else
                {
                    box.BackColor = Color.Linen;
                }

                box.Text = reader["id"].ToString();
                addLabel(reader["name"].ToString(), 1);
                addLabel(reader["date"].ToString(), 2);
                addLabel(reader["time_in"].ToString(), 3);
                addLabel(reader["time_out"].ToString(), 4);
                rowCount++;
            }

            Button deleteButton = new Button();
            deleteButton.Width = 90;
            deleteButton.Height = 44;
            deleteButton.Text = "Delete";
            tableLayoutPanel1.Controls.Add(deleteButton, 4, rowCount +1);
            //deleteButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(deleteButton_Click));
            deleteButton.Click += new EventHandler(deleteButton_Click);
        }

        public void deleteButton_Click(object sender, EventArgs e)
        {
            int count = 0;
           // MessageBox.Show("The number of rows in the table are " + tableLayoutPanel1.RowCount);
            //tableLayoutPanel1.
            for (int i = 1; i < rowCount-1; i++)
            {
               // MessageBox.Show(tableLayoutPanel1.GetControlFromPosition(1,i).Text);
               CheckBox c = (CheckBox)tableLayoutPanel1.GetControlFromPosition(0, i);
               
               if (c.Checked)
                {
                    deleteEntry(c.Text);
                    rowCount--;
                    //tableLayoutPanel1.GetRow.
                    count++;
                }
            }

            MessageBox.Show("There were " + count + " rows checked!");
            rowCount = 0;
            tableLayoutPanel1.Controls.Clear();
            headerRow();
            fillTable();
        }
        
        public void deleteEntry(String idNum)
        {
            String sql = "DELETE FROM timesheet WHERE id='" + idNum + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public Form2()
        {
            InitializeComponent();
            createNewDatabase();
            connectToDatabase();
            createTable();
            headerRow();
            fillTable();
           
        }

        public void headerRow()
        {
            addLabel("Delete(Log #)", 0);
            addLabel("Name", 1);
            addLabel("Date", 2);
            addLabel("Time In", 3);
            addLabel("Time Out", 4);
            rowCount++;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            if (( e.Row) % 2 == 1)
            {
                e.Graphics.FillRectangle(Brushes.SlateGray, e.CellBounds);
            } else
            {
                e.Graphics.FillRectangle(Brushes.Linen, e.CellBounds);
            }
        }
    }
}
