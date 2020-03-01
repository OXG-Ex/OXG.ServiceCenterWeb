using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OXG.ServiceCenterWeb.Models;

namespace OXG.ServiceCenterWeb.Controllers
{
    public class AccountController : Controller
    {
        private ServiceCenterDbContext db;
        public AccountController(ServiceCenterDbContext context)
        {
            db = context;
        }

        public IActionResult Login()
        {
          
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            
                var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == model.Email && e.Password == model.Password);
                if (employeer != null)
                {
                    await Authenticate(employeer.Email, employeer.Role.Name);
                    ViewBag.IsAuthenticated = "true";
                    return RedirectToAction("Index", "Home");
               
                }
            ModelState.AddModelError("", "Неверный Email и(или) пароль");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var employeer = await db.Employeers.FirstOrDefaultAsync(e => e.Email == model.Email);
                if (employeer == null)
                {
                    db.Employeers.Add(new Employeer() { Email = model.Email, Password = model.Password, RoleId = 1 });

                    await db.SaveChangesAsync();
                    await Authenticate(model.Email, "Мастер");
                    return RedirectToAction("Index", "Home");
                }

            }
                ModelState.AddModelError("", "Некоректные данные регистрации");
                return View(model);
        }

        private async Task Authenticate(string employeerName, string employeerRole)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType,employeerName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,employeerRole)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}