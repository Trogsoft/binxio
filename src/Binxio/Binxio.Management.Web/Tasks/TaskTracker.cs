﻿using Binxio.Abstractions;
using Binxio.Management.Web.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Tasks
{
    public class TaskTracker : ITaskTracker
    {
        private readonly IXioLog<ITaskTracker> logger;
        private readonly IHubContext<RealtimeHub> hub;

        public string OperationId { get; private set; } = Guid.Empty.ToString();
        public string UserUrlPart { get; private set; } = null;
        public string TaskTitle { get; private set; }

        public TaskTracker(IXioLog<ITaskTracker> logger, IHubContext<RealtimeHub> hub)
        {
            this.logger = logger;
            this.hub = hub;
        }

        public void Initialize(ClaimsPrincipal user, string operationId, string taskTitle)
        {
            OperationId = operationId;
            UserUrlPart = user.FindFirstValue(ClaimTypes.NameIdentifier);
            TaskTitle = taskTitle;
            hub.Clients.User(UserUrlPart).SendAsync("taskBegin", new { OperationId, TaskTitle });
            logger.DescribeOperation(taskTitle);
            logger.OverrideUser(user);
        }

        public void Complete(bool success)
        {
            logger.LogInformation("Operation completed " + (success ? "successfully" : "with errors"));
            hub.Clients.User(UserUrlPart).SendAsync("taskComplete", new { OperationId, TaskTitle, success });
        }

        public void LogError(string message)
        {
            logger.LogError(message);
            hub.Clients.User(UserUrlPart).SendAsync("taskError", new { OperationId, TaskTitle, message });
        }

        public void LogError(Exception exception)
        {
        }

        public void LogInformation(string message)
        {
            logger.LogInformation(message);
            hub.Clients.User(UserUrlPart).SendAsync("taskInfo", new { OperationId, TaskTitle, message });
        }

        public void LogWarning(string message)
        {
            logger.LogWarning(message);
            hub.Clients.User(UserUrlPart).SendAsync("taskWarning", new { OperationId, TaskTitle, message });
        }

        public void SetProgress(int tasksComplete, int? totalTasks = null)
        {
            hub.Clients.User(UserUrlPart).SendAsync("taskProgress", new { OperationId, TaskTitle, tasksComplete, totalTasks });
        }

        public void SetStatusMessage(string message)
        {
            logger.LogInformation("Current status: " + message);
            hub.Clients.User(UserUrlPart).SendAsync("taskStatus", new { OperationId, TaskTitle, message });
        }

        public T StoreResult<T>(T resultObject)
        {
            return resultObject;
        }

    }
}
