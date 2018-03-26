using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class ChefController : Controller
    {
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public ChefController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpPost]
        public IActionResult Index(string City, string Area)
        {
            List<Chef> chefs = db.Chef.Where<Chef>(i => i.City.Contains(City) && i.Area.Contains(Area)).ToList<Chef>();


            return View(chefs);
        }

        [HttpGet]
        public IActionResult ChefAcc(int id)
        {
            List<Menu> menus = db.Menu.Where<Menu>(i => i.ChefId == id).ToList<Menu>();
            List<Offer> offers = db.Offer.Where<Offer>(i => i.ChefId == id).ToList<Offer>();

            MenuOfferViewModel MenuOffer = new MenuOfferViewModel
            {
                Menus = menus,
                Offers = offers
            };
            return View(MenuOffer);
        }

        [HttpGet]
        public IActionResult ChefMenu()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChefMenu(Menu m)
        {
            m.Status = "Avalible";
            m.CreatedDate = DateTime.Now;
            m.ModifiedDate = DateTime.Now;
            m.ChefId = 5;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Menu.Add(m);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            //return RedirectToAction("ChefAcc");
            return Redirect("ChefAcc/" + m.ChefId);
        }

        [HttpGet]
        public IActionResult ChefOffer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChefOffer(Offer o)
        {
            o.Status = "Avalible";
            o.ChefId = 5;
            o.CreatedDate = DateTime.Now;
            o.ModifiedDate = DateTime.Now;
            o.StartDate = DateTime.Now;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Offer.Add(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Chef c)
        {
            c.CreatedDate = DateTime.Now;
            c.ModifiedDate = DateTime.Now;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Chef c)
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

             var chef = db.Chef.Where(i => i.PhoneNo == c.Choice || i.Email == c.Choice).FirstOrDefault();

            return View();
        }

        [HttpGet]
        public IActionResult ModifyDetails(int id)
        {
            return View(db.Chef.Where(i => i.ChefId == id).FirstOrDefault());
        }

        [HttpPost]
        public IActionResult ModifyDetails(Chef c)
        {
            c.ModifiedDate = DateTime.Now;
            c.Role = "Chef";
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Chef.Add(c);
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

        [HttpPost]
        public IActionResult DeleteChefMenu(int Id)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    
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
        public IActionResult EditChefMenu(int Id)
        {
            return View(db.Menu.Where<Menu>(i => i.MenuId == Id).FirstOrDefault<Menu>());
        }

        [HttpPost]
        public IActionResult EditChefMenu(Menu m)
        {
            m.ChefId = 5;
            m.ModifiedDate = DateTime.Now;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Menu.Update(m);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc");
        }

        [HttpGet]
        public IActionResult EditChefOffer(int id)
        {
            return View(db.Offer.Where<Offer>(i => i.OfferId == id).FirstOrDefault<Offer>());
        }

        [HttpPost]
        public IActionResult EditChefOffer(Offer o)
        {
            o.ChefId = 5; // ya us chef ki id ho gi jo login ha 
            o.ModifiedDate = DateTime.Now;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Offer.Update(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc");
        }
    }
}