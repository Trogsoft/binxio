using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Manage;
using Binxio.Common.Projects;
using Binxio.Data;
using Binxio.Management.Web.Tasks.Projects;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    [XioEntityManager("project")]
    public class ProjectManager : IProjectManager
    {
        private readonly IDataProtectionProvider dpp;
        private readonly IDataProtector protector;
        private readonly ITaskManager taskManager;
        private readonly BinxioDb db;
        private readonly Mapper mapper;

        public ProjectManager(IDataProtectionProvider dpp, ITaskManager taskManager, BinxioDb db, Mapper mapper)
        {
            this.dpp = dpp;
            this.taskManager = taskManager;
            this.db = db;
            this.mapper = mapper;
            this.protector = dpp.CreateProtector("ProjectDb");
        }

        [XioCreate(typeof(ProjectCreateModel))]
        public async Task<XioResult> Create(ProjectCreateModel model)
        {
            var result = taskManager.Create<CreateProjectTask, ProjectCreateModel, ProjectModel>(model, $"Creating new project '{model.Title}'");
            return result;
        }

        [XioEntityActions]
        public async Task<XioResult<IEnumerable<EntityAction>>> GetEntityActions(string project)
        {
            return new XioResult<IEnumerable<EntityAction>>(true, new List<EntityAction> { 
                new EntityAction
                {
                    Title = "Permissions",
                    Component = "ProjectUserManager",
                    Icon = "fa-user"
                }
            });
        }

        [XioGetEntity(typeof(ProjectModel))]
        public async Task<XioResult<ProjectModel>> GetProject(string project)
        {
            var result = db.Projects.Include(x=>x.Client).SingleOrDefault(x => x.UrlPart == project);
            if (result == null)
                return new XioResult<ProjectModel>(false, "Project not found.");

            return new XioResult<ProjectModel>(true, mapper.Map<ProjectModel>(result));

        }

        [XioList(typeof(ProjectModel))] // , typeof(ProjectModelQuery)]
        public async Task<XioResult<IEnumerable<ProjectModel>>> GetProjects()
        {
            return new XioResult<IEnumerable<ProjectModel>>(true,
                db.Projects.Include(x=>x.Client).Select(x => mapper.Map<ProjectModel>(x)).ToList());
        }

    }
}
