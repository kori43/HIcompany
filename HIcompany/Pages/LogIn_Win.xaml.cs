using HIcompany.db;
using HIcompany.Pages;
using System.Data.SqlClient;
using System.Windows;

namespace HIcompany
{
    public partial class LogIn_Win : Window
    {
        Database database = new Database();
        public LogIn_Win()
        {
            InitializeComponent();
        }

        private void Btn_Entry_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBox_Login.Text;
            string userpassword = PasswordBox_Password.Password;
            if (username == "")
                MessageBox.Show("Введите логин!");
            else if (userpassword == "")
                MessageBox.Show("Введите пароль!");
            else
            {
                string query = "SELECT Id, Username, Role FROM Users WHERE Username = @Username AND Password = @Password";

                database.OpenConnection();

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", userpassword);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int Id = reader.GetInt32(0);
                    string userName = reader.GetString(1);
                    int Role = reader.GetInt32(2);

                    try
                    {
                        database.OpenConnection();
                        switch (Role)
                        {
                            case 1:
                                Admin_Win admin = new Admin_Win();
                                admin.Show();
                                this.Close();
                                break;
                            case 2:
                                Operator_Win operator_Win = new Operator_Win();
                                operator_Win.Show();
                                this.Close();
                                break;
                            default:
                                MessageBox.Show("Некорректная роль пользователя!");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка авторизации: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Такого аккаунта не существует!");
                    database.CloseConnection();
                }
            }
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Login.Clear();
            PasswordBox_Password.Clear();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}