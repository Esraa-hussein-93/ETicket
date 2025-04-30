using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using New_Eticket.Models;
using New_Eticket.Models.ViewModels;
using System.Reflection.Metadata.Ecma335;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using System.Security.Claims;




namespace New_Eticket.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager,RoleManager<IdentityRole> roleManager) 
        {
            this._userManager =  userManager;
            this._signInManager =signInManager;
            this._roleManager = roleManager;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_roleManager.Roles.IsNullOrEmpty())
            {
                //IdentityRole identityRole = new IdentityRole(roleName: " Admin");
                //IdentityRole identityRole = new IdentityRole(roleName: "SuperAdmin ");
                //IdentityRole identityRole = new IdentityRole(roleName: " Customer");
                //IdentityRole identityRole = new IdentityRole(roleName: "Company ");
                //IdentityRole identityRole = new IdentityRole(roleName: " ");

                await _roleManager.CreateAsync(role: new IdentityRole(roleName: "Admin"));
                await _roleManager.CreateAsync(role: new IdentityRole(roleName: "SuperAdmin"));
                await _roleManager.CreateAsync(role: new IdentityRole(roleName: "Customer"));
                await _roleManager.CreateAsync(role: new IdentityRole(roleName :"Company"));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVM.UserName,
                    Email = registerVM.Email,
                   // PasswordHash = registerVM.Password,
                    Address = registerVM.Address,
                };
                var result = await _userManager.CreateAsync(applicationUser , registerVM.Password);

                if (result.Succeeded)
                {
                    // success Registration >> 
                    await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                    await _userManager.AddToRoleAsync(applicationUser, role: "Customer");

                    return RedirectToAction("Index", "Home", new { area = "User" });
                }
                else
                {
                    //Error
                    ModelState.AddModelError("Password" , "Don`t Match the Constraints");                  
                }
            }
           return View(registerVM);
        }

        //---------------------------------------------------

        //للأمانة تمت الاستعانة بشات جي بي تي .. لاني مكنتش عارفة ازاي اشتغل من غير
        // razor ..
        // حاسة بتوهان بصراحة
        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { provider });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        public async Task<IActionResult> ExternalLoginCallback(string provider)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                // If the user does not exist, we can create a new user
                user = new ApplicationUser
                {
                    UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
                }
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            else
            {
                // If the external login fails, handle this case
                return RedirectToAction("ExternalLoginFailure");
            }
        }

       // ---------------------------------------------------------------------

        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInVM logInVM)
        {
            if (ModelState.IsValid)
            {// check the User Email in DB
                var appUser = await _userManager.FindByEmailAsync(logInVM.Email);

                if (appUser != null)
                {
                   var result=   await   _userManager.CheckPasswordAsync(appUser, logInVM.Password);
                    
                    if(result)
                    {
                        // Login
                       await  _signInManager.SignInAsync(appUser , logInVM.RememberMe);

                        return RedirectToAction("Index", "Home", new {area ="User" });
                    }
                    else
                    {
                        ModelState.AddModelError("Email"   , "Can not Found The Email");
                        ModelState.AddModelError("Password", "Password Don`t Match");
                    }
                }
                else 
                {
                    ModelState.AddModelError("Email"   , "Can not Found The Email");
                    ModelState.AddModelError("Password", "Password Don`t Match");
                }

            }
            return View(logInVM);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "User" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
