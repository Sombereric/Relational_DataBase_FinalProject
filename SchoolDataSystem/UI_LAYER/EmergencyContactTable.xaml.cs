//
// FILE        : EmergencyContactTable.xaml.cs
// PROJECT     : Final Database Project – Phase 5 (CRUD Operations)
// PROGRAMMER  : Kevin Napoles
// FIRST VERSION : 2025-12-08
// DESCRIPTION : 
//   UI logic for the EmergencyContact table. Loads contact records
//   and lets the user create, edit, and delete rows using ADO.NET.
//

using MySql.Data.MySqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;   // <-- added

namespace SchoolDataSystem.UI_LAYER
{
    //
    // CLASS : EmergencyContactTable
    // PURPOSE :
    //   WPF window that displays and manages EmergencyContact data.
    //
    public partial class EmergencyContactTable : Window
    {
        private string connectionString = "Server=localhost;Uid=root;Pwd=HelloWorld;DataBase=SchoolDataBase;";
        private MySqlDataAdapter dataAdapter;
        private DataSet dataSet;

        //
        // FUNCTION : EmergencyContactTable
        // DESCRIPTION :
        //   Constructor. Initializes the window and loads the data.
        //
        public EmergencyContactTable()
        {
            InitializeComponent();
            LoadContactTable();
        }

        //
        // FUNCTION : LoadContactTable
        // DESCRIPTION :
        //   Reads all rows from the EmergencyContact table and shows them in the DataGrid.
        //
        private void LoadContactTable()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                dataAdapter = new MySqlDataAdapter("SELECT * FROM EmergencyContact", connection);
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "emergencyContact");

                dgContacts.ItemsSource = dataSet.Tables["emergencyContact"].DefaultView;
            }
        }

        //
        // FUNCTION : btnCreate_Click
        // DESCRIPTION :
        //   Creates a new EmergencyContact row using the values in the text boxes.
        //
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            int contactID;
            int studentID;

            if (!int.TryParse(txtContactID.Text, out contactID))
            {
                MessageBox.Show("Contact ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtStudentID.Text, out studentID))
            {
                MessageBox.Show("Student ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRow newRow = dataSet.Tables["emergencyContact"].NewRow();

            newRow["ContactID"] = contactID;
            newRow["StudentID"] = studentID;
            newRow["ContactName"] = txtContactName.Text;
            newRow["ContactAddress"] = txtContactAddress.Text;
            newRow["ContactPhoneNumber"] = txtContactPhoneNumber.Text;
            newRow["RelationshipToStudent"] = txtRelationshipToStudent.Text;

            dataSet.Tables["emergencyContact"].Rows.Add(newRow);

            SaveChanges();
            LoadContactTable();
        }

        //
        // FUNCTION : btnUpdate_Click
        // DESCRIPTION :
        //   Updates the selected EmergencyContact row using the values in the text boxes.
        //
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgContacts.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            int contactID;
            int studentID;

            if (!int.TryParse(txtContactID.Text, out contactID))
            {
                MessageBox.Show("Contact ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }
            if (!int.TryParse(txtStudentID.Text, out studentID))
            {
                MessageBox.Show("Student ID must be a valid number.", "Invalid Input", MessageBoxButton.OK);
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgContacts.SelectedItem;

            selectedDataRowView["ContactID"] = contactID;
            selectedDataRowView["StudentID"] = studentID;
            selectedDataRowView["ContactName"] = txtContactName.Text;
            selectedDataRowView["ContactAddress"] = txtContactAddress.Text;
            selectedDataRowView["ContactPhoneNumber"] = txtContactPhoneNumber.Text;
            selectedDataRowView["RelationshipToStudent"] = txtRelationshipToStudent.Text;

            SaveChanges();
            MessageBox.Show("Updated!");
            LoadContactTable();
        }

        //
        // FUNCTION : btnDelete_Click
        // DESCRIPTION :
        //   Deletes the selected EmergencyContact row.
        //
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgContacts.SelectedItem == null)
            {
                MessageBox.Show("Select a row first.");
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgContacts.SelectedItem;
            selectedDataRowView.Row.Delete();

            SaveChanges();
            LoadContactTable();
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

                dataAdapter = new MySqlDataAdapter("SELECT * FROM EmergencyContact", connection);
                MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                dataAdapter.Update(dataSet, "emergencyContact");
            }
        }

        //
        // FUNCTION : dgContacts_SelectionChanged
        // DESCRIPTION :
        //   Copies the selected row values into the text boxes for editing.
        //
        private void dgContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgContacts.SelectedItem == null)
            {
                return;
            }

            DataRowView selectedDataRowView = (DataRowView)dgContacts.SelectedItem;

            txtContactID.Text = selectedDataRowView["ContactID"].ToString();
            txtStudentID.Text = selectedDataRowView["StudentID"].ToString();
            txtContactName.Text = selectedDataRowView["ContactName"].ToString();
            txtContactAddress.Text = selectedDataRowView["ContactAddress"].ToString();
            txtContactPhoneNumber.Text = selectedDataRowView["ContactPhoneNumber"].ToString();
            txtRelationshipToStudent.Text = selectedDataRowView["RelationshipToStudent"].ToString();
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
