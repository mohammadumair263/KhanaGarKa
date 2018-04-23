using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class OrderController : Controller
    {
        private const string SessionCNIC = "_UserC";
        private const string SessionRole = "_UserR";
        private const string SessionId = "_UserI";
        private const string SessionImgUrl = "_UserIU";
        private const string SessionName = "_UserN";
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
        
        public IActionResult Details(int id)
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
                    Role = HttpContext.Session.GetString(SessionRole)
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
                ItemGroup i = HttpContext.Session.Get<ItemGroup>("CartData");
                if (i != null)
                {
                    //ICollection<OrderLine> ol = null;
                    List<OrderLine> ol = new List<OrderLine>();
                    foreach (var items in i.Items)
                    {
                        ol.Add(new OrderLine
                        {
                            Name = items.Name,
                            Price = items.Price,
                            Quantity = items.Quantity
                        });
                    }
                    Orders o = new Orders
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = "Pending",
                        OrderType = "Collection",
                        ChefId = 2,
                        DeliveryBoyId = 1,
                        CustomerId = 1,
                        OrderLine = ol
                    };
                    
                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Orders.Add(o);
                            db.SaveChanges();
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
        
        public IActionResult History()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {

                if (string.Equals(HttpContext.Session.GetString(SessionRole), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View(new OrderHistoryViewModel
                    {
                        Orders = db.Orders.Where<Orders>(i => i.ChefId == HttpContext.Session.GetInt32(SessionId)).ToList<Orders>(),
                        Role = "chef"
                    });
                }
                else if (string.Equals(HttpContext.Session.GetString(SessionRole), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    return View(new OrderHistoryViewModel
                    {
                        Orders = db.Orders.Where<Orders>(i => i.CustomerId == HttpContext.Session.GetInt32(SessionId)).ToList<Orders>(),
                        Role = "customer"
                    });
                }
                else if (string.Equals(HttpContext.Session.GetString(SessionRole), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    return View(new OrderHistoryViewModel
                    {
                        Orders = db.Orders.Where<Orders>(i => i.DeliveryBoyId == HttpContext.Session.GetInt32(SessionId)).ToList<Orders>(),
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

        public IActionResult Process()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                return View(HttpContext.Session.Get<ItemGroup>("CartData"));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public JsonResult PostJson([FromBody]ItemGroup data)
        {
            if (data != null)
            {
                //ds = data;
                HttpContext.Session.Set<ItemGroup>("CartData", data);
            }

            return Json(new
            {
                state = 0,
                msg = string.Empty
            });
        }

        
    }
}