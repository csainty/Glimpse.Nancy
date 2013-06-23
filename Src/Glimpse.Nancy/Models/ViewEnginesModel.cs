using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.ViewEngines;

namespace Glimpse.Nancy.Models
{
    public class ViewEnginesModel : List<ViewEngineModel>
    {
        public ViewEnginesModel(NancyContext ctx, IEnumerable<IViewEngine> viewEngines)
        {
            this.AddRange(viewEngines.Select(x => new ViewEngineModel(x)));
        }
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
}