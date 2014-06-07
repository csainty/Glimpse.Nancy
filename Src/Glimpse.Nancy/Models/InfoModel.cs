using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ErrorHandling;

namespace Glimpse.Nancy.Models
{
    internal class InfoModel
    {
        private readonly IRootPathProvider rootPathProvider;

        private static readonly IEnumerable<Type> SettingTypes = new[] { typeof(StaticConfiguration) }.Union(typeof(StaticConfiguration).GetNestedTypes(BindingFlags.Static | BindingFlags.Public));

        public InfoModel(IRootPathProvider rootPathProvider, NancyInternalConfiguration configuration, IEnumerable<IStatusCodeHandler> statusCodeHandlers)
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

            var properties = SettingTypes
                .SelectMany(t => t.GetProperties(BindingFlags.Static | BindingFlags.Public))
                .Where(x => x.PropertyType == typeof(bool));

            Settings = from property in properties
                       orderby property.Name
                       let value = (bool)property.GetValue(null, null)
                       select new SettingsModel
                       {
                           Name = Regex.Replace(property.Name, "[A-Z]", " $0"),
                           Value = value
                       };

            StatusCodeHandlers = statusCodeHandlers.Select(x => x.GetType().FullName);
        }

        public string NancyVersion { get { return string.Format("v{0}", typeof(NancyContext).Assembly.GetName().Version.ToString()); } }

        public string RootPath { get { return this.rootPathProvider.GetRootPath(); } }

        public string LocatedBootstrapper { get { return NancyBootstrapperLocator.Bootstrapper.GetType().ToString(); } }

        public string BootstrapperContainer
        {
            get
            {
                var name = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Select(asm => asm.GetName())
                    .FirstOrDefault(asmName => asmName.Name != null && asmName.Name.StartsWith("Nancy.Bootstrappers."));

                return (name == null) ?
                    "TinyIoC" :
                    string.Format("{0} (v{1})", name.Name.Split('.').Last(), name.Version);
            }
        }

        public string Hosting
        {
            get
            {
                var name = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .Select(asm => asm.GetName())
                    .FirstOrDefault(asmName => asmName.Name != null && asmName.Name.StartsWith("Nancy.Hosting."));

                return (name == null) ?
                    "Unknown" :
                    string.Format("{0} (v{1})", name.Name.Split('.').Last(), name.Version);
            }
        }

        public Dictionary<string, IEnumerable<string>> Configuration { get; set; }

        public IEnumerable<SettingsModel> Settings { get; set; }

        public IEnumerable<string> StatusCodeHandlers { get; set; }

        public class SettingsModel
        {
            public string Name { get; set; }

            public bool Value { get; set; }
        }
    }
}