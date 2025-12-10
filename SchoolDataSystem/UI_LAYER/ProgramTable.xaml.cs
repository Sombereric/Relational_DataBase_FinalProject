//
// FILE        : ProgramTable.xaml.cs
// PROJECT     : Final Database Project – Phase 5 (CRUD Operations)
// PROGRAMMER  : Kevin Napoles
// FIRST VERSION : 2025-12-08
// DESCRIPTION : 
//   This class provides the UI logic for interacting with the Program table.
//   It loads Program records from the database and allows the user
//   to Create, Read, Update, and Delete entries using ADO.NET.
//


using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace SchoolDataSystem.UI_LAYER
{
    //
    // CLASS : ProgramTable
    // PURPOSE :
    //   Represents the WPF window that displays and manages Program data.
    //   Provides methods to load, insert, update, and delete rows from
    //   the Program table in the SchoolDataBase.
    //
    public partial class ProgramTable : Window
    {
        private string connectionString = "Server=localhost;Uid=root;Pwd=HelloWorld;DataBase=SchoolDataBase;";
        private MySqlDataAdapter dataAdapter;
        private DataSet dataSet;

        //
        // FUNCTION : ProgramTable (constructor)
        // DESCRIPTION :
        //   Initializes the UI and loads all Program records on startup.
        // PARAMETERS : none
        // RETURNS    : nothing
        //
        public ProgramTable()
        {
            InitializeComponent();
            LoadProgramTable();
        }

        //
        // FUNCTION : LoadProgramTable
        // DESCRIPTION :
        //   Reads all rows from the Program table and displays them
        //   inside the DataGrid.
        // PARAMETERS : none
        // RETURNS    : nothing
        //
        private void LoadProgramTable()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Retrieve all Program rows
                dataAdapter = new MySqlDataAdapter("SELECT * FROM Program", connection);

                dataSet = new DataSet();

                dataAdapter.Fill(dataSet, "program");

                // Bind the table to the DataGrid
                dgPrograms.ItemsSource = dataSet.Tables["program"].DefaultView;
            }
        }

        //
        // FUNCTION : btnCreate_Click
        // DESCRIPTION :
        //   Creates a new Program row using user input and saves it
        //   to the database.
        // PARAMETERS :
        //   object sender : event source
        //   RoutedEventArgs e : event arguments
        // RETURNS : nothing
        //
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int programID;

            if (!int.TryParse(txtProgramID.Text, out programID))
            {
                MessageBox.Show("Program ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRow newRow = dataSet.Tables["program"].NewRow();

            newRow["ProgramID"] = programID;
            newRow["ProgramName"] = txtProgramName.Text;
            newRow["ProgramType"] = txtProgramType.Text;

            dataSet.Tables["program"].Rows.Add(newRow);

            SaveChanges();
            LoadProgramTable();
        }

        //
        // FUNCTION : btnUpdate_Click
        // DESCRIPTION :
        //   Saves all changes made directly inside the DataGrid.
        // PARAMETERS :
        //   object sender : event source
        //   RoutedEventArgs e : event arguments
        // RETURNS : nothing
        //
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            MessageBox.Show("Updated!");
            LoadProgramTable();
        }

        //
        // FUNCTION : btnDelete_Click
        // DESCRIPTION :
        //   Deletes the selected Program row from both the DataGrid
        //   and the database.
        // PARAMETERS :
        //   object sender : event source
        //   RoutedEventArgs e : event arguments
        // RETURNS : nothing
        //
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgPrograms.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            // Get selected row and delete it
            DataRowView selectedDataRowView = (DataRowView)dgPrograms.SelectedItem;
            selectedDataRowView.Row.Delete();

            SaveChanges();
            LoadProgramTable();
        }

        //
        // FUNCTION : SaveChanges
        // DESCRIPTION :
        //   Sends INSERT, UPDATE, and DELETE commands generated
        //   automatically by MySqlCommandBuilder to persist changes
        //   in the Program table.
        // PARAMETERS : none
        // RETURNS    : nothing
        //
        private void SaveChanges()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                dataAdapter = new MySqlDataAdapter("SELECT * FROM Program", connection);

                // Auto-generate UPDATE / INSERT / DELETE commands
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                dataAdapter.Update(dataSet, "program");
            }
        }

        //
        // FUNCTION : btnBack_Click
        // DESCRIPTION :
        //   Closes this window and returns to the previous screen.
        // PARAMETERS :
        //   object sender : event source
        //   RoutedEventArgs e : event arguments
        // RETURNS : nothing
        //
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
