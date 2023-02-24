using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Recrutement.Models;
using Recrutement.ViewModels;
using System.Threading.Tasks;

namespace Recrutement.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdministrationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel User)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(User.UserName, User.Password, true, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Applications");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(User);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

    }
}
