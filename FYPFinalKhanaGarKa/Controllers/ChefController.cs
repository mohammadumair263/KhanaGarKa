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
        private const string SessionCNIC = "_UserC";
        private const string SessionRole = "_UserR";
        private const string SessionId = "_UserI";

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
        public IActionResult ChefAcc()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    List<Menu> menus = db.Menu.Where<Menu>(i => i.ChefId == HttpContext.Session.GetInt32(SessionId)).ToList<Menu>();
                    List<Offer> offers = db.Offer.Where<Offer>(i => i.ChefId == HttpContext.Session.GetInt32(SessionId)).ToList<Offer>();

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
        public IActionResult ChefMenu(MenuViewModel vm)
        {
            Menu m = new Menu {
                DishName = vm.DishName,
                Price = vm.Price,
                Description = vm.Description,
                Status = "Avalible",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ChefId = (int)HttpContext.Session.GetInt32(SessionId)
        };
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        m.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/" + HttpContext.Session.GetString(SessionCNIC).Trim()+"/Menu/", vm.Image);
                    }

                    db.Menu.Add(m);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc");
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
        public IActionResult ChefOffer(OfferViewModel vm)
        {
            Offer o = new Offer
            {
                OfferName = vm.OfferName,
                Price = vm.Price,
                Description = vm.Description,
                EndDate = vm.EndDate,
                Percentage = vm.Percentage,
                Status = "Avalible",
                ChefId = (int)HttpContext.Session.GetInt32(SessionId),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now
            };
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        o.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/" + HttpContext.Session.GetString(SessionCNIC).Trim()+"/Offer/", vm.Image);
                    }
                    db.Offer.Add(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc#Offer");
        }

        [HttpPost]
        public IActionResult DeleteChefMenu(MenuViewModel vm)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    Utils.DeleteImage(env.WebRootPath+vm.ImgUrl);
                    Menu m = new Menu() { MenuId = vm.MenuId };
                    db.Menu.Attach(m);
                    db.Menu.Remove(m);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc#Menu");
        }

        [HttpPost]
        public IActionResult DeleteChefOffer(OfferViewModel vm)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    Utils.DeleteImage(env.WebRootPath + vm.ImgUrl);
                    Offer o = new Offer() { OfferId = vm.OfferId };
                    db.Offer.Remove(o);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc#Offer");
        }

        [HttpGet]
        public IActionResult EditChefMenu(int Id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
               HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Menu m = db.Menu.Where<Menu>(i => i.MenuId == Id && i.ChefId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault<Menu>();
                    return View(new MenuViewModel {
                        MenuId = m.MenuId,
                        DishName = m.DishName,
                        Description = m.Description,
                        ImgUrl = m.ImgUrl,
                        Status = m.Status,
                        Price = m.Price
                    });
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
        public IActionResult EditChefMenu(MenuViewModel vm)
        {
            Menu menu = db.Menu.Where(i => i.MenuId == vm.MenuId).FirstOrDefault();
            menu.ModifiedDate = DateTime.Now;
            menu.DishName = vm.DishName;
            menu.Price = vm.Price;
            menu.Status = vm.Status;
            menu.Description = vm.Description;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        menu.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/" + HttpContext.Session.GetString(SessionCNIC).Trim() + "/Menu/", vm.Image, menu.ImgUrl);
                    }
                    db.Menu.Update(menu);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc#Menu");
        }

        [HttpGet]
        public IActionResult EditChefOffer(int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
               HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Offer o = db.Offer.Where<Offer>(i => i.OfferId == id && i.ChefId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault<Offer>();
                    return View(new OfferViewModel {
                        OfferId = o.OfferId,
                        OfferName = o.OfferName,
                        Percentage = o.Percentage,
                        EndDate = o.EndDate,
                        ImgUrl = o.ImgUrl,
                        Price = o.Price,
                        Description = o.Description
                    });
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
        public IActionResult EditChefOffer(OfferViewModel vm)
        {
            Offer o = db.Offer.Where(i => i.OfferId == vm.OfferId).FirstOrDefault();
            o.ModifiedDate = DateTime.Now;
            o.OfferName = vm.OfferName;
            o.Description = vm.Description;
            o.Price = vm.Price;
            o.EndDate = vm.EndDate;
            o.Percentage = vm.Percentage;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if(vm.Image != null && vm.Image.Length > 0)
                    {
                        o.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/" + HttpContext.Session.GetString(SessionCNIC).Trim() + "/Offer/", vm.Image, o.ImgUrl);
                    }
                    db.Offer.Update(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc#Offer");
        }
    }
}