using ClassLibrary;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace WPF
{
    public partial class AddEmployeeManagement : Window
    {
        private ApiService ApiService = new ApiService();

        public AddEmployeeManagement()
        {
            InitializeComponent();
            LoadSitesAndServices();
        }

        private async void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs saisies par l'utilisateur
            string lastName = LastNameTextBox.Text;
            string firstName = FirstNameTextBox.Text;
            string email = EmailTextBox.Text;
            string phone = PhoneNumberTextBox.Text;
            DateTime? birthDate = BirthDatePicker.SelectedDate; // Récupérer la date sélectionnée par user

            if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(email) || birthDate == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Vérifie que l'utilisateur a bien sélectionné un site et un service
            if (SiteComboBox.SelectedItem is Site selectedSite && ServiceComboBox.SelectedItem is Service selectedService)
            {
                Employee newEmployee = new Employee
                {
                    LastName = lastName,
                    FirstName = firstName,
                    Email = email,
                    PhoneNumber = phone,
                    Birthday = birthDate.Value,
                    IdSite = (int)SiteComboBox.SelectedValue,
                    IdService = (int)ServiceComboBox.SelectedValue
                };


                try
                {
                    await ApiService.AddEmployeeAsync(newEmployee);
                    MessageBox.Show("Employé ajouté avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de l'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un site et un service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Annuler
        }

        private async void LoadSitesAndServices()
        {
            try
            {
                var sites = await ApiService.GetSitesAsync();
                var services = await ApiService.GetServicesAsync();

                SiteComboBox.ItemsSource = sites;
                SiteComboBox.DisplayMemberPath = "City"; // Ce qui s'affichera dans la ComboBox
                SiteComboBox.SelectedValuePath = "IdSite"; // Ce qui sera récupéré comme valeur sélectionnée

                ServiceComboBox.ItemsSource = services;
                ServiceComboBox.DisplayMemberPath = "Name";
                ServiceComboBox.SelectedValuePath = "IdService";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des données : " + ex.Message);
            }
        }

    }
}
