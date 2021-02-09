using System;

namespace Binxio.Common.Manage
{
    public class ModelReferenceAttribute : Attribute
    {
        public ModelReferenceAttribute(string type)
        {
            Type = type;
        }

        public string Type { get; }
    }
}