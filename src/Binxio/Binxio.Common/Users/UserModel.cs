using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Users
{
    public class UserModel : UserPublicInfoModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Created { get; set; }
        public string PreferredLanguage { get; set; }
        public string JobTitle { get; set; }
    }
}
