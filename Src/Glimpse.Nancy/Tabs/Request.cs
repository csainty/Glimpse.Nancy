using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;
using Nancy;

namespace Glimpse.Nancy.Tabs
{
    public class Request : NancyTab, IKey
    {
        private readonly IRootPathProvider rootPath;

        public Request(IRootPathProvider rootPath)
        {
            this.rootPath = rootPath;
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new RequestModel(ctx, rootPath);
        }

        public override string Name
        {
            get { return "Request"; }
        }

        public string Key
        {
            get { return "glimpse_nancy_request"; }
        }
    }
}