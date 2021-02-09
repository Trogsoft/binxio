using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class XioTypeAttribute : Attribute
    {
        public XioTypeAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}
