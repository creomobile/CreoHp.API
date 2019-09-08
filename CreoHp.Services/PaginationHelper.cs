using System.Linq;
using System.Threading.Tasks;
using CreoHp.Common;
using CreoHp.Dto.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CreoHp.Services
{
    static class PaginationHelper
    {
        public const int DefaultItemsPerPage = 20;

        public static async Task<SimplePage<TSource>> GetSimplePage<TSource>(
            this IQueryable<TSource> source, PaginationCriteria criteria)
        {
            var itemsPerPage = criteria.ItemsPerPage ?? DefaultItemsPerPage;
            if (itemsPerPage < 1)
                throw new AppException($"Invalid ItemsPerPage ({criteria.ItemsPerPage})");

            var items = await source
                .Skip(itemsPerPage * criteria.PageNum)
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