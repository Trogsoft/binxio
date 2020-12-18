using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Common.Projects;
using Binxio.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Tasks.Projects
{
    public class CreateProjectTask : ITaskBase<ProjectCreateModel, ProjectModel>
    {
        //private readonly BinxioDb db;

        public CreateProjectTask()
        {
            //this.db = db;
        }

        public ProjectModel Execute(ProjectCreateModel model, ITaskTracker tracker)
        {
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

            });
        }

        public XioResult PerformChecks(ProjectCreateModel model)
        {
            return new XioResult(true);
        }

        public XioResult Rollback(ProjectCreateModel model)
        {
            throw new NotImplementedException();
        }
    }
}
