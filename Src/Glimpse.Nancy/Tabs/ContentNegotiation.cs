using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;

namespace Glimpse.Nancy.Tabs
{
    public class ContentNegotiation : NancyTab, IKey
    {
        public override object GetData(ITabContext context)
        {
            return new ContentNegotiationModel(context.GetNancyContext());
        }

        public override string Name
        {
            get { return "Content Negotiation"; }
        }

        public string Key
        {
            get { return "glimpse_nancy_contentnegotiation"; }
        }
    }
}