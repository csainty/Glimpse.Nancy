using System.Collections.Generic;
using System.Linq;
using Glimpse.Nancy.Wrappers;
using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Models
{
    internal class ViewsModel
    {
        public ViewsModel(NancyContext ctx, IEnumerable<IViewEngine> viewEngines, IEnumerable<ViewLocationResult> discoveredViews)
        {
            this.Engines = viewEngines.Select(x => new ViewEngineModel(x));
            this.Views = discoveredViews.Select(x => new ViewLocationModel(x));
        }

        public IEnumerable<ViewEngineModel> Engines { get; set; }

        public IEnumerable<ViewLocationModel> Views { get; set; }
    }

    internal class ViewEngineModel
    {
        private readonly IViewEngine viewEngine;

        public ViewEngineModel(IViewEngine viewEngine)
        {
            this.viewEngine = viewEngine;
        }

        public string Name
        {
            get
            {
                var type = this.viewEngine.GetType();
                if (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(GlimpseViewEngine<>))
                {
                    type = type.GetGenericArguments()[0];
                }
                return type.FullName;
            }
        }

        public IEnumerable<string> Extensions { get { return this.viewEngine.Extensions; } }
    }

    internal class ViewLocationModel
    {
        private readonly ViewLocationResult result;

        public ViewLocationModel(ViewLocationResult result)
        {
            this.result = result;
        }

        public string Location { get { return this.result.Location; } }

        public string Name { get { return this.result.Name; } }

        public string Extension { get { return this.result.Extension; } }
    }
}