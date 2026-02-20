using AutoMapper;
using IMS.Application.Common.Interfaces;
using IMS.Application.Features.Modems.Queries;
using MediatR;

namespace IMS.Application.Features.Modems.Commands.UpdateModem
{
    public record UpdateModemCommand(Guid Id, string Model, string SerialNumber, string MacAddress) : IRequest<ModemDto?>;
    internal class UpdateModemCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<UpdateModemCommand, ModemDto?>
    {
        public async Task<ModemDto?> Handle(UpdateModemCommand request, CancellationToken cancellationToken)
        {
            var modem = await context.Modems.FindAsync(request.Id, cancellationToken);

            if (modem == null)
                return null;
          
            modem.Model = request.Model;
            modem.SerialNumber = request.SerialNumber;
            modem.MacAddress = request.MacAddress;

            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<ModemDto>(modem);
        }
    }
}
