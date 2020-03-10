using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OXG.ServiceCenterWeb.Models;

namespace OXG.ServiceCenterWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ServiceCenterDbContext db;
        public HomeController(ILogger<HomeController> logger, ServiceCenterDbContext context)
        {
            _logger = logger;//не используется
            db = context;
        }
        /// <summary>
        /// Подсчитывает статистику сц(Доход за день/месяц), статистику мастеров (работник дня/месяца) 
        /// и передаёт их в представление
        /// </summary>
        /// <returns>Представление домашней страницы</returns>
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var stat = new Stat();
            stat.NowReceiptsCreated =await db.Receipts.Where(r => r.CreatedDate.Day == DateTime.Now.Day).CountAsync();
            stat.NowReceiptsClosed = await db.Receipts.Where(r => r.ClosedDate.Day == DateTime.Now.Day).CountAsync();
            stat.NowReceiptsSum = await db.Receipts.Where(r => r.ClosedDate.Day == DateTime.Now.Day).SumAsync(r => r.TotalPrice);
            stat.MonthReceiptsCreated = await db.Receipts.Where(r => r.CreatedDate.Month == DateTime.Now.Month).CountAsync();
            stat.MonthReceiptsClosed = await db.Receipts.Where(r => r.ClosedDate.Month == DateTime.Now.Month).CountAsync();
            stat.MonthReceiptsSum = await db.Receipts.Where(r => r.ClosedDate.Month == DateTime.Now.Month).SumAsync(r => r.TotalPrice);
            decimal max = 0;
            decimal temp = 0;
            var emps = db.Employeers;
            foreach (var emp in emps)
            {
                temp = db.Receipts.Include(e => e.Employeer).Where(r => r.ClosedDate.Day == DateTime.Now.Day && r.Employeer.Name == emp.Name).Sum(e => e.TotalPrice);
                if (temp > max)
                {
                    max = temp;
                    stat.DayWorkerName = emp.Name;
                    stat.DayWorkerSum = max;
                    stat.DayWorkerReceipts = await db.Receipts.Include(e => e.Employeer).Where(r => r.ClosedDate.Day == DateTime.Now.Day && r.Employeer.Name == emp.Name).CountAsync();
                }
            }

            temp = 0;
            max=0;
            foreach (var emp in emps)
            {
                temp =  db.Receipts.Include(e => e.Employeer).Where(r => r.ClosedDate.Month == DateTime.Now.Month && r.Employeer.Name == emp.Name).Sum(e => e.TotalPrice);
                if (temp > max)
                {
                    max = temp;
                    stat.MonthWorkerName = emp.Name;
                    stat.MonthWorkerSum = max;
                    stat.MonthWorkerReceipts = await db.Receipts.Include(e => e.Employeer).Where(r => r.ClosedDate.Month == DateTime.Now.Month && r.Employeer.Name == emp.Name).CountAsync();
                }
            }

            return View(stat);
        }

        public IActionResult Privacy()
        {
            //TODO: написать представление для правил пользования
            return View();
        }
        /// <summary>
        /// Возвращает страницу ошибки
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
