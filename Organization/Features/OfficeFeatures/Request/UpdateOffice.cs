using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;
using Organization.Features.OfficeFeatures.DTO;
using Organization.Infrastructure;

namespace Organization.Features.OfficeFeatures.Request
{
    public class UpdateOffice
    {
        public class Query : IRequest<IActionResult>
        {
            public int _officeId;
            public JsonPatchDocument<OfficeForCreationDto> _data;
            public Query(int officeId, JsonPatchDocument<OfficeForCreationDto> data)
            {
                _officeId = officeId;
                _data = data;
            }

        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {

            private readonly OrganizationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Office> _validator;
            public Handler(OrganizationDbContext context, IMapper mapper, IValidator<Office> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;

            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var office = await _context.Office.FirstOrDefaultAsync(o => o.OfficeId == request._officeId);

                if (office == null)
                {
                    return new NotFoundResult();
                }

                var officeToUpdate = _mapper.Map<OfficeForCreationDto>(office);

                request._data.ApplyTo(officeToUpdate);

                var officeToValidate = _mapper.Map<Office>(officeToUpdate);

                ValidationResult result = await _validator.ValidateAsync(officeToValidate);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                _mapper.Map(officeToUpdate, office);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
