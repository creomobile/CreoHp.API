using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CreoHp.Models.PhraseCollections;
using CreoHp.Models.Phrases;
using CreoHp.Models.Tags;
using CreoHp.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CreoHp.Repository
{
    public class AppDbContext : IdentityDbContext<
        AppIdentityUser,
        AppIdentityRole,
        Guid,
        IdentityUserClaim<Guid>,
        AppIdentityUserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>
    {
        readonly IDbInterceptor _interceptor;

        // ReSharper disable once SuggestBaseTypeForParameter
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _interceptor = this.GetService<IDbInterceptor>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurations();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // ReSharper disable once InvertIf
            if (_interceptor != null)
            {
                var entries = ChangeTracker.Entries()
                    .Where(p => p.State != EntityState.Unchanged)
                    .ToArray();

                if (entries.Length > 0)
                    await _interceptor.BeforeSave(entries);
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public DbSet<Phrase> Phrases { get; set; }
        public DbSet<PhraseCollection> PhraseCollections { get; set; }
        public DbSet<BadCollectionUrl> BadCollectionUrls { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagRelation> TagRelations { get; set; }
        public DbSet<PhraseTag> PhraseTags { get; set; }
    }
}