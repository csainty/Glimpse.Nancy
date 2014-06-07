using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;

namespace Glimpse.Nancy.Tabs
{
    public class Info : NancyTab, IKey
    {
        private readonly IRootPathProvider rootPathProvider;
        private readonly NancyInternalConfiguration configuration;
        private readonly IEnumerable<IStatusCodeHandler> statusCodeHandlers;

        public Info(IRootPathProvider rootPathProvider, NancyInternalConfiguration configuration, IEnumerable<IStatusCodeHandler> statusCodeHandlers)
        {
            this.rootPathProvider = rootPathProvider;
            this.configuration = configuration;
            this.statusCodeHandlers = statusCodeHandlers;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new InfoModel(rootPathProvider, configuration, statusCodeHandlers);
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