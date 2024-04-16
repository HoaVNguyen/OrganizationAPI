using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Organization.Features.Addition.DTO;
using Organization.Features.Addition.Request;

namespace Organization.Features.Addition.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {

        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: All
        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            return await _mediator.Send(new GetTeams.Query());

        }

        // GET: api/Teams/5
        [HttpGet("{teamId}", Name = "GetTeam")]
        public async Task<IActionResult> GetTeam(int teamId)
        {
            return await _mediator.Send(new GetTeam.Query(teamId));

        }

        // PUT: api/Teams/5
        [HttpPut("{teamId}")]
        public async Task<IActionResult> UpdateTeam(int teamId, JsonPatchDocument<TeamForCreationDto> patchDocument)
        {
            return await _mediator.Send(new UpdateTeam.Query(teamId, patchDocument));

        }

        // POST: api/Teams
        [HttpPost]
        public async Task<IActionResult> PostTeam(TeamForCreationDto team)
        {
            return await _mediator.Send(new PostTeam.Query(team));

        }

        // Add api/Teams/1/Add/3
        [HttpPost("{teamId}/Add")]
        public async Task<IActionResult> AddEmployeeToTeam(int teamId, int employeeId)
        {
            return await _mediator.Send(new AddEmployeeToTeam.Query(teamId, employeeId));

        }

        [HttpDelete("{teamId}/Remove/{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeFromTeam(int teamId, int employeeId)
        {
            return await _mediator.Send(new DeleteEmployeeFromTeam.Query(teamId, employeeId));


        }

        // DELETE: api/Teams/5
        [HttpDelete("{teamId}")]
        public async Task<IActionResult> DeleteTeam(int teamId)
        {
            return await _mediator.Send(new DeleteTeam.Query(teamId));

        }

    }
}
