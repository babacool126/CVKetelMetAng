namespace CVKetelMetAng.Models
{
    public class AppointmentCreationModel
    {
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerAddress { get; set; }
        public SoortAfspraak AppointmentType { get; set; }
        public DateTime AppointmentDateTime { get; set; }
    }
}
