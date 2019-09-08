using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CreoHp.Repository
{
    public interface IDbInterceptor
    {
        Task BeforeSave(IEnumerable<EntityEntry> entries);
    }
}
