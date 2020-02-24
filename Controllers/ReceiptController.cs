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
    [Authorize]
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
            receipt.ServicesProvidet./*Clear()*/Remove(work);
            //receipt.TotalPrice = receipt.ServicesProvidet.Sum(r => r.Price * r.Num);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit",receipt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Receipt receipt, string closeBtn, string saveBtn, string AddWorkBtn, int work, byte numWrk)
        {
            ViewBag.Employeers = new SelectList(db.Employeers, "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receiptDb = await db.Receipts.Include(e => e.Equipment).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefaultAsync(r => r.Id == receipt.Id);
            if (closeBtn != null)
            {
                receiptDb.Status = "Выдано";
                receiptDb.ClosedDate = DateTime.Now;
                await db.SaveChangesAsync();
                return RedirectToAction("All");
            }

            if (saveBtn != null)
            {
                receiptDb.Status = receipt.Status;
                receiptDb.DiagnosticResult = receipt.DiagnosticResult;
                receiptDb.TotalPrice = receipt.TotalPrice;
                await db.SaveChangesAsync();
                return RedirectToAction("All");
            }

            if (AddWorkBtn != null)
            {//TODO: Добавить редактирование кол-ва услуг

                var wrk = db.Works.FirstOrDefault(w => w.Id == work);

                if (wrk.Num != numWrk)
                {
                    var tempWork = new Work();
                    tempWork.Name = wrk.Name;
                    tempWork.Num = numWrk;
                    await db.Works.AddAsync(tempWork);

                    if (receiptDb.ServicesProvidet == null)
                    {
                        receiptDb.ServicesProvidet = new List<Work>();
                        receiptDb.ServicesProvidet.Add(tempWork);
                    }
                    else
                    {
                        receiptDb.ServicesProvidet.Add(tempWork);
                    }
                }
                
                if (receiptDb.ServicesProvidet == null)
                {
                    receiptDb.ServicesProvidet = new List<Work>();
                    receiptDb.ServicesProvidet.Add(wrk);
                }
                else
                {
                    receiptDb.ServicesProvidet.Add(wrk);
                }
                receiptDb.TotalPrice = receiptDb.ServicesProvidet.Sum(r => r.Price * r.Num);
            }

            await db.SaveChangesAsync();
            return View(receiptDb);
        }
    }
}