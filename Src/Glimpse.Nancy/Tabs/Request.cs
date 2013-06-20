using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy;
using Nancy.Routing;

namespace Glimpse.Nancy.Tabs
{
    public class Request : NancyTab, IKey
    {
        private readonly IRootPathProvider rootPath;
        private readonly IRouteResolver routeResolver;

        public Request(IRootPathProvider rootPath, IRouteResolver routeResolver)
        {
            this.rootPath = rootPath;
            this.routeResolver = routeResolver;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new RequestModel(ctx, rootPath, routeResolver);
        }

        public override string Name
        {
            get { return "Request"; }
        }

        public string Key
        {
            get { return "glimpse_nancy_request"; }
        }
    }
}