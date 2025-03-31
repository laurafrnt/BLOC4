using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    public class Site
    {
        [Key]
        [JsonPropertyName("id")]
        public int IdSite { get; set; }

        [Required, MaxLength(50)]

        [JsonPropertyName("city")]
        public string City { get; set; }

        // Constructeur
        public Site(int idSite, string city)
        {
            IdSite = idSite;
            City = city;
        }

        public Site () { } // Constructor pour EF
    }
}
