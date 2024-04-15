using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class DeleteOffice
    {
        public class Query : IRequest<IActionResult>
        {
            public int _officeId;
            public Query(int officeId)
            {
                _officeId = officeId;
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
                var office = await _context.Office.FirstOrDefaultAsync(o => o.OfficeId == request._officeId);


                if (office == null)
                {
                    return new NotFoundResult();
                }

                var employees = await _context.Employee.Where(e => e.OfficeId == request._officeId).ToListAsync();

                foreach (var employee in employees)
                {
                    employee.Office = null;
                }

                _context.Office.Remove(office);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
