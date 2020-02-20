using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using Metrics.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        
        public async Task<IActionResult> MyAccount()
        {//TODO:Создать отдельное представление + контроллер для загрузки фотографии
            var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == HttpContext.User.Identity.Name);
            if (employeer != null)
            {
                return View(employeer);
            }
            else
            {
                return Content(User.Identity.Name);
            }
            
        }

        public async Task<IActionResult> Edit()
        {
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == HttpContext.User.Identity.Name);
            return View(employeer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employeer employeer)
        {
            employeer.Role = StaticValues.Master;
            var emp = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == User.Identity.Name);
            emp.Name = employeer.Name;
            emp.Percent = employeer.Percent;
            emp.INN = employeer.INN;
            emp.Specialization= employeer.Specialization;
            await db.SaveChangesAsync();
            return RedirectToAction("MyAccount");
        }

        //public IActionResult ChangePhoto(IFormFile uploadedFile)
        //{
        //    string path = "/Files/" + uploadedFile.FileName;

        //    // string filename = Guid.NewGuid.ToString();
        //    return View();
        //}

        public IActionResult ChangePhoto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePhoto(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = @"\Files\" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге 
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                //FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Email == HttpContext.User.Identity.Name);
                employeer.Photo = path;
                await db.SaveChangesAsync();
            }

            return RedirectToAction("MyAccount");
        }
    }
}