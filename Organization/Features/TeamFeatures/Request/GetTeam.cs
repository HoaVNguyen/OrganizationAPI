using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class GetTeam
    {
        public class Query : IRequest<IActionResult>
        {
            public int _teamId;
            public Query(int teamId)
            {
                _teamId = teamId;
            }

        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {

            private readonly OrganizationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(OrganizationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var team = await _context.Team.Include(e => e.Employees).FirstOrDefaultAsync(t => t.TeamId == request._teamId);

                if (team == null)
                {
                    return new NotFoundResult();
                }

                var teamToReturn = _mapper.Map<TeamDto>(team);
                var jsonTeam = JsonConvert.SerializeObject(teamToReturn, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return new OkObjectResult(jsonTeam);
            }
        }

    }
}
