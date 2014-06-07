using Nancy;
using Nancy.Routing;

namespace Glimpse.Nancy.Wrappers
{
    internal class GlimpseRouteResolver<T> : IRouteResolver where T : IRouteResolver
    {
        private readonly IRouteResolver routeResolver;

        public GlimpseRouteResolver(T routeResolver)
        {
            this.routeResolver = routeResolver;
        }

        public ResolveResult Resolve(NancyContext context)
        {
            var request = context.TryGetRequestContext();

            if (request == null)
            {
                return routeResolver.Resolve(context);
            }

            return request.TimeAction("RouteResolver", "Resolve: " + context.Request.Path, () => routeResolver.Resolve(context));
        }
    }
}