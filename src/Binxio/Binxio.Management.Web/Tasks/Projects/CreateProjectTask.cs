using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Clients;
using Binxio.Common.Manage;
using Binxio.Common.Projects;
using Binxio.Data;
using Binxio.Management.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Tasks.Projects
{
    [XioCreate("project", typeof(ProjectCreateModel))]
    public class CreateProjectTask : ITaskBase<ProjectCreateModel, ProjectModel>
    {
        private readonly BinxioDb db;
        private readonly Mapper mapper;

        public CreateProjectTask(BinxioDb db, Mapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public ProjectModel Execute(ProjectCreateModel model, ITaskTracker tracker)
        {

            tracker.SetStatusMessage($"Creating project '{model.Title}'");
            var client = db.Clients.SingleOrDefault(x => x.Id == model.ClientId);

            var np = new Project
            {
                UrlPart = db.GetUrlPart<Project>(model.Title),
                ClientId = model.ClientId,
                Title = model.Title
            };
            db.Projects.Add(np);

            db.SaveChanges();

            tracker.SetStatusMessage("Beginning task...");
            tracker.SetProgress(0, 3);
            Thread.Sleep(6000);
            tracker.LogInformation("Beginning thing to do.");
            tracker.SetProgress(1);
            Thread.Sleep(8000);
            tracker.LogWarning("Couldn't do this thing but continuing anyway.");
            tracker.SetProgress(2);
            Thread.Sleep(12000);
            tracker.SetProgress(3);
            Thread.Sleep(5000);
            tracker.Complete(true);

            return tracker.StoreResult(new ProjectModel
            {
                UrlPart = np.UrlPart,
                Title = np.Title,
                Client = mapper.Map<ClientModel>(client)
            });
        }

        public XioResult PerformChecks(ProjectCreateModel model)
        {

            var client = db.Clients.SingleOrDefault(x => x.Id == model.ClientId);
            if (client == null)
                return new XioResult(false, "No client found.");

            if (string.IsNullOrWhiteSpace(model.Title))
                return new XioResult(false, "No title was entered.");

            if (db.Projects.Any(x => x.ClientId == model.ClientId && x.Title == model.Title))
                return new XioResult(false, "Project already exists.");

            return new XioResult(true);

        }

        public XioResult Rollback(ProjectCreateModel model)
        {
            throw new NotImplementedException();
        }
    }
}
