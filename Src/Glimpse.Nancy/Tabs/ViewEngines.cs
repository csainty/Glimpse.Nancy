using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Tabs
{
    public class ViewEngines : NancyTab, IKey
    {
        private readonly IEnumerable<IViewEngine> viewEngines;
        private readonly IViewLocator viewLocator;

        public ViewEngines(IEnumerable<IViewEngine> viewEngines, IViewLocator viewLocator)
        {
            this.viewEngines = viewEngines;
            this.viewLocator = viewLocator;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new ViewsModel(ctx, this.viewEngines, this.viewLocator.GetAllCurrentlyDiscoveredViews());
        }

        public override string Name
        {
            get { return "Views"; }
        }

        public string Key
        {
            get { return "nancy_glimpse_views"; }
        }
    }
}