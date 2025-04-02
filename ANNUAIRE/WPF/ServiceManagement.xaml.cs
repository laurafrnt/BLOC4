using ClassLibrary;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class ServiceManagement : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<Service> Services { get; set; } = new ObservableCollection<Service>();

        public ServiceManagement()
        {
            InitializeComponent();
            DataContext = this;
            LoadServices();
        }

        private async void LoadServices()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ObservableCollection<Service>>("https://localhost:7163/api/Service");
                if (response != null)
                {
                    Services.Clear();
                    foreach (var service in response)
                    {
                        Services.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement des services : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddService_Click(object sender, RoutedEventArgs e)
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Entrez le nom du service :", "Ajouter un Service", "");
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var newService = new Service { Name = newName };

                try
                {
                    var response = await _httpClient.PostAsJsonAsync("https://localhost:7163/api/Service", newService);
                    if (response.IsSuccessStatusCode)
                    {
                        var createdService = await response.Content.ReadFromJsonAsync<Service>();
                        if (createdService != null)
                        {
                            Services.Add(createdService);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout du service.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur d'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            LoadServices();
        }

        private async void EditService_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Service selectedService)
            {
                string newName = Microsoft.VisualBasic.Interaction.InputBox("Modifiez le nom du service :", "Modifier un Service", selectedService.Name);
                if (!string.IsNullOrWhiteSpace(newName) && newName != selectedService.Name)
                {
                    selectedService.Name = newName;
                    await UpdateService(selectedService);
                }
            }
            LoadServices();
        }

        private async Task UpdateService(Service service)
        {
            try
            {
                string url = $"https://localhost:7163/api/Service/{service.IdService}";
                string json = JsonSerializer.Serialize(service);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour du service {service.IdService}.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de mise à jour : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteService_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int serviceId)
            {
                if (MessageBox.Show("Voulez-vous vraiment supprimer ce service ?", "Confirmation",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    await DeleteService(serviceId);
                }
            }
            LoadServices();
        }

        private async Task DeleteService(int serviceId)
        {
            try
            {
                string url = $"https://localhost:7163/api/Service/{serviceId}";
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var serviceToRemove = Services.FirstOrDefault(s => s.IdService == serviceId);
                    if (serviceToRemove != null)
                    {
                        Services.Remove(serviceToRemove);
                    }
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer ce service, il est à un employé.",
                                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de suppression : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
