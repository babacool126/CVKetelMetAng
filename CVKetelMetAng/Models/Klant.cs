using System.ComponentModel.DataAnnotations;

namespace CVKetelMetAng.Models
{
    public class Klant
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Naam { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Telefoonnummer { get; set; }

        [MaxLength(200)]
        public string Adres { get; set; }

        public ICollection<Afspraak> Afspraken { get; set; } = new List<Afspraak>();
    }
}
