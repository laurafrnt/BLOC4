using ClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            // écoute du clavier user
            this.KeyDown += MainWindow_KeyDown;
        }

        // Charge la liste  des employés depuis l'API
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
            currentPage = 1; // Réinitialise à la première page quand 'on filtre
            UpdatePagination();
        }

        // MAJ de l'affichage des employés selon la pagination
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

            // MAJ btn pagination
            PreviousPageButton.IsEnabled = currentPage > 1;
            NextPageButton.IsEnabled = (currentPage * itemsPerPage) < filteredEmployees.Count();

            // MAJ No de page
            PageIndicator.Text = $"Page {currentPage}";
        }

        // "Précédent"
        private void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                UpdatePagination();
            }
        }

        // "Suivant"
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

        //
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.A)
            {
                new AdminLoginWindow().ShowDialog();
            }
        }
    }
}
