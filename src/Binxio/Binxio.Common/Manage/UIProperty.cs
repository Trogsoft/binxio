using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class UIProperty
    {
        public bool IsRequired { get; set; }
        public string DisplayName { get; set; }
        public string Editor { get; set; }
        public bool Validate { get; set; }
        public string PropertyName { get; set; }
        public object DefaultValue { get; set; }
        public HideUnless? HideUnless { get; set; }
    }
}
