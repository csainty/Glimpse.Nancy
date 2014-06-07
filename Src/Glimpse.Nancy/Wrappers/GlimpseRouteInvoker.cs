using System.Threading;
using System.Threading.Tasks;
using Nancy;
using Nancy.Routing;

namespace Glimpse.Nancy.Wrappers
{
    internal class GlimpseRouteInvoker<T> : IRouteInvoker where T : IRouteInvoker
    {
        private readonly IRouteInvoker routeInvoker;

        public GlimpseRouteInvoker(T routeInvoker)
        {
            this.routeInvoker = routeInvoker;
        }

        public Task<Response> Invoke(Route route, CancellationToken cancellationToken, DynamicDictionary parameters, NancyContext context)
        {
            var request = context.TryGetRequestContext();

            if (request == null)
            {
                return routeInvoker.Invoke(route, cancellationToken, parameters, context);
            }

            return request.TimeTask("RouteInvoker", "Invoke: " + route.Description.Path, () => routeInvoker.Invoke(route, cancellationToken, parameters, context));
        }
    }
}