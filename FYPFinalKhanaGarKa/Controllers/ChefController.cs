using FYPFinalKhanaGarKa.Models;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class ChefController : Controller
    {
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;
        private const string SessionCNIC = "_UserC";
        private const string SessionRole = "_UserR";
        private const string SessionId = "_UserI";

        public ChefController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpPost]
        public IActionResult Index(string City, string Area)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                
                {
                    List<Chef> chefs = db.Chef.Where<Chef>(i => i.City.Contains(City) && i.Area.Contains(Area)).ToList<Chef>();
                    return View(chefs);
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

        [HttpGet]
        public IActionResult ChefAcc(int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    List<Menu> menus = db.Menu.Where<Menu>(i => i.ChefId == id).ToList<Menu>();
                    List<Offer> offers = db.Offer.Where<Offer>(i => i.ChefId == id).ToList<Offer>();

                    MenuOfferViewModel MenuOffer = new MenuOfferViewModel
                    {
                        Menus = menus,
                        Offers = offers
                    };
                    return View(MenuOffer);
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

        [HttpGet]
        public IActionResult ChefMenu()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View();
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
        public IActionResult ChefMenu(MenuViewModel vm)
        {
            Menu m = new Menu {
                DishName = vm.DishName,
                Price = vm.Price,
                Description = vm.Description,
                Status = "Active",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ChefId = (int)HttpContext.Session.GetInt32(SessionId)
        };
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        string menudir = env.WebRootPath + "/Uploads/Chefs/" +
                        HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Menu";

                        if (!Directory.Exists(menudir))
                        {
                            Directory.CreateDirectory(menudir.Replace(" ", string.Empty));

                            bool isUploaded = UploadImage(vm.Image, menudir);
                            if (isUploaded == true)
                            {
                                m.ImgUrl = "/Uploads/Chefs/" +
                                HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Menu/"+
                                GetUniqueName(vm.Image.FileName);
                            }
                            else {
                                // image not uploaded
                                // do somthing 
                            }
                        }
                        else
                        {
                            bool isUploaded = UploadImage(vm.Image, menudir);
                            if (isUploaded == true)
                            {
                                m.ImgUrl = "/Uploads/Chefs/" +
                                HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Menu/" +
                                GetUniqueName(vm.Image.FileName);
                            }
                            else {
                                // image not uploaded
                                // do somthing
                            }
                        }
                    }

                    db.Menu.Add(m);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc/" + m.ChefId);
        }

        [HttpGet]
        public IActionResult ChefOffer()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View();
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
        public IActionResult ChefOffer(OfferViewModel vm)
        {
            Offer o = new Offer
            {
                OfferName = vm.OfferName,
                Price = vm.Price,
                Description = vm.Description,
                EndDate = vm.EndDate,
                Percentage = vm.Percentage,
                Status = "Avalible",
                ChefId = (int)HttpContext.Session.GetInt32(SessionId),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.Now
            };
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        string offerdir = env.WebRootPath + "/Uploads/Chefs/" +
                        HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Offer";

                        if (!Directory.Exists(offerdir))
                        {
                            Directory.CreateDirectory(offerdir.Replace(" ", string.Empty));

                            bool isUploaded = UploadImage(vm.Image, offerdir);
                            if (isUploaded == true)
                            {
                                o.ImgUrl = "/Uploads/Chefs/" +
                                HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Offer/" +
                                GetUniqueName(vm.Image.FileName);
                            }
                            else
                            {
                                // image not uploaded
                                // do somthing 
                            }
                        }
                        else
                        {
                            bool isUploaded = UploadImage(vm.Image, offerdir);
                            if (isUploaded == true)
                            {
                                o.ImgUrl = "/Uploads/Chefs/" +
                                HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Offer/" +
                                GetUniqueName(vm.Image.FileName);
                            }
                            else
                            {
                                // image not uploaded
                                // do somthing
                            }
                        }
                    }
                    db.Offer.Add(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc/" + o.ChefId);
        }

        [HttpPost]
        public IActionResult DeleteChefMenu(MenuViewModel vm)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    DeleteImage(env.WebRootPath+vm.ImgUrl);
                    //db.Menu.Remove(m);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc/"+ (int)HttpContext.Session.GetInt32(SessionId));
        }

        [HttpPost]
        public IActionResult DeleteChefOffer(OfferViewModel vm)
        {
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    DeleteImage(env.WebRootPath + vm.ImgUrl);
                    //db.Offer.Remove(o);
                    db.SaveChanges();
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return RedirectToAction("ChefAcc/" + (int)HttpContext.Session.GetInt32(SessionId));
        }

        [HttpGet]
        public IActionResult EditChefMenu(int Id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
               HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Menu m = db.Menu.Where<Menu>(i => i.MenuId == Id).FirstOrDefault<Menu>();
                    return View(new MenuViewModel {
                        MenuId = m.MenuId,
                        DishName = m.DishName,
                        Description = m.Description,
                        ImgUrl = m.ImgUrl,
                        Status = m.Status,
                        Price = m.Price
                    });
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
        public IActionResult EditChefMenu(MenuViewModel vm)
        {
            Menu menu = db.Menu.Where(i => i.MenuId == vm.MenuId).FirstOrDefault();
            //m.ChefId = (int)HttpContext.Session.GetInt32(SessionId);
            menu.ModifiedDate = DateTime.Now;
            menu.DishName = vm.DishName;
            menu.Price = vm.Price;
            menu.Status = vm.Status;
            menu.Description = vm.Description;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    if (vm.Image != null && vm.Image.Length > 0)
                    {
                        string menudir = env.WebRootPath + "/Uploads/Chefs/" +
                        HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Menu";

                        if (!Directory.Exists(menudir))
                        {
                            Directory.CreateDirectory(menudir.Replace(" ", string.Empty));
                            bool isDeleted = DeleteImage(env.WebRootPath + vm.ImgUrl);
                            if (isDeleted)
                            {
                                bool isUploaded = UploadImage(vm.Image, menudir);
                                if (isUploaded == true)
                                {
                                    menu.ImgUrl = "/Uploads/Chefs/" +
                                    HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Menu/" +
                                    GetUniqueName(vm.Image.FileName);
                                }
                                else
                                {
                                    // image not uploaded
                                    // do somthing 
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            bool isDeleted = DeleteImage(env.WebRootPath + vm.ImgUrl);
                            if (isDeleted)
                            {
                                bool isUploaded = UploadImage(vm.Image, menudir);
                                if (isUploaded == true)
                                {
                                    menu.ImgUrl = "/Uploads/Chefs/" +
                                    HttpContext.Session.GetString(SessionCNIC).Replace(" ", string.Empty) + "/Menu/" +
                                    GetUniqueName(vm.Image.FileName);
                                }
                                else
                                {
                                    // image not uploaded
                                    // do somthing
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                    db.Menu.Update(menu);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc/" + HttpContext.Session.GetInt32(SessionId));
        }

        [HttpGet]
        public IActionResult EditChefOffer(int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
               HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    return View(db.Offer.Where<Offer>(i => i.OfferId == id).FirstOrDefault<Offer>());
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
        public IActionResult EditChefOffer(Offer o)
        {

            o.ChefId = (int)HttpContext.Session.GetInt32(SessionId);
            o.ModifiedDate = DateTime.Now;
            using (var tr = db.Database.BeginTransaction())
            {
                try
                {
                    db.Offer.Update(o);
                    db.SaveChanges();

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }
            return Redirect("ChefAcc/" + o.ChefId);
        }

        //Helper Methods
        private string GetUniqueName(string FileName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") +
                        Path.GetExtension(FileName);
        }

        private bool UploadImage(IFormFile Image, string Name)
        {
            if (Image != null && Image.Length > 0 && Image.Length < 1000000)
            {
                string ext = Path.GetExtension(Image.FileName);

                if (string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    var uniqueFileName = GetUniqueName(Image.FileName);
                    var uploads = Path.Combine(env.WebRootPath, Name);
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                return true;
            }
            else
            {
                return true;
            }
        }

        private bool DeleteImage(string path)
        {
            if (System.IO.File.Exists(path.Trim()))
            {
                System.IO.File.Delete(path);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}