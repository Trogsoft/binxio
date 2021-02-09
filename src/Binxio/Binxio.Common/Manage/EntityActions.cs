using System;
using System.Collections.Generic;
using System.Text;

namespace Binxio.Common.Manage
{
    public class EntityActions<T>
    {
        public EntityActions(T entity)
        {
            Entity = entity;
        }

        public List<EntityAction> Actions { get; set; } = new List<EntityAction>();

        public T Entity { get; }
    }

    public class EntityAction
    {
        public string Title { get; set; }
        public string Component { get; set; }
        public string Icon { get; set; }
    }
}
