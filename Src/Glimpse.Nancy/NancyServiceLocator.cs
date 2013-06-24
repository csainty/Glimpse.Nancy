using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;
using System.Linq;

namespace Glimpse.Nancy
{
    public class NancyServiceLocator : IServiceLocator
    {
        private readonly NancyContext context;

        public NancyServiceLocator(NancyContext ctx)
        {
            this.context = ctx;
        }

        private ILogger logger;

        internal ILogger Logger
        {
            get { return logger ?? (logger = new NullLogger()); }
            set { logger = value; }
        }

        public IEnumerable<ITab> Tabs { get; set; }

        public IEnumerable<IDisplay> Displays { get; set; }

        public ICollection<T> GetAllInstances<T>() where T : class
        {
            var type = typeof(T);
            if (type == typeof(ITab))
            {
                return Tabs.ToArray() as ICollection<T>;
            }
            if (type == typeof(IDisplay))
            {
                return Displays.ToArray() as ICollection<T>;
            }
            return null;
        }

        public T GetInstance<T>() where T : class
        {
            var type = typeof(T);
            if (type == typeof(IFrameworkProvider))
            {
                return new NancyFrameworkProvider(this.context, this.Logger) as T;
            }

            if (type == typeof(ResourceEndpointConfiguration))
            {
                return new NancyEndpointConfiguration(this.context) as T;
            }

            return null;
        }
    }
}