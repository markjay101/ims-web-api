using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.Interfaces;
using IMS.Application.Common.Mappings;
using IMS.Application.Common.Models;
using IMS.Application.Common.Security;
using IMS.Domain.Common.Enums;
using MediatR;

namespace IMS.Application.Features.Users.Queries
{
    [Authorize(Role = UserRoles.SuperAdmin)]
    public record GetAdminsQuery(int PageNumber = 1, int PageSize = 25, string? SearchTerm = null) : IRequest<PaginatedList<UserDto>>;
    public class GetAdminsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetAdminsQuery, PaginatedList<UserDto>>
    {
        public async Task<PaginatedList<UserDto>> Handle(GetAdminsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Users.Where(u => u.Role == Role.SuperAdmin || u.Role == Role.Admin);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {

                query = query.Where(u => u.UserName!.Contains(request.SearchTerm!)
                            || u.FirstName.Contains(request.SearchTerm)
                            || u.LastName.Contains(request.SearchTerm));
            }

            return await query.ProjectTo<UserDto>(mapper.ConfigurationProvider)
                              .PaginatedListAsync(request.PageNumber, request.PageSize);

        }
    }
}
