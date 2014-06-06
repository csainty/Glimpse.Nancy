using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Inspectors
{
    public class GlimpseViewEngine<T> : IViewEngine where T : IViewEngine
    {
        private readonly IViewEngine viewEngine;

        public GlimpseViewEngine(T viewEngine)
        {
            this.viewEngine = viewEngine;
        }

        public IEnumerable<string> Extensions
        {
            get { return viewEngine.Extensions; }
        }

        public void Initialize(ViewEngineStartupContext viewEngineStartupContext)
        {
            viewEngine.Initialize(viewEngineStartupContext);
        }

        public Response RenderView(ViewLocationResult viewLocationResult, dynamic model, IRenderContext renderContext)
        {
            var request = renderContext.Context.TryGetRequestContext();

            if (request == null)
            {
                return viewEngine.RenderView(viewLocationResult, model, renderContext);
            }

            return request.TimeAction(viewEngine.GetType().Name, "CompileView: " + viewLocationResult.Name, () => viewEngine.RenderView(viewLocationResult, model, renderContext));
        }
    }
}