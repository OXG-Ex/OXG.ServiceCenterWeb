using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OXG.ServiceCenterWeb.Models;
using OXG.ServiceCenterWeb.Models.SpecialModels;

namespace OXG.ServiceCenterWeb.Controllers
{
    public class ReceiptController : Controller
    {
        private ServiceCenterDbContext db;
        public ReceiptController(ServiceCenterDbContext context)
        {
            db = context;
        }

        public IActionResult Create()
        {
            ViewBag.Employeers = new SelectList(db.Employeers,"Id","Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(Receipt receipt)
        {//TODO: Добавить проверку значений
            var client = new Client() { Name = receipt.Client.Name, Phone = receipt.Client.Phone };
            var equipment = new Equipment() { Name = receipt.Equipment.Name, Accesories = receipt.Equipment.Accesories };
            var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Id == receipt.Employeer.Id);

            receipt.Employeer = employeer;
            receipt.CreatedDate = DateTime.Now;
            await db.Clients.AddAsync(client);
            await db.Equipments.AddAsync(equipment);
            await db.Receipts.AddAsync(receipt);
            await db.SaveChangesAsync();
            return RedirectToAction("All");
        }


        public IActionResult All()
        {
            var receipts =  db.Receipts.Include(e => e.Equipment);
            return View(receipts);
        }

    }
}