using Binxio.Common;
using System;
using System.Collections.Generic;

namespace Binxio.Data
{
    public class Log
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public long? UserId { get; set; }
        public User User { get; set; }
        public string OperationId { get; set; }
        public string ContextId { get; set; }
        public string Message { get; set; }
        public XioLogLevel Level { get; set; }
        public ICollection<LogContext> Context { get; set; } = new HashSet<LogContext>();
    }
}