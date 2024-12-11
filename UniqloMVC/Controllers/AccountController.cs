using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using UniqloMVC.Enums;
using UniqloMVC.Models;
using UniqloMVC.ViewModels.Auths;

namespace UniqloMVC.Controllers
{
    public class AccountController(UserManager<User> _userManager,SignInManager<User> _signInManager) : Controller
    {
        bool isAuthenticated=>User.Identity?.IsAuthenticated??false;
       
        public IActionResult Register()
        {            
            return View();
        }
                        
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateVM vm)
        {
            if (isAuthenticated) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View();

            User user = new User
            {
                Email = vm.Email,
                Fullname = vm.FullName,
                UserName = vm.UserName,
                ProfileImageUrl = "photo.jpg",
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User));

            if (roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(Login));
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm,string? returnUrl=null)
        {
            if(!ModelState.IsValid) return View();
            User user = null;
            if(vm.UserNameOrEmail.Contains('@')) user= await _userManager.FindByEmailAsync(vm.UserNameOrEmail);
            else
                user = await _userManager.FindByNameAsync(vm.UserNameOrEmail);  

            if(user is null)
            {
                ModelState.AddModelError("","username or password is wrong");
                return View();
            }
           var result = await _signInManager.PasswordSignInAsync(user,vm.Password, vm.RememberMe,true);
            if(!result.Succeeded)
            {
                if (result.IsNotAllowed)
                    ModelState.AddModelError("", "username or password is wrong");
                if(!result.IsLockedOut)
                    ModelState.AddModelError("","Wait until" + user.LockoutEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"));
               return View();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                if(await _userManager.IsInRoleAsync(user,"Admin"))
                {
                    return RedirectToAction("Index", new {Controller="Dashboard",Area="Admin"});
                }

                return RedirectToAction("Index", "Home");
            }
            return LocalRedirect(returnUrl);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));   
        }

    }
}
    