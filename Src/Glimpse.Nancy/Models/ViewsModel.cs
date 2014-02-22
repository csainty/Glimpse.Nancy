using System.Collections.Generic;
using System.Linq;
using Glimpse.Nancy.Replacements;
using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Models
{
    public class ViewsModel
    {
        public ViewsModel(NancyContext ctx, IEnumerable<IViewEngine> viewEngines, IEnumerable<ViewLocationResult> discoveredViews)
        {
            this.Engines = viewEngines
                .Where(x => !(x is GlimpseViewEngine))
                .Select(x => new ViewEngineModel(x));

            this.Views = discoveredViews.Select(x => new ViewLocationModel(x));
        }

        public IEnumerable<ViewEngineModel> Engines { get; set; }

        public IEnumerable<ViewLocationModel> Views { get; set; }
    }

    public class ViewEngineModel
    {
        private readonly IViewEngine viewEngine;

        public ViewEngineModel(IViewEngine viewEngine)
        {
            this.viewEngine = viewEngine;
        }

        public string Name { get { return this.viewEngine.GetType().Name; } }

        public IEnumerable<string> Extensions { get { return this.viewEngine.Extensions; } }
    }

    public class ViewLocationModel
    {
        private readonly ViewLocationResult result;

        public ViewLocationModel(ViewLocationResult result)
        {
            this.result = result;
        }

        public string Name { get { return this.result.Name; } }

        public string Extension { get { return this.result.Extension; } }

        public string Location { get { return this.result.Location; } }
    }
}