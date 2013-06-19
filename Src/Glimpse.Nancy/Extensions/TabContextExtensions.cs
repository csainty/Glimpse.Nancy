using Glimpse.Core.Extensibility;
using Nancy;

namespace Glimpse.Nancy
{
    public static class TabContextExtensions
    {
        public static NancyContext GetNancyContext(this ITabContext tabContext)
        {
            return tabContext.GetRequestContext<NancyContext>();
        }
    }
}