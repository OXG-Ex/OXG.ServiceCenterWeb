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
        public AccountController(ServiceCenterDbContext context)//конструктор создающий объект контекста БД
        {
            db = context;
        }

        public IActionResult Login()//Метод возвращающий представление для входа в аккаунт
        {
          
            return View();
        }
        /// <summary>
        /// Метод отвечающий за поиск пользователя в БД и аутентификацию
        /// </summary>
        /// <param name="model">Модель полученная от пользователя(Логи + пароль)</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод возвращающий представление для регистрации нового пользователя
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// Метод возвращающий представление, в случае недостаточных прав доступа
        /// </summary>
        /// <returns></returns>
        public IActionResult AccessDenied()
        {
            return View();
        }
       
        /// <summary>
        /// Метод отвечающий за регистарцию нового пользователя
        /// </summary>
        /// <param name="model">Модель регистарции (Логин + пароль  +подтверждение пароля)</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var employeer = await db.Employeers.FirstOrDefaultAsync(e => e.Email == model.Email);
                if (employeer == null)
                {
                    db.Employeers.Add(new Employeer() { Email = model.Email, Password = model.Password, RoleId = 2 });

                    await db.SaveChangesAsync();
                    await Authenticate(model.Email, "Мастер");
                    return RedirectToAction("MyAccount", "Personal");
                }

            }
                ModelState.AddModelError("", "Некоректные данные регистрации");
                return View(model);
        }
        /// <summary>
        /// Метод выполняющий вход в аккаунт и добавляющий аутентификационные куки 
        /// </summary>
        /// <param name="employeerName">Логин пользователя</param>
        /// <param name="employeerRole">Роль</param>
        /// <returns></returns>
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
        /// <summary>
        /// Метод выполняющий выход из аккаунта
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}