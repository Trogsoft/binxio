using Binxio.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Abstractions
{
    public interface ILogWriter
    {
        void Write(XioLogEntry entry);
        void Flush();
    }
}
