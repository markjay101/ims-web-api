using IMS.Application.Common.Interfaces;
using IMS.Domain.Common.Enums;
using IMS.Domain.Events;
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
        Guid InternetPlanId
        ) : IRequest<Guid>;
    public class CreateApplicationCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateApplicationCommand, Guid>
    {
        public async Task<Guid> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
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
                InternetPlanId = request.InternetPlanId,
                Status = ApplicationStatus.Pending
            };

            application.AddDomainEvent(new CreatedApplicationEvent(application));

            await context.Applications.AddAsync(application, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            return application.Id;
        }
    }
}
