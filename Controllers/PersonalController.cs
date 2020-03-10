using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OXG.ServiceCenterWeb.Models;
using OXG.ServiceCenterWeb.Models.SpecialModels;

namespace OXG.ServiceCenterWeb.Controllers
{
    [Authorize]
    public class PersonalController : Controller
    {
        IWebHostEnvironment _appEnvironment;
        private ServiceCenterDbContext db;
        public PersonalController(ServiceCenterDbContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        /// <summary>
        /// НАходит пользователя в БД и передает объект в представление
        /// </summary>
        /// <returns>Представление личного кабинета пользователя</returns>
        public async Task<IActionResult> MyAccount()
        {
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == HttpContext.User.Identity.Name);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent / 100);
            ViewBag.ReceiptsCount = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            if (employeer != null)
            {
                return View(employeer);
            }
            else
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            
        }
        /// <summary>
        /// Возвращает представление для редактирования личных данных
        /// </summary>
        public async Task<IActionResult> Edit()
        {
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == HttpContext.User.Identity.Name);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent / 100);
            ViewBag.ReceiptsCount = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            return View(employeer);
        }
        /// <summary>
        /// Сохраняет изменения в БД
        /// </summary>
        /// <param name="employeer">Объект пользователя</param>
        /// <param name="NewPass">Новый пароль</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Employeer employeer, string NewPass)
        {
            employeer.Role = StaticValues.Master;
            var emp = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == User.Identity.Name);
            
            if (!string.IsNullOrEmpty(NewPass))
            {
                if (emp.Password != employeer.Password)
                {
                    ModelState.AddModelError("", "Пароль неверен");
                    return View(emp);
                }
                emp.Password = NewPass;
            }
            emp.Name = employeer.Name;
            emp.INN = employeer.INN;
            emp.Specialization= employeer.Specialization;
            emp.Email = employeer.Email;
            emp.Password = employeer.Password;
            await db.SaveChangesAsync();
            return RedirectToAction("MyAccount");
        }
        /// <summary>
        /// Отвечает за списание средств со счёта сотрудника
        /// </summary>
        /// <param name="employeer">Объект сотрудника</param>
        /// <param name="sum">Сумма списания</param>
        public async Task<IActionResult> Salary(Employeer employeer,decimal sum)
        {
            var emp = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == User.Identity.Name);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent / 100);
            ViewBag.ReceiptsCount = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            if (emp.Balance >= sum && sum>0)
            {
                emp.Balance -= sum;
                await db.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError("","Суммы на вашем счёте недостаточно");
                return View("MyAccount",emp);
            }
            return View("MyAccount", emp);
        }
        /// <summary>
        /// Загружает фото пользователя на сервер и сохраняет данные в БД
        /// </summary>
        /// <param name="uploadedFile">Загружаемы файл</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePhoto(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = @"\Files\" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == HttpContext.User.Identity.Name);
                employeer.Photo = path;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("MyAccount");
        }
        /// <summary>
        /// Возвращает представление для смены фото сотрудника
        /// </summary>
        public async Task<IActionResult> ChangePhoto()
        {
          return View();
        }
    }
}