using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class OrderController : Controller
    {
        private static ItemGroup ds = new ItemGroup();
        private KhanaGarKaFinalContext db = null;

        public OrderController(KhanaGarKaFinalContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int id)
        {
            List<Menu> menus = db.Menu.Where<Menu>(i => i.ChefId == id).ToList<Menu>();
            List<Offer> offers = db.Offer.Where<Offer>(i => i.ChefId == id).ToList<Offer>();
            MenuOfferViewModel ViewModel = new MenuOfferViewModel {
                Menus = menus,
                Offers = offers
            };
            return View(ViewModel);
        }

        public IActionResult Details(int id)
        {
            List<OrderLine> Dishs = db.OrderLine.Where<OrderLine>(i => i.OrderId == id).ToList<OrderLine>();
            Orders Order = db.Orders.Where<Orders>(i => i.OrderId == id).FirstOrDefault();
            Chef c = db.Chef.Where<Chef>(i => i.ChefId == Order.ChefId).FirstOrDefault();
            
            OrderDetailViewModel ViewModel = new OrderDetailViewModel
            {
                Dishis = Dishs,
                Chef = c,
                Order = Order
            };

            return View(ViewModel);
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult History(int id)
        {
            List<Orders> orders = db.Orders.Where<Orders>(i => i.CustomerId == id).ToList<Orders>();

            return View(orders);
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