using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Users
{
    public class SingleUserQueryModel
    {
        public SingleUserQueryModel(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
        public SingleUserQueryModel(Guid microsoftAccountId)
        {
            MicrosoftAccountId = microsoftAccountId;
        }

        public SingleUserQueryModel()
        {

        }


        public string EmailAddress { get; set; }
        public Guid MicrosoftAccountId { get; set; }

    }
}
