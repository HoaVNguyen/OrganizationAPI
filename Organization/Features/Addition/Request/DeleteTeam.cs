using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class DeleteTeam
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

            public Handler(OrganizationDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var team = await _context.Team.FirstOrDefaultAsync(t => t.TeamId == request._teamId);
                if (team == null)
                {
                    return new NotFoundResult();
                }

                _context.Team.Remove(team);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
