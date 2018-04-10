using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class OrderController : Controller
    {
        const string SessionCNIC = "_UserC";
        const string SessionRole = "_UserR";
        const string SessionId = "_UserI";
        private static ItemGroup ds = new ItemGroup();
        private KhanaGarKaFinalContext db = null;

        public OrderController(KhanaGarKaFinalContext db)
        {
            this.db = db;
        }

        public IActionResult Index(int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "customer", StringComparison.OrdinalIgnoreCase))
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
                else
                {
                    return RedirectToAction("Page404", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }

        [Route("Order/Details/{role?}/{id?}")]
        public IActionResult Details(string role,int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {

                List<OrderLine> Dishs = db.OrderLine.Where<OrderLine>(i => i.OrderId == id).ToList<OrderLine>();
                Orders Order = db.Orders.Where<Orders>(i => i.OrderId == id).FirstOrDefault();
                Chef c = db.Chef.Where<Chef>(i => i.ChefId == Order.ChefId).FirstOrDefault();

                OrderDetailViewModel ViewModel = new OrderDetailViewModel
                {
                    Dishis = Dishs,
                    Chef = c,
                    Order = Order,
                    Role = role
                };

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Success()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if(ds != null)
                {
                    Orders o = new Orders
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = "Pending",
                        OrderType = ds.OrderType
                    };
                    foreach (var orli in ds.Items)
                    {
                        OrderLine ol = new OrderLine
                        {

                        };
                    }

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Orders.Add(o);
                            db.SaveChanges();
                            int id = o.OrderId;
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [Route("Order/History/{role?}/{id?}")]
        public IActionResult History(string role,int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {

                if (string.Equals(role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View(new OrderHistoryViewModel
                    {
                        Orders = db.Orders.Where<Orders>(i => i.ChefId == id).ToList<Orders>(),
                        Role = "chef"
                    });
                }
                else if (string.Equals(role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    return View(new OrderHistoryViewModel
                    {
                        Orders = db.Orders.Where<Orders>(i => i.CustomerId == id).ToList<Orders>(),
                        Role = "customer"
                    });
                }
                else if (string.Equals(role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    return View(new OrderHistoryViewModel
                    {
                        Orders = db.Orders.Where<Orders>(i => i.DeliveryBoyId == id).ToList<Orders>(),
                        Role = "DBoy"
                    });
                }

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Summary()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        public IActionResult Process(ItemGroup d)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (d != null)
                {
                    return View(d);
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
        public IActionResult JsonC([FromBody]ItemGroup d)
        {
            return Process(d);
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