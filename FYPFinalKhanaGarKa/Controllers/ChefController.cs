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
            return View();
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