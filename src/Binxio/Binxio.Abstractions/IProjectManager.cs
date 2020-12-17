using Binxio.Common;
using Binxio.Common.Projects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Binxio.Abstractions
{
    public interface IProjectManager
    {
        Task<XioResult<ProjectModel>> GetProject(string project);
        Task<XioResult<IEnumerable<ProjectModel>>> GetProjects();
        Task<XioResult<ProjectModel>> Create(ProjectCreateModel model);
    }
}
