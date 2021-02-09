using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class XioEntityManagerAttribute : Attribute
    {
        public XioEntityManagerAttribute(string entity)
        {
            Entity = entity;
        }

        public string Entity { get; }
    }
}
