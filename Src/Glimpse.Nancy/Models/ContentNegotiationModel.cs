using System.Collections.Generic;
using Nancy;
using Nancy.Responses.Negotiation;

namespace Glimpse.Nancy.Models
{
    public class ContentNegotiationModel
    {
        public ContentNegotiationModel(NancyContext ctx)
        {
            this.ViewName = ctx.NegotiationContext.ViewName;
            this.Module = ctx.NegotiationContext.ModuleName;
            this.ModulePath = ctx.NegotiationContext.ModulePath;
            this.Model = ctx.NegotiationContext.DefaultModel;
            
            this.BuildPermissableMediaRanges(ctx.NegotiationContext.PermissableMediaRanges);
        }

        public string ViewName { get; private set; }

        public string Module { get; private set; }

        public string ModulePath { get; private set; }

        public object Model { get; private set; }

        public IEnumerable<string> PermissableMediaRanges { get; private set; }

        private void BuildPermissableMediaRanges(IList<MediaRange> list)
        {
            var ranges = new List<string>();

            foreach (var range in list)
            {
                ranges.Add(range.ToString());
            }

            this.PermissableMediaRanges = ranges;
        }
    }
}