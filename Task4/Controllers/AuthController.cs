using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.Models;
using UserManagementSystem.Models.DTOs;

namespace UserManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public IActionResult Registration()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registrationDto);
            }
            ApplicationUser applicationUser = ApplicationUser.CreateUser(registrationDto.FullName,registrationDto.Email);
            IdentityResult result= await userManager.CreateAsync(applicationUser,registrationDto.Password);
            if (result.Succeeded)
            {
                await SignInAsync(applicationUser,registrationDto.IsRemember);
                return RedirectToAction("Index", "User");
            }
            foreach (var x in result.Errors)
            {
                ModelState.AddModelError("", x.Description);
            }
            return View(registrationDto);            
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto,string? returnUrl=null)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
            ApplicationUser? applicationUser = await userManager.FindByEmailAsync(loginDto.Email);
            if (applicationUser is null || !await userManager.CheckPasswordAsync(applicationUser, loginDto.Password))
            {
                ModelState.AddModelError("", "Login or password is incorrect");
                return View(loginDto);
            }
            if (applicationUser.LockoutEnd.HasValue && applicationUser.LockoutEnd>DateTime.UtcNow)
            {
                TempData["Block"] = "Your account is blocked";
                return View(loginDto);
            }
            await SignInAsync(applicationUser, loginDto.IsRemember);
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "User");

            return LocalRedirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {          
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "User");
        }

        private async Task SignInAsync(ApplicationUser applicationUser, bool isRemember)
        {
            applicationUser.LastLogin = DateTime.UtcNow;
            await userManager.UpdateAsync(applicationUser);
            await signInManager.SignInAsync(applicationUser, isRemember);
        }
    }
}
