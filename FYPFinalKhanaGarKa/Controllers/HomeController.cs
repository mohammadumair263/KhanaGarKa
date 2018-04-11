using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPFinalKhanaGarKa.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using FYPFinalKhanaGarKa.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FYPFinalKhanaGarKa.Controllers
{
    
    
    public class HomeController : Controller
    {
        const string SessionCNIC = "_UserC";
        const string SessionRole = "_UserR";
        const string SessionId = "_UserI";
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public HomeController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.Equals(vm.Role, "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = db.Chef.Where(i => i.Email == vm.Email && i.Password == vm.Password).FirstOrDefault();
                    if(c != null)
                    {
                        HttpContext.Session.SetString(SessionCNIC,c.Cnic);
                        HttpContext.Session.SetString(SessionRole,c.Role);
                        HttpContext.Session.SetInt32(SessionId, c.ChefId);
                        return Redirect("/Chef/ChefAcc/"+c.ChefId);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Password Or Email is wrong !");
                    }

                }
                else if (string.Equals(vm.Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.Email == vm.Email && i.Password == vm.Password).FirstOrDefault();
                    if (c != null)
                    {
                        HttpContext.Session.SetString(SessionCNIC, c.Cnic);
                        HttpContext.Session.SetString(SessionRole, c.Role);
                        HttpContext.Session.SetInt32(SessionId, c.CustomerId);
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Password Or Email is wrong !");
                    }
                }
                else if (string.Equals(vm.Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy c = db.DeliveryBoy.Where(i => i.Email == vm.Email && i.Password == vm.Password).FirstOrDefault();
                    if (c != null)
                    {
                        HttpContext.Session.SetString(SessionCNIC, c.Cnic);
                        HttpContext.Session.SetString(SessionRole, c.Role);
                        HttpContext.Session.SetInt32(SessionId, c.DeliveryBoyId);
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Password Or Email is wrong !");
                    }
                }
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                if (string.Equals(vm.Role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = new Chef
                    {
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Category = "3 star",
                        Role = vm.Role,
                        Status = "Active",
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email,
                        Password = vm.Password,
                        PhoneNo = vm.PhoneNo,
                        City = vm.City,
                        Area = vm.Area,
                        Street = vm.Street,
                        Cnic = vm.Cnic
                    };

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            string ourftp = env.WebRootPath + "/Chefs/";
                            var directoryinfo = new DirectoryInfo(ourftp);
                            if (!Directory.Exists(ourftp))
                            {
                                Directory.CreateDirectory(ourftp);

                            }
                            if (directoryinfo.Exists)
                            {
                                directoryinfo.CreateSubdirectory("" + c.Cnic);
                            }

                            db.Chef.Add(c);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Cannot add your details at this moment");
                            tr.Rollback();
                        }
                    }
                    return RedirectToAction("Login","Home");
                }
                else if (string.Equals(vm.Role.Trim(), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = new DeliveryBoy
                    {
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Status = "Active",
                        Role = "DBoy",
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email,
                        Password = vm.Password,
                        PhoneNo = vm.PhoneNo,
                        City = vm.City,
                        Area = vm.Area,
                        Street = vm.Street,
                        Cnic = vm.Cnic
                    };

                    using (var tr = db.Database.BeginTransaction())
                    {
                        string ourftp = env.WebRootPath + "/DBoy/";
                        var directoryinfo = new DirectoryInfo(ourftp);
                        if (!Directory.Exists(ourftp))
                        {
                            Directory.CreateDirectory(ourftp);
                        }
                        if (directoryinfo.Exists)
                        {
                            directoryinfo.CreateSubdirectory("" + d.Cnic);
                        }

                        try
                        {
                            db.DeliveryBoy.Add(d);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Cannot add your details at this moment");
                            tr.Rollback();
                        }
                    }
                    return RedirectToAction("Login", "Home");
                }
                else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer cu = new Customer
                    {
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Role = vm.Role,
                        Status = "Active",
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email,
                        Password = vm.Password,
                        PhoneNo = vm.PhoneNo,
                        City = vm.City,
                        Area = vm.Area,
                        Street = vm.Street,
                        Cnic = vm.Cnic
                    };

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            string ourftp = env.WebRootPath + "/Customerss/";
                            var directoryinfo = new DirectoryInfo(ourftp);
                            if (!Directory.Exists(ourftp))
                            {
                                Directory.CreateDirectory(ourftp);

                            }
                            if (directoryinfo.Exists)
                            {
                                directoryinfo.CreateSubdirectory("" + cu.Cnic);
                            }

                            db.Customer.Add(cu);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Cannot add your details at this moment");
                            tr.Rollback();
                        }
                    }
                }
                return RedirectToAction("Login", "Home");
            }
            return View(vm);
        }

        
        [Route("Home/ModifyDetails/{role?}/{id?}")]
        public IActionResult ModifyDetails(string role,int id)
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = db.Chef.Where(i => i.ChefId == id).FirstOrDefault();
                    return View(new RegisterViewModel
                    {
                        Id = c.ChefId,
                        Role = c.Role,
                        Status = c.Status,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Gender = c.Gender,
                        Dob = c.Dob,
                        Email = c.Email,
                        Password = c.Password,
                        PhoneNo = c.PhoneNo,
                        City = c.City,
                        Area = c.Area,
                        Street = c.Street,
                        Cnic = c.Cnic
                    });
                }
                else if (string.Equals(role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == id).FirstOrDefault();
                    return View(new RegisterViewModel
                    {
                        Id = c.CustomerId,
                        Role = c.Role,
                        Status = c.Status,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Gender = c.Gender,
                        Dob = c.Dob,
                        Email = c.Email,
                        Password = c.Password,
                        PhoneNo = c.PhoneNo,
                        City = c.City,
                        Area = c.Area,
                        Street = c.Street,
                        Cnic = c.Cnic
                    });
                }
                else if (string.Equals(role.Trim(), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = db.DeliveryBoy.Where(i => i.DeliveryBoyId == id).FirstOrDefault();
                    return View(new RegisterViewModel
                    {
                        Id = d.DeliveryBoyId,
                        Role = d.Role,
                        Status = d.Status,
                        FirstName = d.FirstName,
                        LastName = d.LastName,
                        Gender = d.Gender,
                        Dob = d.Dob,
                        Email = d.Email,
                        Password = d.Password,
                        PhoneNo = d.PhoneNo,
                        City = d.City,
                        Area = d.Area,
                        Street = d.Street,
                        Cnic = d.Cnic
                    });
                }
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
            
        }

        [HttpPost]
        public IActionResult Update(RegisterViewModel vm)
        {
            //if (ModelState.IsValid)
            //{
                if (string.Equals(vm.Role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    //Chef c = db.Chef.Where(i => i.ChefId == vm.Id).FirstOrDefault();
                    //c.FirstName = vm.FirstName;
                    //c.LastName = vm.LastName;
                    //c.Gender = vm.Gender;
                    //c.Dob = vm.Dob;
                    //c.Email = vm.Email;
                    //c.ModifiedDate = DateTime.Now;
                    //c.PhoneNo = vm.PhoneNo;
                    //c.City = vm.City;
                    //c.Area = vm.Area;
                    //c.Street = vm.Street;
                    //c.Status = vm.Status;
                    Chef c = new Chef {
                        ChefId = vm.Id,
                        FirstName = vm.FirstName,
                        LastName = vm.LastName,
                        Gender = vm.Gender,
                        Dob = vm.Dob,
                        Email = vm.Email,
                        ModifiedDate = DateTime.Now,
                        PhoneNo = vm.PhoneNo,
                        City = vm.City,
                        Area = vm.Area,
                        Status = vm.Status,
                        Street = vm.Street
                    };


                    //using (var tr = db.Database.BeginTransaction())
                    //{
                    //    try
                    //    {
                    //db.Chef.Update(c);
                    //db.SaveChanges();
                    //tr.Commit();
                    //if (ModelState.IsValid)
                    //{
                        db.Entry(c).State = EntityState.Modified;
                        db.Entry(c).Property(x => x.CreatedDate).IsModified = false; //Add fields which you don't want to modify
                        db.Entry(c).Property(x => x.Cnic).IsModified = false;
                        db.Entry(c).Property(x => x.Role).IsModified = false;
                        db.Entry(c).Property(x => x.Category).IsModified = false;
                        db.Entry(c).Property(x => x.Rating).IsModified = false;
                    db.Entry(c).Property(x => x.Password).IsModified = false;
                    db.SaveChanges();
                   // }
                            //db.Chef.Attach(c);
                            //var entry = db.Entry(c);
                            //entry.State = EntityState.Modified;
                            //db.SaveChanges();
                            //tr.Commit();
                    //    }
                    //    catch
                    //    {
                    //        tr.Rollback();
                    //    }
                    //}
                }
                else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == vm.Id).FirstOrDefault();
                    c.FirstName = vm.FirstName;
                    c.LastName = vm.LastName;
                    c.Gender = vm.Gender;
                    c.Dob = vm.Dob;
                    c.Email = vm.Email;
                    c.ModifiedDate = DateTime.Now;
                    c.PhoneNo = vm.PhoneNo;
                    c.City = vm.City;
                    c.Area = vm.Area;
                    c.Status = vm.Street;
                    c.Status = vm.Status;

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.Customer.Update(c);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy c = db.DeliveryBoy.Where(i => i.DeliveryBoyId == vm.Id).FirstOrDefault();
                    c.FirstName = vm.FirstName;
                    c.LastName = vm.LastName;
                    c.Gender = vm.Gender;
                    c.Dob = vm.Dob;
                    c.Email = vm.Email;
                    c.ModifiedDate = DateTime.Now;
                    c.PhoneNo = vm.PhoneNo;
                    c.City = vm.City;
                    c.Area = vm.Area;
                    c.Status = vm.Street;
                    c.Status = vm.Status;

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            db.DeliveryBoy.Update(c);
                            db.SaveChanges();
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                //}
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (string.Equals(vm.Role.Trim(), "chef", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(vm.Choice.Trim(), "Phone", StringComparison.OrdinalIgnoreCase))
                {

                }
                else if(string.Equals(vm.Choice.Trim(), "Email", StringComparison.OrdinalIgnoreCase))
                {

                }

            }
            else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(vm.Choice.Trim(), "Phone", StringComparison.OrdinalIgnoreCase))
                {

                }
                else if (string.Equals(vm.Choice.Trim(), "Email", StringComparison.OrdinalIgnoreCase))
                {

                }
            }
            else if (string.Equals(vm.Role.Trim(), "deliveryboy", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(vm.Choice.Trim(), "Phone", StringComparison.OrdinalIgnoreCase))
                {

                }
                else if (string.Equals(vm.Choice.Trim(), "Email", StringComparison.OrdinalIgnoreCase))
                {

                }
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ComingSoon()
        {
            return View();
        }
        
        public IActionResult Privacy_Policy()
        {
            return View();
        }

        public IActionResult Page404()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
