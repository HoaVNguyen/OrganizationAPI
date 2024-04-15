using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.Domain.Entity;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class PostParish
    {
        public class Query : IRequest<IActionResult>
        {
            public ParishForCreationDto _parish;
            public Query(ParishForCreationDto parish)
            {
                _parish = parish;
            }

        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {

            private readonly OrganizationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Parish> _validator;

            public Handler(OrganizationDbContext context, IMapper mapper, IValidator<Parish> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var parishToCreate = _mapper.Map<Parish>(request._parish);

                var result = await _validator.ValidateAsync(parishToCreate, cancellationToken);
                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                await _context.AddAsync(parishToCreate);
                await _context.SaveChangesAsync();

                var parishToReturn = _mapper.Map<ParishDto>(parishToCreate);

                return new CreatedAtRouteResult("GetParish", new { parishId = parishToReturn.ParishId }, parishToReturn);
            }
        }
    }
}
