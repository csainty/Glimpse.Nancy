using Nancy.ViewEngines;

namespace Glimpse.Nancy.Inspectors
{
    public class GlimpseViewResolver : IViewResolver
    {
        private readonly IViewResolver viewResolver;

        public GlimpseViewResolver(DefaultViewResolver viewResolver)
        {
            this.viewResolver = viewResolver;
        }

        public ViewLocationResult GetViewLocation(string viewName, dynamic model, ViewLocationContext viewLocationContext)
        {
            var request = viewLocationContext.Context.TryGetRequestContext();

            if (request == null)
            {
                return viewResolver.GetViewLocation(viewName, model, viewLocationContext);
            }

            return request.TimeAction("ViewResolver", "GetViewLocation: " + viewName, () => viewResolver.GetViewLocation(viewName, model, viewLocationContext));
        }
    }
}