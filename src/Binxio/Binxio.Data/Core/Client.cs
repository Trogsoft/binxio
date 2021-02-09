using System;
using System.Collections.Generic;

namespace Binxio.Data
{
    public class Client : CodedObject
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? ProviderId { get; set; }
        public Provider ServiceProvider { get; set; }
        public Guid MicrosoftTenantId { get; set; }
        public string CountryCode { get; set; }
        public string PostCode { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<Project> Projects { get; set; } = new HashSet<Project>();
    }
}