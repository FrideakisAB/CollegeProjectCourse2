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
using System.Globalization;

namespace CollegeProjectCourse2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection connection;
        Game game;
        bool player1 = true;

        public MainWindow()
        {
            try
            {
                string dbFilePath = System.IO.Directory.GetCurrentDirectory() + "\\Database.mdf";
                connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + dbFilePath + ";Integrated Security=True");
                connection.Open();
            }
            catch (Exception ex)
            {
                string messageBoxText = ex.Message;
                string caption = "Запуск";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;

                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                this.Close();
            }

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

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            viewerResultMenu.Visibility = Visibility.Hidden;
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            viewerResultMenu.Visibility = Visibility.Visible;
            SqlCommand createCommand = new SqlCommand("SELECT Results.Id AS N'ID игры', time_point AS N'Время', name AS N'Имя', win AS N'Победа' " +
                                                      "FROM Results " +
                                                      "LEFT JOIN Players ON player1_id = Players.Id", connection);
            createCommand.ExecuteNonQuery();

            SqlDataAdapter dataAdp = new SqlDataAdapter(createCommand);
            DataTable dt = new DataTable("Results");
            dataAdp.Fill(dt);
            resultsList.ItemsSource = dt.DefaultView;
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            SqlCommand command = new SqlCommand(
                  "INSERT INTO Results (win, time_point, last_hp) " +
                  "VALUES (1, '20120618 10:34:09 AM', 12) " +
                  "INSERT INTO Results (win, time_point, last_hp) " +
                  "VALUES (0, '20120118 10:34:09 AM', 14) " +
                  "INSERT INTO Results (player1_id, win, time_point, last_hp) " +
                  "VALUES (1, 1, '20120618 10:14:09 AM', 12) ", connection);
            command.ExecuteNonQuery();
        }

        private void resultsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (resultsList.SelectedIndex != -1)
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Results WHERE Id = {((DataRowView)resultsList.SelectedCells.First().Item)["ID игры"]}", connection);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    resWin.Content = reader.GetBoolean(3)? "Победил игрок 1" : "Победил игрок 2";
                    resLifes.Content = $"С {reader.GetInt32(5)} жизнями";
                    resTime.Content = ((DataRowView)resultsList.SelectedCells.First().Item)["Время"].ToString();

                    resFirstName.Content = ((DataRowView)resultsList.SelectedCells.First().Item)["Имя"].ToString();

                    if (!reader.IsDBNull(2))
                    {
                        int s = reader.GetInt32(2);
                        reader.Close();
                        command = new SqlCommand($"SELECT name FROM Players WHERE Id = {s}", connection);
                        resSecondName.Content = (string)command.ExecuteScalar();
                    }
                    else
                    {
                        resSecondName.Content = "";
                        reader.Close();
                    }
                }
                else
                    reader.Close();
            }
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            startGame.Visibility = Visibility.Hidden;
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            startGame.Visibility = Visibility.Visible;

            SqlCommand command = new SqlCommand("SELECT name FROM Players", connection);
            SqlDataReader reader = command.ExecuteReader();

            List<string> pers = new List<string>();

            while (reader.Read())
                pers.Add(reader.GetString(0));
            reader.Close();

            startPers1.ItemsSource = pers;
            startPers2.ItemsSource = pers;
        }

        void updatePole()
        {
            if (player1)
            {
                updateGrid(g1, game.crads1[0], true);
                updateGrid(g2, game.crads1[1], true);
                updateGrid(g3, game.crads1[2], true);
                updateGrid(g4, game.crads1[3], true);
            }
            else
            {
                updateGrid(g1, game.crads2[0], true);
                updateGrid(g2, game.crads2[1], true);
                updateGrid(g3, game.crads2[2], true);
                updateGrid(g4, game.crads2[3], true);
            }

            updateGrid(g5, game.collode1.cards.ElementAtOrDefault(0));
            updateGrid(g6, game.collode1.cards.ElementAtOrDefault(1));
            updateGrid(g7, game.collode1.cards.ElementAtOrDefault(2));

            updateGrid(g8, game.collode2.cards.ElementAtOrDefault(0));
            updateGrid(g9, game.collode2.cards.ElementAtOrDefault(1));
            updateGrid(g10, game.collode2.cards.ElementAtOrDefault(2));
        }

        void updateGrid(Grid grid, Card card, bool c=false)
        {
            if (card == null)
            {
                grid.Visibility = Visibility.Hidden;
                return;
            }
            else
                grid.Visibility = Visibility.Visible;

            ((Label)grid.Children[1]).Content = card.Damage.ToString();
            ((Label)grid.Children[2]).Content = card.HP.ToString();

            if (c)
                ((Label)grid.Children[4]).Content = card.Cost.ToString();
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            if (startPers1.Text != startPers2.Text && startPers1.Text != "" && startPers2.Text != "")
            {
                int live1, d1, live2, d2;
                if (Int32.TryParse(startHP1.Text, out live1) && Int32.TryParse(startHP2.Text, out live2) && Int32.TryParse(startD1.Text, out d1) && Int32.TryParse(startD1.Text, out d2) && live1 > 0 && live2 > 0 && d1 > 0 && d2 > 0)
                {
                    gp1.Content = startPers1.Text;
                    gp2.Content = startPers2.Text;
                    game = new Game(d1, live1, d2, live2);
                    startGame.Visibility = Visibility.Hidden;
                    gameWin.Visibility = Visibility.Visible;

                    insert(game.crads1);
                    insert(game.crads1);
                    insert(game.crads1);
                    insert(game.crads2);
                    insert(game.crads2);
                    insert(game.crads2);

                    gameS1.Content = $"Сила {game.mana1}/8";
                    gameS2.Content = $"Сила {game.mana2}/8";

                    gameL1.Content = $"Здоровье: {game.SecPlayer1HP}";
                    gameL2.Content = $"Здоровье: {game.SecPlayer2HP}";

                    updatePole();
                }
                else
                {
                    string messageBoxText = "Введите корректные целые числа больше 0";
                    string caption = "Запуск игры";
                    MessageBoxButton button = MessageBoxButton.OK;
                    MessageBoxImage icon = MessageBoxImage.Error;
                    MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                }
            }
            else
            {
                string messageBoxText = "Выберите 2-х различных персонажей, или создайте их при необходимости";
                string caption = "Запуск игры";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var ddd = (Grid)sender;
            int i = Int32.Parse(ddd.Name.Replace("g", "")) - 1;

            if (player1)
            {
                if (game.UseCard1(game.crads1[i]))
                    game.crads1[i] = null;
            }
            else
            {
                if (game.UseCard2(game.crads2[i]))
                    game.crads2[i] = null;
            }

            updatePole();
        }

        Card[] cardsAll = new Card[5]{ new Card(0, 5, 3, 3), new Card(1, 9, 5, 8), new Card(2, 3, 3, 2), new Card(3, 2, 0, 1), new Card(4, 1, 3, 2) };

        void insert(Card[] cards)
        {
            Random rnd = new Random();
            for (int i = 0; i < cards.Length; ++i)
            {
                if (cards[i] == null)
                    cards[i] = cardsAll[rnd.Next(0, 4)].Clone();
            }
        }

        int PlayerNameToId(string name)
        {
            return (int)(new SqlCommand($"SELECT Id FROM Players WHERE name = N'{name}'", connection)).ExecuteScalar();
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            if (player1)
            {
                secPlayer.Content = "Игрок 2";
                game.Step1();
                insert(game.crads1);
            }
            else
            {
                secPlayer.Content = "Игрок 1";
                game.Step2();
                insert(game.crads2);
            }

            player1 = !player1;

            gameS1.Content = $"Сила {game.mana1}/8";
            gameS2.Content = $"Сила {game.mana2}/8";

            gameL1.Content = $"Здоровье: {game.SecPlayer1HP}";
            gameL2.Content = $"Здоровье: {game.SecPlayer2HP}";

            updatePole();

            if (game.GameIsFinish())
            {
                string messageBoxText = "Победил игрок " + ((Math.Max(game.SecPlayer1HP, game.SecPlayer2HP) == game.SecPlayer1HP) ? startPers1.Text : startPers2.Text);
                string caption = "Завершение игры";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;

                int id1 = PlayerNameToId(startPers1.Text);
                int id2 = PlayerNameToId(startPers2.Text);
                DateTime localDate = DateTime.Now;
                SqlCommand command = new SqlCommand(
                  "INSERT INTO Results (player1_id, player2_id, win, time_point, last_hp) " +
                  $"VALUES ({id1}, {id2}, {((Math.Max(game.SecPlayer1HP, game.SecPlayer2HP) == game.SecPlayer1HP) ? 1 : 0)}, '{localDate.ToString("yyyyMMdd hh:mm:ss tt")}', {Math.Max(game.SecPlayer1HP, game.SecPlayer2HP)}) ", connection);
                command.ExecuteNonQuery();

                MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);

                gameWin.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            gameWin.Visibility = Visibility.Hidden;
        }
    }
}
