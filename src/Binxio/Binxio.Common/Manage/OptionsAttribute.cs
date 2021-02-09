using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class OptionsAttribute : Attribute
    {
        public OptionsAttribute(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}
