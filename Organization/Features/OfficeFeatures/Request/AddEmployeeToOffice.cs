using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.OfficeFeatures.Request
{
    public class AddEmployeeToOffice
    {
        public class Query : IRequest<IActionResult>
        {
            public int _officeId;
            public int _employeeId;
            public Query(int officeId, int employeeId)
            {
                _officeId = officeId;
                _employeeId = employeeId;
            }
        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {
            private readonly OrganizationDbContext _context;


            public Handler(OrganizationDbContext context, IMapper mapper)
            {
                _context = context;

            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var office = await _context.Office.Include(e => e.Employees).FirstOrDefaultAsync(t => t.OfficeId == request._officeId);

                if (office == null)
                {
                    return new NotFoundResult();
                }
                var employeeExists = office.Employees.FirstOrDefault(e => e.EmployeeId == request._employeeId);
                if (employeeExists != null)
                {
                    return new BadRequestResult();
                }

                var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == request._employeeId);

                if (employee == null)
                {
                    return new NotFoundResult();
                }

                office.Employees.Add(employee);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
