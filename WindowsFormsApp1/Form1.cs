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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SQLiteConnection m_dbConnection;
       
        public Form1()
        {
            System.Diagnostics.Debug.WriteLine("Program has started");
            InitializeComponent();
            createNewDatabase();
            connectToDatabase();
            createTable();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
        }
        
        private void ShowMessage(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_Selected(object sender, EventArgs e)
        {
           
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        // LOG IT! button. Shows user the entry they have logged and adds it to the database
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Show the current single log, not all logs...
                String log = timeLog();
                MessageBox.Show(log);

                //System.IO.File.WriteAllText(@"C:\Users\carlosf\time_sheet\timeSheet.txt", log); // replaced with sql implementation
            } catch (Exception exception)
            {
                MessageBox.Show("Please fill out all fields\n" + exception);
            }
        }

        private void dateTimePicker1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        // If the user does not select a Name then the dateTimePicker's visibility changes to false
        private void datePicker_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                comboBox1.SelectedItem.ToString();
            }
            catch (NullReferenceException exception)
            {
                MessageBox.Show("Please select your name");
                datePicker.Visible = false;
            }
        }

        // Once the user has selected a name the dateTimePicker's visibility is changed to true
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            datePicker.Visible = true;
        }

        // Calls validateUser() method to give ADMIN user ability to chose a date other than today's current date
        private void datePicker_Enter(object sender, EventArgs e)
        {
            validateUserAccess();
        }

        private void dateTimePickerStart_Enter(object sender, EventArgs e)
        {
            // Restricts the user, cannot enter a time earlier than the current time
            //dateTimePickerStart.MinDate = DateTime.Now;
        }

        private void dateTimePickerEnd_Enter(object sender, EventArgs e)
        {
            // Restricts the user, cannot enter a time past the current time
            //dateTimePickerEnd.MaxDate = DateTime.Now;
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Prints the time log for that user in a MessageBox
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                String name = comboBox1.SelectedItem.ToString();
                printResults(name);
            }
            catch (Exception exp)
            {
                MessageBox.Show("Select a name\n");
            }

        }

        // Opens another form to view the entire log
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }

        // Shows current log to user then adds it to the database
        private String timeLog()
        {
            // Variables to store information necessary for a log
            String name = getName();
            String theDate = getDate();
            String startTime = getStringStartTime();
            String endTime = getStringEndTime();

            double timeWorked = getTimeWorked();

            String log = name + "\t" + theDate + "\t" + startTime + " to " + endTime + "\tTotal hours: " + timeWorked;

            String sql = "INSERT INTO timesheet (name, date, time_in, time_out, hours_worked) VALUES ('" + name + "', '" + theDate + "', '" + startTime + "', '" + endTime + "', '" + timeWorked + "')";
            SQLiteCommand command2 = new SQLiteCommand(sql, m_dbConnection);
            command2.ExecuteNonQuery();

            return log;
        }

        // Creates the database if it does not exist
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
            m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        // Creates timesheet table in the database if it does not exist
        void createTable()
        {
            String sql = "CREATE TABLE IF NOT EXISTS timesheet (id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL , name TEXT NOT NULL, date TEXT NOT NULL, time_in TEXT NOT NULL, time_out TEXT NOT NULL, hours_worked REAL)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // Gets entries from timesheet table for a SPECIFIC person and displays results in a MessageBox
        public void printResults(String name)
        {
            String sql = "SELECT * FROM timesheet WHERE name = '" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            String output = "";
            while (reader.Read())
            {
                output += "+ " + reader["id"] + " + " + reader["name"] + " + " + reader["date"] + " + " + reader["time_in"] + " + " + reader["time_out"] + " +\n";
            }

            MessageBox.Show("+------+------+------+---------+----------+\n" +
                    "+ ID # + Name + Date + Time In + Time Out +\n" +
                 output +
                "+------+------+------+---------+----------+\n");
        }

        // Gives ADMIN the ability to log in hour from a previous date (up to the first of the current month)
        public void validateUserAccess()
        {
            try
            {
                if (comboBox1.SelectedItem.ToString() != "ADMIN")
                {
                    datePicker.MinDate = DateTime.Now;
                }
                else
                {
                    datePicker.MinDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }
            }
            catch (NullReferenceException exception)
            {

            }
        }

        // Creates a PDF document with all of the log entries
        public void printToPDF()
        {
            // Path to the pdf that will be created (desktop)
            String outputFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Time Sheet.pdf");

            // Create a standard .Net filestream for the file, setting various flags
            using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (Document doc = new Document(PageSize.A4))
                {
                    // Binding the PDF document to the filstream using an itextsharp Pdfwriter
                    using (PdfWriter w = PdfWriter.GetInstance(doc, fs))
                    {
                        // Open the document for writing 
                        doc.Open();

                        String sql = "SELECT * FROM timesheet ORDER BY name";
                        SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                        SQLiteDataReader reader = command.ExecuteReader();

                        // Create a table with 4 columns
                        PdfPTable t = new PdfPTable(5);
                        t.DefaultCell.Border = 1;

                        while (reader.Read())
                        {
                           
                            t.AddCell(reader["name"].ToString());
                            t.AddCell(reader["date"].ToString());
                            t.AddCell(reader["time_in"].ToString());
                            t.AddCell(reader["time_out"].ToString());
                            t.AddCell(reader["hours_worked"].ToString());
                        }

                        // Add the table to the document
                        doc.Add(t);

                        // Close the document
                        doc.Close();
                    }
                }

                //this.Close();
            }

        }

        public String getName()
        {
            String name = comboBox1.SelectedItem.ToString();

            return name;
        }

        public String getDate()
        {
            String theDate = datePicker.Value.ToShortDateString();

            return theDate;
        }

        public String getStringStartTime()
        {
            String startTime = dateTimePickerStart.Value.ToShortTimeString();
            return startTime;
        }

        public String getStringEndTime()
        {
            String endTime = dateTimePickerEnd.Value.ToShortTimeString();

            return endTime;
        }

        public int getStartHour()
        {
            String sHour = dateTimePickerStart.Value.Hour.ToString();
            int sH = Convert.ToInt32(sHour);

            return sH;
        }

        public int getStartMinute()
        {
            int startMin = Convert.ToInt32(dateTimePickerStart.Value.Minute.ToString());

            return startMin;
        }

        public int getEndHour()
        {
            int endHour = Convert.ToInt32(dateTimePickerEnd.Value.Hour.ToString());

            return endHour;
        }

        public int getEndMinute()
        {
            int endMin = Convert.ToInt32(dateTimePickerEnd.Value.Minute.ToString());

            return endMin;
        }

        public double getTimeWorked()
        {
            int startHour = getStartHour();
            int startMin = getStartMinute();
            int endHour = getEndHour();
            int endMin = getEndMinute();

            int hoursWorked = endHour - startHour;
            int minutesWorked = Math.Abs(endMin - startMin);
            double timeWorked = hoursWorked + (minutesWorked / 60.0);

            return Math.Round(timeWorked, 2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            printToPDF();
        }
    }
}
 