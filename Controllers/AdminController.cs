using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OXG.ServiceCenterWeb.Models;
using OXG.ServiceCenterWeb.Models.SpecialModels;

namespace OXG.ServiceCenterWeb.Controllers
{
    [Authorize(Roles="Администратор")]
    public class AdminController : Controller
    {
        private ServiceCenterDbContext db;
        public AdminController(ServiceCenterDbContext context)
        {
            db = context;
            //servicesProvidet = new List<Work>();
        }

        public IActionResult Index()
        {//TODO: Сделать красивый INDEX
            return View();
        }

        public IActionResult Employeers()
        {
            var employeers = db.Employeers.Include(e => e.Role);
            return View(employeers);
        }

        public async Task<IActionResult> EditEmployeer(int id)
        {//TODO: добавить выбор роли текстом
            var employeer =await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(r => r.Id == id);
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status== "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent/100);
            ViewBag.ReceiptsCount =await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            return View(employeer);
        }


        [HttpPost]
        public async Task<IActionResult> EditEmployeer(Employeer employeer)
        {//TODO: добавить выбор роли текстом
            var employeerDb = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(r => r.Id == employeer.Id);
            employeerDb = employeer;
            await db.SaveChangesAsync();
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent / 100);
            ViewBag.ReceiptsCount = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            return View(employeerDb);
        }

        public IActionResult Works()
        {
            var works = db.Works;
            return View(works);
        }

        public async Task<IActionResult> AddWork(string NameWork, byte NumWork, decimal PriceWork)
        {
            var work = new Work() {Name=NameWork, Num=NumWork, Price =PriceWork };
            await db.Works.AddAsync(work);
            await db.SaveChangesAsync();
            return RedirectToAction("Works");
        }

        public async Task<IActionResult> DeleteWork(int id)
        {
            var work =await db.Works.FirstOrDefaultAsync(r => r.Id == id);
            db.Works.Remove(work);
            await db.SaveChangesAsync();
            return RedirectToAction("Works");
        }
    }
}