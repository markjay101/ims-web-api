using AutoMapper;
using IMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Customers.Queries.GetCustomerById
{
    public record GetCustomerByIdQuery(string Id) : IRequest<CustomerDto?>;
    public class GetCustomerByIdQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
    {
        public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.Id);

            var customer = await context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == guid, cancellationToken);

            return mapper.Map<CustomerDto>(customer);
        }
    }
}
