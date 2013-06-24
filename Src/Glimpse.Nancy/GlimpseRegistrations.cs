using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy
{
    public class GlimpseRegistrations : IApplicationRegistrations
    {
        public GlimpseRegistrations()
        {
            AppDomainAssemblyTypeScanner.AddAssembliesToScan(typeof(Glimpse.Core.Tab.Timeline).Assembly);
        }

        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get
            {
                var tabs = AppDomainAssemblyTypeScanner.TypesOf<ITab>();
                var displays = AppDomainAssemblyTypeScanner.TypesOf<IDisplay>();
                return new[] {
                    new CollectionTypeRegistration(typeof(ITab), tabs),
                    new CollectionTypeRegistration(typeof(IDisplay), displays),
                };
            }
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get { return null; }
        }

        public IEnumerable<TypeRegistration> TypeRegistrations
        {
            get { return null; }
        }
    }
}