using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class CustomerController : Controller
    {
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public CustomerController(KhanaGarKaFinalContext _db,IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Customer c)
        {
            c.CreatedDate = DateTime.Now;
            c.ModifiedDate = DateTime.Now;
            c.Status = "Active";
            c.Role = "Customer";
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    string ourftp = env.WebRootPath + "/Customers/";
                    var directoryinfo = new DirectoryInfo(ourftp);
                    if (!Directory.Exists(ourftp))
                    {
                        Directory.CreateDirectory(ourftp);

                    }
                    if (directoryinfo.Exists)
                    {
                        directoryinfo.CreateSubdirectory("" + c.Cnic);
                    }

                    db.Customer.Add(c);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel c)
        {
            var customer = db.Customer.Where(i => i.PhoneNo == c.Choice || i.Email == c.Choice).FirstOrDefault();

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Customer c)
        {
            return View();
        }

        [HttpGet]
        public IActionResult ModifyDetails(int id)
        {
            return View(db.Customer.Where(i => i.CustomerId == id).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult ModifyDetails(Customer c)
        {
            c.ModifiedDate = DateTime.Now;
            c.Role = "Customer";
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Customer.Add(c);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ModifyDetails");
        }
    }
}