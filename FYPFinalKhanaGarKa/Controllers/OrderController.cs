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
        private static ItemGroup ds = new ItemGroup();

        public IActionResult Index()
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
            return View(ds);
        }

        [HttpPost]
        public JsonResult PostJson([FromBody]ItemGroup data)
        {
            if (data != null)
            {
                ds = data;
            }

            return Json(new
            {
                state = 0,
                msg = string.Empty
            });
        }
    }
}