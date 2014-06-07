using System;
using Nancy.ModelBinding;

namespace Glimpse.Nancy.Wrappers
{
    internal class GlimpseBinder<T> : IBinder where T : IBinder
    {
        private readonly T binder;

        public GlimpseBinder(T binder)
        {
            this.binder = binder;
        }

        public object Bind(global::Nancy.NancyContext context, Type modelType, object instance, BindingConfig configuration, params string[] blackList)
        {
            var request = context.TryGetRequestContext();

            if (request == null)
            {
                return binder.Bind(context, modelType, instance, configuration, blackList);
            }

            return request.TimeAction("ModelBinder", "Bind: " + modelType.FullName, () => binder.Bind(context, modelType, instance, configuration, blackList));
        }
    }
}