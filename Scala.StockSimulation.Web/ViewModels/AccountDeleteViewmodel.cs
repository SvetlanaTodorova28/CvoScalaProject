using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class AccountDeleteViewmodel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
