using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using MediatR;

namespace IMS.Application.Features.PaymentMethods.Queries
{
    [Authorize]
    public record GetPaymentMethodsQuery(int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedList<PaymentMethodDto>>;
    public class GetPaymentMethodsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetPaymentMethodsQuery, PaginatedList<PaymentMethodDto>>
    {
        public async Task<PaginatedList<PaymentMethodDto>> Handle(GetPaymentMethodsQuery request, CancellationToken cancellationToken)
        {
            return await context.PaymentMethods
                                .OrderBy(pm => pm.MethodName)
                                .ProjectTo<PaymentMethodDto>(mapper.ConfigurationProvider)
                                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
