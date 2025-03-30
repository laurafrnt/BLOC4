using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Model
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }

        [Required, MaxLength(50)]
        public string  FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public DateTime Birthday { get; set; }

        // Clé étrangère pour Site
        [ForeignKey("Site")]
        public int IdSite { get; set; }
        public Site Site { get; set; }  // Relation avec Site

        // Clé étrangère pour Service
        [ForeignKey("Service")]
        public int IdService { get; set; }
        public Service Service { get; set; }  // Relation avec Service


        // Constructeur
        public Employee(int idEmployee, string firstName, string lastName, string phoneNumber, string email, DateTime birthday, int idSite, int idService)
        {
            IdEmployee = idEmployee;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
            IdSite = idSite;
            IdService = idService;
        }

        public Employee() { } // Constructeur pour EF
    }
}
