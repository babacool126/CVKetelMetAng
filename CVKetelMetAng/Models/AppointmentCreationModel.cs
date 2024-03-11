using System.ComponentModel.DataAnnotations;

namespace CVKetelMetAng.Models
{
    public class AppointmentCreationModel
    {
        [Required(ErrorMessage = "L'adresse e-mail est requise.")]
        [EmailAddress(ErrorMessage = "L'adresse e-mail n'est pas valide.")]
        [MaxLength(100, ErrorMessage = "L'adresse e-mail doit contenir moins de 100 caractères.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [MaxLength(50, ErrorMessage = "Le nom doit contenir moins de 50 caractères.")]
        public string CustomerName { get; set; }

        [MaxLength(20, ErrorMessage = "Le numéro de téléphone doit contenir moins de 20 caractères.")]
        public string CustomerPhoneNumber { get; set; }

        [MaxLength(200, ErrorMessage = "L'adresse doit contenir moins de 200 caractères.")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "Le type de rendez-vous est requis.")]
        public SoortAfspraak AppointmentType { get; set; }

        [Required(ErrorMessage = "La date et l'heure du rendez-vous sont requises.")]
        public DateTime AppointmentDateTime { get; set; }
    }
}
