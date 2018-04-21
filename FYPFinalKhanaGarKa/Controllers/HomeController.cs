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
using System.Net.Mail;

namespace FYPFinalKhanaGarKa.Controllers
{
    
    
    public class HomeController : Controller
    {
        private const string SessionCNIC = "_UserC";
        private const string SessionRole = "_UserR";
        private const string SessionId = "_UserI";
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
                        AddInfoToSession(c.Cnic, c.Role, c.ChefId);
                        return Redirect("/Chef/ChefAcc/"+c.ChefId);
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
                        AddInfoToSession(c.Cnic, c.Role, c.CustomerId);
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
                        AddInfoToSession(d.Cnic, d.Role, d.DeliveryBoyId);
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
                                c.ImgUrl =  UploadImageR("/Uploads/Chefs/", vm.Cnic, vm.Image);
                            }

                            db.Chef.Add(c);
                            GreetingsEmail(c.Email, c.FirstName, c.LastName);
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
                                d.ImgUrl = UploadImageR("/Uploads/DBoy/", vm.Cnic, vm.Image);
                            }

                            db.DeliveryBoy.Add(d);
                            GreetingsEmail(d.Email, d.FirstName, d.LastName);
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
                                cu.ImgUrl = UploadImageR("/Uploads/Customer/", vm.Cnic, vm.Image);
                            }

                            db.Customer.Add(cu);
                            GreetingsEmail(cu.Email, cu.FirstName, cu.LastName);
                            db.SaveChanges();

                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                        }
                    }
                }
            }
            return View();
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
                        Cnic = c.Cnic,
                        ImgUrl = c.ImgUrl
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
                        Cnic = c.Cnic,
                        ImgUrl = c.ImgUrl
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
           // {
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
                //Chef c = new Chef {
                //    FirstName = vm.FirstName,
                //    LastName = vm.LastName,
                //    Gender = vm.Gender,
                //    Dob = vm.Dob,
                //    Email = vm.Email,
                //    ModifiedDate = DateTime.Now,
                //    PhoneNo = vm.PhoneNo,
                //    City = vm.City,
                //    Area = vm.Area,
                //    Street = vm.Street,
                //    Status = vm.Status
                //};

                using (var tr = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (vm.Image != null && vm.Image.Length > 0)
                            {
                            //c.ImgUrl = UploadImageU("/Uploads/Chefs/", vm.Cnic, vm.Image, vm.ImgUrl);
                            string uploaddir = env.WebRootPath + "/Uploads/";
                            string chefdir = env.WebRootPath + "/Uploads/Chefs";

                            if (!Directory.Exists(uploaddir) ||
                                !Directory.Exists(chefdir))
                            {
                                Directory.CreateDirectory(uploaddir);
                                Directory.CreateDirectory(chefdir);

                                new DirectoryInfo(chefdir).CreateSubdirectory(vm.Cnic);

                                bool isDeleted = DeleteImage(env.WebRootPath + c.ImgUrl);
                                if (isDeleted == true)
                                {
                                    bool isUploaded = UploadImage(vm.Image, "Uploads/Chefs/" + vm.Cnic.Trim());
                                    if (isUploaded == true)
                                    {
                                        c.ImgUrl = "/Uploads/Chefs/" + vm.Cnic.Trim() + "/" + GetUniqueName(vm.Image.FileName);
                                    }
                                    else
                                    {
                                        // image is not uploded do somthing
                                    }
                                }
                                else
                                {
                                    // image is not deleted and uploaded do somthing
                                }
                            }
                            else
                            {
                                new DirectoryInfo(chefdir).CreateSubdirectory(vm.Cnic);
                                bool isDeleted = DeleteImage(env.WebRootPath + c.ImgUrl);
                                if (isDeleted)
                                {
                                    bool isUploaded = UploadImage(vm.Image, "Uploads/Chefs/" + vm.Cnic.Trim());
                                    if (isUploaded == true)
                                    {
                                        c.ImgUrl = "/Uploads/Chefs/" + vm.Cnic.Trim() + "/" + GetUniqueName(vm.Image.FileName);
                                    }
                                    else { }
                                }
                                else
                                {

                                }
                            }
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
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                            c.ImgUrl = UploadImageU("/Uploads/Customer/", vm.Cnic, vm.Image, vm.ImgUrl);
                            //string uploaddir = env.WebRootPath + "/Uploads/";
                            //string cusdir = env.WebRootPath + "/Uploads/Customer";

                            //if (!Directory.Exists(uploaddir) ||
                            //    !Directory.Exists(cusdir))
                            //{
                            //    Directory.CreateDirectory(uploaddir);
                            //    Directory.CreateDirectory(cusdir);

                            //    new DirectoryInfo(cusdir).CreateSubdirectory(vm.Cnic);

                            //    bool isDeleted = DeleteImage(env.WebRootPath + c.ImgUrl);
                            //    if (isDeleted == true)
                            //    {
                            //        bool isUploaded = UploadImage(vm.Image, "Uploads/Customer/" + vm.Cnic.Trim());
                            //        if (isUploaded == true)
                            //        {
                            //            c.ImgUrl = "/Uploads/Customer/" + vm.Cnic.Trim() + "/" + GetUniqueName(vm.Image.FileName);
                            //        }
                            //        else
                            //        {
                            //            // image is not uploaded do somthing
                            //        }
                            //    }
                            //    else
                            //    {
                            //        // image is not deleted and uploaded do somthing
                            //    }
                            //}
                            //else
                            //{
                            //    new DirectoryInfo(cusdir).CreateSubdirectory(vm.Cnic);
                            //    bool isDeleted = DeleteImage(env.WebRootPath + c.ImgUrl);
                            //    if (isDeleted)
                            //    {
                            //        bool isUploaded = UploadImage(vm.Image, "Uploads/Customer/" + vm.Cnic.Trim());
                            //        if (isUploaded == true)
                            //        {
                            //            c.ImgUrl = "/Uploads/Customer/" + vm.Cnic.Trim() + "/" + GetUniqueName(vm.Image.FileName);
                            //        }
                            //        else
                            //        {
                            //            // image is not deleted do somthing
                            //        }
                            //    }
                            //    else
                            //    {
                            //        // image is not deleted and not uploaded do somthing
                            //    }
                            //}
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
                        if (vm.Image != null && vm.Image.Length > 0)
                        {
                            // c.ImgUrl = UploadImageU("/Uploads/DBoy/", vm.Cnic, vm.Image, vm.ImgUrl);
                            string uploaddir = env.WebRootPath + "/Uploads/";
                            string cusdir = env.WebRootPath + "/Uploads/DBoy";

                            if (!Directory.Exists(uploaddir) ||
                                !Directory.Exists(cusdir))
                            {
                                Directory.CreateDirectory(uploaddir);
                                Directory.CreateDirectory(cusdir);

                                new DirectoryInfo(cusdir).CreateSubdirectory(vm.Cnic);

                                bool isDeleted = DeleteImage(env.WebRootPath + c.ImgUrl);
                                if (isDeleted == true)
                                {
                                    bool isUploaded = UploadImage(vm.Image, "Uploads/DBoy/" + vm.Cnic.Trim());
                                    if (isUploaded == true)
                                    {
                                        c.ImgUrl = "/Uploads/DBoy/" + vm.Cnic.Trim() + "/" + GetUniqueName(vm.Image.FileName);
                                    }
                                    else
                                    {
                                        // image is not uploaded do somthing
                                    }
                                }
                                else
                                {
                                    // image is not deleted and uploaded do somthing
                                }
                            }
                            else
                            {
                                new DirectoryInfo(cusdir).CreateSubdirectory(vm.Cnic);
                                bool isDeleted = DeleteImage(env.WebRootPath + c.ImgUrl);
                                if (isDeleted)
                                {
                                    bool isUploaded = UploadImage(vm.Image, "Uploads/DBoy/" + vm.Cnic.Trim());
                                    if (isUploaded == true)
                                    {
                                        c.ImgUrl = "/Uploads/DBoy/" + vm.Cnic.Trim() + "/" + GetUniqueName(vm.Image.FileName);
                                    }
                                    else
                                    {
                                        // image is not deleted do somthing
                                    }
                                }
                                else
                                {
                                    // image is not deleted and not uploaded do somthing
                                }
                            }
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
           // }
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
        private string GetUniqueName(string FileName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmm") +
                        Path.GetExtension(FileName);
        }

        private bool UploadImage(IFormFile Image, string path)
        {
            if (Image != null && Image.Length > 0 && Image.Length < 1000000)
            {
                string ext = Path.GetExtension(Image.FileName);

                if (string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = env.WebRootPath + path +"/"+ GetUniqueName(Image.FileName);
                    Image.CopyTo(new FileStream(filePath.Trim(), FileMode.Create));
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool DeleteImage(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            return true;
        }

        private void AddInfoToSession(string cnic, string role, int id)
        {
            HttpContext.Session.SetString(SessionCNIC, cnic);
            HttpContext.Session.SetString(SessionRole, role);
            HttpContext.Session.SetInt32(SessionId, id);
        }

        private string UploadImageR(string Dir, string cnic, IFormFile Image)
        {
            //string uploaddir = env.WebRootPath + "/Uploads/";
            string chefdir = env.WebRootPath + Dir;

            if (/*!Directory.Exists(uploaddir) ||*/
                !Directory.Exists(chefdir))
            {
                //Directory.CreateDirectory(uploaddir);
                Directory.CreateDirectory(chefdir);

                new DirectoryInfo(chefdir).CreateSubdirectory(cnic);

                bool isUploaded = UploadImage(Image, Dir + cnic);
                if (isUploaded)
                {
                    return Dir + cnic.Trim() + "/" + GetUniqueName(Image.FileName);
                }
                else
                {
                    // image is not uploaded do somthing
                    return null;
                }
            }
            else
            {
                new DirectoryInfo(chefdir).CreateSubdirectory(cnic);
                bool isUploaded = UploadImage(Image, Dir + cnic);
                if (isUploaded == true)
                {
                    return Dir + cnic.Trim() + "/" + GetUniqueName(Image.FileName);
                }
                else
                {
                    // image is not uploaded do somthing
                    return null;
                }
            }
        }

        private string UploadImageU(string Dir, string cnic, IFormFile Image, string ImgUrl)
        {
            string uploaddir = env.WebRootPath + "/Uploads/";
            string dir = env.WebRootPath + Dir;

            if (!Directory.Exists(uploaddir) ||
                !Directory.Exists(dir))
            {
                Directory.CreateDirectory(uploaddir);
                Directory.CreateDirectory(dir);

                new DirectoryInfo(dir).CreateSubdirectory(cnic);

                bool isDeleted = DeleteImage(env.WebRootPath + ImgUrl);
                if (isDeleted == true)
                {
                    bool isUploaded = UploadImage(Image, Dir + cnic.Trim());
                    if (isUploaded == true)
                    {
                        return Dir + cnic.Trim() + "/" + GetUniqueName(Image.FileName);
                    }
                    else
                    {
                        // image is not uploded do somthing
                        return null;
                    }
                }
                else
                {
                    // image is not deleted and uploaded do somthing
                    return null;
                }
            }
            else
            {
                new DirectoryInfo(dir).CreateSubdirectory(cnic);
                bool isDeleted = DeleteImage(env.WebRootPath + ImgUrl);
                if (isDeleted)
                {
                    bool isUploaded = UploadImage(Image, Dir + cnic.Trim());
                    if (isUploaded == true)
                    {
                        return Dir + cnic.Trim() + "/" + GetUniqueName(Image.FileName);
                    }
                    else {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public void GreetingsEmail(string mailid, string Fname, string Lname)
        {
            MailMessage MM = new MailMessage();
            MM.From = new MailAddress("khanagarka@gmail.com");
            MM.To.Add(mailid);
            MM.Subject = ("Welcome to KhanGarKa.com");
            MM.Body = "<h1>Dear " + Fname + " " + Lname + "</h1><br>Thanks for registering on our website.<br><br>----<br>Regards,<br> KhanaGarKa Team";
            MM.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient("smtp.gmail.com", 587);
            sc.Credentials = new System.Net.NetworkCredential("khanagarka@gmail.com", "stm-7063");
            sc.EnableSsl = true;

            sc.Send(MM);
        }
    }
}