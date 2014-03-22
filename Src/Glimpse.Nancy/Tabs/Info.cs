using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy.Tabs
{
    public class Info : NancyTab, IKey
    {
        private readonly IRootPathProvider rootPathProvider;
        private readonly NancyInternalConfiguration configuration;

        public Info(IRootPathProvider rootPathProvider, NancyInternalConfiguration configuration)
        {
            this.rootPathProvider = rootPathProvider;
            this.configuration = configuration;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new InfoModel(rootPathProvider, configuration);
        }

        public override string Name
        {
            get { return "Nancy"; }
        }

        public string Key
        {
            get { return "glimpse_nancy_info"; }
        }
    }
}