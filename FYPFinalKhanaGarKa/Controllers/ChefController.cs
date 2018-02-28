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
                Address="Daska",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 2,
                Name = "Rashid",
                Address = "Sialkot",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Tayyab",
                Address = "Addah",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Sameer",
                Address = "Sambriyal",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Saqlain",
                Address = "Sialkot",
                ImageUrl = "img/logo-google.jpg"
            });
            chefs.Add(new Chef
            {
                ID = 1,
                Name = "Husnain",
                Address = "Sialkot",
                ImageUrl = "img/logo-google.jpg"
            });

            return View(chefs);
        }
        public IActionResult ChefAcc()
        {
            return View();
        }
    }
}