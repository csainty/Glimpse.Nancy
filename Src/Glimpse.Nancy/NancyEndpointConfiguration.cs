using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Nancy;
using Nancy.Extensions;

namespace Glimpse.Nancy
{
    public class NancyEndpointConfiguration : ResourceEndpointConfiguration
    {
        private readonly NancyContext context;

        public NancyEndpointConfiguration(NancyContext context)
        {
            this.context = context;
        }

        protected override string GenerateUriTemplate(string resourceName, string baseUri, IEnumerable<ResourceParameterMetadata> parameters, ILogger logger)
        {
            var root = context.ToFullPath(baseUri);

            var stringBuilder = new StringBuilder(string.Format(@"{0}?n={1}", root, resourceName));

            var requiredParams = parameters.Where(p => p.IsRequired);
            foreach (var parameter in requiredParams)
            {
                stringBuilder.Append(string.Format("&{0}={{{1}}}", parameter.Name, parameter.Name));
            }

            var optionalParams = parameters.Except(requiredParams).Select(p => p.Name).ToArray();

            // Format based on Form-style query continuation from RFC6570: http://tools.ietf.org/html/rfc6570#section-3.2.9
            if (optionalParams.Any())
            {
                stringBuilder.Append(string.Format("{{&{0}}}", string.Join(",", optionalParams)));
            }

            return stringBuilder.ToString();
        }

        public override bool IsResourceRequest(Uri requestUri, string endpointBaseUri)
        {
            return base.IsResourceRequest(requestUri, context.ToFullPath(endpointBaseUri));
        }
    }
}