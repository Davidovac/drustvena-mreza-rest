using System.Globalization;
using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace DrustvenaMreza.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisniciController : ControllerBase
    {
        private RepositoryKorisnici repositoryKorisnici = new RepositoryKorisnici();
        private RepositoryGrupe repositoryGrupe = new RepositoryGrupe();
        private RepositoryClanstva repositoryClanstva = new RepositoryClanstva();
        // GET: api/korisnici
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source=Data/data.db");
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
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
            return Ok(korisnici);
        }
        // GET: api/korisnici/{id}
        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            if (!RepositoryKorisnici.Data.ContainsKey(id))
            {
                return NotFound();
            }
            var korisnik = RepositoryKorisnici.Data[id];
            return Ok(korisnik);
        }
        // POST: api/korisnici
        [HttpPost]
        public ActionResult<Korisnik> CreateKorisnik([FromBody] Korisnik korisnik)
        {
            if (RepositoryKorisnici.Data.ContainsKey(korisnik.Id))
            {
                return Conflict();
            }
            if (string.IsNullOrWhiteSpace(korisnik.Name) || string.IsNullOrWhiteSpace(korisnik.Surname) || string.IsNullOrWhiteSpace(korisnik.Username))
            {
                return BadRequest();
            }
            korisnik.Id = SracunajId();
            RepositoryKorisnici.Data.Add(korisnik.Id, korisnik);
            repositoryKorisnici.SaveData();
            return Ok(korisnik);
        }
        // PUT: api/korisnici/{id}
        [HttpPut("{id}")]
        public ActionResult<Korisnik> UpdateKorisnik(int id, [FromBody] Korisnik korisnik)
        {
            if (string.IsNullOrWhiteSpace(korisnik.Name) || string.IsNullOrWhiteSpace(korisnik.Surname) || string.IsNullOrWhiteSpace(korisnik.Username))
            {
                return BadRequest();
            }
            if (!RepositoryKorisnici.Data.ContainsKey(id))
            {
                return NotFound();
            }
            korisnik.Id = id;
            RepositoryKorisnici.Data[id] = korisnik;
            repositoryKorisnici.SaveData();
            return Ok(korisnik);
        }
        // DELETE: api/korisnici/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteKorisnik(int id)
        {
            if (!RepositoryKorisnici.Data.ContainsKey(id))
            {
                return NotFound();
            }
            RepositoryKorisnici.Data.Remove(id);
            List<Clanstvo> clanstva = new List<Clanstvo>();
            foreach (var c in RepositoryClanstva.Data.Values)
            {
                if (c.Korisnik.Id == id)
                {
                    clanstva.Add(c);
                }
            }
            foreach (var clanstvo in clanstva)
            {
                RepositoryClanstva.Data.Remove(clanstvo.Id);
            }
            repositoryKorisnici.SaveData();
            repositoryClanstva.SaveData();
            return NoContent();
        }

        private int SracunajId()
        {
            int maxId = 0;
            foreach (var korisnik in RepositoryKorisnici.Data.Values)
            {
                if (korisnik.Id > maxId)
                {
                    maxId = korisnik.Id;
                }
            }
            return maxId + 1;
        }
    }
}
