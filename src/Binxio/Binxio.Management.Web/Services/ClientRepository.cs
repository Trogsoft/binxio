using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Clients;
using Binxio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class ClientRepository : IClientRepository
    {
        private readonly BinxioDb db;
        private readonly IXioMapper mapper;

        public ClientRepository(BinxioDb db, IXioMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public XioResult<ClientModel> GetClient(ClientSpecModel model)
        {

            if (model.MicrosoftTenantId != Guid.Empty)
            {
                var existing = db.Clients.SingleOrDefault(x => x.MicrosoftTenantId == model.MicrosoftTenantId);
                if (existing != null)
                    return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(existing));
            }

            if (!string.IsNullOrWhiteSpace(model.UrlPart))
            {
                var existing = db.Clients.SingleOrDefault(x => x.UrlPart == model.UrlPart);
                if (existing != null)
                    return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(existing));
            }

            if (model.Id > 0)
            {
                var existing = db.Clients.SingleOrDefault(x => x.Id == model.Id);
                if (existing != null)
                    return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(existing));
            }

            // this is a create
            var newClient = db.Clients.Add(new Client
            {
                CountryCode = model.CountryCode,
                PostCode = model.PostCode,
                ProviderId = 1, // todo: something with providder
                Title = model.Title,
                MicrosoftTenantId = model.MicrosoftTenantId,
                UrlPart = db.GetUrlPart<Client>(model.Title)
            });
            db.SaveChanges();
            return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(newClient.Entity));

        }
    }
}
