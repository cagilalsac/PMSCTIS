#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CORE.APP.Features;
using APP.Projects.Features.Projects;

//Generated from Custom Template.
namespace API.Projects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IMediator _mediator;

        public ProjectsController(ILogger<ProjectsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new ProjectQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("ProjectsGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during ProjectsGet.")); 
            }
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new ProjectQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("ProjectsGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during ProjectsGetById.")); 
            }
        }

	//	// POST: api/Projects
 //       [HttpPost]
 //       public async Task<IActionResult> Post(ProjectCreateRequest request)
 //       {
 //           try
 //           {
 //               if (ModelState.IsValid)
 //               {
 //                   var response = await _mediator.Send(request);
 //                   if (response.IsSuccessful)
 //                   {
 //                       //return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
 //                       return Ok(response);
 //                   }
 //                   ModelState.AddModelError("ProjectsPost", response.Message);
 //               }
 //               return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
 //           }
 //           catch (Exception exception)
 //           {
 //               _logger.LogError("ProjectsPost Exception: " + exception.Message);
 //               return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during ProjectsPost.")); 
 //           }
 //       }

 //       // PUT: api/Projects
 //       [HttpPut]
 //       public async Task<IActionResult> Put(ProjectUpdateRequest request)
 //       {
 //           try
 //           {
 //               if (ModelState.IsValid)
 //               {
 //                   var response = await _mediator.Send(request);
 //                   if (response.IsSuccessful)
 //                   {
 //                       //return NoContent();
 //                       return Ok(response);
 //                   }
 //                   ModelState.AddModelError("ProjectsPut", response.Message);
 //               }
 //               return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
 //           }
 //           catch (Exception exception)
 //           {
 //               _logger.LogError("ProjectsPut Exception: " + exception.Message);
 //               return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during ProjectsPut.")); 
 //           }
 //       }

 //       // DELETE: api/Projects/5
 //       [HttpDelete("{id}")]
 //       public async Task<IActionResult> Delete(int id)
 //       {
 //           try
 //           {
 //               var response = await _mediator.Send(new ProjectDeleteRequest() { Id = id });
 //               if (response.IsSuccessful)
 //               {
 //                   //return NoContent();
 //                   return Ok(response);
 //               }
 //               ModelState.AddModelError("ProjectsDelete", response.Message);
 //               return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
 //           }
 //           catch (Exception exception)
 //           {
 //               _logger.LogError("ProjectsDelete Exception: " + exception.Message);
 //               return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during ProjectsDelete.")); 
 //           }
 //       }
	}
}
