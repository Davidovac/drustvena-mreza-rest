using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DrustvenaMreza.Models;
namespace DrustvenaMreza.Repositories
{
    public class RepositoryClanstva
    {
        private const string filePath = "data/clanstva.csv";
        public static Dictionary<int, Clanstvo> Data;
        public RepositoryClanstva()
        {
            if (Data == null)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            Data = new Dictionary<int, Clanstvo>();
            Dictionary<int, Grupa> grupe = RepositoryGrupe.Data;
            Dictionary<int, Korisnik> korisnici = RepositoryKorisnici.Data;
            List<Korisnik> korisniciList = new List<Korisnik>();
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
                Clanstvo clanstvo = new Clanstvo(SracunajId(), grupe[groupId], korisnici[userId]);
                Data.Add(clanstvo.Id, clanstvo);
            }
        }

        public void SaveData()
        {
            List<string> lines = new List<string>();
            foreach (var clanstvo in Data.Values)
            {
                lines.Add($"{clanstvo.Korisnik.Id},{clanstvo.Grupa.Id}");
            }
            File.WriteAllLines(filePath, lines);
        }

        private int SracunajId()
        {
            int maxId = 0;
            foreach (var key in RepositoryClanstva.Data.Keys)
            {
                if (key > maxId)
                {
                    maxId = key;
                }
            }
            return maxId + 1;
        }
    }
}
