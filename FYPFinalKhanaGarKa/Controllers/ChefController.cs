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
        private KhanaGarKaFinalContext db = null;

        public ChefController(KhanaGarKaFinalContext db)
        {
            this.db = db;
        }

        [HttpPost]
        public IActionResult Index(string City, string Area)
        {
            List<Chef> chefs = db.Chef.Where<Chef>(i => i.City == City && i.Area == Area).ToList<Chef>();

            return View(chefs);
        }
        public IActionResult ChefAcc(int id)
        {
            List<Menu> menus = db.Menu.Where<Menu>(i => i.ChefId == id).ToList<Menu>();
            List<Offer> offers = db.Offer.Where<Offer>(i => i.ChefId == id).ToList<Offer>();
            MenuOfferViewModel ViewModel = new MenuOfferViewModel
            {
                Menus = menus,
                Offers = offers
            };
            return View(ViewModel);
        }
    }
}