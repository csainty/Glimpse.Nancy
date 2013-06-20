using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Routing;

namespace Glimpse.Nancy.Models
{
    public class ModulesModel : List<ModuleModel>
    {
        public ModulesModel(NancyContext ctx, INancyModuleCatalog catalog)
        {
            this.AddRange(catalog.GetAllModules(ctx).Select(x => new ModuleModel(x)));
        }
    }

    public class ModuleModel
    {
        private readonly INancyModule module;

        public ModuleModel(INancyModule module)
        {
            this.module = module;
            this.Routes = module.Routes.Select(x => new RouteModel(x));
        }

        public string Name { get { return module.GetType().Name; } }

        public string Path { get { return module.ModulePath; } }

        public IEnumerable<RouteModel> Routes { get; private set; }
    }

    public class RouteModel
    {
        private readonly Route route;

        public RouteModel(Route route)
        {
            this.route = route;
        }

        public string Method { get { return route.Description.Method; } }

        public string Path { get { return route.Description.Path; } }

        public bool HasCondition { get { return route.Description.Condition != null; } }

        public string Description { get { return route.Description.Description; } }

        public IEnumerable<string> Segments { get { return route.Description.Segments; } }
    }
}