using Binxio.Abstractions;
using Binxio.Management.Web.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Tasks
{
    public class TaskTracker : ITaskTracker
    {
        private readonly ILogger<ITaskTracker> logger;
        private readonly WebSocketMessenger wms;

        public Guid OperationId { get; private set; } = Guid.Empty;

        public TaskTracker(ILogger<ITaskTracker> logger, WebSocketMessenger wms)
        {
            this.logger = logger;
            this.wms = wms;
        }

        public void SetOperationId(Guid id)
        {
            if (OperationId == Guid.Empty)
                OperationId = id;
            else
                throw new Exception("Operation is already set.");
        }

        public void Complete(bool success)
        {
            logger.LogInformation("Operation completed " + (success ? "successfully" : "with errors"));
            wms.Complete(OperationId, success);
        }

        public void LogError(string message)
        {
            logger.LogError(message, OperationId);
            wms.LogError(OperationId, message);
        }

        public void LogError(Exception exception)
        {
            logger.LogError(exception, exception.Message, OperationId);
            wms.LogError(OperationId, exception.Message);
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message, OperationId);
            wms.LogInformation(OperationId, message);
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message, OperationId);
            wms.LogWarning(OperationId, message);
        }

        public void SetProgress(int tasksComplete, int? totalTasks = null)
        {
            wms.SetProgress(OperationId, tasksComplete, totalTasks);
        }

        public void SetStatusMessage(string message)
        {
            logger.LogInformation("Current status: " + message, OperationId);
            wms.SetStatusMessage(OperationId, message);
        }

        public T StoreResult<T>(T resultObject)
        {
            wms.StoreResult(OperationId, resultObject);
            return resultObject;
        }
    }
}
