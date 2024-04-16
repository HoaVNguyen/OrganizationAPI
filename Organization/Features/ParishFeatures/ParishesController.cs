using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Organization.Features.ParishFeatures
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParishesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ParishesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetParishes()
        {
            return await _mediator.Send(new GetParishes.Query());

        }

        [HttpGet("{parishId}", Name = "GetParish")]
        public async Task<IActionResult> GetParish(int parishId)
        {
            return await _mediator.Send(new GetParish.Query(parishId));

        }

        [HttpPut("{parishId}")]
        public async Task<IActionResult> UpdateParish(int parishId, JsonPatchDocument<ParishForCreationDto> patchDocument)
        {
            return await _mediator.Send(new UpdateParish.Query(parishId, patchDocument));

        }

        [HttpPost]
        public async Task<IActionResult> PostParish(ParishForCreationDto parish)
        {
            return await _mediator.Send(new PostParish.Query(parish));

        }

        [HttpDelete("{parishId}")]
        public async Task<IActionResult> DeleteParish(int parishId)
        {
            return await _mediator.Send(new DeleteParish.Query(parishId));


        }


    }
}
