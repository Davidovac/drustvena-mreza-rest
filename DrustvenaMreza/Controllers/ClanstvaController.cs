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
        private UserDbRepository userDbRepository;
        private GroupDbRepository groupDbRepository;
        private GroupMembershipDbRepository groupMembershipDbRepository;

        public ClanstvaController(IConfiguration configuration)
        {
            userDbRepository = new UserDbRepository(configuration);
            groupDbRepository = new GroupDbRepository(configuration);
            groupMembershipDbRepository = new GroupMembershipDbRepository(configuration);
        }

        // GET: api/clanstva/{id}?page={page}&pageSize={pageSize}
        [HttpGet]
        public ActionResult<string> GetGroupWithUsers([FromQuery] int groupId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest("Page and pageSize must be greater than zero.");
            }
            List<Korisnik> korisnici = new List<Korisnik>();
            try
            {
                Grupa? group = groupDbRepository.GetById(groupId);
                if (group == null)
                {
                    return NotFound("Group not found.");
                }
                korisnici = groupMembershipDbRepository.GetAllUsersByGroup(groupId, page, pageSize);
                Object obj = new
                {
                    korisnici,
                    group,
                    page,
                    pageSize
                };
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while retrieving users by group.");
            }   
        }
    }
}
