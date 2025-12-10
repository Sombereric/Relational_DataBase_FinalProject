//
// FILE        : EnrollmentTable.xaml.cs
// PROJECT     : Final Database Project – Phase 5 (CRUD Operations)
// PROGRAMMER  : Kevin Napoles
// FIRST VERSION : 2025-12-08
// DESCRIPTION : 
//   UI logic for interacting with the Enrollment table.
//   Loads Enrollment records and lets the user create, edit, and delete
//   rows using ADO.NET.
//

using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace SchoolDataSystem.UI_LAYER
{
    //
    // CLASS : EnrollmentTable
    // PURPOSE :
    //   WPF window that displays and manages Enrollment data.
    //
    public partial class EnrollmentTable : Window
    {
        private string connectionString = "Server=localhost;Uid=root;Pwd=root;Database=SchoolDataBase;";
        private MySqlDataAdapter dataAdapter;
        private DataSet dataSet;

        //
        // FUNCTION : EnrollmentTable
        // DESCRIPTION :
        //   Constructor. Initializes the window and loads the data.
        //
        public EnrollmentTable()
        {
            InitializeComponent();
            LoadEnrollmentTable();
        }

        //
        // FUNCTION : LoadEnrollmentTable
        // DESCRIPTION :
        //   Reads all rows from the Enrollment table and shows them in the DataGrid.
        //
        private void LoadEnrollmentTable()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                dataAdapter = new MySqlDataAdapter("SELECT * FROM Enrollment", connection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "enrollment");

                dgEnrollments.ItemsSource = dataSet.Tables["enrollment"].DefaultView;
            }
        }

        //
        // FUNCTION : btnCreate_Click
        // DESCRIPTION :
        //   Creates a new Enrollment row using the values in the text boxes.
        //
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int enrollmentID;
            int studentID;
            int courseID;
            int termValue;
            int gradeValue;

            if (!int.TryParse(txtEnrollmentID.Text, out enrollmentID))
            {
                MessageBox.Show("Enrollment ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtStudentID.Text, out studentID))
            {
                MessageBox.Show("Student ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtCourseID.Text, out courseID))
            {
                MessageBox.Show("Course ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtTerm.Text, out termValue))
            {
                MessageBox.Show("Term must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtGrade.Text, out gradeValue))
            {
                MessageBox.Show("Grade must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRow newRow = dataSet.Tables["enrollment"].NewRow();

            newRow["EnrollmentID"] = enrollmentID;
            newRow["StudentID"] = studentID;
            newRow["CourseID"] = courseID;
            newRow["Term"] = termValue;
            newRow["Grade"] = gradeValue;

            dataSet.Tables["enrollment"].Rows.Add(newRow);

            SaveChanges();
            LoadEnrollmentTable();
        }

        //
        // FUNCTION : btnUpdate_Click
        // DESCRIPTION :
        //   Updates the selected Enrollment row using the values in the text boxes.
        //
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgEnrollments.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            int enrollmentID;
            int studentID;
            int courseID;
            int termValue;
            int gradeValue;

            if (!int.TryParse(txtEnrollmentID.Text, out enrollmentID))
            {
                MessageBox.Show("Enrollment ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtStudentID.Text, out studentID))
            {
                MessageBox.Show("Student ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtCourseID.Text, out courseID))
            {
                MessageBox.Show("Course ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtTerm.Text, out termValue))
            {
                MessageBox.Show("Term must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtGrade.Text, out gradeValue))
            {
                MessageBox.Show("Grade must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgEnrollments.SelectedItem;

            selectedDataRowView["EnrollmentID"] = enrollmentID;
            selectedDataRowView["StudentID"] = studentID;
            selectedDataRowView["CourseID"] = courseID;
            selectedDataRowView["Term"] = termValue;
            selectedDataRowView["Grade"] = gradeValue;

            SaveChanges();
            MessageBox.Show("Updated!");
            LoadEnrollmentTable();
        }

        //
        // FUNCTION : btnDelete_Click
        // DESCRIPTION :
        //   Deletes the selected Enrollment row.
        //
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgEnrollments.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgEnrollments.SelectedItem;
            selectedDataRowView.Row.Delete();

            SaveChanges();
            LoadEnrollmentTable();
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

                dataAdapter = new MySqlDataAdapter("SELECT * FROM Enrollment", connection);
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                dataAdapter.Update(dataSet, "enrollment");
            }
        }

        //
        // FUNCTION : dgEnrollments_SelectionChanged
        // DESCRIPTION :
        //   Copies the selected row values into the text boxes for editing.
        //
        private void dgEnrollments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEnrollments.SelectedItem == null)
            {
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgEnrollments.SelectedItem;

            txtEnrollmentID.Text = selectedDataRowView["EnrollmentID"].ToString();
            txtStudentID.Text = selectedDataRowView["StudentID"].ToString();
            txtCourseID.Text = selectedDataRowView["CourseID"].ToString();
            txtTerm.Text = selectedDataRowView["Term"].ToString();
            txtGrade.Text = selectedDataRowView["Grade"].ToString();
        }

        //
        // FUNCTION : btnBack_Click
        // DESCRIPTION :
        //   Closes this window and returns to the previous screen.
        //
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
