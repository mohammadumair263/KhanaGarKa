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
        public IActionResult Index()
        {
            List<Chef> chefs = new List<Chef>();
            chefs.Add(new Chef {
                ID = 1,
                Name = "Umair",
                City="Sialkot",
                Area="Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 2,
                Name = "Rashid",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Tayyab",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Saqlain",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Husnain",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });

            return View(chefs);
        }
        public IActionResult ChefAcc()
        {
            List<Menu> menus = new List<Menu>();
            List<Offer> offers = new List<Offer>();

            menus.Add(new Menu
            {
                Id = 1,
                DishName = "Chicken Krahi",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.",
                Price = 700,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            menus.Add(new Menu
            {
                Id = 2,
                DishName = "Daal Chawal",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.",
                Price = 100,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            menus.Add(new Menu
            {
                Id = 3,
                DishName = "Alo Gohbi",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.",
                Price = 50,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            menus.Add(new Menu
            {
                Id = 4,
                DishName = "Chicken Krahi",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.",
                Price = 700,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            menus.Add(new Menu
            {
                Id = 5,
                DishName = "Daal Chawal",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.",
                Price = 100,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            menus.Add(new Menu
            {
                Id = 6,
                DishName = "Alo Gohbi",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.",
                Price = 50,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });

            offers.Add(new Offer
            {
                Id = 1,
                OfferName = "Dhmaka Offer!",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce. 1 liter coke free.",
                Price = 1000,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            offers.Add(new Offer
            {
                Id = 2,
                OfferName = "Weekly Offer!",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.\n 1 liter coke free.",
                Price = 900,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            offers.Add(new Offer
            {
                Id = 3,
                OfferName = "Monthly Offer!",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.\n 1 liter coke free.",
                Price = 4000,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            offers.Add(new Offer
            {
                Id = 4,
                OfferName = "Dhmaka Offer!",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.\n 1 liter coke free.",
                Price = 1000,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            offers.Add(new Offer
            {
                Id = 5,
                OfferName = "Weekly Offer!",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.\n 1 liter coke free.",
                Price = 900,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });
            offers.Add(new Offer
            {
                Id = 6,
                OfferName = "Monthly Offer!",
                Description = "Too deep fried balls of sliced onion, in chana daal batter. With mint sauce.\n 1 liter coke free.",
                Price = 4000,
                ImageUrl = "img/foodimg/menuitem.jpg"
            });

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
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Umair",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 2,
                Name = "Rashid",
                City = "Sialkot",
                Area = "Sialkot",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Tayyab",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name="Sameer",
                City = "Sialkot",
                Area = "Sambrial",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Saqlain",
                City = "Sialkot",
                Area = "Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Husnain",
                City = "Sialkot",
                Area = "Pasrur",
                ImageUrl = "img/logo-google.jpg"
            });

            IList<Chef> searchedchefs = chefs.Where(m => m.City.Contains(City) && m.Area.Contains(Area)).ToList<Chef>();

            return View("searchedchefs",searchedchefs);
        }
        public IActionResult searchedchefs()
        {
            return View();
        }
    }
}