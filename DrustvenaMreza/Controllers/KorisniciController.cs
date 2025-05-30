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
        private UserDbRepository userDbRepository = new UserDbRepository();
        // GET: api/korisnici
        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            List<Korisnik> korisnici = userDbRepository.GetAll();
            
            return Ok(korisnici);
        }
        // GET: api/korisnici/{id}
        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            var korisnik = userDbRepository.GetById(id);
            if (korisnik == null)
            {
                return NotFound();
            }
            return Ok(korisnik);
        }
        // POST: api/korisnici
        [HttpPost]
        public ActionResult<Korisnik> CreateKorisnik([FromBody] Korisnik korisnik)
        {
            if (string.IsNullOrWhiteSpace(korisnik.Name) || string.IsNullOrWhiteSpace(korisnik.Surname) || string.IsNullOrWhiteSpace(korisnik.Username))
            {
                return BadRequest();
            }
            userDbRepository.Create(korisnik);
            return Ok();
        }
        // PUT: api/korisnici/{id}
        [HttpPut("{id}")]
        public ActionResult<Korisnik> UpdateKorisnik(int id, [FromBody] Korisnik korisnik)
        {
            if (string.IsNullOrWhiteSpace(korisnik.Name) || string.IsNullOrWhiteSpace(korisnik.Surname) || string.IsNullOrWhiteSpace(korisnik.Username))
            {
                return BadRequest();
            }
            if (userDbRepository.GetById(id) == null)
            {
                return NotFound();
            }
            korisnik.Id = id;
            userDbRepository.Update(korisnik);
            return Ok(korisnik);
        }
        // DELETE: api/korisnici/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteKorisnik(int id)
        {
            if (userDbRepository.GetById(id) == null)
            {
                return NotFound();
            }
            userDbRepository.Delete(id);
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
