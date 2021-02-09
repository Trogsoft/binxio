using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class XioCreateAttribute : XioTypeAttribute
    {

        public XioCreateAttribute(Type type) : base(type)
        {
        }

        public XioCreateAttribute(string typeName, Type type) : base(type)
        {
            TypeName = typeName;
        }

        public string TypeName { get; }
    }
}
