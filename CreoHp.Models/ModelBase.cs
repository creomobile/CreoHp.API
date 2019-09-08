using System;
using AutoMapper;
using CreoHp.Models.Users;

namespace CreoHp.Models
{
    public abstract class ModelBase
    {
        protected ModelBase()
        {
            Id = Guid.NewGuid();
            var date = DateTime.UtcNow;
            CreatedAt = date;
            UpdatedAt = date;
        }

        public Guid Id { get; set; }

        [IgnoreMap] public DateTime CreatedAt { get; set; }

        [IgnoreMap] public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? ModifiedByUserId { get; set; }

        public virtual AppIdentityUser ModifiedByUser { get; set; }
    }
}