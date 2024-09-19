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
            Waiting,
            Accept,
            Cancel
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заявок: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
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

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            Create_App_Win create_App_Win = new Create_App_Win();
            create_App_Win.Show();
            this.Close();
        }

        private void Btn_Accept_Status_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Claims selectedClaim = DGClaims.SelectedItem as Claims;

                if (selectedClaim == null)
                {
                    MessageBox.Show("Выберите заявку для принятия.", "Внимание");
                    return;
                }

                if (selectedClaim.ClaimStatus != ApplicationStatus.Waiting.ToString())
                {
                    MessageBox.Show("Заявка уже обработана.");
                    return;
                }

                DateTime startDate = DateTime.Now;
                DateTime endDate = startDate.AddYears(1);
                database.OpenConnection();
                SqlTransaction transaction = database.GetConnection().BeginTransaction();
                {
                    try
                    {
                        string claimQuery = "UPDATE Claims SET ClaimStatus = @Status WHERE Id = @ClaimId AND ClaimStatus = @CurrentStatus";                        
                        SqlCommand claimCommand = new SqlCommand(claimQuery, database.GetConnection(), transaction);
                        claimCommand.Parameters.AddWithValue("@Status", ApplicationStatus.Accept.ToString());
                        claimCommand.Parameters.AddWithValue("@ClaimId", selectedClaim.Id);
                        claimCommand.Parameters.AddWithValue("@CurrentStatus", ApplicationStatus.Waiting.ToString());
                        claimCommand.ExecuteNonQuery();                       

                        string policyQuery = "INSERT INTO Policies (ClientId, Type, Status, StartDate, EndDate) " +
                            "VALUES (@ClientId, @Type, @Status, @StartDate, @EndDate)";
                        SqlCommand policyCommand = new SqlCommand(policyQuery, database.GetConnection(), transaction);
                        policyCommand.Parameters.AddWithValue("@ClientId", selectedClaim.ClientId);
                        policyCommand.Parameters.AddWithValue("@Type", selectedClaim.Type);
                        policyCommand.Parameters.AddWithValue("@Status", "Accept");
                        policyCommand.Parameters.AddWithValue("@StartDate", startDate);
                        policyCommand.Parameters.AddWithValue("@EndDate", endDate);
                        policyCommand.ExecuteNonQuery();

                        transaction.Commit();

                        MessageBox.Show("Заявка успешно принята, полис создан!");

                        LoadClaims();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Ошибка при обработке заявки: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при принятии заявки: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }
        }

        private void Btn_Cancel_Status_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Claims selectedClaim = DGClaims.SelectedItem as Claims;

                if (selectedClaim == null)
                {
                    MessageBox.Show("Выберите заявку для отклонения.", "Внимание");
                    return;
                }

                if (selectedClaim.ClaimStatus != ApplicationStatus.Waiting.ToString())
                {
                    MessageBox.Show("Заявка уже обработана.");
                    return;
                }

                string query = "UPDATE Claims SET ClaimStatus = @Status WHERE Id = @ClaimId AND ClaimStatus = @CurrentStatus";

                database.OpenConnection();

                SqlCommand command = new SqlCommand(query, database.GetConnection());

                command.Parameters.AddWithValue("@Status", ApplicationStatus.Cancel.ToString());
                command.Parameters.AddWithValue("@ClaimId", selectedClaim.Id);
                command.Parameters.AddWithValue("@CurrentStatus", ApplicationStatus.Waiting.ToString());

                command.ExecuteNonQuery();

                MessageBox.Show("Заявка успешно отклонена!");

                LoadClaims();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка при отклонении заявки: " + ex.Message);
            }
            finally
            {
                database.CloseConnection();
            }
        }
    }
}
