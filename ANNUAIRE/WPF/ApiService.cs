using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WPF
{
    internal class ApiService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;

        // Méthode pour vérifier le mot de passe
        public async Task<bool> VerifyPasswordAsync(string password)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/auth/verify-password", password);
            return response.IsSuccessStatusCode;
        }

        public ApiService()
        {
            _baseUrl = "https://localhost:7163/api";
            _httpClient = new HttpClient();
        }

        // EMPLOYEE

        // GET Employees
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

        // Add Employee
        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            var json = JsonSerializer.Serialize(employee);
            Console.WriteLine($"Envoi JSON : {json}");  // Affiche le JSON envoyé

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}/Employee", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erreur API : {response.StatusCode} - {error}");
            }

            return await response.Content.ReadFromJsonAsync<Employee>();
        }

        // Update Empployee
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            using HttpClient client = new HttpClient();
            string url = $"https://localhost:7163/api/Employee/{employee.IdEmployee}";
            string json = JsonSerializer.Serialize(employee);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PutAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erreur : {response.StatusCode}");
            }
        }


        public async Task DeleteEmployeeAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{_baseUrl}/Employee/{id}");
            response.EnsureSuccessStatusCode();
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



       

    }




}

