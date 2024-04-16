using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.ParishFeatures
{
    public class GetParishes
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
                var parishes = await _context.Parish.Include(o => o.Office).ToListAsync();

                return new OkObjectResult(_mapper.Map<IEnumerable<ParishDto>>(parishes));
            }
        }
    }
}
