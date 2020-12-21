using Binxio.Common.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Users
{
    public class UserPublicInfoModel
    {
        public long Id { get; set; }
        public ClientModel Client { get; set; }
        public string UrlPart { get; set; }
        public string DisplayName { get; set; }
    }
}
