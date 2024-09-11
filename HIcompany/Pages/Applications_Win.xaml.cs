using HIcompany.Classes;
using HIcompany.db;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;

namespace HIcompany.Pages
{
    public partial class Applications_Win : Window
    {
        enum ApplicationStatus : ushort
        {
            waiting = 0,
            accept = 1,
            cancel = 2,
        }

        Database database = new Database();
        private ObservableCollection<Claims> claims = new ObservableCollection<Claims>();

        public Applications_Win()
        {
            InitializeComponent();
            DGClaims.ItemsSource = claims;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadClaims();
        }

        private void LoadClaims()
        {
            try
            {
                claims.Clear();
                database.OpenConnection();
                string query = "SELECT * FROM Claims";

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Claims claim = new Claims
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        ClientId = Convert.ToInt32(reader["ClientId"]),
                        Type = reader["Type"].ToString(),
                        ClaimDate = Convert.ToDateTime(reader["ClaimDate"]),
                        ClaimAmount = Convert.ToInt32(reader["ClaimAmount"]),
                        ClaimStatus = reader["ClaimStatus"].ToString()
                    };
                    claims.Add(claim);
                }
                database.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заявок: " + ex.Message);
            }
        }

        private void Clients_Click(object sender, RoutedEventArgs e)
        {
            Operator_Win operator_Win = new Operator_Win();
            operator_Win.Show();
            this.Close();
        }

        private void Policies_Click(object sender, RoutedEventArgs e)
        {
            Policies_Win policies = new Policies_Win();
            policies.Show();
            this.Close();
        }

        private void Btn_Edit_Status_Click(object sender, RoutedEventArgs e)
        {
            // to do изменение статуса
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            Create_App_Win create_App_Win = new Create_App_Win();
            create_App_Win.Show();
            this.Close();
        }
    }
}
