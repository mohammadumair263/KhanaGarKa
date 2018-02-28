using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using Microsoft.AspNetCore.Mvc;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            List<Menu> menus = new List<Menu>();
            List<Offer> offers = new List<Offer>();

            menus.Add(new Menu {
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

            offers.Add(new Offer {
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

            MenuOfferViewModel MenuOffer = new MenuOfferViewModel {
                Menus = menus,
                Offers = offers
            };

            return View(MenuOffer);
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Process()
        {
            return View();
        }
    }
}