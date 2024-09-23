using HIcompany.Classes;
using HIcompany.db;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace HIcompany.Pages
{
    public partial class Policies_Win : Window
    {

        Database database = new Database();
        private ObservableCollection<Policies> policies = new ObservableCollection<Policies>();
        public Policies_Win()
        {
            InitializeComponent();
            DGPolicies.ItemsSource = policies;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPolicies();
        }

        private void LoadPolicies()
        {
            try
            {
                policies.Clear();

                database.OpenConnection();

                string query = "SELECT * FROM Policies";

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Policies policy = new Policies()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Type = reader["Type"].ToString(),
                        Status = reader["Status"].ToString(),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                    };

                    if (policy.EndDate < DateTime.Now && policy.Status != ApplicationStatus.Expired.ToString())
                    {
                        UpdatePolicyStatus(policy.Id);
                        policy.Status = ApplicationStatus.Expired.ToString();
                    }

                    policies.Add(policy);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке полисов: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }
        }

        private void UpdatePolicyStatus(int policyId)
        {
            try
            {
                string query = "UPDATE Policies SET Status = @Status WHERE Id = @Id";

                SqlCommand command = new SqlCommand(query, database.GetConnection());
                command.Parameters.AddWithValue("@Status", ApplicationStatus.Expired);
                command.Parameters.AddWithValue("@Id", policyId);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении статуса полиса: " + ex.Message);
            }
        }

        private void Applications_Click(object sender, RoutedEventArgs e)
        {
            Applications_Win applications = new Applications_Win();
            applications.Show();
            this.Close();
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Operator_Win operator_Win = new Operator_Win();
            operator_Win.Show();
            this.Close();
        }

        private void Btn_Update_Click(object sender, RoutedEventArgs e)
        {
            LoadPolicies();
        }
    }
}
