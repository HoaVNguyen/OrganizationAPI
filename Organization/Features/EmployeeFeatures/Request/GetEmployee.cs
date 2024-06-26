﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Features.EmployeeFeatures.DTO;
using Organization.Infrastructure;

namespace Organization.Features.EmployeeFeatures.Request
{
    public static class GetEmployee
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
            private readonly IMapper _mapper;

            public Handler(OrganizationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;

            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {

                var employee = await _context.Employee.Include(t => t.Teams).FirstOrDefaultAsync(
                    e => e.EmployeeId == request._employeeId && e.OfficeId == request._officeId);

                if (employee == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(_mapper.Map<EmployeeDto>(employee));
            }
        }
    }
}
