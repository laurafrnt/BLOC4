using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Site
    {
        [Key]
        public int IdSite { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        public List<EmployeeSite> EmployeesSites { get; set; }

        // Constructor
        public Site(int idSite, string city)
        {
            IdSite = idSite;
            City = city;
        }

        public Site () { } // Constructor for EF
    }
}
