namespace MarketingWebHooks.Entities.Base
{
    public abstract class EntityBaseWithLock : EntityBase, IEntityBaseWithLock, IEntity
    {
        public bool IsLocked { get; set; }

        public DateTime LockDate { get; set; }
    }
}
