using ClassLibrary;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class SeederDbFaker : Controller
    {
        private readonly AppDbContext _context;

        public static int rowEmployees = 1000;
        public static int rowSites = 10;
        public static int rowServices = 7;

        public SeederDbFaker(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedDatabaseAsync()
        {
            Random rnd = new Random();

            // Ajouter des sites si la base est vide
            if (!_context.Sites.Any())
            {
                for (int i = 1; i <= rowSites; i++)
                {
                    _context.Sites.Add(new Site { IdSite = i, City = $"Ville-{i}" });
                }
                await _context.SaveChangesAsync(); // Sauvegarder les sites une fois ajoutés
            }

            // Ajouter des services si la base est vide
            if (!_context.Services.Any())
            {
                string[] serviceNames = {
            "Informatique", "Ressources Humaines", "Comptabilité et Finance",
            "Production", "Vente et Marketing", "Direction et Administration Générale",
            "Communication"
        };

                for (int i = 0; i < serviceNames.Length; i++)
                {
                    _context.Services.Add(new Service { IdService = i + 1, Name = serviceNames[i] });
                }
                await _context.SaveChangesAsync(); // Sauvegarder les services une fois ajoutés
            }

            int totalSites = _context.Sites.Count();
            int totalServices = _context.Services.Count();

            // Ajouter des employés
            try
            {
                for (int i = 0; i < rowEmployees; i++)
                {
                    int siteId = rnd.Next(1, totalSites + 1);
                    int serviceId = rnd.Next(1, totalServices + 1);

                    var site = _context.Sites.FirstOrDefault(s => s.IdSite == siteId);
                    var service = _context.Services.FirstOrDefault(s => s.IdService == serviceId);

                    if (site != null && service != null)
                    {
                        var employee = FakerEmployee(site, service);
                        _context.Employees.Add(employee); // Ajouter l'employé à la base de données
                    }
                }

                // Sauvegarder après avoir ajouté tous les employés
                await _context.SaveChangesAsync();
                Console.WriteLine($"{rowEmployees} employés ajoutés avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }



        public static Employee FakerEmployee(Site site, Service service)
        {
            var faker = new Faker<Employee>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
                .RuleFor(e => e.Birthday, f => f.Date.Past(30, DateTime.Now.AddYears(-20)))
                .FinishWith((f, e) =>
                {
                    Console.WriteLine($"Generated Employee: {e.FirstName} {e.LastName}");
                    e.Site = site; // site est affecté
                    e.Service = service; // service est affecté
                });

            return faker.Generate();
        }

    }
}
