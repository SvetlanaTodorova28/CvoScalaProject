using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Paswoord is verplicht")]
        [Display(Name = "Paswoord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
