using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class UpdateTeam
    {
        public class Query : IRequest<IActionResult>
        {
            public int _teamId;
            public JsonPatchDocument<TeamForCreationDto> _data;
            public Query(int teamId, JsonPatchDocument<TeamForCreationDto> data)
            {
                _teamId = teamId;
                _data = data;
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
                var team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == request._teamId);

                if (team == null)
                {
                    return new BadRequestResult();
                }
                var teamToUpdate = _mapper.Map<TeamForCreationDto>(team);

                request._data.ApplyTo(teamToUpdate);

                var teamToValidate = _mapper.Map<Team>(teamToUpdate);

                ValidationResult result = await _validator.ValidateAsync(teamToValidate, cancellationToken);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                _mapper.Map(teamToUpdate, team);
                await _context.SaveChangesAsync();
                return new NoContentResult();
            }
        }
    }
}
