using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Tabs
{
    public class ViewEngines : NancyTab, IKey
    {
        private readonly IEnumerable<IViewEngine> viewEngines;

        public ViewEngines(IEnumerable<IViewEngine> viewEngines)
        {
            this.viewEngines = viewEngines;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new ViewEnginesModel(ctx, this.viewEngines);
        }

        public override string Name
        {
            get { return "View Engines"; }
        }

        public string Key
        {
            get { return "nancy_glimpse_viewengines"; }
        }
    }
}