using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Features.EmployeeFeatures.DTO;
using Organization.Infrastructure;

namespace Organization.Features.EmployeeFeatures.Request
{
    public class GetEmployees
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

                var employeeEntities = await _context.Employee.Include(t => t.Teams).ToListAsync();
                return new OkObjectResult(_mapper.Map<IEnumerable<EmployeeDto>>(employeeEntities));
            }
        }
    }
}
