namespace DrustvenaMreza.Models
{
    public class Grupa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public Grupa(int id, string name, DateTime dateOfCreation)
        {
            Id = id;
            Name = name;
            DateOfCreation = dateOfCreation;
        }
    }
}
