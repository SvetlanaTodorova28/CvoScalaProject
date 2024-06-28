using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Utilities.Authorization
{
    public class AuthorizeMultiplePolicyAttribute : TypeFilterAttribute
    {
        public AuthorizeMultiplePolicyAttribute(string policies, bool IsAll) : base(typeof(AuthorizeMultiplePolicyFilter))
        {
            Arguments = new object[] { policies, IsAll };
        }
    }
}
