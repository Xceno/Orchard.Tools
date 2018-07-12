namespace Orchard.Tools.Helpers.Routing {
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Orchard.Mvc.Routes;
    using Orchard.Tools.Helpers.Reflection;

    public class MvcRouteFactory {
        private readonly string moduleAreaName;

        public MvcRouteFactory(string moduleAreaName) {
            this.moduleAreaName = moduleAreaName;
        }

        public RouteDescriptor BuildRoute<TController>(Expression<Func<TController, Func<ActionResult>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam>(Expression<Func<TController, Func<TParam, ActionResult>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam1, TParam2>(Expression<Func<TController, Func<TParam1, TParam2, ActionResult>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, ActionResult>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3, TParam4>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, ActionResult>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController>(Expression<Func<TController, Func<Task<ActionResult>>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam>(Expression<Func<TController, Func<TParam, Task<ActionResult>>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam1, TParam2>(Expression<Func<TController, Func<TParam1, TParam2, Task<ActionResult>>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, Task<ActionResult>>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        public RouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3, TParam4>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, Task<ActionResult>>>> expression, params string[] routeParts)
            where TController : Controller {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return BuildMvcRoute(this.moduleAreaName, typeof(TController), actionName, string.Join("/", routeParts));
        }

        private static RouteDescriptor BuildMvcRoute(string area, Type controller, string action, string url, int priority = 5) {
            var ctrlIndex = controller.Name.LastIndexOf("Controller", StringComparison.Ordinal);
            var controllerName = controller.Name.Substring(0, ctrlIndex);

            return RoutingHelpers.BuildMvcRoute(area, controllerName, action, url, priority);
        }
    }
}
