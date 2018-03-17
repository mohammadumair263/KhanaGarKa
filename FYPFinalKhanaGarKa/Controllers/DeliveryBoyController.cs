using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using Microsoft.AspNetCore.Mvc;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class DeliveryBoyController : Controller
    {
        private KhanaGarKaFinalContext db;
        public DeliveryBoyController(KhanaGarKaFinalContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(DeliveryBoy d)
        {
            d.Status = "Active";
            d.Role = "DBoy";
            db.DeliveryBoy.Add(d);
            db.SaveChanges();
            return View();
        }
    }
}