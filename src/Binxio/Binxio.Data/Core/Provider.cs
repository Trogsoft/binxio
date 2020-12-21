using System.Collections.Generic;

namespace Binxio.Data
{
    public class Provider : CodedObject
    {
        public long Id { get; set; }
        public string UrlPart { get; set; }
        public string Title { get; set; }
        public string Domain { get; set; }
        public ICollection<Client> Clients { get; set; } = new HashSet<Client>();
    }
}