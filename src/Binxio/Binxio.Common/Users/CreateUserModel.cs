using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Users
{
    public class CreateUserModel : UserModel
    {
        public Guid MicrosoftAccountId { get; set; }
        public long ClientId { get; set; }
        public string AccessToken { get; set; }
    }
}
