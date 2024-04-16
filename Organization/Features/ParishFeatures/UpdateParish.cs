using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Domain.Entity;
using Organization.Infrastructure;

namespace Organization.Features.ParishFeatures
{
    public class UpdateParish
    {
        public class Query : IRequest<IActionResult>
        {
            public int _parishId;
            public JsonPatchDocument<ParishForCreationDto> _data;
            public Query(int parishId, JsonPatchDocument<ParishForCreationDto> data)
            {
                _parishId = parishId;
                _data = data;
            }

        }

        public class Handler : IRequestHandler<Query, IActionResult>
        {

            private readonly OrganizationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IValidator<Parish> _validator;

            public Handler(OrganizationDbContext context, IMapper mapper, IValidator<Parish> validator)
            {
                _context = context;
                _mapper = mapper;
                _validator = validator;
            }

            public async Task<IActionResult> Handle(Query request, CancellationToken cancellationToken)
            {
                var parish = await _context.Parish.FirstOrDefaultAsync(p => p.ParishId == request._parishId);

                if (parish == null)
                {
                    return new NotFoundResult();
                }

                var parishToUpdate = _mapper.Map<ParishForCreationDto>(parish);

                request._data.ApplyTo(parishToUpdate);

                var parishToValidate = _mapper.Map<Parish>(parishToUpdate);

                ValidationResult result = await _validator.ValidateAsync(parishToValidate, cancellationToken);

                if (!result.IsValid)
                {
                    return new BadRequestObjectResult(result.Errors);
                }

                _mapper.Map(parishToUpdate, parish);

                await _context.SaveChangesAsync();

                return new NoContentResult();
            }
        }
    }
}
