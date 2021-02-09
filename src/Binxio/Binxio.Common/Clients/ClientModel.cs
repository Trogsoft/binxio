using Binxio.Common.Manage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Clients
{
    [XioModel("Client", "client", "Client")]
    public class ClientModel : XioModel
    {
        public ClientModel()
        {
        }

        [PropertyType(PropertyType.NumericId)]
        public long Id { get; set; }
        [PropertyType(PropertyType.UrlPart)]   
        public string UrlPart { get; set; }
        [PropertyType(PropertyType.DisplayName)]
        [EditorLink]
        public string Title { get; set; }
        [PropertyType(PropertyType.Value)]
        public string CountryCode { get; set; }
        [PropertyType(PropertyType.Value)]
        public string PostCode { get; set; }
    }
}
