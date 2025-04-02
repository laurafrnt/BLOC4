using System.Windows;

namespace WPF
{
    public partial class AdminDashboardWindow : Window
    {
        private readonly ApiService _apiService = new ApiService();

        public AdminDashboardWindow()
        {
            InitializeComponent();
        }

        private void ManageEmployees_Click(object sender, RoutedEventArgs e)
        {
            new EmployeeManagement().ShowDialog();
        }

        private void ManageSites_Click(object sender, RoutedEventArgs e)
        {
            new SiteManagement().ShowDialog();
        }

        private void ManageServices_Click(object sender, RoutedEventArgs e)
        {
            new ServiceManagement().ShowDialog();
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Déconnexion en cours...");
            this.Close();
        }
    }
}
