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

        // POST : api/clanstva/{groupId}/{userId}
        [HttpPost("{groupId}/{userId}")]
        public ActionResult<string> AddUserToGroup(int groupId, int userId)
        {
            try
            {
                Grupa? group = groupDbRepository.GetById(groupId);
                if (group == null)
                {
                    return NotFound("Group not found.");
                }
                Korisnik? user = userDbRepository.GetById(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                groupMembershipDbRepository.AddUserToGroup(groupId, userId);
                return Ok($"User {user.Username} added to group {group.Name} successfully.");
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while adding user to group.");
            }
        }

        // DELETE: api/clanstva/{groupId}/{userId}
        [HttpDelete("{groupId}/{userId}")]
        public ActionResult<string> RemoveUserFromGroup(int groupId, int userId)
        {
            try
            {
                Grupa? group = groupDbRepository.GetById(groupId);
                if (group == null)
                {
                    return NotFound("Group not found.");
                }
                Korisnik? user = userDbRepository.GetById(userId);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                bool isDeleted = false;
                isDeleted = groupMembershipDbRepository.RemoveUserFromGroup(groupId, userId);
                if (isDeleted)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("User is not a member of the group.");
                }
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while removing user from group.");
            }
        }
    }
}
