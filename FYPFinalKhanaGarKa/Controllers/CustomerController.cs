using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using Microsoft.AspNetCore.Mvc;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class CustomerController : Controller
    {
        private KhanaGarKaFinalContext db;
        public CustomerController(KhanaGarKaFinalContext _db)
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
        public IActionResult Register(Customer c)
        {
            c.Status = "Active";
            c.Role = "Customer";
            db.Customer.Add(c);
            db.SaveChanges();
            return View();
        }
    }
}