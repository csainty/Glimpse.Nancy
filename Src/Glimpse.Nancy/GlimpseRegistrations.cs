using System.Collections.Generic;
using Glimpse.Core.Extensibility;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy
{
    public class GlimpseRegistrations : IRegistrations
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
                var inspectors = AppDomainAssemblyTypeScanner.TypesOf<IInspector>();
                return new[] {
                    new CollectionTypeRegistration(typeof(ITab), tabs),
                    new CollectionTypeRegistration(typeof(IInspector), inspectors)
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