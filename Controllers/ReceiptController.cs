using System;
using static System.Math;
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
        private ServiceCenterDbContext db;
        public ReceiptController(ServiceCenterDbContext context)
        {
            db = context;
        }

        /// <summary>
        /// Возвращает представление для создания квитанции
        /// </summary>
        public IActionResult Create()
        {
            ViewBag.Employeers = new SelectList(db.Employeers.Include(e => e.Role).Where(r => r.Role.Name == "Мастер"), "Id","Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            return View();
        }

        /// <summary>
        /// Создает новую квитанцию, привязывает к ней мастера, оборудование и клиента.
        /// Сохраняет квитанцию  в бд
        /// </summary>
        /// <param name="receipt">Объект квитанции</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(Receipt receipt)
        {//TODO: Добавить проверку значений
            
            var client = new Client() { Name = receipt.Client.Name, Phone = receipt.Client.Phone };
            var equipment = new Equipment() { Name = receipt.Equipment.Name, Accesories = receipt.Equipment.Accesories };
            var employeer = await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(e => e.Id == receipt.Employeer.Id);
            receipt.Employeer = employeer;
            receipt.Client = client;
            receipt.Equipment = equipment;
            receipt.CreatedDate = DateTime.Now;
            await db.Receipts.AddAsync(receipt);
            await db.SaveChangesAsync();
            return RedirectToAction("All");
        }

        /// <summary>
        /// Возвращает представление с таблицей всех квитанций
        /// </summary>
        /// <returns></returns>
        public IActionResult All()
        {
            var receipts =  db.Receipts.Include(e => e.Equipment);
            return View(receipts);
        }
        /// <summary>
        /// Находит квитанцию в БД и передаёт её в представление
        /// </summary>
        /// <param name="id">ID квитанции</param>
        /// <returns>Представление для редактирования квитанции</returns>
        public IActionResult Edit(int id)
        {
            ViewBag.Employeers = new SelectList(db.Employeers.Include(e => e.Role).Where(r => r.Role.Name == "Мастер"), "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receipt = db.Receipts.Include(e => e.Employeer).Include(e => e.Equipment).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefault(r => r.Id == id);
            return View(receipt);
        }
        /// <summary>
        /// Удаляет услугу из квитанции
        /// </summary>
        /// <param name="id">Id квитанции</param>
        /// <param name="RID">Id услуги</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteWork(int id, int RID)
        {
            ViewBag.Employeers = new SelectList(db.Employeers.Include(e => e.Role).Where(r => r.Role.Name == "Мастер"), "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receipt = db.Receipts.Include(e => e.Equipment).Include(e => e.Employeer).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefault(r => r.Id == RID);
            var work = await db.Works.FirstOrDefaultAsync(w => w.Id == id);
            receipt.ServicesProvidet./*Clear()*/Remove(work);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit",receipt);
        }
        /// <summary>
        /// Метод сохранения любых изменений в квитанции
        /// </summary>
        /// <param name="receipt">Квитанция</param>
        /// <param name="closeBtn">Кнопка закрытия квитанции</param>
        /// <param name="saveBtn">Кнопка сохранения изменений в квитанции</param>
        /// <param name="AddWorkBtn">Кнопка добавления услуги к квитанции</param>
        /// <param name="work">Услуга</param>
        /// <param name="numWrk">Кол-во услуг</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Receipt receipt, string closeBtn, string saveBtn, string AddWorkBtn, int work, byte numWrk)
        {
            ViewBag.Employeers = new SelectList(db.Employeers.Include(e => e.Role).Where(r => r.Role.Name=="Мастер"), "Id", "Name");
            ViewBag.Status = new SelectList(StaticValues.Statuses);
            ViewBag.Works = new SelectList(db.Works, "Id", "Name");
            var receiptDb = await db.Receipts.Include(e => e.Equipment).Include(e => e.Employeer).Include(c => c.Client).Include(w => w.ServicesProvidet).FirstOrDefaultAsync(r => r.Id == receipt.Id);
           
            ///Закрытие квитанции
            if (closeBtn != null)
            {
                receiptDb.Status = "Выдано";
                receiptDb.Warranty = receipt.Warranty;
                receiptDb.ClosedDate = DateTime.Now;
                receiptDb.DiagnosticResult = receipt.DiagnosticResult;
                receiptDb.TotalPrice = receiptDb.ServicesProvidet.Sum(r => r.Price * r.Num);
                receiptDb.Employeer.Balance += (Decimal)receiptDb.TotalPrice * ((Decimal)receiptDb.Employeer.Percent/100);
                await db.SaveChangesAsync();
                decimal receiptCount = db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == receiptDb.Employeer.Name).Count();
                decimal warrantiedReceipt = db.Receipts.Include(e => e.Employeer).Where(r => r.Warranty == true && r.Employeer.Name == receiptDb.Employeer.Name).Count();
                try
                {
                    decimal temp = (warrantiedReceipt / receiptCount)*100;
                    if (temp != 0)
                    {
                        receiptDb.Employeer.Reit =(byte)(Round(100 - temp));
                    }
                    else
                    {
                        receiptDb.Employeer.Reit = 100;
                    }
                }
                catch (Exception)
                {
                    receiptDb.Employeer.Reit = 0;
                }
                
                await db.SaveChangesAsync();
                return RedirectToAction("All");
            }
            
            ///Сохранение изменнений в квитанции
            if (saveBtn != null)
            {
                receiptDb.Status = receipt.Status;
                receiptDb.Warranty = receipt.Warranty;
                receiptDb.DiagnosticResult = receipt.DiagnosticResult;
                receiptDb.TotalPrice = receiptDb.ServicesProvidet.Sum(r => r.Price * r.Num);
                await db.SaveChangesAsync();
                return RedirectToAction("All");
            }
           
            ///Добавление услуги в квитанцию
            if (AddWorkBtn != null)
            {//TODO: Добавить редактирование кол-ва услуг

                var wrk = db.Works.FirstOrDefault(w => w.Id == work);

                if (wrk.Num != numWrk)
                {
                    var tempWork = new Work();
                    tempWork.Name = wrk.Name;
                    tempWork.Num = numWrk;
                    tempWork.Price = wrk.Price;
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
                else
                {
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
                receiptDb.TotalPrice = receiptDb.ServicesProvidet.Sum(r => r.Price * r.Num);
            }

            await db.SaveChangesAsync();
            return View(receiptDb);
        }
    }
}