using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Projects;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class ProjectManager : IProjectManager
    {
        private readonly IDataProtectionProvider dpp;
        private readonly IDataProtector protector;

        public ProjectManager(IDataProtectionProvider dpp)
        {
            this.dpp = dpp;
            this.protector = dpp.CreateProtector("ProjectDb");
        }
        
        public async Task<XioResult<ProjectModel>> Create(ProjectCreateModel model)
        {
            var cs = "a connection string";
            protector.Protect(cs);
            throw new NotImplementedException();
        }

        public Task<XioResult<ProjectModel>> GetProject(string project)
        {
            throw new NotImplementedException();
        }

        public Task<XioResult<IEnumerable<ProjectModel>>> GetProjects()
        {
            throw new NotImplementedException();
        }

    }
}
