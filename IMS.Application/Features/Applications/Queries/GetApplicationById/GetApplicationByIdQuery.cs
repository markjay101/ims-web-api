using AutoMapper;
using IMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Applications.Queries.GetApplicationById
{
    public record GetApplicationByIdQuery(Guid Id) : IRequest<ApplicationDto?>;
    public class GetApplicationByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetApplicationByIdQuery, ApplicationDto?>
    {
        public async Task<ApplicationDto?> Handle(GetApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var application = await context.Applications.AsNoTracking().FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
            
            return mapper.Map<ApplicationDto>(application);
        }
    }
}
