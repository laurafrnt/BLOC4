using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace WPF
{
    internal class ApiService
    {
        private readonly string _baseUrl = "https://localhost:7163/api"; // URL API
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        // EMPLOYEE
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            try
            {
                // Envoi de la requête GET à l'API pour récupérer les employés
                var response = await _httpClient.GetAsync($"{_baseUrl}/Employee");

                // Assurez-vous que la requête a réussi (code HTTP 2xx)
                response.EnsureSuccessStatusCode();

                // Lire le contenu de la réponse sous forme de chaîne JSON
                var json = await response.Content.ReadAsStringAsync();

                // Désérialiser la réponse JSON en une liste d'employés
                var employees = JsonSerializer.Deserialize<List<Employee>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                // Si des informations supplémentaires sont nécessaires (par exemple, site ou service), vous pouvez les récupérer de cette manière :
                foreach (var employee in employees)
                {
                    // Si l'employé a un SiteId, récupérez le site
                    if (employee.IdSite != 0)
                    {
                        employee.Site = await GetSiteByIdAsync(employee.IdSite);  // Méthode pour récupérer le site par son ID
                    }

                    // Si l'employé a un ServiceId, récupérez le service
                    if (employee.IdService != 0)
                    {
                        employee.Service = await GetServiceByIdAsync(employee.IdService);  // Méthode pour récupérer le service par son ID
                    }
                }

                // Retourner la liste d'employés (ou une liste vide si l'API renvoie null)
                return employees ?? new List<Employee>();
            }
            catch (Exception ex)
            {
                // En cas d'erreur, afficher l'exception dans la console
                Console.WriteLine("Erreur de récupération des données : " + ex.Message);
                return new List<Employee>();  // Retourner une liste vide en cas d'erreur
            }
        }


        // SITE

        // Méthode pour récupérer un site par son ID
        public async Task<Site> GetSiteByIdAsync(int siteId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Site/{siteId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var site = JsonSerializer.Deserialize<Site>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return site;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du site : {ex.Message}");
                return null;
            }
        }


        // SERVICE

        // Méthode pour récupérer un service par son ID
        public async Task<Service> GetServiceByIdAsync(int serviceId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Service/{serviceId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var service = JsonSerializer.Deserialize<Service>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return service;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération du service : {ex.Message}");
                return null;
            }
        }
    }


}

