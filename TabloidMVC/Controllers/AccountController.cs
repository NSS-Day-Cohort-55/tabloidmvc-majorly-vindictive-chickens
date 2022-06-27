using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using System;

namespace TabloidMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public AccountController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Credentials credentials)
        {
            var userProfile = _userProfileRepository.GetByEmail(credentials.Email);

            if (userProfile == null)
            {
                ModelState.AddModelError("Email", "Invalid email");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, userProfile.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            UserProfile up = new UserProfile();

            return View(up);
        }

        // POST: Account/Create
        [HttpPost]
        public async Task<IActionResult> Create(UserProfile newUserProfile)
        {
            var userProfile = _userProfileRepository.GetByEmail(newUserProfile.Email);

            if (userProfile != null)
            {
                ModelState.AddModelError("Email", "User already exists");
                return View();
            }

            UserType userType = new UserType
            {
                Id = 2,
                Name = "Author"
            };

            newUserProfile.UserType = userType;
            newUserProfile.UserTypeId = 2;
            newUserProfile.CreateDateTime = DateTime.Now;

            _userProfileRepository.Register(newUserProfile);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUserProfile.Id.ToString()),
                new Claim(ClaimTypes.Email, newUserProfile.Email),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
