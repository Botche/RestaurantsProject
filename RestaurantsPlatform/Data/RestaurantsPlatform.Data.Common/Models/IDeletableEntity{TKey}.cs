namespace RestaurantsPlatform.Data.Common.Models
{
    using System;

    public abstract class IDeletableEntity<TKey> : BaseModel<TKey>, IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
