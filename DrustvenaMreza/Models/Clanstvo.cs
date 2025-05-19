namespace DrustvenaMreza.Models
{
    public class Clanstvo
    {
        public int Id { get; set; }
        public Grupa Grupa { get; set; }
        public Korisnik Korisnik { get; set; }
        public Clanstvo(int id, Grupa grupa, Korisnik korisnik)
        {
            Id = id;
            Grupa = grupa;
            Korisnik = korisnik;
        }
        
    }
}
