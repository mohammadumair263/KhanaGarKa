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
            db=_db;
        }
        public IActionResult Index()
        {
            List<Chef> chefs = new List<Chef>();
            

            return View(chefs);
        }
        public IActionResult ChefAcc()
        {
            List<Menu> menus = new List<Menu>();
            List<Offer> offers = new List<Offer>();

            ;

            MenuOfferViewModel MenuOffer = new MenuOfferViewModel
            {
                Menus = menus,
                Offers = offers
            };
            return View(MenuOffer);
        }
        [HttpGet]
        public IActionResult searchChef()
        {
            return View();
        }
        [HttpPost]
        public IActionResult searchChef(string City, string Area)
        {
            List<Chef> chefs = new List<Chef>();
            

            IList<Chef> searchedchefs = chefs.Where(m => m.City.Contains(City) && m.Area.Contains(Area)).ToList<Chef>();

            return View("searchedchefs",searchedchefs);
        }
        public IActionResult searchedchefs()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterChef()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterChef(Chef c)
        {
            
            
                
                    db.Chef.Add(c);
                    db.SaveChanges();
                    
                
                
            
            return View();
        }
    }
}