using Binxio.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Binxio.Abstractions
{

    public interface ITaskBase
    {
    }

    public interface ITaskBase<TModel, TResult> : ITaskBase
    {
        XioResult PerformChecks(TModel model);
        TResult Execute(TModel model, ITaskTracker tracker);
        XioResult Rollback(TModel model);
    }
}
