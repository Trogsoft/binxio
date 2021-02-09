using Binxio.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class XioServiceResolver
    {

        private Dictionary<string, Type> modelValidators { get; }

        public XioServiceResolver(Dictionary<string, Type> modelValidators1)
        {
            modelValidators = modelValidators1;
        }

        public IModelValidator GetModelValidator(string modelType)
        {
            if (modelValidators.ContainsKey(modelType))
                return (IModelValidator)modelValidators[modelType];

            throw new Exception("Model Validator not found.");
        }

    }
}
