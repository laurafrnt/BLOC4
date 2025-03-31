using ClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class MainWindow : Window
    {
        private ApiService ApiService = new ApiService();

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();
        private List<Employee> AllEmployees = new List<Employee>(); // Stocke tous les employés pour filtrage

        private int currentPage = 1;
        private int itemsPerPage = 25; // Nombre d'employés par page

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            LoadEmployees();
            LoadFilters();
        }

        // Charge la liste complète des employés depuis l'API
        private async void LoadEmployees()
        {
            try
            {
                var employees = await ApiService.GetEmployeesAsync();
                AllEmployees = employees.ToList(); // Stocke la liste complète
                FilterChanged(null, null); // Applique les filtres avec pagination
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }

        // Charge les sites et services pour les filtres
        private async void LoadFilters()
        {
            try
            {
                var sites = await ApiService.GetSitesAsync();
                var services = await ApiService.GetServicesAsync();

                // Ajoute une option "Tous"
                sites.Insert(0, new Site { IdSite = 0, City = "Tous" });
                services.Insert(0, new Service { IdService = 0, Name = "Tous" });

                SiteFilter.ItemsSource = sites;
                SiteFilter.DisplayMemberPath = "City";
                SiteFilter.SelectedValuePath = "IdSite";
                SiteFilter.SelectedIndex = 0;

                ServiceFilter.ItemsSource = services;
                ServiceFilter.DisplayMemberPath = "Name";
                ServiceFilter.SelectedValuePath = "IdService";
                ServiceFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de chargement des filtres : " + ex.Message);
            }
        }

        // Applique les filtres et la pagination
        private void FilterChanged(object sender, EventArgs e)
        {
            currentPage = 1; // Réinitialise à la première page lorsqu'on filtre
            UpdatePagination();
        }

        // Mise à jour de l'affichage des employés selon la pagination
        private void UpdatePagination()
        {
            var filteredEmployees = ApplyFilters();
            var paginatedEmployees = filteredEmployees
                .Skip((currentPage - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToList();

            Employees.Clear();
            foreach (var emp in paginatedEmployees)
            {
                Employees.Add(emp);
            }

            // Mise à jour des boutons de pagination
            PreviousPageButton.IsEnabled = currentPage > 1;
            NextPageButton.IsEnabled = (currentPage * itemsPerPage) < filteredEmployees.Count();

            // Mise à jour de l'affichage du numéro de page
            PageIndicator.Text = $"Page {currentPage}";
        }

        // Méthode appelée lorsqu'on clique sur "Précédent"
        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdatePagination();
            }
        }

        // Méthode appelée lorsqu'on clique sur "Suivant"
        private void NextPage(object sender, RoutedEventArgs e)
        {
            var filteredEmployees = ApplyFilters();
            if ((currentPage * itemsPerPage) < filteredEmployees.Count())
            {
                currentPage++;
                UpdatePagination();
            }
        }

        // Applique les filtres et retourne la liste filtrée
        private IEnumerable<Employee> ApplyFilters()
        {
            if (AllEmployees == null || AllEmployees.Count == 0)
                return Enumerable.Empty<Employee>();

            var filteredEmployees = AllEmployees.AsEnumerable();

            // Filtre par nom
            string lastNameFilter = LastNameFilter.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(lastNameFilter))
            {
                filteredEmployees = filteredEmployees
                    .Where(emp => emp.LastName.ToLower().Contains(lastNameFilter));
            }

            // Filtre par prénom
            string firstNameFilter = FirstNameFilter.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(firstNameFilter))
            {
                filteredEmployees = filteredEmployees
                    .Where(emp => emp.FirstName.ToLower().Contains(firstNameFilter));
            }

            // Filtre par Site
            if (SiteFilter.SelectedValue is int selectedSiteId && selectedSiteId != 0)
            {
                filteredEmployees = filteredEmployees
                    .Where(emp => emp.IdSite == selectedSiteId);
            }

            // Filtre par Service
            if (ServiceFilter.SelectedValue is int selectedServiceId && selectedServiceId != 0)
            {
                filteredEmployees = filteredEmployees
                    .Where(emp => emp.IdService == selectedServiceId);
            }

            return filteredEmployees;
        }
    }
}
