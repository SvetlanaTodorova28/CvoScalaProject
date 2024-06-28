using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class CheckboxViewModel : BaseProductViewModel
    {
        public bool IsSelected { get; set; }
    }
}
