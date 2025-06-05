using System.Globalization;
using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class GroupMembershipDbRepository
    {
        private readonly string connectionString;

        public GroupMembershipDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Korisnik> GetAllUsersByGroup(int groupId, int page, int pageSize)
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT Users.* FROM Users INNER JOIN GroupMemberships ON GroupMemberships.UserId = Users.Id INNER JOIN Groups ON Groups.Id = GroupMemberships.GroupId WHERE GroupId = @GroupId LIMIT @PageSize OFFSET @Offset";

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.Parameters.AddWithValue("@PageSize", pageSize);
                command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                using SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Korisnik k = new Korisnik(
                        reader.GetInt32(0), // Id
                        reader.GetString(1), // Username
                        reader.GetString(2), // Name
                        reader.GetString(3), // Surname
                        DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", CultureInfo.InvariantCulture)); // DateOfBirth
                    korisnici.Add(k);
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
            return korisnici;
        }

        public void AddUserToGroup(int groupId, int userId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO GroupMemberships (UserId, GroupId) VALUES (@UserId, @GroupId)";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@GroupId", groupId);
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greška pri konekciji ili izvršavanju neispravnih SQL upita: {ex.Message}");
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
        }

        
    }
}
