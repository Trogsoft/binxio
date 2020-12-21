using System;

namespace Binxio.Data
{
    public class User : CodedObject
    {
        public long Id { get; set; }
        public long? ClientId { get; set; }
        public Client Client { get; set; }
        public bool IsClientAdmin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Guid MicrosoftAccountId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUsed { get; set; }
        public string PreferredLanguage { get; set; }
        public string JobTitle { get; set; }
        public string MicrosoftAccessToken { get; set; }
    }
}