using System.ComponentModel.DataAnnotations;

namespace CVKetelMetAng.Models
{
    public class Klant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(50, ErrorMessage = "Le nom doit contenir moins de 50 caractères.")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        [MaxLength(100, ErrorMessage = "L'adresse e-mail doit contenir moins de 100 caractères.")]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "Le numéro de téléphone doit contenir moins de 20 caractères.")]
        public string Telefoonnummer { get; set; }

        [MaxLength(200, ErrorMessage = "L'adresse doit contenir moins de 200 caractères.")]
        public string Adres { get; set; }

        public ICollection<Afspraak> Afspraken { get; set; } = new List<Afspraak>();
    }
}
