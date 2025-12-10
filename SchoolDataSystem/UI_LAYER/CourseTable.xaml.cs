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


using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;   // <--- added
using MySql.Data.MySqlClient;

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
            txtCourseID.IsEnabled = false;
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
            int sectionValue;

            if (!int.TryParse(txtSection.Text, out sectionValue))
            {
                MessageBox.Show("Section must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRow newRow = dataSet.Tables["course"].NewRow();

            newRow["CourseID"] = CourseIDGenerator();
            newRow["Section"] = sectionValue;
            newRow["CourseName"] = txtCourseName.Text;

            dataSet.Tables["course"].Rows.Add(newRow);

            SaveChanges();
            LoadCourseTable();
        }

        //
        // FUNCTION : btnUpdate_Click
        // DESCRIPTION :
        //   Updates the selected Course row using the values in the text boxes.
        //
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourses.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

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

            DataRowView selectedDataRowView = (DataRowView)dgCourses.SelectedItem;

            selectedDataRowView["CourseID"] = courseID;
            selectedDataRowView["Section"] = sectionValue;
            selectedDataRowView["CourseName"] = txtCourseName.Text;

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
        // FUNCTION : dgCourses_SelectionChanged
        // DESCRIPTION :
        //   Copies the selected row values into the text boxes for editing.
        //
        private void dgCourses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCourses.SelectedItem == null)
            {
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgCourses.SelectedItem;

            txtCourseID.Text = selectedDataRowView["CourseID"].ToString();
            txtSection.Text = selectedDataRowView["Section"].ToString();
            txtCourseName.Text = selectedDataRowView["CourseName"].ToString();
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
        /// <summary>
        /// generates a unqiue Course id
        /// </summary>
        /// <returns>The course id</returns>
        private int CourseIDGenerator()
        {
            //starting studentID
            int suggestedValue = 101;
            List<int> StudentIDS = new List<int>();

            //where the data is loaded into the program
            using (MySqlConnection sqlConnection = new MySqlConnection(connectionString))
            {
                sqlConnection.Open();

                MySqlCommand commandSQL = new MySqlCommand("SELECT CourseID FROM Course", sqlConnection);

                using (MySqlDataReader reader = commandSQL.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        StudentIDS.Add(reader.GetInt32(0));
                    }
                }
            }

            StudentIDS.Sort();

            //where it checks if the studentID is unqiue while also keeping a suggested id open
            foreach (int id in StudentIDS)
            {
                if (id == suggestedValue)
                {
                    suggestedValue++;
                }
            }

            return suggestedValue;
        }
    }
}
