namespace Orchard.Tools.Helpers.Routing {
    using System.Web.Mvc;
    using System.Web.Routing;
    using Orchard.Mvc.Routes;

    public static class RoutingHelpers {
        public static RouteDescriptor BuildMvcRoute(string area, string controller, string action, string url, int priority = 5) {
            return new RouteDescriptor {
                Priority = priority,
                Route = new Route(
                    url,
                    new RouteValueDictionary
                    {
                        { "area", area },
                        { "controller", controller },
                        { "action", action }
                    },
                    new RouteValueDictionary(),
                    new RouteValueDictionary { { "area", area } },
                    new MvcRouteHandler())
            };
        }
    }
}
