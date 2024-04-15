using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class DeleteParish
    {
        public class Query : IRequest<IActionResult>
        {
            public int _parishId;
            public Query(int parishId)
            {
                _parishId = parishId;
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
                var parish = await _context.Parish.FirstOrDefaultAsync(p => p.ParishId == request._parishId);

                if (parish == null)
                {
                    return new NotFoundResult();
                }

                _context.Parish.Remove(parish);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
