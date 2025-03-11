namespace TrybeHotel.Models
{
    using System.ComponentModel.DataAnnotations;
    // 1. Implemente as models da aplicação
    public class City {
        [Key]
        public int CityId { get; set; }
        public string? Name { get; set; }
        public ICollection<Hotel>? Hotels { get; set; }
    }
}
