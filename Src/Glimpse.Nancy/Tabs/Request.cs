using Glimpse.Core.Extensibility;

namespace Glimpse.Nancy.Tabs
{
    public class Request : NancyTab
    {
        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new
            {
                path = ctx.Request.Path
            };
        }

        public override string Name
        {
            get { return "Request"; }
        }
    }
}