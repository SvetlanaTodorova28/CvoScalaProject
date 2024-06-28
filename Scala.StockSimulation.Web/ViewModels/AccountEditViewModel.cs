using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class AccountEditViewModel
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        public string LastName { get; set; }
    }
}
