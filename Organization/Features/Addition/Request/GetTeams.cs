using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class GetTeams
    {
        public class Query : IRequest<IActionResult>
        {
            public Query()
            {
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
                var teams = await _context.Team.Include(e => e.Employees).ToListAsync();
                var teamsToReturn = _mapper.Map<IEnumerable<TeamDto>>(teams);
                var jsonTeams = JsonConvert.SerializeObject(teamsToReturn, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                return new OkObjectResult(jsonTeams);
            }
        }
    }
}
