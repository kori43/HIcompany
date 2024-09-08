using HIcompany.db;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

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
            string firstName = TextBox_FirstName.Text;
            string lastName = TextBox_LastName.Text;
            string strphone = TextBox_Phone.Text;
            string strdate = DatePicker.Text;
            long phone = 0;
            if (long.TryParse(strphone, out phone))
                phone = long.Parse(strphone);
            else
                MessageBox.Show("Номер некорректен!");              
            if (firstName == "" || lastName == "" || phone == 0 || strdate == "")
                MessageBox.Show("Не удалось зарегистрировать клиента!");            
            else
            {
                if (CheckUser())
                    return;
                try
                {
                    DateTime date = Convert.ToDateTime(strdate);
                    string query = $"INSERT INTO Clients(FirstName, LastName, DateOfBirth, Phone) VALUES ('{firstName}', '{lastName}', '{date}', '{phone}')";
                    SqlCommand command = new SqlCommand(query, database.GetConnection());
                    database.OpenConnection();

                    if (command.ExecuteNonQuery() == 1)
                        MessageBox.Show("Успешно!");
                    else
                        MessageBox.Show("Не удалось зарегистрировать клиента!");

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
            TextBox_FirstName.Clear();
            TextBox_LastName.Clear();
            TextBox_Phone.Clear();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            Operator_Win operator_Win = new Operator_Win();
            operator_Win.Show();
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

            if (table.Rows.Count > 0)
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
