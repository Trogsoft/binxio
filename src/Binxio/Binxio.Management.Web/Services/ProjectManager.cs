using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Projects;
using Binxio.Management.Web.Tasks.Projects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class ProjectManager : IProjectManager
    {
        private readonly IDataProtectionProvider dpp;
        private readonly IDataProtector protector;
        private readonly ITaskManager taskManager;

        public ProjectManager(IDataProtectionProvider dpp, ITaskManager taskManager)
        {
            this.dpp = dpp;
            this.taskManager = taskManager;
            this.protector = dpp.CreateProtector("ProjectDb");
        }

        public async Task<XioResult> Create(ProjectCreateModel model)
        {
            var result = taskManager.Create<CreateProjectTask, ProjectCreateModel, ProjectModel>(model, $"Creating new project {model}");
            return result;
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
