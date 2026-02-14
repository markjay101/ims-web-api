using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Modems.Queries
{
    public record GetAvailableModemsQuery(string? AssignedModemId = null, string? SearchTerm = null) : IRequest<List<ModemDto>>;
    public class GetAvailableModemsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAvailableModemsQuery, List<ModemDto>>
    {
        public async Task<List<ModemDto>> Handle(GetAvailableModemsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Modems.AsNoTracking().Include(m => m.Customer).AsQueryable();

            if (!string.IsNullOrEmpty(request.AssignedModemId))
            {
                var modemId = Guid.Parse(request.AssignedModemId);
                query = query.Where(m => m.Customer == null || m.Id == modemId);
            }
            else
                query = query.Where(m => m.Customer == null);


            if(!string.IsNullOrEmpty(request.SearchTerm))
               query = query.Where(m => m.Model.Contains(request.SearchTerm)
                                        || m.SerialNumber.Contains(request.SearchTerm));

            return await query.ProjectTo<ModemDto>(mapper.ConfigurationProvider)
                                .ToListAsync(cancellationToken: cancellationToken);

        }
    }
}
