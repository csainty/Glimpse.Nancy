using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Routing;

namespace Glimpse.Nancy.Models
{
    public class ModulesModel : List<RouteModel>
    {
        public ModulesModel(NancyContext ctx, INancyModuleCatalog catalog)
        {
            var routes = from module in catalog.GetAllModules(ctx)
                         from route in module.Routes
                         select new RouteModel(module, route);
            this.AddRange(routes);
        }
    }

    public class RouteModel
    {
        private readonly Route route;
        private readonly INancyModule module;

        public RouteModel(INancyModule module, Route route)
        {
            this.route = route;
            this.module = module;
        }

        public string Method { get { return route.Description.Method; } }

        public string Path { get { return String.IsNullOrEmpty(module.ModulePath) ? route.Description.Path : module.ModulePath + "/" + route.Description.Path; } }

        public string RegisteredIn { get { return this.module.GetType().Name; } }

        public bool HasCondition { get { return route.Description.Condition != null; } }
    }
}