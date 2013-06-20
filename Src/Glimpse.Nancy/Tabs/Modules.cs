using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy;

namespace Glimpse.Nancy.Tabs
{
    public class Modules : NancyTab, IKey
    {
        private readonly INancyModuleCatalog catalog;

        public Modules(INancyModuleCatalog catalog)
        {
            this.catalog = catalog;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new ModulesModel(ctx, catalog);
        }

        public override string Name
        {
            get { return "Modules"; }
        }

        public string Key
        {
            get { return "glimpse_nancy_modules"; }
        }
    }
}