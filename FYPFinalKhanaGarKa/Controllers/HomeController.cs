﻿using System;
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
using System.Net.Mail;

namespace FYPFinalKhanaGarKa.Controllers
{
    
    
    public class HomeController : Controller
    {
        private const string SessionCNIC = "_UserC";
        private const string SessionRole = "_UserR";
        private const string SessionId = "_UserI";
        private const string SessionImgUrl = "_UserIU";
        private const string SessionName = "_UserN";
        private KhanaGarKaFinalContext db;
        private IHostingEnvironment env = null;

        public HomeController(KhanaGarKaFinalContext _db, IHostingEnvironment _env)
        {
            db = _db;
            env = _env;
        }

        [HttpGet]
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
                        AddInfoToSession(c.Cnic, c.Role, c.ChefId, c.ImgUrl, c.FirstName + " " + c.LastName);
                        return Redirect("/Chef/ChefAcc");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password");
                    }

                }
                else if (string.Equals(vm.Role, "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.Email == vm.Email && i.Password == vm.Password).FirstOrDefault();
                    if (c != null)
                    {
                        AddInfoToSession(c.Cnic, c.Role, c.CustomerId, c.ImgUrl, c.FirstName + " " + c.LastName);
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password");
                    }
                }
                else if (string.Equals(vm.Role, "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = db.DeliveryBoy.Where(i => i.Email == vm.Email && i.Password == vm.Password).FirstOrDefault();
                    if (d != null)
                    {
                        AddInfoToSession(d.Cnic, d.Role, d.DeliveryBoyId, d.ImgUrl, d.FirstName + " " + d.LastName);
                        return Redirect("/Home/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Incorrect Email or Password");
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
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                                c.ImgUrl = Utils.UploadImageR(env, "/Uploads/Chefs/"+vm.Cnic, vm.Image);
                            }

                            db.Chef.Add(c);
                            //GreetingsEmail(c.Email, c.FirstName, c.LastName);
                            db.SaveChanges();

                            tr.Commit();
                        }

                        catch(Exception e)
                        {
                            tr.Rollback();
                        }
                    }
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
                        try
                        {
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                                d.ImgUrl = Utils.UploadImageR(env, "/Uploads/DBoy/"+vm.Cnic, vm.Image);
                            }

                            db.DeliveryBoy.Add(d);
                            //GreetingsEmail(d.Email, d.FirstName, d.LastName);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch(Exception e)
                        {
                            tr.Rollback();
                        }
                    }
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
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                                cu.ImgUrl = Utils.UploadImageR(env, "/Uploads/Customer/"+vm.Cnic, vm.Image);
                            }

                            db.Customer.Add(cu);
                            //GreetingsEmail(cu.Email, cu.FirstName, cu.LastName);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch(Exception e)
                        {
                            tr.Rollback();
                        }
                    }
                }
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult ModifyDetails()
        {
            if (HttpContext.Session.GetString(SessionCNIC) != null &&
            HttpContext.Session.GetString(SessionRole) != null)
            {
                if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "chef", StringComparison.OrdinalIgnoreCase))
                {
                    Chef c = db.Chef.Where(i => i.ChefId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault();
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
                        Cnic = c.Cnic,
                        ImgUrl = c.ImgUrl
                    });
                }
                else if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault();
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
                        Cnic = c.Cnic,
                        ImgUrl = c.ImgUrl
                    });
                }
                else if (string.Equals(HttpContext.Session.GetString(SessionRole).Trim(), "DBoy", StringComparison.OrdinalIgnoreCase))
                {
                    DeliveryBoy d = db.DeliveryBoy.Where(i => i.DeliveryBoyId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault();
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
                        Cnic = d.Cnic,
                        ImgUrl = d.ImgUrl
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
                Chef c = db.Chef.Where(i => i.ChefId == vm.Id).FirstOrDefault();
                c.FirstName = vm.FirstName;
                c.LastName = vm.LastName;
                c.Gender = vm.Gender;
                c.Dob = vm.Dob;
                c.Email = vm.Email;
                c.ModifiedDate = DateTime.Now;
                c.PhoneNo = vm.PhoneNo;
                c.City = vm.City;
                c.Area = vm.Area;
                c.Street = vm.Street;
                c.Status = vm.Status;

                using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                                c.ImgUrl = Utils.UploadImageU(env, "/Uploads/Chefs/"+vm.Cnic, vm.Image, vm.ImgUrl);
                        }
                        db.Chef.Update(c);
                        db.SaveChanges();
                        tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
                else if (string.Equals(vm.Role.Trim(), "customer", StringComparison.OrdinalIgnoreCase))
                {
                    Customer c = db.Customer.Where(i => i.CustomerId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault();
                    c.FirstName = vm.FirstName;
                    c.LastName = vm.LastName;
                    c.Gender = vm.Gender;
                    c.Dob = vm.Dob;
                    c.Email = vm.Email;
                    c.ModifiedDate = DateTime.Now;
                    c.PhoneNo = vm.PhoneNo;
                    c.City = vm.City;
                    c.Area = vm.Area;
                    c.Street = vm.Street;
                    c.Status = vm.Status;

                    using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                            c.ImgUrl = Utils.UploadImageU(env,"/Uploads/Customer/"+vm.Cnic, vm.Image, vm.ImgUrl);
                        }

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
                    DeliveryBoy c = db.DeliveryBoy.Where(i => i.DeliveryBoyId == HttpContext.Session.GetInt32(SessionId)).FirstOrDefault();
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
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                            c.ImgUrl = Utils.UploadImageU(env,"/Uploads/DBoy/"+vm.Cnic, vm.Image, vm.ImgUrl);
                        }
                            db.DeliveryBoy.Update(c);
                            db.SaveChanges();
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
            //}
            return RedirectToAction("ChefAcc","Chef");
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

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ComingSoon()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewsLetter(NewsLetter n)
        {
            if (ModelState.IsValid)
            {
                using(var tr = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.NewsLetter.Add(n);
                        db.SaveChanges();
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                }
            }
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult Privacy_Policy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Page404()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //Helper Methods
        
        private void AddInfoToSession(string cnic, string role, int id, string imgurl, string name)
        {
            HttpContext.Session.SetString(SessionCNIC, cnic);
            HttpContext.Session.SetString(SessionRole, role);
            HttpContext.Session.SetString(SessionImgUrl, imgurl);
            HttpContext.Session.SetString(SessionName, name);
            HttpContext.Session.SetInt32(SessionId, id);
        }
    }
}