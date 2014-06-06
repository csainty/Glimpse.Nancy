using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Inspectors
{
    public class GlimpseViewLocator : IViewLocator
    {
        private readonly IViewLocator viewLocator;

        public GlimpseViewLocator(DefaultViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public IEnumerable<ViewLocationResult> GetAllCurrentlyDiscoveredViews()
        {
            return this.viewLocator.GetAllCurrentlyDiscoveredViews();
        }

        public ViewLocationResult LocateView(string viewName, NancyContext context)
        {
            var request = context.TryGetRequestContext();

            if (request == null)
            {
                return viewLocator.LocateView(viewName, context);
            }

            return request.TimeAction("ViewLocator", "LocateView: " + viewName, () => viewLocator.LocateView(viewName, context));
        }
    }
}