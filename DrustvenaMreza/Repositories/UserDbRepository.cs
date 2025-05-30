using System.Globalization;
using DrustvenaMreza.Models;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Repositories
{
    public class UserDbRepository
    {
        private const string connectionString = "Data Source=Data/data.db";
        public List<Korisnik> GetAll()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);
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

        public Korisnik GetById(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using SqliteDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Korisnik(
                        reader.GetInt32(0), // Id
                        reader.GetString(1), // Username
                        reader.GetString(2), // Name
                        reader.GetString(3), // Surname
                        DateTime.ParseExact(reader.GetString(4), "yyyy-MM-dd", CultureInfo.InvariantCulture)); // DateOfBirth
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

        public void Create(Korisnik korisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "INSERT INTO Users (Username, Name, Surname, Birthday) VALUES (@username, @name, @surname, @birthday)";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@username", korisnik.Username);
                command.Parameters.AddWithValue("@name", korisnik.Name);
                command.Parameters.AddWithValue("@surname", korisnik.Surname);
                command.Parameters.AddWithValue("@birthday", korisnik.DateOfBirth.ToString("yyyy-MM-dd"));
                command.ExecuteNonQuery();
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
        }

        public void Update(Korisnik korisnik)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "UPDATE Users SET Username = @username, Name = @name, Surname = @surname, Birthday = @birthday WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", korisnik.Id);
                command.Parameters.AddWithValue("@username", korisnik.Username);
                command.Parameters.AddWithValue("@name", korisnik.Name);
                command.Parameters.AddWithValue("@surname", korisnik.Surname);
                command.Parameters.AddWithValue("@birthday", korisnik.DateOfBirth.ToString("yyyy-MM-dd"));
                command.ExecuteNonQuery();
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
        }

        public void Delete(int id)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @id";
                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
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
