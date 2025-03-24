using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Employee
    {
        [Key]
        public int IdEmployee { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public DateTime Birthday { get; set; }


        // Constructor
        public Employee(int idEmployee, string firstName, string lastName, string phoneNumber, string email, DateTime birthday)
        {
            IdEmployee = idEmployee;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            Birthday = birthday;
        }

        public Employee() { } // Constructor for EF
    }
}
