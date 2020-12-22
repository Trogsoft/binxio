using Binxio.Abstractions;
using Binxio.Common;
using Binxio.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Services
{
    public class LogWriter : ILogWriter
    {
        private readonly IServiceScopeFactory scopeFactory;
        private readonly Thread writer;

        public LogWriter(IServiceScopeFactory issf)
        {
            this.scopeFactory = issf;
            this.writer = new Thread(writerThread);
            writer.Start();
        }

        private List<XioLogEntry> pending = new List<XioLogEntry>();

        public void Flush()
        {
        }

        private void writerThread()
        {
            while (true)
            {
                lock (pending)
                {
                    if (pending.Any())
                    {
                        using (var scope = scopeFactory.CreateScope())
                        using (var db = scope.ServiceProvider.GetService<BinxioDb>())
                        {
                            foreach (var item in pending.OrderBy(x => x.Time))
                            {

                                var l = new Log();
                                l.OperationId = item.OperationId;
                                l.ContextId = item.ContextId;
                                l.DescribesContextOperation = item.IsContextTitle;
                                l.Message = item.Message;
                                l.Time = item.Time;
                                l.UserId = db.Users.SingleOrDefault(x => x.UrlPart == item.UserId)?.Id;

                                if (item.Context != null && item.Context.Any())
                                {
                                    foreach (var ctx in item.Context)
                                    {
                                        l.Context.Add(new LogContext { Key = ctx.Key, Value = ctx.Value });
                                    }
                                }

                                db.Log.Add(l);

                            }
                            pending.Clear();
                            db.SaveChanges();
                        }
                    }
                }

                Thread.Sleep(1000);
            }
        }

        public void Write(XioLogEntry entry)
        {
            lock (pending)
            {
                pending.Add(entry);
            }
        }
    }
}
