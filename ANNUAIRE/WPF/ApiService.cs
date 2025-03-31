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
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;


        public ApiService()
        {
            _baseUrl = "https://localhost:7163/api";
            _httpClient = new HttpClient();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            using var client = new HttpClient();

            var response = await client.GetAsync($"{_baseUrl}/Employee"); // Récupération des articles
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var employees = JsonSerializer.Deserialize<List<Employee>>(json, options);

            return employees;
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

        /* Méthode pour récupérer la liste des sites
        public async Task<List<Site>> GetSitesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Site");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var sites = JsonSerializer.Deserialize<List<Site>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return sites;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des sites : {ex.Message}");
                return new List<Site>();
            }
        }
        */


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

        public async Task<List<Site>> GetSitesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Site");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Site>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<List<Service>> GetServicesAsync()
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/Service");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Service>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }



        /* Méthode pour récupérer la liste des services
        public async Task<List<Service>> GetServicesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/Service");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var services = JsonSerializer.Deserialize<List<Service>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la récupération des services : {ex.Message}");
                return new List<Service>();
            }
        }
        */

    }




}

