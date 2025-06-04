namespace DrustvenaMreza.Models
{
    public class Clanstvo
    {
        public int Id { get; set; }
        public int GrupaID { get; set; }
        public int KorisnikID { get; set; }
        public Clanstvo(int id, int GrupaID, int KorisnikID)
        {
            Id = id;
            GrupaID = GrupaID;
            KorisnikID = KorisnikID;
        }
    }
}
