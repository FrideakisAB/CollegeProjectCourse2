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
using System.Data.SqlClient;

namespace CollegeProjectCourse2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;

        public MainWindow()
        {
            string dbFilePath = System.IO.Directory.GetCurrentDirectory() + "\\Database.mdf";
            connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + dbFilePath + ";Integrated Security=True");
            connection.Open();

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            connection.Close();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            createUserMenu.Visibility = Visibility.Visible;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            createUserMenu.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            viewerUserMenu.Visibility = Visibility.Visible;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            viewerUserMenu.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // Need check all values, and return if data broken
            SqlCommand command = new SqlCommand(
                "INSERT INTO Players (name, history, legend, forces, family, gender, fireball, vampire, image)" +
                $"VALUES (N'{createName.Text}', N'{createHistory.Text}', N'{createLegend.Text}', N'{createForces.Text}', N'{createFamily.Text}', " +
                $"{(((bool)createGender.IsChecked) ? 1 : 0)}, {(((bool)createFire.IsChecked) ? 1 : 0)}, {(((bool)createVamp.IsChecked) ? 1 : 0)}, 0)", connection); // Set image!!!
            command.ExecuteNonQuery();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connection.Close();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand($"SELECT COUNT(name) FROM Players WHERE name = N'{createName.Text}'", connection);
            int cnt = (Int32)command.ExecuteScalar();
            if (cnt == 0)
                createName.Text = "True"; // Test, on production display message
        }
    }
}
