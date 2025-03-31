using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    public class Service
    {
        [Key]
        [JsonPropertyName("id")]
        public int IdService { get; set; }

        [Required, MaxLength(50)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public Service(int idService, string name)
        {
            IdService = idService;
            Name = name;
        }

        public Service() { } // Constructor pour EF
    }
}
