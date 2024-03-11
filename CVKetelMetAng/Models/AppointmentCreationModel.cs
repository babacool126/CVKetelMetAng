using System.ComponentModel.DataAnnotations;

namespace CVKetelMetAng.Models
{
    public class AppointmentCreationModel
    {
        [Required(ErrorMessage = "The email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [MaxLength(100, ErrorMessage = "The email address must be less than 100 characters.")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(50, ErrorMessage = "The name must be less than 50 characters.")]
        public string CustomerName { get; set; }

        [MaxLength(20, ErrorMessage = "The phone number must be less than 20 characters.")]
        public string CustomerPhoneNumber { get; set; }

        [MaxLength(200, ErrorMessage = "The address must be less than 200 characters.")]
        public string CustomerAddress { get; set; }

        [Required(ErrorMessage = "The appointment type is required.")]
        public SoortAfspraak AppointmentType { get; set; }

        [Required(ErrorMessage = "The appointment date and time are required.")]
        public DateTime AppointmentDateTime { get; set; }
    }
}
