using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class ModelValidatorAttribute : Attribute
    {
        public ModelValidatorAttribute(string entity)
        {
            Entity = entity;
        }

        public string Entity { get; }
    }
}
