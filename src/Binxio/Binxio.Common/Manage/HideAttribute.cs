using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class HideAttribute : Attribute
    {
        public HideAttribute(HideFlags flags)
        {
            Flags = flags;
        }

        public HideFlags Flags { get; }
    }

    public enum HideFlags
    {
        List = 1,
        Editor = 2,
        Both = 3
    }

}
