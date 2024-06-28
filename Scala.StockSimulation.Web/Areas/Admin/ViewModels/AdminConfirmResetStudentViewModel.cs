using Microsoft.AspNetCore.Mvc;

namespace Scala.StockSimulation.Web.Areas.Admin.ViewModels
{
    public class AdminConfirmResetStudentViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
