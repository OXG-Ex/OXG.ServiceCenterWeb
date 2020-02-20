using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Create(Receipt receipt)
        {
            
            return View();
        }
    }
}