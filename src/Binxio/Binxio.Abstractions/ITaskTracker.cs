using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Binxio.Abstractions
{
    public interface ITaskTracker
    {
        void SetProgress(int tasksComplete, int? totalTasks = null);
        void SetStatusMessage(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception);
        void Complete(bool success);
        T StoreResult<T>(T resultObject);
        void Initialize(ClaimsPrincipal user, string operationId, string taskTitle);
    }
}
