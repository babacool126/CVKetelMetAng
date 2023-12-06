namespace CVKetelMetAng.Models
{
    public class Afspraak
    {
        public int Id { get; set; }
        public int KlantId { get; set; }
        public Klant Klant { get; set; }
        public SoortAfspraak Soort { get; set; }
        public DateTime DatumTijd { get; set; }
    }
}
