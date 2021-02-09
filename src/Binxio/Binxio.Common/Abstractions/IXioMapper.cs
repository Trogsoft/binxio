using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Abstractions
{
    public interface IXioMapper
    {
        T Map<T>(object source);
        TDest Map<TSource, TDest>(TSource source);
    }
}
