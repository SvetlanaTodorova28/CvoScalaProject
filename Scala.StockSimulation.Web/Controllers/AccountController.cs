using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Scala.StockSimulation.Core.Entities;
using Scala.StockSimulation.Web.Data;
using Scala.StockSimulation.Web.ViewModels;
using SmartBreadcrumbs.Attributes;
using System.Security.Claims;
using Scala.StockSimulation.Web.Services.Interfaces;
using static System.Net.WebRequestMethods;

namespace Scala.StockSimulation.Web.Controllers
{

    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ScalaStockSimulationDbContext _scalaStockSimulationDbContext;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClaimsService _claimsService;

        public AccountController(ScalaStockSimulationDbContext scalaStockSimulationDbContext, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IClaimsService claimsService)
        {
            _scalaStockSimulationDbContext = scalaStockSimulationDbContext;
            _signInManager = signInManager;
            _userManager = userManager;
            _claimsService = claimsService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountLoginViewModel);
            }
            var user = await _signInManager.UserManager.FindByEmailAsync(accountLoginViewModel.Email);
            var claims = await _claimsService.GenerateClaimsForUser(user); 
            await _signInManager.UserManager.AddClaimsAsync(user, claims);
            var result = await _signInManager.PasswordSignInAsync(accountLoginViewModel.Email,
                accountLoginViewModel.Password, false, true);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Geen geldige inlog gegevens.");
                return View(accountLoginViewModel);
            }

            if (user.IsTeacher)
            {
                return RedirectToAction("Index", "Admin", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Overview");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Register()
        {
            AccountRegisterViewmodel accountRegisterViewModel = new();
            return View(accountRegisterViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterViewmodel accountRegisterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountRegisterViewModel);
            }

            if (await _userManager.FindByEmailAsync(accountRegisterViewModel.Email) == null)
            {
                // Create a new student user
                ApplicationUser newStudent = new()
                {
                    UserName = accountRegisterViewModel.Email,
                    FirstName = accountRegisterViewModel.FirstName,
                    LastName = accountRegisterViewModel.LastName,
                    Email = accountRegisterViewModel.Email,
                    Created = DateTime.Now,
                    EmailConfirmed = true,
                };

                var result = await _userManager.CreateAsync(newStudent, accountRegisterViewModel.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newStudent, "Student");

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("StudentAccounts", "Admin", new { Area = "Admin" });
                    }
                    else
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Er is reeds een user met deze email.");
            }
            return View(accountRegisterViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            AccountEditViewModel accountEditViewModel = new()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(accountEditViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountEditViewModel accountEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountEditViewModel);
            }

            var user = await _userManager.FindByIdAsync(accountEditViewModel.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = accountEditViewModel.FirstName;
            user.LastName = accountEditViewModel.LastName;
            user.Email = accountEditViewModel.Email;
            user.UserName = accountEditViewModel.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("StudentAccounts", "Admin", new { Area = "Admin" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(accountEditViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            AccountDeleteViewmodel accountDeleteViewModel = new()
            {
                Id = user.Id,
                Email = user.Email
            };

            return View(accountDeleteViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AccountDeleteViewmodel accountDeleteViewModel)
        {
            var user = await _userManager.FindByIdAsync(accountDeleteViewModel.Id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("StudentAccounts", "Admin", new { Area = "Admin" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(accountDeleteViewModel);
        }


    }
}
