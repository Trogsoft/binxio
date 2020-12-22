using Binxio.Abstractions;
using Binxio.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class TaskManager : ITaskManager
    {
        private readonly IServiceScope scope;
        private readonly IHttpContextAccessor httpcx;

        public TaskManager(IServiceScopeFactory scopeFactory, IHttpContextAccessor httpcx)
        {
            this.scope = scopeFactory.CreateScope();
            this.httpcx = httpcx;
        }

        public XioResult Create<TTask, TModel, TResult>(TModel model, string title) where TTask : ITaskBase<TModel, TResult>
        {
            var task = scope.ServiceProvider.GetService<TTask>();
            var checkResult = task.PerformChecks(model);
            if (checkResult.Status == ResultStatus.Success)
            {
                var tracker = scope.ServiceProvider.GetService<ITaskTracker>();
                tracker.Initialize(httpcx.HttpContext.User, checkResult.OperationId, title);
                checkResult.Status = ResultStatus.Pending;
                Task.Run(() =>
                {
                    var result = task.Execute(model, tracker);
                });
            }
            return checkResult;
        }
    }
}
