//
// FILE        : StudentTable.xaml.cs
// PROJECT     : Final Database Project – Phase 5 (CRUD Operations)
// PROGRAMMER  : Eric Moutoux
// FIRST VERSION : 2025-12-08
// DESCRIPTION : 
//   Where the STudent data is loaded and available for edit to the user
//   
//   
//

using System;
using System.Data;
using System.Data.Common;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace SchoolDataSystem.UI_LAYER
{
    /// <summary>
    /// Interaction logic for StudentTable.xaml
    /// </summary>
    public partial class StudentTable : Window
    {
        private string connectionString = "Server=localhost;Uid=root;Pwd=HelloWorld;DataBase=SchoolDataBase;";
        private MySqlDataAdapter sqlDataAdapter = null;
        private DataSet studentDataSet = null; 
        /// <summary>
        /// WPF on start setup
        /// </summary>
        public StudentTable()
        {
            InitializeComponent();
            BtnDelete.IsEnabled = false;
            BtnUpdate.IsEnabled = false;
            LoadStudentData();
        }
        /// <summary>
        /// Loads the database student table into the program for the user to interact with
        /// </summary>
        private void LoadStudentData()
        {
            try
            {
                //where the data is loaded into the program
                using (MySqlConnection sqlConnection = new MySqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    sqlDataAdapter = new MySqlDataAdapter("SELECT * FROM student", sqlConnection);

                    studentDataSet = new DataSet();

                    sqlDataAdapter.Fill(studentDataSet, "Student");

                    DGStudentList.ItemsSource = studentDataSet.Tables["Student"].DefaultView;
                }
            }
            //catches if the data from the database fails to load
            catch(Exception exception)
            {
                MessageBox.Show("Failed to Load Student Data: " + exception.Message);
            }
        }
        /// <summary>
        /// brings user back to the main menu
        /// </summary>
        /// <param name="sender">Object which triggered the event</param>
        /// <param name="e">Event Arguments</param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// when a user selectes a item from the datagrid it moves its values to the textboxes
        /// </summary>
        /// <param name="sender">Object which triggered the event</param>
        /// <param name="e">Event Arguments</param>
        private void DGStudentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DGStudentList.SelectedItem != null)
            {
                //brings all the data from the datagrid into the text boxes for editing
                DataRowView row = (DataRowView)DGStudentList.SelectedItem;
                tbStudentID.Text = row["StudentID"].ToString();
                tbFirstName.Text = row["FirstName"].ToString();
                tbLastName.Text = row["LastName"].ToString();
                tbAddress.Text = row["Address"].ToString();
                tbPhoneNumber.Text = row["PhoneNumber"].ToString();
                tbDateBirth.Text = row["DateofBirth"].ToString();
                tbProgramID.Text = row["ProgramId"].ToString();
                BtnUpdate.IsEnabled = true;
                BtnDelete.IsEnabled = true;    
            }
        }
        /// <summary>
        /// For creating a new student in the system
        /// </summary>
        /// <param name="sender">Object which triggered the event</param>
        /// <param name="e">Event Arguments</param>
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbStudentID.Text, out int StudentID))
            {
                MessageBox.Show("Student ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(tbProgramID.Text, out int programID))
            {
                MessageBox.Show("Program ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRow newRow = studentDataSet.Tables["program"].NewRow();

            newRow["StudentID"] = StudentID;
            newRow["FirstName"] = tbFirstName.Text;
            newRow["LastName"] = tbLastName.Text;
            newRow["Address"] = tbAddress.Text;
            newRow["PhoneNumber"] = tbPhoneNumber.Text;
            newRow["DateofBirth"] = tbDateBirth.Text;
            newRow["ProgramId"] = programID;

            studentDataSet.Tables["Student"].Rows.Add(newRow);

            SaveChanges();
            LoadStudentData();
        }
        /// <summary>
        /// Updates a student within the list
        /// </summary>
        /// <param name="sender">Object which triggered the event</param>
        /// <param name="e">Event Arguments</param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            MessageBox.Show("Updated!");
            LoadStudentData();
        }
        /// <summary>
        /// Deletes selected student
        /// </summary>
        /// <param name="sender">Object which triggered the event</param>
        /// <param name="e">Event Arguments</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DGStudentList.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            // Get selected row and delete it
            DataRowView selectedDataRowView = (DataRowView)DGStudentList.SelectedItem;
            selectedDataRowView.Row.Delete();

            SaveChanges();
            LoadStudentData();
        }
        /// <summary>
        /// Saves all changes from the data base to the server
        /// </summary>
        private void SaveChanges()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                sqlDataAdapter = new MySqlDataAdapter("SELECT * FROM Program", connection);

                // Auto-generate UPDATE / INSERT / DELETE commands
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(sqlDataAdapter);

                sqlDataAdapter.Update(studentDataSet, "program");
            }
        }
        /// <summary>
        /// Cloeses the wpf to return to main menu
        /// </summary>
        /// <param name="sender">Object which triggered the event</param>
        /// <param name="e">Event Arguments</param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}