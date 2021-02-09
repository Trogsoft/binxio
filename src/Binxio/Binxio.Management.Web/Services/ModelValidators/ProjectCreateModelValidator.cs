using Binxio.Common;
using Binxio.Common.Abstractions;
using Binxio.Common.Manage;
using Binxio.Common.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services.ModelValidators
{
    [ModelValidator("project")]
    public class ProjectCreateModelValidator : IModelValidator<ProjectCreateModel>
    {
        public XioResult<ProjectCreateModel> VaidateModel(string property, ProjectCreateModel model)
        {
            throw new NotImplementedException();
        }

        public XioResult ValidatePropertyValue(string property, string value)
        {
            throw new NotImplementedException();
        }
    }
}
