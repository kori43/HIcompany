using HIcompany.Classes;
using HIcompany.db;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace HIcompany.Pages
{
    public partial class Admin_Win : Window
    {
        Database database = new Database();
        private ObservableCollection<Users> users = new ObservableCollection<Users>();
        public Admin_Win()
        {
            InitializeComponent();
            DGUsers.ItemsSource = users;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                users.Clear();

                database.OpenConnection();

                string query = "SELECT * FROM Users";

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Users user = new Users
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        RoleNum = Convert.ToInt32(reader["Role"])
                    };
                    users.Add(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке пользователей: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
