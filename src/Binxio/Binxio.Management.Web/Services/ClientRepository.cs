using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Clients;
using Binxio.Common.Manage;
using Binxio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    [XioEntityManager("client")]
    public class ClientRepository : IClientRepository
    {
        private readonly BinxioDb db;
        private readonly IXioMapper mapper;
        private readonly IXioLog<ClientRepository> log;

        public ClientRepository(BinxioDb db, IXioMapper mapper, IXioLog<ClientRepository> log)
        {
            this.db = db;
            this.mapper = mapper;
            this.log = log;
        }


        [XioGetEntity(typeof(ClientModel))]
        public XioResult<ClientModel> GetClient(string urlPart)
        {
            return GetClient(new ClientSpecModel { UrlPart = urlPart });
        }

        public XioResult<ClientModel> GetClient(ClientSpecModel model)
        {

            log.DescribeOperation("GetClient");

            if (model.MicrosoftTenantId != Guid.Empty)
            {
                var existing = db.Clients.SingleOrDefault(x => x.MicrosoftTenantId == model.MicrosoftTenantId);
                if (existing != null)
                {
                    log.LogInformation("GetClient returned existing client.", ("client", existing.UrlPart));
                    return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(existing));
                }
            }

            if (!string.IsNullOrWhiteSpace(model.UrlPart))
            {
                var existing = db.Clients.SingleOrDefault(x => x.UrlPart == model.UrlPart);
                if (existing != null)
                {
                    log.LogInformation("GetClient returned existing client.", ("client", existing.UrlPart));
                    return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(existing));
                }
            }

            if (model.Id > 0)
            {
                var existing = db.Clients.SingleOrDefault(x => x.Id == model.Id);
                if (existing != null)
                {
                    log.LogInformation("GetClient returned existing client.", ("client", existing.UrlPart));
                    return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(existing));
                }
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
            log.LogInformation("GetClient created new client.", ("client", newClient.Entity.UrlPart));
            return new XioResult<ClientModel>(true, mapper.Map<ClientModel>(newClient.Entity));

        }
    }
}
