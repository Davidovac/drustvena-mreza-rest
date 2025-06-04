using System.Globalization;
using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class GroupDbRepository
    {
        private readonly string connectionString;

        public GroupDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public Grupa GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM Groups WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Grupa(
                        reader.GetInt32(0), // Id
                        reader.GetString(1), // Name
                        DateTime.ParseExact(reader.GetString(2), "yyyy-MM-dd", CultureInfo.InvariantCulture)); // DateOfCreation
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
            return null;
        }
    }
}
