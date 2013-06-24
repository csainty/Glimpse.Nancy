using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;

namespace Glimpse.Nancy
{
    public static class NancyContextExtensions
    {
        private const string RuntimeKey = "_glimpse_runtime";
        private const string FactoryKey = "_glimpse_factory";

        public static IExecutionTimer GetTimer(this NancyContext context)
        {
            return (context.Items[FactoryKey] as Factory).InstantiateTimerStrategy()();
        }

        public static IMessageBroker GetMessageBroker(this NancyContext context)
        {
            return (context.Items[FactoryKey] as Factory).InstantiateMessageBroker();
        }

        public static bool TryGetGlimpseRuntime(this NancyContext context, out IGlimpseRuntime runtime)
        {
            if (context.Items.ContainsKey(RuntimeKey))
            {
                runtime = (IGlimpseRuntime)context.Items[RuntimeKey];
                return true;
            }
            runtime = null;
            return false;
        }

        public static void SetGlimpseFactory(this NancyContext context, Factory factory)
        {
            context.Items[FactoryKey] = factory;
        }

        public static void SetGlimpseRuntime(this NancyContext context, IGlimpseRuntime runtime)
        {
            context.Items[RuntimeKey] = runtime;
        }
    }
}