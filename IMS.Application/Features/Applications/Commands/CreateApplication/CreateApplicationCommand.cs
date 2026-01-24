using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using MediatR;

namespace IMS.Application.Features.Applications.Commands.CreateApplication
{
    public record CreateApplicationCommand(
        string FirstName,
        string LastName,
        string Email,
        string ContactNumber,
        string Address,
        string City,
        string Country,
        string PostalCode,
        string InternetPlanId
        ) : IRequest<bool>;
    public class CreateApplicationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateApplicationCommand, bool>
    {
        public async Task<bool> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = new Domain.Entities.Application
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                ContactNumber = request.ContactNumber,
                Address = request.Address,
                City = request.City,
                Country = request.Country,
                PostalCode = request.PostalCode,
                InternetPlanId = Guid.Parse(request.InternetPlanId),
                Status = ApplicationStatus.Pending
            };

            await context.Applications.AddAsync(application, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
