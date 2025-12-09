using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SchoolDataSystem.UI_LAYER;

namespace SchoolDataSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStudents_Click(object sender, RoutedEventArgs e)
        {
            StudentTable studentTable = new StudentTable();
            this.Hide();
            studentTable.ShowDialog();
            this.Show();
        }

        private void btnProgram_Click(object sender, RoutedEventArgs e)
        {
            ProgramTable programWindow = new ProgramTable();
            this.Hide();
            programWindow.ShowDialog();
            this.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            ProgramTable programTable = new ProgramTable();
            programTable.ShowDialog();
        }

        private void btnEnrollment_Click(object sender, RoutedEventArgs e)
        {
            EnrollmentTable enrollmentTable = new EnrollmentTable();
            enrollmentTable.ShowDialog();
        }

        private void btnCourses_Click(object sender, RoutedEventArgs e)
        {
            CourseTable courseTable = new CourseTable();
            courseTable.ShowDialog();
        }

        private void btnEmergencyContact_Click(object sender, RoutedEventArgs e)
        {
            EmergencyContactTable emergencyTable = new EmergencyContactTable();
            emergencyTable.ShowDialog();
        }
    }
}
