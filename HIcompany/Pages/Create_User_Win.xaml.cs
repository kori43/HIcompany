using HIcompany.db;
using System.Data.SqlClient;
using System.Windows;


namespace HIcompany.Pages
{
    public partial class Create_User_Win : Window
    {
        Database database = new Database();
        public Create_User_Win()
        {
            InitializeComponent();
            LoadRolesIntoComboBox();
        }

        private void LoadRolesIntoComboBox()
        {
            try
            {
                database.OpenConnection();

                string query = "SELECT Role FROM Role";
                SqlCommand command = new SqlCommand(query, database.GetConnection());

                SqlDataReader reader = command.ExecuteReader();

                ComboBox_Role.Items.Clear();

                while (reader.Read())
                {
                    string role = reader["Role"].ToString();
                    ComboBox_Role.Items.Add(role);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке ролей: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }
        }


        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBox_Username.Text;
            string password = PasswordBox_Password.Password;
            string role = ComboBox_Role.SelectedItem.ToString();
            int roleId = CheckRole(role);

            if (username == "" || password == "" || role == "")
            {
                MessageBox.Show("Не удалось зарегистрировать пользователя!");
                return;
            }

            try
            {
                string query = $"INSERT INTO Users(Username, Password, Role) VALUES " +
                    $"(@Username, @Password, @Role)";
                SqlCommand command = new SqlCommand(query, database.GetConnection());

                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Role", roleId);

                database.OpenConnection();

                if (command.ExecuteNonQuery() == 1)
                    MessageBox.Show("Успешно!");
                else
                    MessageBox.Show("Не удалось зарегистрировать пользователя!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }
        }

        static int CheckRole(string role)
        {
            if (role == "Администратор")
                return 1;
            else if (role == "Оператор")
                return 2;
            else
                return 0;
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_Username.Clear();
            PasswordBox_Password.Clear();
            ComboBox_Role.Text = "";
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Admin_Win admin_Win = new Admin_Win();
            admin_Win.Show();
            this.Close();
        }
    }
}
