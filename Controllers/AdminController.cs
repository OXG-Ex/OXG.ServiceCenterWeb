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
        public AdminController(ServiceCenterDbContext context)//конструктор создающий объект контекста БД
        {
            db = context;
        }

        /// <summary>
        /// Подсчитывает статистику доходов за последние 30 дней, и статистику дохода мастеров 
        /// </summary>
        /// <returns>Главный экран панели администратора</returns>
        public async Task<IActionResult> Index()
        {
            var data = new AdminIndexModel();
            for (int i = 0; i <= 30; i++)
            {
                    data.Dates.Add( DateTime.Now.AddDays(-i).ToShortDateString());
                    data.Moneys.Add(await db.Receipts.Where(r => r.ClosedDate.DayOfYear == DateTime.Now.AddDays(-i).DayOfYear).SumAsync(r => r.TotalPrice));
            }
            data.Masters.AddRange(db.Employeers.Where(e => e.RoleId == 2).Select(e => e.Name));
            foreach (var item in data.Masters)
            {
                data.Salaries.Add(await db.Receipts.Include(r => r.Employeer).Where(r => r.Employeer.Name==item).SumAsync(r => r.TotalPrice));
            }
            data.Moneys.Reverse();
            data.Dates.Reverse();
            return View(data);
        }

        /// <summary>
        /// Возвращает список мастеров в виде таблицы
        /// </summary>
        public IActionResult Employeers()
        {
            var employeers = db.Employeers.Include(e => e.Role);
            return View(employeers);
        }

        /// <summary>
        /// Возвращает представление редактирования сотрудника
        /// </summary>
        /// <param name="id">ID сотрудника</param>
        /// <returns></returns>
        public async Task<IActionResult> EditEmployeer(int id)
        {//TODO: добавить выбор роли текстом
            var employeer =await db.Employeers.Include(e => e.Role).FirstOrDefaultAsync(r => r.Id == id);
            ViewBag.Spetializations = new SelectList(StaticValues.MasterSpecializations);
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status== "Выдано").Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent/100);
            ViewBag.ReceiptsCount =await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            return View(employeer);
        }

        /// <summary>
        /// Сохраняет изменения внесённые администратором в базу данных
        /// </summary>
        /// <param name="employeer">Объект сотрудника</param>
        /// <returns></returns>
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
            ViewBag.EmployeerSum = await db.Receipts.Include(e => e.Employeer)
                .Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано")
                .Select(r => r.TotalPrice).SumAsync();
            ViewBag.EmployeerSalary = (double)ViewBag.EmployeerSum * (employeer.Percent / 100);
            ViewBag.ReceiptsCount = await db.Receipts.Include(e => e.Employeer).Where(r => r.Employeer.Name == employeer.Name && r.Status == "Выдано").CountAsync();
            return View(employeer);
        }

        /// <summary>
        /// Возвращает список услуг оказываемый СЦ
        /// </summary>
        /// <returns></returns>
        public IActionResult Works()
        {
            var works = db.Works;
            return View(works);
        }

        /// <summary>
        /// Добавлякт новую услугу в БД
        /// </summary>
        /// <param name="NameWork">Наименование</param>
        /// <param name="NumWork">Кол-во</param>
        /// <param name="PriceWork">Стоимость услуги</param>
        /// <returns></returns>
        public async Task<IActionResult> AddWork(string NameWork, byte NumWork, decimal PriceWork)
        {
            var work = new Work() {Name=NameWork, Num=NumWork, Price =PriceWork };
            await db.Works.AddAsync(work);
            await db.SaveChangesAsync();
            return RedirectToAction("Works");
        }
        /// <summary>
        /// Удаляет услугу из БД
        /// </summary>
        /// <param name="id">ID Услуги</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteWork(int id)
        {
            var work =await db.Works.FirstOrDefaultAsync(r => r.Id == id);
            db.Works.Remove(work);
            await db.SaveChangesAsync();
            return RedirectToAction("Works");
        }
        /// <summary>
        /// Возвращает список клиентов СЦ
        /// </summary>
        /// <returns></returns>
        public IActionResult Clients()
        {
            var clients = db.Clients;
            return View(clients);
        }
        /// <summary>
        /// Удаление клиента из БД
        /// </summary>
        /// <param name="id">ID клиента</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await db.Clients.FirstOrDefaultAsync(r => r.Id == id);
            db.Clients.Remove(client);
            await db.SaveChangesAsync();
            return RedirectToAction("Clients");
        }
        /// <summary>
        /// Удаление аккаунта сотрудника из БД
        /// </summary>
        /// <param name="id">ID сотрудника</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteEmployeer(int id)
        {
            var employeer = await db.Employeers.FirstOrDefaultAsync(r => r.Id == id);
            if (employeer.Email != User.Identity.Name)
            {
                db.Employeers.Remove(employeer);
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Employeers");
        }
    }
}