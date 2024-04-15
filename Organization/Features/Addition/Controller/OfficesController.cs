using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Organization.Features.Addition.DTO;
using Organization.Features.Addition.Request;

namespace Organization.Features.Addition.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OfficesController(IMediator mediator)
        {

            _mediator = mediator;
        }

        // GET: api/Offices
        [HttpGet]
        public async Task<IActionResult> GetOffices()
        {
            return await _mediator.Send(new GetOffices.Query());
        }

        // GET: api/Offices/5
        [HttpGet("{officeId}", Name = "GetOffice")]
        public async Task<IActionResult> GetOffice(int officeId)
        {
            return await _mediator.Send(new GetOffice.Query(officeId));
        }

        // Update: api/Office/1
        [HttpPut("{officeId}")]

        public async Task<IActionResult> UpdateOffice(int officeId, JsonPatchDocument<OfficeForCreationDto> patchDocument)
        {
            return await _mediator.Send(new UpdateOffice.Query(officeId, patchDocument));

        }

        // Create: 
        [HttpPost]
        public async Task<IActionResult> PostOffice(OfficeForCreationDto office)
        {

            return await _mediator.Send(new PostOffice.Query(office));
        }

        // Add api/Offices/1/Add/3
        [HttpPost("{officeId}/Add")]
        public async Task<IActionResult> AddEmployeeToOffice(int officeId, int employeeId)
        {
            return await _mediator.Send(new AddEmployeeToOffice.Query(officeId, employeeId));
        }

        [HttpDelete("{officeId}/Remove/{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeFromOffice(int officeId, int employeeId)
        {
            return await _mediator.Send(new DeleteEmployeeFromOffice.Query(officeId, employeeId));

        }

        [HttpDelete("{officeId}")]
        public async Task<IActionResult> DeleteOffice(int officeId)
        {
            return await _mediator.Send(new DeleteOffice.Query(officeId));

        }
    }
}
