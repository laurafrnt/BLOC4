using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class AppDbContext : DbContext
    {

        // Constructeur prenant les options de configuration pour le contexte
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        // On définit nos modèles
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Service> Services { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Site)
                .WithMany()
                .HasForeignKey(e => e.IdSite)
                .OnDelete(DeleteBehavior.Restrict); // Empeche de supprimer si un lien avec employe

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Service)
                .WithMany()
                .HasForeignKey(e => e.IdService)
                .OnDelete(DeleteBehavior.Restrict); // Empeche de supprimer si un lien avec employe
            /*
            // Fake Data Employee
            modelBuilder.Entity<Employee>().HasData(
                new Employee { IdEmployee = 1, FirstName = "Laura", LastName = "Fourniat", PhoneNumber = "0678627362", Email = "laura.fourniat@viacesi.fr", Birthday = new DateTime(2003, 9, 12), IdService = 1, IdSite = 3 },
                new Employee { IdEmployee = 2, FirstName = "Rafaël", LastName = "Marsico", PhoneNumber = "0628378210", Email = "rafael.marsico@viacesi.fr", Birthday = new DateTime(2005, 10, 28), IdService = 1, IdSite = 3 },
                new Employee { IdEmployee = 3, FirstName = "Simon", LastName = "Vendé", PhoneNumber = "0673827382", Email = "simon.vende@viacesi.fr", Birthday = new DateTime(2001, 04, 24), IdService = 1, IdSite = 3 }
            );
            */

            // Fake Data Service
            modelBuilder.Entity<Service>().HasData(
                new Service { IdService = 1, Name = "Informatique" },
                new Service { IdService = 2, Name = "Ressource humaine" },
                new Service { IdService = 3, Name = "Comptabilité et Finance" },
                new Service { IdService = 4, Name = "Production" },
                new Service { IdService = 5, Name = "Vente et Marketing" },
                new Service { IdService = 6, Name = "Direction et Administration Générale" },
                new Service { IdService = 7, Name = "Communication" }
            );

            // Fake Fata Site
            modelBuilder.Entity<Site>().HasData(
                new Site { IdSite = 1, City = "Paris" },
                new Site { IdSite = 2, City = "Marseille" },
                new Site { IdSite = 3, City = "Lyon" },
                new Site { IdSite = 4, City = "Toulouse" },
                new Site { IdSite = 5, City = "Lille" },
                new Site { IdSite = 6, City = "Bordeaux" },
                new Site { IdSite = 7, City = "Strasbourg" }
            );


        }



    }
}
