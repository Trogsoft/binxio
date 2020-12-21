using Binxio.Common;
using Binxio.Common.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Abstractions
{
    public interface IClientRepository 
    {
        XioResult<ClientModel> GetClient(ClientSpecModel model);
    }
}
