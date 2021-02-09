using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;

namespace Binxio.Common.Manage
{
    public class XioModel
    {
        public XioModel()
        {
            var xm = this.GetType().GetCustomAttributes<XioModelAttribute>();
            if (xm.Any())
            {
                Model = xm.FirstOrDefault().UrlPart;
            }
            else
            {
                var m = this.GetType().Name;
                Model = Char.ToLowerInvariant(m[0]) + m.Substring(1);
            }
        }

        public string Model { get; }
    }
}
