//
// FILE        : CourseTable.xaml.cs
// PROJECT     : Final Database Project – Phase 5 (CRUD Operations)
// PROGRAMMER  : Kevin Napoles
// FIRST VERSION : 2025-12-08
// DESCRIPTION : 
//   UI logic for interacting with the Course table.
//   Loads Course records and lets the user create, edit, and delete
//   rows using ADO.NET.
//


using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;

namespace SchoolDataSystem.UI_LAYER
{
    //
    // CLASS : CourseTable
    // PURPOSE :
    //   WPF window that displays and manages Course data.
    //
    public partial class CourseTable : Window
    {
        private string connectionString = "Server=localhost;Uid=root;Pwd=HelloWorld;DataBase=SchoolDataBase;";
        private MySqlDataAdapter dataAdapter;
        private DataSet dataSet;

        //
        // FUNCTION : CourseTable
        // DESCRIPTION :
        //   Constructor. Initializes the window and loads the data.
        //
        public CourseTable()
        {
            InitializeComponent();
            LoadCourseTable();
        }

        //
        // FUNCTION : LoadCourseTable
        // DESCRIPTION :
        //   Reads all rows from the Course table and shows them in the DataGrid.
        //
        private void LoadCourseTable()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                dataAdapter = new MySqlDataAdapter("SELECT * FROM Course", connection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "course");

                dgCourses.ItemsSource = dataSet.Tables["course"].DefaultView;
            }
        }

        //
        // FUNCTION : btnCreate_Click
        // DESCRIPTION :
        //   Creates a new Course row using the values in the text boxes.
        //
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int courseID;
            int sectionValue;

            if (!int.TryParse(txtCourseID.Text, out courseID))
            {
                MessageBox.Show("Course ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            if (!int.TryParse(txtSection.Text, out sectionValue))
            {
                MessageBox.Show("Section must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRow newRow = dataSet.Tables["course"].NewRow();

            newRow["CourseID"] = courseID;
            newRow["Section"] = sectionValue;
            newRow["CourseName"] = txtCourseName.Text;

            dataSet.Tables["course"].Rows.Add(newRow);

            SaveChanges();
            LoadCourseTable();
        }

        //
        // FUNCTION : btnUpdate_Click
        // DESCRIPTION :
        //   Saves any edits made directly in the DataGrid.
        //
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            MessageBox.Show("Updated!");
            LoadCourseTable();
        }

        //
        // FUNCTION : btnDelete_Click
        // DESCRIPTION :
        //   Deletes the selected Course row.
        //
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourses.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgCourses.SelectedItem;
            selectedDataRowView.Row.Delete();

            SaveChanges();
            LoadCourseTable();
        }

        //
        // FUNCTION : SaveChanges
        // DESCRIPTION :
        //   Uses MySqlCommandBuilder to send changes in the DataSet to the database.
        //
        private void SaveChanges()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                dataAdapter = new MySqlDataAdapter("SELECT * FROM Course", connection);
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                dataAdapter.Update(dataSet, "course");
            }
        }

        //
        // FUNCTION : btnBack_Click
        // DESCRIPTION :
        //   Closes this window.
        //
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
