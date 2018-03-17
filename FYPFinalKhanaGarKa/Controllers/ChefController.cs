using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPFinalKhanaGarKa.Models;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class ChefController : Controller
    {
        private KhanaGarKaFinalContext db;
        public ChefController(KhanaGarKaFinalContext _db)
        {
            db = _db;
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
            db.Chef.Add(c);
            db.SaveChanges();

            return View();
        }
    }
}