using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public class GetOffices
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

                var offices = await _context.Office.Include(p => p.Parish).Include(e => e.Employees).ToListAsync();

                return new OkObjectResult(_mapper.Map<IEnumerable<OfficeDto>>(offices));
            }
        }
    }
}