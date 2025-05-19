namespace DrustvenaMreza.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Grupa>? Grupe { get; set; }

        public Korisnik(int id, string username, string name, string surname, DateTime dateOfBirth)
        {
            Id = id;
            Username = username;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }
    }
}
