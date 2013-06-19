using System.Collections.Generic;
using Glimpse.Core.Framework;
using Nancy;

namespace Glimpse.Nancy
{
    public class NancyServiceLocator : IServiceLocator
    {
        private readonly NancyContext context;

        public NancyServiceLocator(NancyContext ctx)
        {
            this.context = ctx;
        }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            return null;
        }

        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);
            if (type == typeof(IFrameworkProvider))
            {
                return new NancyFrameworkProvider(this.context) as T;
            }

            if (type == typeof(ResourceEndpointConfiguration))
            {
                return new NancyEndpointConfiguration() as T;
            }

            return null;
        }
    }
}