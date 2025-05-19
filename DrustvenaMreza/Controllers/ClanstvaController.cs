using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DrustvenaMreza.Models;
using DrustvenaMreza.Repositories;

namespace DrustvenaMreza.Controllers
{
    [Route("api/clanstva")]
    [ApiController]
    public class ClanstvaController : ControllerBase
    {
        private RepositoryGrupe repositoryGrupe = new RepositoryGrupe();
        private RepositoryKorisnici repositoryKorisnici = new RepositoryKorisnici();
        private RepositoryClanstva repositoryClanstva = new RepositoryClanstva();

        // GET: api/clanstva/{id}
        [HttpGet]
        public ActionResult<string> GetAllUsersByGroup([FromQuery] int groupId)
        {
            if (!RepositoryGrupe.Data.ContainsKey(groupId))
            {
                return NotFound();
            }
            List<Korisnik> korisnici = new List<Korisnik>();
            foreach (var clanstvo in RepositoryClanstva.Data.Values)
            {
                if (clanstvo.Grupa.Id == groupId)
                {
                    korisnici.Add(clanstvo.Korisnik);
                }
            }
            return Ok(korisnici);
        }
    }
}
