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

