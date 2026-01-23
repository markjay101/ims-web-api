using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Security;
using MediatR;
using System.Reflection;

namespace IMS.Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse>(ICurrentUserService currentUserService) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                if (currentUserService.UserId == null)
                {
                    throw new UnauthorizedAccessException("Unauthorized. No valid token found.");
                }

                var authorizeAttributesWithRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Role));

                if (authorizeAttributesWithRoles.Any())
                {
                    bool authorized = false;

                    foreach (var authorizeAttribute in authorizeAttributesWithRoles)
                    {
                        if(authorizeAttribute.Role == currentUserService.Role)
                            authorized = true;
                    }

                    if(!authorized)
                    {
                        throw new UnauthorizedAccessException("Unauthorized. You do not have the necessary role for this request");
                    }
                }
            }

            return await next(cancellationToken);
        }
    }
}
