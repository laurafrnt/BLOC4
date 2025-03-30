using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class Service
    {
        [Key]
        public int IdService { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        public Service(int idService, string name)
        {
            IdService = idService;
            Name = name;
        }

        public Service() { } // Constructor pour EF
    }
}
