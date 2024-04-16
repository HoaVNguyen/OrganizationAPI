using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;
using Organization.Features.OfficeFeatures.DTO;
using Organization.Infrastructure;

namespace Organization.Features.OfficeFeatures.Request
{
    public class PostOffice
    {
        public class Query : IRequest<IActionResult>
        {
            public OfficeForCreationDto _office;
            public Query(OfficeForCreationDto office)
            {
                _office = office;
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
                var parish = await _context.Parish.FirstOrDefaultAsync(p => p.ParishId == request._office.ParishId);

                if (parish == null)
                {
                    return new BadRequestResult();
                }

                var officeToCreate = _mapper.Map<Office>(request._office);

                ValidationResult result = await _validator.ValidateAsync(officeToCreate);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }
                officeToCreate.Parish = parish;
                await _context.AddAsync(officeToCreate);
                await _context.SaveChangesAsync();

                var officeToReturn = _mapper.Map<OfficeDto>(officeToCreate);

                return new CreatedAtRouteResult("GetOffice", new
                {
                    officeId = officeToReturn.OfficeId
                }, officeToReturn);
            }
        }
    }
}