using IMS.Application.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace IMS.Application.Common.Behaviours
{
    public class LoggingBehaviour<TRequest, TRespnse>(ILogger<LoggingBehaviour<TRequest, TRespnse>> logger, ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TRespnse> where TRequest: notnull
    {
        public async Task<TRespnse> Handle(TRequest request, RequestHandlerDelegate<TRespnse> next, CancellationToken cancellationToken)
        {
            var userId = currentUserService?.UserId ?? string.Empty;
            var userName = currentUserService?.Email ?? string.Empty;

            logger.LogInformation("Handling {RequestName} - User: {Username} [{UserId}]", typeof(TRequest).Name, userName, userId);

            var timer = Stopwatch.StartNew();

            var response = await next();

            timer.Stop();

            logger.LogInformation("Finished {RequestName} in {Elapsed}ms",
                typeof(TRequest).Name, timer.ElapsedMilliseconds);

            return response;
        }
    }
}
