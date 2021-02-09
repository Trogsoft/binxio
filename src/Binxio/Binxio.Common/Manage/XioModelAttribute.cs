using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class XioModelAttribute : Attribute
    {
        public XioModelAttribute(string title, string urlpart, string description)
        {
            Title = title;
            UrlPart = urlpart;
            Description = description;
        }

        public string Title { get; }
        public string UrlPart { get; }
        public string Description { get; }
    }
}
