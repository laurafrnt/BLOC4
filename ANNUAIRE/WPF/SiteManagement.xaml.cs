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
    public partial class SiteManagement : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<Site> Sites { get; set; } = new ObservableCollection<Site>();

        public SiteManagement()
        {
            InitializeComponent();
            DataContext = this;
            LoadSites();
        }

        private async void LoadSites()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<ObservableCollection<Site>>("https://localhost:7163/api/Site");
                if (response != null)
                {
                    Sites.Clear();
                    foreach (var site in response)
                    {
                        Sites.Add(site);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement des sites : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddSite_Click(object sender, RoutedEventArgs e)
        {
            string newCity = Microsoft.VisualBasic.Interaction.InputBox("Entrez le nom de la ville :", "Ajouter un Site", "");
            if (!string.IsNullOrWhiteSpace(newCity))
            {
                var newSite = new Site { City = newCity };

                try
                {
                    var response = await _httpClient.PostAsJsonAsync("https://localhost:7163/api/Site", newSite);
                    if (response.IsSuccessStatusCode)
                    {
                        var createdSite = await response.Content.ReadFromJsonAsync<Site>();
                        if (createdSite != null)
                        {
                            Sites.Add(createdSite);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout du site.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur d'ajout : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            LoadSites();
        }

        private async void EditSite_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Site selectedSite)
            {
                string newCity = Microsoft.VisualBasic.Interaction.InputBox("Modifiez le nom de la ville :", "Modifier un Site", selectedSite.City);
                if (!string.IsNullOrWhiteSpace(newCity) && newCity != selectedSite.City)
                {
                    selectedSite.City = newCity;
                    await UpdateSite(selectedSite);
                }
            }
            LoadSites();
        }

        private async Task UpdateSite(Site site)
        {
            try
            {
                string url = $"https://localhost:7163/api/Site/{site.IdSite}";
                string json = JsonSerializer.Serialize(site);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Erreur lors de la mise à jour du site {site.IdSite}.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de mise à jour : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteSite_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int siteId)
            {
                if (MessageBox.Show("Voulez-vous vraiment supprimer ce site ?", "Confirmation",
                                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    await DeleteSite(siteId);
                }
            }
            LoadSites();
        }

        private async Task DeleteSite(int siteId)
        {
            try
            {
                string url = $"https://localhost:7163/api/Site/{siteId}";
                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var siteToRemove = Sites.FirstOrDefault(s => s.IdSite == siteId);
                    if (siteToRemove != null)
                    {
                        Sites.Remove(siteToRemove);
                    }
                }
                else
                {
                    MessageBox.Show("Impossible de supprimer ce site, il est peut-être rattaché à un employé.",
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
