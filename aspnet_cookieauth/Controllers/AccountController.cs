using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnet_cookieauth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace aspnet_cookieauth.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        //public IActionResult Login()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            return View();
        }



        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("user is not set.");
            }

            //Get actual user from the Database or external service provider 


            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, loginUser.Username.ToString()));
            identity.AddClaim(new Claim("DisplayName", loginUser.Username));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    IssuedUtc = DateTimeOffset.UtcNow,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });
            return RedirectToLocal(string.Empty);
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }


        [HttpGet("[action]"), HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToLocal(string.Empty);
        }
        [HttpGet("[action]"), HttpPost("[action]")]
        public async Task<IActionResult> Authorized()
        {
            return View();
        }

    }
}