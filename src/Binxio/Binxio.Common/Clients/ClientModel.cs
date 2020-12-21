using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Clients
{
    public class ClientModel
    {
        public long Id { get; set; }
        public string UrlPart { get; set; }
        public string Title { get; set; }
        public string CountryCode { get; set; }
        public string PostCode { get; set; }
    }
}
