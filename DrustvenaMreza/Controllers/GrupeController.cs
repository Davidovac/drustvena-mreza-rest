using DrustvenaMreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DrustvenaMreza.Controllers
{
    [Route("api/grupe")]
    [ApiController]
    public class GrupeController : ControllerBase
    {
        private GroupDbRepository groupDbRepository;

        public GrupeController(IConfiguration configuration)
        {
            groupDbRepository = new GroupDbRepository(configuration);
        }

        // GET: api/grupe/{id}
        [HttpGet("{id}")]
        public ActionResult<string> GetById(int id)
        {
            var group = groupDbRepository.GetById(id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }
    }
}
