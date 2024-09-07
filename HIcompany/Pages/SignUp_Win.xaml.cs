using System.Windows;
using System.Data.SqlClient;
using HIcompany.db;
using System.Data;

namespace HIcompany.Pages
{
    public partial class SignUp_Win : Window
    {
        Database database = new Database();
        public SignUp_Win()
        {
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUser())
                return;
            string firstName = TextBox_FirstName.Text;
            string lastName = TextBox_LastName.Text;
            DateTime date = Convert.ToDateTime(DatePicker.Text);
            string phone = TextBox_Phone.Text;
            try
            {
                string query = $"INSERT INTO Clients(FirstName, LastName, DateOfBirth, Phone) VALUES ('{firstName}', '{lastName}', '{date}', '{phone}')";
                SqlCommand command = new SqlCommand(query, database.GetConnection());
                database.OpenConnection();
                if (firstName == "" || lastName == "" || date == null || phone == "")
                    MessageBox.Show("Не удалось зарегистрировать клиента!");
                else
                {
                    if (command.ExecuteNonQuery() == 1)
                        MessageBox.Show("Успешно!");
                    else
                        MessageBox.Show("Не удалось зарегистрировать клиента!");
                }
                database.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }         
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_FirstName.Clear();
            TextBox_LastName.Clear();
            TextBox_Phone.Clear();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            // todo возврат на окно оператора
            this.Close();
        }

        private bool CheckUser()
        {
            string firstname = TextBox_FirstName.Text;
            string lastName = TextBox_LastName.Text;
            DateTime dateofbirth = Convert.ToDateTime(DatePicker.Text);
            string phone = TextBox_Phone.Text;
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string query = $"SELECT * from Clients WHERE Phone = '{phone}'";

            SqlCommand command = new SqlCommand(query, database.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if(table.Rows.Count > 0)
            {
                MessageBox.Show("Такой клиент уже существует!");
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
