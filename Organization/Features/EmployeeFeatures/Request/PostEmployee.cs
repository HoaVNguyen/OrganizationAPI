using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;
using Organization.Features.EmployeeFeatures.DTO;
using Organization.Infrastructure;

namespace Organization.Features.EmployeeFeatures.Request
{
    public class PostEmployee
    {
        public class Query : IRequest<IActionResult>
        {
            public EmployeeForCreationDto _employee;
            public int _officeId;

            public string _teamName;

            public Query(EmployeeForCreationDto employee, int officeId, string teamName)
            {
                _employee = employee;
                _officeId = officeId;
                _teamName = teamName;
            }

        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {
            private readonly OrganizationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Employee> _validator;

            public Handler(OrganizationDbContext context, IMapper mapper, IValidator<Employee> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {


                var office = await _context.Office.FirstOrDefaultAsync(o => o.OfficeId == request._officeId);

                if (office == null)
                {
                    return new BadRequestResult();
                }

                var team = await _context.Team.FirstOrDefaultAsync(t => t.Name == request._teamName);

                if (team == null)
                {
                    return new BadRequestResult();
                }

                var employeeToCreate = _mapper.Map<Employee>(request._employee);

                // validate the created employee to the model
                ValidationResult result = await _validator.ValidateAsync(employeeToCreate, cancellationToken);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                employeeToCreate.Teams.Add(team);
                office.Employees.Add(employeeToCreate);
                await _context.SaveChangesAsync();

                var employeeToReturn = _mapper.Map<EmployeeDto>(employeeToCreate);

                return new CreatedAtRouteResult("GetEmployee", routeValues:
                new
                {
                    employeeId = employeeToReturn.EmployeeId,
                    officeId = request._officeId

                }, value: employeeToReturn);
            }
        }
    }
}
