using System.Windows;
using System.Data.SqlClient;
using HIcompany.db;

namespace HIcompany.Pages
{
    public partial class Create_App_Win : Window
    {
        enum ApplicationStatus
        {
            Waiting,
            Confirmed,
            Canceled,
        }

        Database database = new Database();

        public Create_App_Win()
        {
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            string clientId = TextBox_ClientId.Text;
            string type = TextBox_Type.Text;
            DateTime claimDate = DateTime.Now;
            string claimAmount = TextBox_ClaimAmount.Text;
            string status = ApplicationStatus.Waiting.ToString();

            int ClientId = 0;
            long ClaimAmount = 0;
            if (int.TryParse(clientId, out ClientId))
                ClientId = int.Parse(clientId);
            else
                MessageBox.Show("Идентификатор некорректен!");
            if (long.TryParse(claimAmount, out ClaimAmount))
                ClaimAmount = long.Parse(claimAmount);
            else
                MessageBox.Show("Цена некорректная!");
            if (ClientId == 0 || type == "" || ClaimAmount == 0)
                MessageBox.Show("Не удалось зарегистрировать клиента!");
            else
            {
                try
                {
                    string query = $"INSERT INTO Claims(ClientId, Type, ClaimDate, ClaimAmount, ClaimStatus) VALUES ('{ClientId}', '{type}', '{claimDate}', '{ClaimAmount}', '{status}')";
                    SqlCommand command = new SqlCommand(query, database.GetConnection());
                    database.OpenConnection();

                    if (command.ExecuteNonQuery() == 1)
                        MessageBox.Show("Успешно!");
                    else
                        MessageBox.Show("Не удалось создать заявку!");

                    database.CloseConnection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_ClientId.Clear();
            TextBox_Type.Clear();
            TextBox_ClaimAmount.Clear();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Applications_Win app_Win = new Applications_Win();
            app_Win.Show();
            this.Close();
        }
    }
}
