using AutoMapper;
using IMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Queries
{
    public record GetApplicationByIdQuery(string Id) : IRequest<ApplicationDto?>;
    public class GetApplicationByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetApplicationByIdQuery, ApplicationDto?>
    {
        public async Task<ApplicationDto?> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.Id);

            var application = await context.Applications.AsNoTracking().FirstOrDefaultAsync(a => a.Id == guid, cancellationToken);

            
            return mapper.Map<ApplicationDto>(application);
        }
    }
}
