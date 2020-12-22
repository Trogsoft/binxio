using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Common
{
    public class XioLogEntry
    {
        public string OperationId { get; set; }
        public string ContextId { get; set; }
        public string UserId { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public XioLogLevel Level { get; set; }
        public Dictionary<string, string> Context { get; set; } = new Dictionary<string, string>();
        public bool IsContextTitle { get; set; }
    }
}
