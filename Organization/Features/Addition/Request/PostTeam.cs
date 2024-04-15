using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.Domain.Entity;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class PostTeam
    {
        public class Query : IRequest<IActionResult>
        {
            public TeamForCreationDto _team;
            public Query(TeamForCreationDto team)
            {
                _team = team;
            }

        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {

            private readonly OrganizationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Team> _validator;

            public Handler(OrganizationDbContext context, IMapper mapper, IValidator<Team> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var teamToCreate = _mapper.Map<Team>(request._team);

                ValidationResult result = await _validator.ValidateAsync(teamToCreate, cancellationToken);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                await _context.AddAsync(teamToCreate);
                await _context.SaveChangesAsync();

                var teamToReturn = _mapper.Map<TeamDto>(teamToCreate);
                return new CreatedAtRouteResult("GetTeam", new { teamId = teamToReturn.TeamId }, request._team);
            }
        }
    }
}
