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

        static async Task<TPage> GetPageBase<TPage, TItem>(IQueryable<TItem> source, PaginationCriteriaBase criteria)
            where TPage : SimplePage<TItem>, new()
        {
            var itemsPerPage = criteria.ItemsPerPage ?? DefaultItemsPerPage;
            if (itemsPerPage < 1)
                throw new AppException($"Invalid ItemsPerPage ({criteria.ItemsPerPage})");

            if (criteria is PaginationCriteria c) source = source.Skip(itemsPerPage * c.PageNum);

            var items = await source
                .Take(itemsPerPage + 1)
                .ToArrayAsync();

            return new TPage
            {
                Items = items.Take(itemsPerPage).ToArray(),
                HasMore = items.Length > itemsPerPage
            };
        }

        public static Task<SimplePage<T>> GetSimplePage<T>(
            this IQueryable<T> source, PaginationCriteriaBase criteria) => GetPageBase<SimplePage<T>, T>(source, criteria);

        public static async Task<Page<T>> GetPage<T>(this IQueryable<T> source, PaginationCriteriaBase criteria)
        {
            var page = await GetPageBase<Page<T>, T>(source, criteria);
            page.Total = await source.CountAsync();
            return page;
        }
    }
}