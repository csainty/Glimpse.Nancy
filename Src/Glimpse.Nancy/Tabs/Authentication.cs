using Glimpse.Core.Extensibility;
using Glimpse.Nancy.Models;

namespace Glimpse.Nancy.Tabs
{
    public class Authentication : NancyTab, IKey
    {
        public string Key
        {
            get { return "glimpse_nancy_authentication"; }
        }

        public override object GetData(ITabContext context)
        {
            var ctx = context.GetNancyContext();
            return new AuthenticationModel(ctx.CurrentUser);
        }

        public override string Name
        {
            get { return "Authentication"; }
        }
    }
}