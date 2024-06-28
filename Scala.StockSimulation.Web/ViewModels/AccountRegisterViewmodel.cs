using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Scala.StockSimulation.Web.ViewModels
{
    public class AccountRegisterViewmodel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        [Display(Name = "Voornaam")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email is verplicht")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Paswoord is verplicht")]
        [DataType(DataType.Password)]
        [Display(Name = "Paswoord")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Paswoord is verplicht")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Bevestig paswoord")]
        public string ConfirmPassword { get; set; }
    }
}
