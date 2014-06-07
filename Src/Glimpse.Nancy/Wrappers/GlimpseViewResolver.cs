using Nancy.ViewEngines;

namespace Glimpse.Nancy.Wrappers
{
    internal class GlimpseViewResolver<T> : IViewResolver where T : IViewResolver
    {
        private readonly IViewResolver viewResolver;

        public GlimpseViewResolver(T viewResolver)
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