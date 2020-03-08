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

        public async Task<IActionResult> Index()
        {//TODO: Сделать красивый INDEX
            var data = new AdminIndexModel();
            for (int i = 0; i <= 30; i++)
            {
                    data.Dates.Add( DateTime.Now.AddDays(-i).ToShortDateString());
                    data.Moneys.Add(await db.Receipts.Where(r => r.ClosedDate.DayOfYear == DateTime.Now.AddDays(-i).DayOfYear).SumAsync(r => r.TotalPrice));
            }
            data.Masters.AddRange(db.Employeers.Where(e => e.RoleId == 1).Select(e => e.Name));
            foreach (var item in data.Masters)
            {
                data.Salaries.Add(await db.Receipts.Include(r => r.Employeer).Where(r => r.Employeer.Name==item).SumAsync(r => r.TotalPrice));
            }
            data.Moneys.Reverse();
            data.Dates.Reverse();
            return View(data);
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
            employeerDb.Name = employeer.Name;
            employeerDb.RoleId = employeer.RoleId;
            employeerDb.Specialization = employeer.Specialization;
            employeerDb.Percent = employeer.Percent;
            employeerDb.INN = employeer.INN;
            await db.SaveChangesAsync();
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent / 100);
            ViewBag.ReceiptsCount = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            return View(employeer);
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

        public IActionResult Clients()
        {
            var clients = db.Clients;
            return View(clients);
        }

        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await db.Clients.FirstOrDefaultAsync(r => r.Id == id);
            db.Clients.Remove(client);
            await db.SaveChangesAsync();
            return RedirectToAction("Clients");
        }

        public async Task<IActionResult> DeleteEmployeer(int id)
        {
            var employeer = await db.Employeers.FirstOrDefaultAsync(r => r.Id == id);
            if (employeer.Name!= User.Identity.Name)
            {
                db.Employeers.Remove(employeer);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Employeers");
        }
    }
}