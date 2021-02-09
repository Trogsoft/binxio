using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Abstractions
{

    public interface IModelValidator
    {
        XioResult ValidatePropertyValue(string property, string value);
    }

    public interface IModelValidator<T> : IModelValidator
    {
        XioResult<T> VaidateModel(string property, T model);
    }
}
