using ClassLibrary;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace WPF
{
    public partial class EditEmployeeManagement : Window
    {
        private readonly ApiService _apiService = new ApiService();
        private readonly Employee _employee;

        public EditEmployeeManagement(Employee employee)
        {
            InitializeComponent();
            _employee = employee;
            LoadEmployeeData();
            LoadSitesAndServices();
        }

        private void LoadEmployeeData()
        {
            // Remplir les champs avec les données de l'employé à modifier
            LastNameTextBox.Text = _employee.LastName;
            FirstNameTextBox.Text = _employee.FirstName;
            EmailTextBox.Text = _employee.Email;
            PhoneNumberTextBox.Text = _employee.PhoneNumber;
            BirthDatePicker.SelectedDate = _employee.Birthday;
        }

        private async void UpdateEmployee_Click(object sender, RoutedEventArgs e)
        {
            _employee.LastName = LastNameTextBox.Text;
            _employee.FirstName = FirstNameTextBox.Text;
            _employee.Email = EmailTextBox.Text;
            _employee.PhoneNumber = PhoneNumberTextBox.Text;
            _employee.Birthday = BirthDatePicker.SelectedDate ?? DateTime.Now;

            if (SiteComboBox.SelectedItem is Site selectedSite)
            {
                _employee.IdSite = selectedSite.IdSite;
            }

            if (ServiceComboBox.SelectedItem is Service selectedService)
            {
                _employee.IdService = selectedService.IdService;
            }

            try
            {
                await _apiService.UpdateEmployeeAsync(_employee);
                MessageBox.Show("Employé mis à jour avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void LoadSitesAndServices()
        {
            try
            {
                var sites = await _apiService.GetSitesAsync();
                var services = await _apiService.GetServicesAsync();

                SiteComboBox.ItemsSource = sites;
                SiteComboBox.DisplayMemberPath = "City";
                SiteComboBox.SelectedValuePath = "IdSite";
                SiteComboBox.SelectedItem = sites.Find(s => s.IdSite == _employee.IdSite);

                ServiceComboBox.ItemsSource = services;
                ServiceComboBox.DisplayMemberPath = "Name";
                ServiceComboBox.SelectedValuePath = "IdService";
                ServiceComboBox.SelectedItem = services.Find(s => s.IdService == _employee.IdService);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
            }
        }
    }
}
