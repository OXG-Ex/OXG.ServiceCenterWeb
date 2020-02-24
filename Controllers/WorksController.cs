using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OXG.ServiceCenterWeb.Models;
using OXG.ServiceCenterWeb.Models.SpecialModels;

namespace OXG.ServiceCenterWeb.Controllers
{
    [Authorize]
    public class WorksController : Controller
    {
        private ServiceCenterDbContext db;
        public WorksController(ServiceCenterDbContext context)
        {
            db = context;
        }

        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> Initialize()
        {
            await db.Works.AddRangeAsync(StaticValues.Works);
            await db.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}