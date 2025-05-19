using DrustvenaMreza.Models;
namespace DrustvenaMreza.Repositories
{
    public class RepositoryClanstva
    {
        private const string filePath = "data/clanstva.csv";
        public static Dictionary<int, int> Data;

        public RepositoryClanstva()
        {
            if (Data == null)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            Dictionary<int, Grupa> grupe = RepositoryGrupe.Data;
            Dictionary<int, Korisnik> korisnici = RepositoryKorisnici.Data;
            Data = new Dictionary<int, int>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length != 2) continue; // Skip invalid lines
                int userId = int.Parse(parts[0]);
                int groupId = int.Parse(parts[1]);
                if (grupe[groupId].Korisnici == null)
                {
                    grupe[groupId].Korisnici = new List<Korisnik>();
                }
                if (korisnici[userId].Grupe == null)
                {
                    korisnici[userId].Grupe = new List<Grupa>();
                }
                grupe[groupId].Korisnici.Add(korisnici[userId]);
                korisnici[userId].Grupe.Add(grupe[groupId]);
                Data.Add(userId, groupId);
            }
        }

        public void SaveData()
        {
            List<string> lines = new List<string>();
            {
                foreach (var entry in Data)
                {
                    lines.Add($"{entry.Key},{entry.Value}");
                }
            }
        }
    }
}
