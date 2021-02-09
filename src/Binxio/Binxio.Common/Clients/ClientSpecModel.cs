using Binxio.Common.Manage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Clients
{
    [XioModel("Client Spec", "client-spec", "Client Spec")]
    public class ClientSpecModel : ClientModel
    {
        public ClientSpecModel()
        {
        }

        public Guid MicrosoftTenantId { get; set; }
    }
}
