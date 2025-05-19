using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;
using System.Globalization;

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
            List<Korisnik> korisnici = RepositoryKorisnici.Data.Values.ToList();
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
            RepositoryKorisnici.Data[id] = korisnik;
            repositoryKorisnici.SaveData();
            return Ok();
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
