using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using IMS.Domain.Entities;
using MediatR;

namespace IMS.Application.Features.Modems.Commands.CreateModem
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    [Authorize(Role = UserRoles.Admin)]
    public record CreateModemCommand(string SerialNumber, string MacAddress) : IRequest<Guid>;
    public class CreateModemCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateModemCommand, Guid>
    {
        public async Task<Guid> Handle(CreateModemCommand request, CancellationToken cancellationToken)
        {
            var newModem = new Modem
            {
                SerialNumber = request.SerialNumber,
                MacAddress = request.MacAddress
            };

            await context.Modems.AddAsync(newModem, cancellationToken);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0 ? newModem.Id : Guid.Empty;
        }
    }
}
