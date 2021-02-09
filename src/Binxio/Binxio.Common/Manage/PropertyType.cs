using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class PropertyTypeAttribute : Attribute
    {
        public PropertyTypeAttribute(PropertyType type)
        {
            Type = type;
        }

        public PropertyType Type { get; }
    }

    public enum PropertyType
    {
        DisplayName,
        UrlPart,
        NumericId,
        ReferenceType,
        Value
    }
}
