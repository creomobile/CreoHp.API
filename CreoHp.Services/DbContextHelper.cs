using CreoHp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace CreoHp.Services
{
    static class DbContextHelper
    {
        public static void ModifyCollection<T>(this AppDbContext dbContext, IEnumerable<T> source, IEnumerable<T> modified)
        {
            dbContext.AddRange((IEnumerable<object>)modified.Except(source));
            dbContext.RemoveRange((IEnumerable<object>)source.Except(modified));
        }
    }
}
