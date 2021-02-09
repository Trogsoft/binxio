using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class HideUnlessAttribute : Attribute
    {
        public HideUnlessAttribute(HideUnless opts)
        {
            this.Options = opts;
        }

        public HideUnless Options { get; private set; }
    }

    public enum HideUnless
    {
        MoreThanOneOption = 1
    }
}
