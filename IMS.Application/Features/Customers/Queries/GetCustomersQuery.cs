using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using MediatR;

namespace IMS.Application.Features.Customers.Queries
{
    [Authorize(Role = "SuperAdmin")]
    [Authorize(Role = "Admin")]
    public record GetCustomersQuery(int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedList<CustomerDto>>;
    public class GetCustomersQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCustomersQuery, PaginatedList<CustomerDto>>
    {
        public async Task<PaginatedList<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await context.Customers
                .ProjectTo<CustomerDto>(mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
