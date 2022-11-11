using MarketingWebHooks.Entities.Base;

namespace MarketingWebHooks.Entities
{
    public class InvalidPhoneNumber : EntityBaseWithLock
    {
        public string? ResourceSid { get; set; }

        public static InvalidPhoneNumber CreateInvalidPhoneNumber(string resourceSid)
        {
            return new InvalidPhoneNumber()
            {
                Id = Guid.NewGuid().ToString(),
                ResourceSid = resourceSid,
            };
        }
    }
}
