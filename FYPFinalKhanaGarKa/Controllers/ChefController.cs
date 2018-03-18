using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPFinalKhanaGarKa.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class ChefController : Controller
    {
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public ChefController(KhanaGarKaFinalContext _db,IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpPost]
        public IActionResult Index(string City,string Area)
        {
            List<Chef> chefs = db.Chef.Where<Chef>(i => i.City.Contains(City) && i.Area.Contains(Area)).ToList<Chef>();
            

            return View(chefs);
        }

        public IActionResult ChefAcc()
        {
            List<Menu> menus = new List<Menu>();
            List<Offer> offers = new List<Offer>();

            MenuOfferViewModel MenuOffer = new MenuOfferViewModel
            {
                Menus = menus,
                Offers = offers
            };
            return View(MenuOffer);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Chef c)
        {
            c.Category = "3 star";
            c.Role = "chef";
            c.Status = "Active";
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    string ourftp = env.WebRootPath + "/Chefs/";
                    var directoryinfo = new DirectoryInfo(ourftp);
                    if (!Directory.Exists(ourftp))
                    {
                        Directory.CreateDirectory(ourftp);

                    }
                    if (directoryinfo.Exists)
                    {
                        directoryinfo.CreateSubdirectory("" + c.Cnic);
                    }

                    db.Chef.Add(c);
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
    }
}