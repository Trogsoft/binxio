using Binxio.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Abstractions
{
    public interface ITaskManager
    {
        XioResult Create<TTask, TModel, TResult>(TModel model, string title) where TTask : ITaskBase<TModel, TResult>;
    }
}
