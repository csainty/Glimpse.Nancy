using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nancy;
using Nancy.Bootstrapper;

namespace Glimpse.Nancy.Models
{
    public class InfoModel
    {
        private readonly IRootPathProvider rootPathProvider;

        public InfoModel(IRootPathProvider rootPathProvider, NancyInternalConfiguration configuration)
        {
            this.rootPathProvider = rootPathProvider;

            Configuration = new Dictionary<string, IEnumerable<string>>();
            foreach (var propertyInfo in configuration.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var value = propertyInfo.GetValue(configuration, null);

                Configuration[propertyInfo.Name] = (!typeof(IEnumerable).IsAssignableFrom(value.GetType())) ?
                    new[] { value.ToString() } :
                    ((IEnumerable<object>)value).Select(x => x.ToString());
            }
        }

        public string NancyVersion { get { return string.Format("v{0}", typeof(NancyContext).Assembly.GetName().Version.ToString()); } }

        public string RootPath { get { return this.rootPathProvider.GetRootPath(); } }

        public bool TracesDisabled { get { return StaticConfiguration.DisableErrorTraces; } }

        public bool CaseSensitivity { get { return StaticConfiguration.CaseSensitive; } }

        public string LocatedBootstrapper { get { return NancyBootstrapperLocator.Bootstrapper.GetType().ToString(); } }

        public Dictionary<string, IEnumerable<string>> Configuration { get; set; }
    }
}