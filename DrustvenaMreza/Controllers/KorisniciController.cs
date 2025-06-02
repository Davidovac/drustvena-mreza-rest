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
        //private RepositoryGrupe repositoryGrupe = new RepositoryGrupe();
        //private RepositoryClanstva repositoryClanstva = new RepositoryClanstva();
        private UserDbRepository userDbRepository;

        public KorisniciController(IConfiguration configuration)
        {
            userDbRepository = new UserDbRepository(configuration);
        }

        // GET /api/books?page={page}&pageSize={pageSize}
        [HttpGet]
        public ActionResult<List<Korisnik>> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than zero.");
            }
            try
            {
                List<Korisnik> korisnici = userDbRepository.GetPaged(page, pageSize);
                int totalCount = userDbRepository.CountAll();
                Object result = new
                {
                    TotalCount = totalCount,
                    Data = korisnici
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while retrieving users.");
            }

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
            Korisnik newKorisnik = userDbRepository.Create(korisnik);
            return Ok(newKorisnik);
        }
        // PUT: api/korisnici/{id}
        [HttpPut("{id}")]
        public ActionResult<Korisnik> UpdateKorisnik(int id, [FromBody] Korisnik korisnik)
        {
            if (korisnik == null || string.IsNullOrWhiteSpace(korisnik.Name) || string.IsNullOrWhiteSpace(korisnik.Surname) || string.IsNullOrWhiteSpace(korisnik.Username))
            {
                return BadRequest();
            }
            try
            {
                korisnik.Id = id;
                Korisnik updatedKorisnik = userDbRepository.Update(korisnik);
                if (updatedKorisnik == null)
                {
                    return NotFound();
                }
                return Ok(updatedKorisnik);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while updating the book.");
            }
            
        }
        // DELETE: api/korisnici/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteKorisnik(int id)
        {
            try
            {
                bool isDeleted = userDbRepository.Delete(id);
                if (isDeleted)
                {
                    /*List<Clanstvo> clanstva = new List<Clanstvo>();
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
                    repositoryClanstva.SaveData();*/

                    return NoContent();
                }
                return NotFound($"User with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while deleting the user.");
            }
        }
    }
}
