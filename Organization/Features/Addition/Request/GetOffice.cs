using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Organization.Features.Addition.DTO;
using Organization.Infrastructure;

namespace Organization.Features.Addition.Request
{
    public static class GetOffice
    {
        public class Query : IRequest<IActionResult>
        {
            public int _officeId;
            public Query(int officeId)
            {
                _officeId = officeId;
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

                var office = await _context.Office.Include(p => p.Parish).Include(e => e.Employees).FirstOrDefaultAsync(o => o.OfficeId == request._officeId);

                if (office == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(_mapper.Map<OfficeDto>(office));
            }
        }
    }
}