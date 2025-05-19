using System.Globalization;
using DrustvenaMreza.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DrustvenaMreza.Repositories
{
    public class RepositoryGrupe
    {
        private const string filePath = "data/grupe.csv";
        public static Dictionary<int, Grupa> Data;

        public RepositoryGrupe()
        {
            if (Data == null)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            Data = new Dictionary<int, Grupa>();
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found: " + filePath);
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length != 3) continue; // Skip invalid lines
                int id = int.Parse(parts[0]);
                string name = parts[1];
                DateTime dateOfCreation = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                Grupa grupa = new Grupa(id, name, dateOfCreation);
                Data.Add(id, grupa);
            }
        }

        public void SaveData()
        {
            List<string> lines = new List<string>();
            foreach (var entry in Data.Values)
            {
                lines.Add($"{entry.Id},{entry.Name},{entry.DateOfCreation:yyyy-MM-dd}");
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
