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
        //private List<Work> servicesProvidet;
        private ServiceCenterDbContext db;
        public ReceiptController(ServiceCenterDbContext context)
        {
            db = context;
           //servicesProvidet = new List<Work>();
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

        public IActionResult Edit(int id)
        {
            ViewBag.Employeers = new SelectList(db.Employeers, "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receipt = db.Receipts.Include(e => e.Equipment).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefault(r => r.Id == id);
            return View(receipt);
        }

        public async Task<IActionResult> DeleteWork(int id, int RID)
        {
            ViewBag.Employeers = new SelectList(db.Employeers, "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receipt = db.Receipts.Include(e => e.Equipment).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefault(r => r.Id == RID);
            var work = await db.Works.FirstOrDefaultAsync(w => w.Id == id);
            receipt.ServicesProvidet.Remove(work);
            await db.SaveChangesAsync();
            return View("Edit",receipt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Receipt receipt, string closeBtn, string saveBtn, string AddWorkBtn, int work)
        {
            ViewBag.Employeers = new SelectList(db.Employeers, "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receiptDb = await db.Receipts.Include(e => e.Equipment).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefaultAsync(r => r.Id == receipt.Id);
            if (closeBtn != null)
            {

            }

            if (saveBtn != null)
            {

            }

            if (AddWorkBtn != null)
            {//TODO: Добавить редактирование кол-ва услуг
                var wrk = db.Works.FirstOrDefault(w => w.Id == work);
                if (receiptDb.ServicesProvidet == null)
                {
                    receiptDb.ServicesProvidet = new List<Work>();
                    receiptDb.ServicesProvidet.Add(wrk);
                }
                else
                {
                    receiptDb.ServicesProvidet.Add(wrk);
                }
                
            }

            await db.SaveChangesAsync();
            return View(receiptDb);
        }
    }
}