using Binxio.Abstractions;
using Binxio.Common.Manage;
using Binxio.Data;
using Binxio.Management.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Binxio.Management.Web.Controllers
{
    [Authorize]
    [Route("/manage")]
    public class ManageController : Controller
    {
        private readonly BinxioDb db;
        private readonly XioServiceResolver sr;
        private readonly IServiceProvider isp;
        private readonly ITaskManager tm;

        public ManageController(Data.BinxioDb db, XioServiceResolver sr, IServiceProvider isp, ITaskManager tm)
        {
            this.db = db;
            this.sr = sr;
            this.isp = isp;
            this.tm = tm;
        }

        public Type findType(string type)
        {
            var t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.Name == type && y.IsPublic && !y.IsInterface && !y.IsAbstract)).SingleOrDefault();
            return t;
        }

        public Type getModelType<T>(string type) where T : XioTypeAttribute
        {
            var t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.Name == type && y.IsPublic && !y.IsInterface && !y.IsAbstract)).SingleOrDefault();
            var a = t.GetCustomAttribute<T>();
            if (a == null)
                return t;
            else
                return a.Type;
        }

        public Type getModelType(string type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.GetCustomAttributes<XioModelAttribute>().Any() && y.IsPublic && !y.IsInterface && !y.IsAbstract));
            var t = types.SingleOrDefault(x => x.GetCustomAttributes<XioModelAttribute>().FirstOrDefault().UrlPart.Equals(type, StringComparison.CurrentCultureIgnoreCase));
            if (t != null)
                return t;
            else
                throw new Exception("Model not found.");
        }

        public Type getModelType(string type, XioOperationType operation)
        {
            if (operation == XioOperationType.Create)
            {
                var t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y =>
                    y.GetCustomAttribute<XioCreateAttribute>() != null &&
                    y.GetCustomAttribute<XioCreateAttribute>().TypeName.Equals(type, StringComparison.CurrentCultureIgnoreCase) &&
                    y.IsPublic && !y.IsInterface && !y.IsAbstract)).SingleOrDefault();
                return t.GetCustomAttribute<XioCreateAttribute>().Type;
            }
            else if (operation == XioOperationType.List)
                return getModelType<XioListAttribute>(type);

            return findType(type);
        }


        [Route("ui/entities/{type}")]
        public IActionResult GetEntities(string type)
        {
            return Json(db.GetEntities(type));
        }

        [Route("ui/props/{type}/{operation}")]
        public IActionResult GetModelProperties(string type, XioOperationType operation)
        {
            var t = getModelType(type, operation);
            List<UIProperty> properties = new List<UIProperty>();
            foreach (var prop in t.GetProperties())
            {

                var dn = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
                var required = prop.GetCustomAttribute<RequiredAttribute>() != null;
                var editor = prop.GetCustomAttribute<XioEditorAttribute>()?.Editor ?? prop.PropertyType.Name;
                var defValue = prop.GetCustomAttribute<DefaultValueAttribute>()?.Value;
                var hideUnless = prop.GetCustomAttribute<HideUnlessAttribute>()?.Options;

                var p = new UIProperty
                {
                    PropertyName = prop.Name,
                    DisplayName = dn ?? prop.Name,
                    Editor = editor,
                    HideUnless = hideUnless,
                    DefaultValue = defValue,
                    IsRequired = required,
                    Validate = false
                };

                properties.Add(p);

            }
            return Json(properties);
        }

        [Route("ui/validate/{type}/{prop}")]
        public IActionResult ValidateProposedValue(string type, string prop, string value)
        {
            var validator = sr.GetModelValidator(type);
            return Json(validator.ValidatePropertyValue(prop, value));
        }

        [Route("ui/create/{type}")]
        public IActionResult CreateObject(string type, string objectValues)
        {

            var em = getEntityManager(type);
            var entityManager = isp.GetService(em.GetInterfaces().First());

            var method = em.GetMethods().SingleOrDefault(x => x.GetCustomAttribute<XioCreateAttribute>() != null);
            if (method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null)
            {
                // this is async
                var modelType = method.GetCustomAttribute<XioCreateAttribute>().Type;
                var model = JsonConvert.DeserializeObject(objectValues, modelType);
                var result = (Task)method.Invoke(entityManager, new[] { model });
                result.ConfigureAwait(false);
                var rd = result.GetType().GetProperty("Result").GetValue(result);
                return Json(rd);
            }
            else
            {
                var modelType = method.GetCustomAttribute<XioCreateAttribute>().Type;
                var model = JsonConvert.DeserializeObject(objectValues, modelType);
                var result = (Task)method.Invoke(entityManager, new[] { model });
                return Json(result);
            }

        }

        private Type getEntityManager(string type)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes().Where(y => y.GetCustomAttribute<XioEntityManagerAttribute>() != null));
            var em = types.SingleOrDefault(x => x.GetCustomAttribute<XioEntityManagerAttribute>().Entity.Equals(type, StringComparison.CurrentCultureIgnoreCase));
            return em;
        }

        [Route("ui/list/{type}")]
        public IActionResult ListObjects(string type)
        {
            var em = getEntityManager(type);
            var entityManager = isp.GetService(em.GetInterfaces().First());

            var method = em.GetMethods().SingleOrDefault(x => x.GetCustomAttribute<XioListAttribute>() != null);
            var modelType = method.GetCustomAttribute<XioListAttribute>().Type;
            var modelInfo = getModelInfo(modelType);

            if (method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null)
            {
                var result = (Task)method.Invoke(entityManager, null);
                result.ConfigureAwait(false);
                var rd = result.GetType().GetProperty("Result").GetValue(result);
                return Json(new { model = modelInfo, entities = rd });
            }
            else
            {
                var result = (Task)method.Invoke(entityManager, null);
                return Json(new { model = modelInfo, entities = result });
            }

        }

        [Route("ui/entity/{type}/{id}")]
        public IActionResult GetEntity(string type, string id)
        {

            var em = getEntityManager(type);
            var entityManager = isp.GetService(em.GetInterfaces().First());

            var method = em.GetMethods().SingleOrDefault(x => x.GetCustomAttribute<XioGetEntityAttribute>() != null);
            var modelType = method.GetCustomAttribute<XioGetEntityAttribute>().Type;
            var modelInfo = getModelInfo(modelType);

            var amethod = em.GetMethods().SingleOrDefault(x => x.GetCustomAttribute<XioEntityActionsAttribute>() != null);
            var actions = callMethod(amethod, entityManager, new[] { id });
            var entity = callMethod(method, entityManager, new[] { id });

            return Json(new { model = modelInfo, result = entity, actions });

        }

        private object callMethod(MethodInfo method, object entityManager, object[] para = default) 
        {
            if (method.ReturnType.GetMethod(nameof(Task.GetAwaiter)) != null)
            {
                var result = (Task)method.Invoke(entityManager, para);
                result.ConfigureAwait(false);
                var rd = result.GetType().GetProperty("Result").GetValue(result);
                return rd;
            }
            else
            {
                var result = method.Invoke(entityManager, para);
                return result;
            }
        }

        //public new IActionResult Json(object data)
        //{
        //    var xm = data.GetType().GetCustomAttributes<XioModelAttribute>();
        //    if (xm.Any())
        //    {
        //        var m = xm.FirstOrDefault();
        //        var o = JObject.FromObject(data);
        //        o.Add("$model", m.UrlPart);
        //        return Content(o.ToString(), "application/json");
        //    }
        //    else
        //    {
        //        return base.Json(data);
        //    }
        //}

        private Dictionary<string, ModelInfo> getModelInfo(Type modelType)
        {

            Stack<Type> types = new Stack<Type>();
            types.Push(modelType);
            Dictionary<string, ModelInfo> result = new Dictionary<string, ModelInfo>();

            while (types.TryPop(out Type mt))
            {

                var modelInfo = mt.GetCustomAttribute<XioModelAttribute>();

                ModelInfo mi = new ModelInfo
                {
                    Title = modelInfo.Title,
                    UrlPart = modelInfo.UrlPart
                };

                foreach (var prop in mt.GetProperties())
                {
                    var p = new ModelPropertyInfo();
                    var hide = prop.GetCustomAttribute<HideAttribute>();
                    p.IsEditorLink = prop.GetCustomAttribute<EditorLinkAttribute>() != null;
                    p.UrlPart = Char.ToLowerInvariant(prop.Name[0]) + prop.Name.Substring(1);
                    p.IsReadOnly = prop.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? false;
                    p.Priority = 1;
                    p.ReferenceModel = prop.GetCustomAttribute<ModelReferenceAttribute>()?.Type;

                    if (p.ReferenceModel != null)
                    {
                        var nextModel = getModelType(p.ReferenceModel);
                        types.Push(nextModel);
                    }

                    p.Title = prop.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? prop.Name;
                    if (hide != null)
                    {
                        p.ShowInEditor = !hide.Flags.HasFlag(HideFlags.Editor);
                        p.ShowInList = !hide.Flags.HasFlag(HideFlags.List);
                    }
                    else
                    {
                        p.ShowInList = true;
                        p.ShowInEditor = true;
                    }
                    mi.Properties.Add(p);
                }

                result.Add(modelInfo.UrlPart, mi);

            }

            return result;
        }
    }
}
