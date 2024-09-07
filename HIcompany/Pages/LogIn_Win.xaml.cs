using System.Windows;
using System.Data.SqlClient;
using HIcompany.db;
using HIcompany.Pages;

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

            string query = "SELECT Id, Username, Role FROM Users WHERE Username = @Username AND Password";

            database.OpenConnection();

            SqlCommand command = new SqlCommand(query, database.GetConnection());

            command.Parameters.AddWithValue("@Username", username);
            command.Parameters.AddWithValue("@Password", userpassword);

            SqlDataReader reader = command.ExecuteReader();
            if(reader.Read())
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
                            Console.WriteLine();
                            this.Close();
                            break;
                        case 2:
                            Console.WriteLine();
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
            }            
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Login.Clear();
            PasswordBox_Password.Clear();
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            SignUp_Win signUpWin = new SignUp_Win();
            signUpWin.Show();
            this.Close();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}