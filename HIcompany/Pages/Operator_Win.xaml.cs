using System.Windows;
using System.Data.SqlClient;
using HIcompany.db;
using System.Collections.ObjectModel;
using HIcompany.Classes;

namespace HIcompany.Pages
{
    public partial class Operator_Win : Window
    {
        Database database = new Database();
        private ObservableCollection<Clients> clients = new ObservableCollection<Clients>();
        enum ApplicationStatus : ushort
        {
            waiting = 0,
            accept = 1,
            cancel = 2,
        }
        public Operator_Win()
        {
            InitializeComponent();
            DGClients.ItemsSource = clients;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClients();
        }

        private void LoadClients()
        {
            try
            {
                clients.Clear();
                database.OpenConnection();
                string query = "SELECT * FROM Clients";

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Clients client = new Clients
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Phone = reader["Phone"].ToString()
                    };
                    clients.Add(client);
                }
                database.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке клиентов: " + ex.Message);
            }
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            SignUp_Win signUpWin = new SignUp_Win();
            signUpWin.Show();
            this.Close();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clients selectedClient = DGClients.SelectedItem as Clients;
                if (selectedClient == null)
                {
                    MessageBox.Show("Выберите клиента для удаления.", "Внимание");
                    return;
                }
                int id = selectedClient.Id;
                bool success = DeleteClient(id);
                if (success)
                {
                    LoadClients();
                    MessageBox.Show("Клиент успешно удален!");
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении записи");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
            }
        }

        private bool DeleteClient(int Id)
        {
            try
            {
                var selectedcClient = clients.FirstOrDefault(item => item.Id == Id);

                clients.Remove(selectedcClient);

                database.OpenConnection();

                string query = "DELETE FROM Clients WHERE id = @id";

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                command.Parameters.AddWithValue("@id", Id);
                command.ExecuteNonQuery();

                database.CloseConnection();
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении записи: " + ex.Message);
                return false;
            }
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
