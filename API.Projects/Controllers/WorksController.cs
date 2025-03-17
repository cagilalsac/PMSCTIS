#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CORE.APP.Features;
using APP.Projects.Features.Works;

//Generated from Custom Template.
namespace API.Projects.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorksController : ControllerBase
    {
        private readonly ILogger<WorksController> _logger;
        private readonly IMediator _mediator;

        public WorksController(ILogger<WorksController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        // GET: api/Works
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _mediator.Send(new WorkQueryRequest());
                var list = await response.ToListAsync();
                if (list.Any())
                    return Ok(list);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("WorksGet Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during WorksGet.")); 
            }
        }

        // GET: api/Works/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var response = await _mediator.Send(new WorkQueryRequest());
                var item = await response.SingleOrDefaultAsync(r => r.Id == id);
                if (item is not null)
                    return Ok(item);
                return NoContent();
            }
            catch (Exception exception)
            {
                _logger.LogError("WorksGetById Exception: " + exception.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during WorksGetById.")); 
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetWorksByFilter(WorkQueryRequest filter)
        {
            var response = await _mediator.Send(filter);
            var list = await response.ToListAsync();
            if (list.Any())
                return Ok(list);
            return NotFound(); // 404
        }

		//// POST: api/Works
  //      [HttpPost]
  //      public async Task<IActionResult> Post(WorkCreateRequest request)
  //      {
  //          try
  //          {
  //              if (ModelState.IsValid)
  //              {
  //                  var response = await _mediator.Send(request);
  //                  if (response.IsSuccessful)
  //                  {
  //                      //return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
  //                      return Ok(response);
  //                  }
  //                  ModelState.AddModelError("WorksPost", response.Message);
  //              }
  //              return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
  //          }
  //          catch (Exception exception)
  //          {
  //              _logger.LogError("WorksPost Exception: " + exception.Message);
  //              return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during WorksPost.")); 
  //          }
  //      }

  //      // PUT: api/Works
  //      [HttpPut]
  //      public async Task<IActionResult> Put(WorkUpdateRequest request)
  //      {
  //          try
  //          {
  //              if (ModelState.IsValid)
  //              {
  //                  var response = await _mediator.Send(request);
  //                  if (response.IsSuccessful)
  //                  {
  //                      //return NoContent();
  //                      return Ok(response);
  //                  }
  //                  ModelState.AddModelError("WorksPut", response.Message);
  //              }
  //              return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
  //          }
  //          catch (Exception exception)
  //          {
  //              _logger.LogError("WorksPut Exception: " + exception.Message);
  //              return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during WorksPut.")); 
  //          }
  //      }

  //      // DELETE: api/Works/5
  //      [HttpDelete("{id}")]
  //      public async Task<IActionResult> Delete(int id)
  //      {
  //          try
  //          {
  //              var response = await _mediator.Send(new WorkDeleteRequest() { Id = id });
  //              if (response.IsSuccessful)
  //              {
  //                  //return NoContent();
  //                  return Ok(response);
  //              }
  //              ModelState.AddModelError("WorksDelete", response.Message);
  //              return BadRequest(new CommandResponse(false, string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))));
  //          }
  //          catch (Exception exception)
  //          {
  //              _logger.LogError("WorksDelete Exception: " + exception.Message);
  //              return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse(false, "An exception occured during WorksDelete.")); 
  //          }
  //      }
	}
}
