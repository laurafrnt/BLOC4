using Microsoft.EntityFrameworkCore;

namespace API.Model
{
    public class AppDbContext : DbContext
    {
        // We define our models
        public DbSet<Employee> Employees { get; set; }
        //public DbSet<Site> Sites { get; set; }
        //public DbSet<Service> Services { get; set; }
       

        // Config DB file with sqlite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=c:/bloc4.db");

        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*

            // Config of relation manyToMany EmployeeService
            modelBuilder.Entity<EmployeeService>().HasKey(es => new { es.EmployeeId, es.ServiceId }); // es = EmployeeService

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Employee) // An EmployeeService has one Employee
                .WithMany(e => e.EmployeesServices) // an Employee has serveral EmployeeServices
                .HasForeignKey(es => es.EmployeeId); // the fk is EmployeeId

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Service) // an EmployeeService has one Service
                .WithMany(s => s.EmployeesServices) // a Service has several EmployeeServices
                .HasForeignKey(es => es.ServiceId); // the fk is ServiceId


            // Config of relation manyToMany EmployeeSite
            modelBuilder.Entity<EmployeeSite>().HasKey(es => new { es.EmployeeId, es.SiteId });

            modelBuilder.Entity<EmployeeSite>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeesSites)
                .HasForeignKey(es => es.EmployeeId);

            modelBuilder.Entity<EmployeeSite>()
                .HasOne(es => es.Site)
                .WithMany(s => s.EmployeesSites)
                .HasForeignKey(es => es.SiteId);

        */
            // Fake Data Employee
            modelBuilder.Entity<Employee>().HasData(
                new Employee { IdEmployee = 1, FirstName = "Laura", LastName = "Fourniat", PhoneNumber = "0678627362", Email = "laura.fourniat@viacesi.fr", Birthday = new DateTime(2003, 9, 12) },
                new Employee { IdEmployee = 2, FirstName = "Rafaël", LastName = "Marsico", PhoneNumber = "0628378210", Email = "rafael.marsico@viacesi.fr", Birthday = new DateTime(2005, 10, 28) },
                new Employee { IdEmployee = 3, FirstName = "Simon", LastName = "Vendé", PhoneNumber = "0673827382", Email = "simon.vende@viacesi.fr", Birthday = new DateTime(2001, 04, 24) }
            );

            /* Fake Data Service
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
            */

        }
       


    }
}
