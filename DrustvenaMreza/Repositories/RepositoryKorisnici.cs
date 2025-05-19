using System.Globalization;
using System.Runtime.CompilerServices;
using DrustvenaMreza.Models;
namespace DrustvenaMreza.Repositories
{
    public class RepositoryKorisnici
    {
        private const string filePath = "data/korisnici.csv";
        public static Dictionary<int, Korisnik> Data;

        public RepositoryKorisnici()
        {
            if (Data == null)
            {
                LoadData();
            }
            
        }

        public void LoadData()
        {
            Data = new Dictionary<int, Korisnik>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length != 5) continue;
                int id = int.Parse(parts[0]);
                string username = parts[1];
                string name = parts[2];
                string surname = parts[3];
                DateTime dateOfBirth = DateTime.ParseExact(parts[4], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Korisnik korisnik = new Korisnik(id, username, name, surname, dateOfBirth);
                Data.Add(id, korisnik);
            }
        }

        public void SaveData()
        {
            List<string> lines = new List<string>();
            foreach (var entry in Data.Values)
            {
                lines.Add($"{entry.Id},{entry.Username},{entry.Name},{entry.Surname},{entry.DateOfBirth:yyyy-MM-dd}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
