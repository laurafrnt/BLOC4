using ClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPF
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService; // Correction du nom

        public ObservableCollection<Employee> Employees { get; set; } = new ObservableCollection<Employee>();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _apiService = new ApiService(new HttpClient());
            Console.WriteLine("Chargement des employés...");
            LoadEmployees();
        }


        // Charge la liste des employés depuis l'API
        private async void LoadEmployees()
        {
            try
            {
                var employees = await _apiService.GetEmployeesAsync();
                Employees.Clear();
                foreach (var emp in employees)
                {
                    Employees.Add(emp);
                }

                Console.WriteLine($"DataContext après chargement: {DataContext}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion : " + ex.Message);
            }
        }

        // Accès admin caché (Ctrl + Alt + A)
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftAlt) && e.Key == Key.A)
            {
                MessageBox.Show("Accès Admin : Veuillez entrer le mot de passe.");
                // Ici, ouvrir une fenêtre d'authentification
            }
        }
    }
}
