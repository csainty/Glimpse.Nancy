using Glimpse.Core.Extensibility;
using Glimpse.Core.Tab.Assist;
using Glimpse.Nancy.Models;
using Nancy;

namespace Glimpse.Nancy.Tabs
{
    public class Routes : NancyTab, IKey, ITabLayout
    {
        private readonly INancyModuleCatalog catalog;

        public Routes(INancyModuleCatalog catalog)
        {
            this.catalog = catalog;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new RoutesModel(ctx, catalog);
        }

        public override string Name
        {
            get { return "Routes"; }
        }

        public string Key
        {
            get { return "glimpse_nancy_routes"; }
        }

        public object GetLayout()
        {
            return TabLayout.Create();
        }
    }
}