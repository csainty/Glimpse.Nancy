using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Wrappers
{
    public class GlimpseViewFactory<T> : IViewFactory where T : IViewFactory
    {
        private readonly IViewFactory viewFactory;

        public GlimpseViewFactory(T viewFactory)
        {
            this.viewFactory = viewFactory;
        }

        public Response RenderView(string viewName, dynamic model, ViewLocationContext viewLocationContext)
        {
            var request = viewLocationContext.Context.TryGetRequestContext();

            if (request == null)
            {
                return viewFactory.RenderView(viewName, model, viewLocationContext);
            }

            return request.TimeAction("ViewFactory", "RenderView: " + viewName, () => viewFactory.RenderView(viewName, model, viewLocationContext));
        }
    }
}