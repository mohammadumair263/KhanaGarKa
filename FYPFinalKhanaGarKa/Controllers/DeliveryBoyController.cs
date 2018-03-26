using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class DeliveryBoyController : Controller
    {
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env;

        public DeliveryBoyController(KhanaGarKaFinalContext _db,IHostingEnvironment _env)
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
        public IActionResult Register(DeliveryBoy d)
        {
            d.CreatedDate = DateTime.Now;
            d.ModifiedDate = DateTime.Now;
            d.Status = "Active";
            d.Role = "DBoy";
            using (var tr = db.Database.BeginTransaction())
            {
                string ourftp = env.WebRootPath + "/DBoy/";
                var directoryinfo = new DirectoryInfo(ourftp);
                if (!Directory.Exists(ourftp))
                {
                    Directory.CreateDirectory(ourftp);
                }
                if (directoryinfo.Exists)
                {
                    directoryinfo.CreateSubdirectory("" + d.Cnic);
                }

                try
                {
                    db.DeliveryBoy.Add(d);
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(DeliveryBoy d)
        {
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
            var chef = db.DeliveryBoy.Where(i => i.PhoneNo == c.Choice || i.Email == c.Choice).FirstOrDefault();

            return View();
        }
    }
}