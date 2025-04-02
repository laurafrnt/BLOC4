using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Logique d'interaction pour AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        private ApiService ApiService = new ApiService();


        public AdminLoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string enteredPassword = PasswordBox.Password;
            bool isAuthenticated = await ApiService.VerifyPasswordAsync(enteredPassword);

            if (isAuthenticated)
            {
                this.DialogResult = true;
                new AdminDashboardWindow().ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Mot de passe incorrect", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}