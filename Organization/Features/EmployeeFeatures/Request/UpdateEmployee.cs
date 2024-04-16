using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;
using Organization.Features.EmployeeFeatures.DTO;
using Organization.Infrastructure;

namespace Organization.Features.EmployeeFeatures.Request
{
    public class UpdateEmployee
    {
        public class Query : IRequest<IActionResult>
        {
            public int _officeId;
            public int _employeeId;
            public JsonPatchDocument<EmployeeForCreationDto> _data;
            public Query(int officeId, int employeeId, JsonPatchDocument<EmployeeForCreationDto> data)
            {
                _officeId = officeId;
                _employeeId = employeeId;
                _data = data;
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
                var office = await _context.Office.Include(e => e.Employees).FirstOrDefaultAsync(o => o.OfficeId == request._officeId);

                if (office == null)
                {
                    return new BadRequestResult();
                }

                var employee = await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == request._employeeId && e.OfficeId == request._officeId);

                if (employee == null)
                {
                    return new BadRequestResult();
                }

                var employeeToUpdate = _mapper.Map<EmployeeForCreationDto>(employee);

                request._data.ApplyTo(employeeToUpdate);

                var employeeToValidate = _mapper.Map<Employee>(employeeToUpdate);

                ValidationResult result = await _validator.ValidateAsync(employeeToValidate);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);

                }
                _mapper.Map(employeeToUpdate, employee);
                await _context.SaveChangesAsync();

                return new OkObjectResult(employeeToUpdate);

            }
        }
    }
}
