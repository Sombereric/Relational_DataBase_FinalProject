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
            textBoxDisable();
            BtnDelete.IsEnabled = false;
            LoadStudentData();
        }
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
                //enables the text boxes for editing
                textBoxEnable();
                BtnDelete.IsEnabled = true;    
            }
        }
        /// <summary>
        /// disables text boxes on program startup
        /// </summary>
        private void textBoxDisable()
        {
            tbStudentID.IsEnabled = false;
            tbFirstName.IsEnabled = false;
            tbLastName.IsEnabled = false;
            tbAddress.IsEnabled = false;
            tbPhoneNumber.IsEnabled = false;
            tbDateBirth.IsEnabled = false;
            tbProgramID.IsEnabled = false;
        }
        /// <summary>
        /// enables all text boxes once a data point is filled
        /// </summary>
        private void textBoxEnable()
        {
            tbStudentID.IsEnabled = true;
            tbFirstName.IsEnabled = true;
            tbLastName.IsEnabled = true;
            tbAddress.IsEnabled = true;
            tbPhoneNumber.IsEnabled = true;
            tbDateBirth.IsEnabled = true;
            tbProgramID.IsEnabled = true;
        }
    }
}