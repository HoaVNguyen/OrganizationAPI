using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Organization.Features.EmployeeFeatures.DTO;
using Organization.Features.EmployeeFeatures.Request;

namespace Organization.Features.EmployeeFeatures.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {

            _mediator = mediator;
        }

        //Get All
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            return await _mediator.Send(new GetEmployees.Query());

        }

        //Get
        [HttpGet("office/{officeId}/Employees/{employeeId}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployee(int officeId, int employeeId)
        {

            return await _mediator.Send(new GetEmployee.Query(officeId, employeeId));

        }

        //Update
        [HttpPut("office/{officeId}/Employees/{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int officeId, int employeeId, JsonPatchDocument<EmployeeForCreationDto> patchDocument)
        {

            return await _mediator.Send(new UpdateEmployee.Query(officeId, employeeId, patchDocument));
        }

        //Create
        [HttpPost]
        public async Task<IActionResult> PostEmployee(EmployeeForCreationDto employee, int _officeId, string teamName)
        {


            return await _mediator.Send(new PostEmployee.Query(employee, _officeId, teamName));

        }

        //Delete
        [HttpDelete("office/{officeId}/Employees/{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(int officeId, int employeeId)
        {

            return await _mediator.Send(new DeleteEmployee.Query(officeId, employeeId));
        }

    }
}
