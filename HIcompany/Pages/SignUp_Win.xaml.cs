using System.Windows;

namespace HIcompany.Pages
{
    public partial class SignUp_Win : Window
    {

        public SignUp_Win()
        {
            InitializeComponent();
        }

        private void Btn_Create_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            TextBox_FirstName.Clear();
            TextBox_LastName.Clear();
            TextBox_DateOfBirth.Clear();
            TextBox_Phone.Clear();
        }

        private void Btn_Back_Click(object sender, RoutedEventArgs e)
        {
            // todo возврат на окно оператора
            this.Close();
        }
    }
}
