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
using System.Data;

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
            SqlCommand createCommand = new SqlCommand("SELECT name AS N'Имя', history AS N'История' FROM Players", connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("Players");
            dataAdp.Fill(dt);
            playersList.ItemsSource = dt.DefaultView;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            viewerUserMenu.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (createName.Text == "" || createHistory.Text == "" || createLegend.Text == "" || createForces.Text == "" || createFamily.Text == "" || (!(bool)createWGender.IsChecked && !(bool)createGender.IsChecked))
            {
                string messageBoxText = "Вы не заполнили все текстовые поля или пол персонажа, пожалуйста заполните их и попробуйте снова";
                string caption = "Создание персонажа";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                return;
            }

            SqlCommand command = new SqlCommand($"SELECT COUNT(name) FROM Players WHERE name = N'{createName.Text}'", connection);
            int cnt = (Int32)command.ExecuteScalar();
            if (cnt == 0)
            {
                command = new SqlCommand(
                  "INSERT INTO Players (name, history, legend, forces, family, gender, fireball, vampire, image)" +
                  $"VALUES (N'{createName.Text}', N'{createHistory.Text}', N'{createLegend.Text}', N'{createForces.Text}', N'{createFamily.Text}', " +
                  $"{(((bool)createGender.IsChecked) ? 1 : 0)}, {(((bool)createFire.IsChecked) ? 1 : 0)}, {(((bool)createVamp.IsChecked) ? 1 : 0)}, 0)", connection); // Set image!!!
                command.ExecuteNonQuery();
            }
            else
            {
                string messageBoxText = "Имя персонажа занято, персонаж не создан";
                string caption = "Создание персонажа";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
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
            {
                string messageBoxText = "Имя персонажа свободно";
                string caption = "Проверка имени";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
            else
            {
                string messageBoxText = "Имя персонажа занято";
                string caption = "Проверка имени";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private void playersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (playersList.SelectedIndex != -1)
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Players WHERE name = N'{((DataRowView)playersList.SelectedCells.First().Item)["Имя"]}'", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    viewName.Content = reader.GetString(1);
                    viewHistory.Content = reader.GetString(2);
                    viewLegend.Content = reader.GetString(3);
                    viewForce.Content = reader.GetString(4);
                    viewFamily.Content = reader.GetString(5);

                    if (reader.GetBoolean(6))
                        viewMen.IsChecked = true;
                    else
                        viewWomen.IsChecked = true;

                    viewFireball.IsChecked = reader.GetBoolean(7);
                    viewVamp.IsChecked = reader.GetBoolean(8);

                    int image = reader.GetInt32(9);
                }

                reader.Close();
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (playersList.SelectedIndex != -1)
            {
                string messageBoxText = "Вы хотите удалить этого персонажа?";
                string caption = "Удаление персонажа";
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;

                if (MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes) != MessageBoxResult.Yes)
                    return;

                SqlCommand command = new SqlCommand($"DELETE FROM Players WHERE name = N'{((DataRowView)playersList.SelectedCells.First().Item)["Имя"]}'" +
                                                     "SELECT name AS N'Имя', history AS N'История' FROM Players", connection);
                playersList.ItemsSource = null;
                command.ExecuteNonQuery();

                SqlDataAdapter dataAdp = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Players");
                dataAdp.Fill(dt);
                playersList.ItemsSource = dt.DefaultView;

                viewName.Content = "";
                viewHistory.Content = "";
                viewLegend.Content = "";
                viewForce.Content = "";
                viewFamily.Content = "";

                viewMen.IsChecked = false;
                viewWomen.IsChecked = false;

                viewFireball.IsChecked = false;
                viewVamp.IsChecked = false;
            }
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (playersList.SelectedIndex != -1 && playersList.SelectedIndex < playersList.Items.Count)
                ++playersList.SelectedIndex;
            else
                playersList.SelectedIndex = 0;
        }
    }
}
