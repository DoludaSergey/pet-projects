namespace MarketingWebHooks.Entities.Base
{
    public interface IEntityBaseWithLock : IEntity
    {
        public bool IsLocked { get; set; }

        public DateTime LockDate { get; set; }
    }
}