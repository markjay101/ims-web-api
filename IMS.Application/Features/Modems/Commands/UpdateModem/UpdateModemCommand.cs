using IMS.Application.Common.Interfaces;
using MediatR;

namespace IMS.Application.Features.Modems.Commands.UpdateModem
{
    public record UpdateModemCommand(Guid Id, string Model, string SerialNumber, string MacAddress) : IRequest<bool>;
    internal class UpdateModemCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateModemCommand, bool>
    {
        public async Task<bool> Handle(UpdateModemCommand request, CancellationToken cancellationToken)
        {
            var modem = await context.Modems.FindAsync(request.Id, cancellationToken);
          
            modem?.Model = request.Model;
            modem?.SerialNumber = request.SerialNumber;
            modem?.MacAddress = request.MacAddress;

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
