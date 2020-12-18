using Binxio.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class WebSocketMessenger
    {

        public event Action<Guid, string> OnError;
        public event Action<Guid, string> OnWarning;
        public event Action<Guid, string> OnInformation;
        public event Action<Guid, int, int?> OnProgress;
        public event Action<Guid, string> OnStatusChanged;
        public event Action<Guid, object> OnResultAvailable;
        public event Action<Guid, bool> OnComplete;

        public void Complete(Guid operationId, bool success) => OnComplete?.Invoke(operationId, success);

        public void LogError(Guid operationId, string message) => OnError?.Invoke(operationId, message);

        public void LogError(Guid operationId, Exception exception) => OnError?.Invoke(operationId, exception.Message);

        public void LogInformation(Guid operationId, string message) => OnInformation?.Invoke(operationId, message);

        public void LogWarning(Guid operationId, string message) => OnWarning?.Invoke(operationId, message);

        public void SetProgress(Guid operationId, int tasksComplete, int? totalTasks = null) => OnProgress?.Invoke(operationId, tasksComplete, totalTasks);

        public void SetStatusMessage(Guid operationId, string message) => OnStatusChanged?.Invoke(operationId, message);

        public T StoreResult<T>(Guid operationId, T resultObject)
        {
            OnResultAvailable?.Invoke(operationId, resultObject); 
            return resultObject;
        }
    }
}
