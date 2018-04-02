using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        const string SessionCNIC = "_UserC";
        const string SessionRole = "_UserR";
        const string SessionId = "_UserI";

        public ChefController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpPost]
        public IActionResult Index(string City, string Area)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                
                {
                    List<Chef> chefs = db.Chef.Where<Chef>(i => i.City.Contains(City) && i.Area.Contains(Area)).ToList<Chef>();
                    return View(chefs);
                }
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
        }

        [HttpGet]
        public IActionResult ChefAcc(int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
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
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpGet]
        public IActionResult ChefMenu()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public IActionResult ChefMenu(Menu m)
        {
            m.Status = "Avalible";
            m.CreatedDate = DateTime.Now;
            m.ModifiedDate = DateTime.Now;
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
            return Redirect("ChefAcc/" + m.ChefId);
        }

        [HttpGet]
        public IActionResult ChefOffer()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
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
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
               HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View(db.Menu.Where<Menu>(i => i.MenuId == Id).FirstOrDefault<Menu>());
                }
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
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
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
               HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View(db.Offer.Where<Offer>(i => i.OfferId == id).FirstOrDefault<Offer>());
                }
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            
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