using System.Linq;
using System.Threading.Tasks;
using CreoHp.Common;
using CreoHp.Dto.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CreoHp.Services
{
    static class PaginationHelper
    {
        public const int DefaultItemsPerPage = 30;

        public static async Task<SimplePage<TSource>> GetPage<TSource>(
            this IQueryable<TSource> source, PaginationCriteriaBase criteria)
        {
            var itemsPerPage = criteria.ItemsPerPage ?? DefaultItemsPerPage;
            if (itemsPerPage < 1)
                throw new AppException($"Invalid ItemsPerPage ({criteria.ItemsPerPage})");

            if (criteria is PaginationCriteria c) source = source.Skip(itemsPerPage * c.PageNum);

            var items = await source
                .Take(itemsPerPage + 1)
                .ToArrayAsync();

            return new SimplePage<TSource>
            {
                Items = items.Take(itemsPerPage).ToArray(),
                HasMore = items.Length > itemsPerPage
            };
        }
    }
}