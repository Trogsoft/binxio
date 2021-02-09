using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class XioEditorAttribute : Attribute
    {
        public XioEditorAttribute(string editor)
        {
            Editor = editor;
        }

        public string Editor { get; }
    }
}
