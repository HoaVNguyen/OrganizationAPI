﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class DeleteEmployeeFromTeam
    {
        public class Query : IRequest<IActionResult>
        {
            public int _teamId;
            public int _employeeId;
            public Query(int teamId, int employeeId)
            {
                _teamId = teamId;
                _employeeId = employeeId;
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
                var team = await _context.Team.Include(e => e.Employees).FirstOrDefaultAsync(t => t.TeamId == request._teamId);

                if (team == null)
                {
                    return new NotFoundResult();
                }

                var employee = team.Employees.FirstOrDefault(e => e.EmployeeId == request._employeeId);

                if (employee == null)
                {
                    return new NotFoundResult();
                }

                team.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
