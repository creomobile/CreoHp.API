using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CreoHp.Models;
using CreoHp.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CreoHp.Api.Services
{
    public sealed class DbInterceptor : IDbInterceptor
    {
        readonly IHttpContextAccessor _httpContextAccessor;

        public DbInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task BeforeSave(IEnumerable<EntityEntry> entries)
        {
            var userId = PrincipalService.GetCurrentUserId(_httpContextAccessor);
            if (!userId.HasValue || userId.Value == Guid.Empty) return Task.CompletedTask;

            entries
                .Select(p => p.Entity)
                .OfType<ModelBase>()
                .ToList()
                .ForEach(p => p.ModifiedByUserId = userId.Value);

            return Task.CompletedTask;
        }
    }
}