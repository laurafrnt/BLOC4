using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPF
{
    /// <summary>
    /// Logique d'interaction pour EmployeeManagement.xaml
    /// </summary>
    public partial class EmployeeManagement : Window
    {
        private ApiService ApiService = new ApiService();

        public EmployeeManagement()
        {
            InitializeComponent();
            LoadEmployees();
        }

        // Charger la liste des employés au démarrage
        private async void LoadEmployees()
        {
            try
            {
                var employees = await ApiService.GetEmployeesAsync();
                var sites = await ApiService.GetSitesAsync();
                var services = await ApiService.GetServicesAsync();

                // Associer les sites et services aux employés
                foreach (var employee in employees)
                {
                    employee.Site = sites.FirstOrDefault(s => s.IdSite == employee.IdSite);
                    employee.Service = services.FirstOrDefault(s => s.IdService == employee.IdService);
                }

                EmployeeDataGrid.ItemsSource = employees;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }


        // Ajouter un employé
        private async void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            new AddEmployeeManagement().ShowDialog();
            LoadEmployees();
        }

        // Modifier un employé
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDataGrid.SelectedItem is Employee selectedEmployee)
            {
                EditEmployeeManagement editWindow = new EditEmployeeManagement(selectedEmployee);
                editWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un employé à modifier.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            LoadEmployees();
        }


        // Supprimer un employé
        private async void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Tag is Employee employee)
            {
                var result = MessageBox.Show($"Voulez-vous vraiment supprimer {employee.FirstName} {employee.LastName} ?",
                                             "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    await ApiService.DeleteEmployeeAsync(employee.IdEmployee);
                    LoadEmployees(); // Rafraîchir les données après suppression
                }
            }
        }
    }
}
