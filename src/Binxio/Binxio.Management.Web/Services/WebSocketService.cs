using Fleck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class WebSocketService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly WebSocketMessenger websocketMessenger;
        private readonly ILogger<WebSocketService> logger;
        private readonly WebSocketServer wssServer;
        private ConcurrentBag<IWebSocketConnection> sockets = new ConcurrentBag<IWebSocketConnection>();

        public WebSocketService(IServiceScopeFactory scopeFactory, ILogger<WebSocketService> logger)
        {

            this.scopeFactory = scopeFactory;
            this.websocketMessenger = this.scopeFactory.CreateScope().ServiceProvider.GetService<WebSocketMessenger>();
            this.logger = logger;
            wssServer = new WebSocketServer("ws://0.0.0.0:5003/");
            wssServer.RestartAfterListenError = true;

            websocketMessenger.OnComplete += WebsocketMessenger_OnComplete;
            websocketMessenger.OnError += WebsocketMessenger_OnError;
            websocketMessenger.OnInformation += WebsocketMessenger_OnInformation;
            websocketMessenger.OnProgress += WebsocketMessenger_OnProgress;
            websocketMessenger.OnResultAvailable += WebsocketMessenger_OnResultAvailable;
            websocketMessenger.OnStatusChanged += WebsocketMessenger_OnStatusChanged;
            websocketMessenger.OnWarning += WebsocketMessenger_OnWarning;

        }

        private void WebsocketMessenger_OnWarning(Guid operationId, string message) => sendToTaskSubscribers(operationId, new { type = "warning", message });

        private void WebsocketMessenger_OnStatusChanged(Guid operationId, string status) => sendToTaskSubscribers(operationId, new { type = "status", status });

        private void WebsocketMessenger_OnResultAvailable(Guid operationId, object result) => sendToTaskSubscribers(operationId, new { type = "resultAvailable" });

        private void WebsocketMessenger_OnProgress(Guid operationId, int processesComplete, int? totalProcesses = null) => sendToTaskSubscribers(operationId, new { type = "progress", complete = processesComplete, total = totalProcesses });

        private void WebsocketMessenger_OnInformation(Guid operationId, string message) => sendToTaskSubscribers(operationId, new { type = "info", message });

        private void WebsocketMessenger_OnError(Guid operationId, string message) => sendToTaskSubscribers(operationId, new { type = "error", message });

        private void WebsocketMessenger_OnComplete(Guid operationId, bool success) => sendToTaskSubscribers(operationId, new { type = "complete", success });

        private void sendToTaskSubscribers(Guid operationId, object p)
        {
            foreach (var s in sockets.Where(x => x.ConnectionInfo.Path.Contains(operationId.ToString())).ToList())
            {
                s.Send(JsonConvert.SerializeObject(p));
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            wssServer.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    sockets.Add(socket);
                    logger.LogInformation("Client connected from " + socket.ConnectionInfo.ClientIpAddress);
                };
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
        }
    }
}
