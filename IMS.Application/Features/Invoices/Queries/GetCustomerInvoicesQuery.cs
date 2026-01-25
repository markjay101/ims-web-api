using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Invoices.Queries
{
    public record GetCustomerInvoicesQuery(Guid CustomerId, int PageNumber = 1, int PageSize = 25) : IRequest<PaginatedList<InvoiceDto>>;
    public class GetCustomerInvoicesQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCustomerInvoicesQuery, PaginatedList<InvoiceDto>>
    {
        public async Task<PaginatedList<InvoiceDto>> Handle(GetCustomerInvoicesQuery request, CancellationToken cancellationToken)
        {
            return await context.Invoices
                .Include(i => i.Payment)
                .Where(i => i.CustomerId == request.CustomerId)
                .OrderByDescending(i => i.CreatedAt)
                .ProjectTo<InvoiceDto>(mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
