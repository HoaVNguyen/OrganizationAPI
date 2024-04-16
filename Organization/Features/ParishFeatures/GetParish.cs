using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Infrastructure;

namespace Organization.Features.ParishFeatures
{
    public class GetParish
    {
        public class Query : IRequest<IActionResult>
        {
            public int _parishId;
            public Query(int parishId)
            {
                _parishId = parishId;
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
                var parish = await _context.Parish.Include(o => o.Office).FirstOrDefaultAsync(p => p.ParishId == request._parishId);

                if (parish == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(_mapper.Map<ParishDto>(parish));
            }
        }
    }
}
