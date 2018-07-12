namespace Orchard.Tools.Helpers {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Routing;
    using System.Web.Http.Routing.Constraints;
    using Orchard.Mvc.Routes;

    /// <summary>
    /// The ApiRouteFactoy makes it possible to easily write WebApi Routes in the style of AttributeRouting,
    /// which is currently not supported in Orchard.
    /// </summary>
    public class ApiRouteFactory {
        private readonly string baseApiRoute;
        private readonly string moduleAreaName;

        public ApiRouteFactory(string moduleAreaName, string baseApiRoute) {
            this.baseApiRoute = baseApiRoute.EndsWith("/") ? baseApiRoute : string.Format("{0}/", baseApiRoute);
            this.moduleAreaName = moduleAreaName;
        }

        /// <summary>
        /// Creates a new RouteDescriptor in the style of attributeRouting. Uses the module as default area
        /// </summary>
        /// <typeparam name="T">The type of the controller</typeparam>
        /// <param name="routeSlug">The route of the action. Will be appended to the base api route.</param>
        /// <param name="action">
        /// The name of the action.
        /// <remarks>Prefix it with the desired httpVerb for the action</remarks>
        /// <example>GetUser, PostUser, PutUser</example>
        /// </param>
        /// <returns>A fully configured HttpRouteDescriptor</returns>
        public HttpRouteDescriptor BuildRoute<T>(string routeSlug, string action)
            where T : ApiController {
            return this.MakeRouteDescriptor(typeof(T), routeSlug, action);
        }

        /// <summary>
        /// Creates a new RouteDescriptor in the style of attributeRouting. Uses the module as default area
        /// </summary>
        /// <typeparam name="TController">The type of the controller</typeparam>
        /// <param name="expression">
        /// An expression to get the actionName.
        /// <remarks>Prefix it with the desired httpVerb for the action</remarks>
        /// <example>GetUser, PostUser, PutUser</example>
        /// </param>
        /// <param name="routeParts">The separate route  parts of the action. Will be appended to the base api route.</param>
        /// <returns>A fully configured HttpRouteDescriptor</returns>
        public HttpRouteDescriptor BuildRouteExplicit<TController, TParam, TResult>(Expression<Func<TController, Func<TParam, TResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRouteExplicit<TController, TParam1, TParam2, TResult>(Expression<Func<TController, Func<TParam1, TParam2, TResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRouteExplicit<TController, TParam1, TParam2, TParam3, TResult>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRouteExplicit<TController, TParam1, TParam2, TParam3, TParam4, TResult>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, TResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController>(Expression<Func<TController, Func<IHttpActionResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam>(Expression<Func<TController, Func<TParam, IHttpActionResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2>(Expression<Func<TController, Func<TParam1, TParam2, IHttpActionResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, IHttpActionResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3, TParam4>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, IHttpActionResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3, TParam4, TParam5>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, TParam5, IHttpActionResult>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController>(Expression<Func<TController, Func<Task<IHttpActionResult>>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam>(Expression<Func<TController, Func<TParam, Task<IHttpActionResult>>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2>(Expression<Func<TController, Func<TParam1, TParam2, Task<IHttpActionResult>>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, Task<IHttpActionResult>>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3, TParam4>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, Task<IHttpActionResult>>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        public HttpRouteDescriptor BuildRoute<TController, TParam1, TParam2, TParam3, TParam4, TParam5>(Expression<Func<TController, Func<TParam1, TParam2, TParam3, TParam4, TParam5, Task<IHttpActionResult>>>> expression, params string[] routeParts)
            where TController : ApiController {
            var actionName = ReflectOnMethod<TController>.NameOf(expression);
            return this.MakeRouteDescriptor(typeof(TController), string.Join("/", routeParts), actionName);
        }

        /// <summary>
        /// Creates a new RouteDescriptor in the style of attributeRouting. Uses the module name as default area.
        /// </summary>
        /// <param name="controller">The type of the controller</param>
        /// <param name="routeSlug">The route of the action. Will be appended to the base api route.</param>
        /// <param name="action">
        /// The name of the action.
        /// <remarks>Prefix it with the desired httpVerb for the action</remarks>
        /// <example>GetUser, PostUser, PutUser</example>
        /// </param>
        /// <returns>A fully configured HttpRouteDescriptor</returns>
        public HttpRouteDescriptor MakeRouteDescriptor(Type controller, string routeSlug, string action) {
            var route = this.baseApiRoute + routeSlug;
            var ctrlIndex = controller.Name.LastIndexOf("Controller", StringComparison.Ordinal);
            var controllerName = controller.Name.Substring(0, ctrlIndex);

            return MakeRouteDescriptor(this.moduleAreaName, route, controllerName, action, httpMethod: HttpMethodOfAction(action));
        }

#pragma warning disable SA1204 // Static elements must appear before instance elements
        /// <summary>
        /// Creates a new <c>HttpRouteDescriptor</c>. Will parse the <paramref name="route"/> to add parameter-constrains.
        /// <example>
        /// "http://example.com/api/users/{id:int}" will create an integer contstraint for the id parameter.
        /// </example>
        /// </summary>
        public static HttpRouteDescriptor MakeRouteDescriptor(string area, string route, string controller, string action, int priority = 5, HttpMethod httpMethod = null)
#pragma warning restore SA1204 // Static elements must appear before instance elements
        {
            var defaults = new ExpandoObject();
            var underlyingObject = (IDictionary<string, object>)defaults;
            underlyingObject.Add("area", area);
            underlyingObject.Add("controller", controller);
            underlyingObject.Add("action", action);

            var constraints = ParseRouteAttributeConstraints(route, defaults, httpMethod);

            var routeDescriptor = new HttpRouteDescriptor {
                RouteTemplate = CleanupRoute(route),
                Defaults = defaults,
                Priority = priority,
                Constraints = constraints
            };

            return routeDescriptor;
        }

        private static HttpMethod HttpMethodOfAction(string actionName) {
            if ( actionName.StartsWith(HttpMethod.Post.Method, StringComparison.OrdinalIgnoreCase) )
                return HttpMethod.Post;

            if ( actionName.StartsWith(HttpMethod.Put.Method, StringComparison.OrdinalIgnoreCase) )
                return HttpMethod.Put;

            // Yep, there is no official HttpMethod.Patch!
            if ( actionName.StartsWith("Patch", StringComparison.OrdinalIgnoreCase) )
                return new HttpMethod("PATCH");

            if ( actionName.StartsWith(HttpMethod.Delete.Method, StringComparison.OrdinalIgnoreCase) )
                return HttpMethod.Delete;

            return HttpMethod.Get;
        }

        private static string CleanupRoute(string route) {
            var split = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var cleanAttr = split.Select(a => a.Contains(":") ? string.Format("{{{0}}}", a.Trim('{', '}').Substring(0, a.IndexOf(':') - 1)) : a);
            var cleanRoute = string.Join("/", cleanAttr);
            return cleanRoute;
        }

        /// <summary>
        /// <para>
        /// Creates route constraints from the given route.
        /// Use the return value as input for <c>HttpRouteDescriptor.Contraints</c>.
        /// </para>
        /// <example>
        /// "http://example.com/api/users/{id:int}" will create an integer contstraint for the id parameter.
        /// </example>
        /// </summary>
        /// <param name="route">The absolute route to be parsed.</param>
        /// <param name="defaults">An object that will be filled with all optional parameters. Should be used as value for <c>HttpRouteDescriptor.Defaults</c>.</param>
        /// <param name="httpMethod">The routes http-verb.</param>
        private static dynamic ParseRouteAttributeConstraints(string route, ExpandoObject defaults, HttpMethod httpMethod = null) {
            var split = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var attributes = split.Where(x => x.StartsWith("{") && x.EndsWith("}"));

            dynamic constraints = new ExpandoObject();
            var underlyingObject = (IDictionary<string, object>)constraints;

            foreach ( var attr in attributes ) {
                var tmpSplit = attr.Trim('{', '}').Split(':');
                var key = tmpSplit[0];
                if ( tmpSplit.Length == 2 && !underlyingObject.ContainsKey(key) ) {
                    var typeName = tmpSplit[1];
                    var isOptional = typeName.EndsWith("?");

                    if ( isOptional ) {
                        typeName = typeName.TrimEnd('?');
                        ((IDictionary<string, object>)defaults).Add(key, RouteParameter.Optional);
                    }

                    var constraint = ConstraintForTypename(typeName, isOptional);
                    if ( constraint != null ) {
                        underlyingObject.Add(key, constraint);
                    }
                    else {
                        Debug.WriteLine("No matching IHttpRouteConstraint found for type '{0}'", typeName);
                    }
                }
            }

            if ( httpMethod != null ) {
                underlyingObject.Add("httpMethod", new HttpMethodConstraint(httpMethod, HttpMethod.Options));
            }

            return constraints;
        }

        private static IHttpRouteConstraint ConstraintForTypename(string typename, bool isOptional) {
            /* List of all route constraints:
             * https://msdn.microsoft.com/en-us/library/system.web.http.routing.constraints(v=vs.118).aspx
             */

            IHttpRouteConstraint constraint = null;
            switch ( typename ) {
                case "bool":
                case "boolean":
                constraint = new BoolRouteConstraint();
                break;
                case "int":
                constraint = new IntRouteConstraint();
                break;
                case "long":
                constraint = new LongRouteConstraint();
                break;
                case "float":
                constraint = new FloatRouteConstraint();
                break;
                case "double":
                constraint = new DoubleRouteConstraint();
                break;
                case "string":
                constraint = new AlphaRouteConstraint();
                break;
            }

            return isOptional ? new OptionalRouteConstraint(constraint) : constraint;
        }
    }
}