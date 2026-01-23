using IMS.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Common.Mappings
{
    public static class MappingExtension
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)  where TDestination : class
            => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageNumber, pageSize);
    }
}
