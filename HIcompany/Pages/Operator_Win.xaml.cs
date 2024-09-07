using System.Windows;

namespace HIcompany.Pages
{
    public partial class Operator_Win : Window
    {
        public Operator_Win()
        {
            InitializeComponent();
        }

        private void Btn_Registration_Click(object sender, RoutedEventArgs e)
        {
            SignUp_Win signUpWin = new SignUp_Win();
            signUpWin.Show();
            this.Close();
        }


    }
}
