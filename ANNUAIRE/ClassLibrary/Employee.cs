using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    public class Employee
    {
        [Key]
        [JsonPropertyName("idEmployee")]
        public int IdEmployee { get; set; }

        [JsonPropertyName("firstName")]
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [JsonPropertyName("phoneNumber")]
        [Required, Phone]
        public string PhoneNumber { get; set; }

        [JsonPropertyName("email")]
        [Required, EmailAddress]
        public string Email { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; }

        [ForeignKey("Site")]
        [JsonPropertyName("idSite")]
        public int IdSite { get; set; }
        public Site Site { get; set; } 

        [ForeignKey("Service")]
        [JsonPropertyName("idService")]
        public int IdService { get; set; }
        public Service Service { get; set; } 
    

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
