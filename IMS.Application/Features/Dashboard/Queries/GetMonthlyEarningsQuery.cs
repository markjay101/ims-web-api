using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Features.Dashboard.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record GetMonthlyEarningsQuery : IRequest<IEnumerable<MonthlyEarningDto>>;
    public class GetMonthlyEarningsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetMonthlyEarningsQuery, IEnumerable<MonthlyEarningDto>>
    {
        public async Task<IEnumerable<MonthlyEarningDto>> Handle(GetMonthlyEarningsQuery request, CancellationToken cancellationToken)
        {
            var currentYear = DateTime.UtcNow.Year;

            var monthlyEarnings = await context.Invoices
                                                .AsNoTracking()
                                                .Where(i => i.Status == InvoiceStatus.PaidConfirmed
                                                            && i.Payment != null
                                                            && i.Payment.PaymentDate.Year == currentYear)
                                                .GroupBy(i => i.Payment!.PaymentDate.Month)
                                                .Select(g => new MonthlyEarningDto
                                                {
                                                    Month = g.Key,
                                                    Earnings = g.Sum(i => i.Payment!.Amount)
                                                })
                                                .OrderBy(me => me.Month)
                                                .ToListAsync(cancellationToken);

            return monthlyEarnings;

        }
    }
}
