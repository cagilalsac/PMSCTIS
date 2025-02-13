using APP.Projects.Features.Tags;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Projects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new TagQueryRequest());
            var list = await response.ToListAsync();
            if (list.Any()) //if (list.Count > 0)
                return Ok(list);
            return NoContent();
        }

        // GET: api/Tags/13
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new TagQueryRequest());
            var item = await response.SingleOrDefaultAsync(i => i.Id == id);
            if (item is not null) //if (item != null)
                return Ok(item);
            return NoContent();
        }
    }
}
