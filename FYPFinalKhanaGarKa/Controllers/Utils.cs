using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Controllers
{
    public class Utils
    {
        private const string SessionCNIC = "_UserC";
        private const string SessionRole = "_UserR";
        private const string SessionId = "_UserI";

        //Helper Methods
        public static string GetUniqueName(string FileName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmm") +
                        Path.GetExtension(FileName);
        }

        private static bool UploadImage(IHostingEnvironment env, IFormFile Image, string path)
        {
            if (Image != null && Image.Length > 0 && Image.Length < 1000000)
            {
                string ext = Path.GetExtension(Image.FileName);

                if (string.Equals(ext, ".jpeg", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".png", StringComparison.OrdinalIgnoreCase) ||
                   string.Equals(ext, ".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    var filePath = env.WebRootPath + path + "/" + GetUniqueName(Image.FileName);
                    Image.CopyTo(new FileStream(filePath.Trim(), FileMode.Create));
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return true;
        }

        public static string UploadImageR(IHostingEnvironment env, string Dir, IFormFile Image)
        {
            string dir = env.WebRootPath + Dir;

            if (!Directory.Exists(dir.Trim()))
            {
                Directory.CreateDirectory(dir.Trim());

                bool isUploaded = UploadImage(env, Image, Dir.Trim());
                if (isUploaded)
                {
                    return Dir.Trim() + "/" + GetUniqueName(Image.FileName);
                }
                else
                {
                    // image is not uploaded do somthing
                    return null;
                }
            }
            else
            {
                bool isUploaded = UploadImage(env, Image, Dir.Trim());
                if (isUploaded == true)
                {
                    return Dir.Trim() + "/" + GetUniqueName(Image.FileName);
                }
                else
                {
                    // image is not uploaded do somthing
                    return null;
                }
            }
        }

        public static string UploadImageU(IHostingEnvironment env, string Dir, IFormFile Image, string ImgUrl)
        {
            string dir = env.WebRootPath + Dir;

            if (!Directory.Exists(dir.Trim()))
            {
                Directory.CreateDirectory(dir.Trim());

                bool isDeleted = DeleteImage(env.WebRootPath + ImgUrl.Trim());
                if (isDeleted == true)
                {
                    bool isUploaded = UploadImage(env, Image, Dir.Trim());
                    if (isUploaded == true)
                    {
                        return Dir.Trim()+ "/" + GetUniqueName(Image.FileName);
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
                bool isDeleted = DeleteImage(env.WebRootPath + ImgUrl);
                if (isDeleted)
                {
                    bool isUploaded = UploadImage(env, Image, Dir.Trim());
                    if (isUploaded == true)
                    {
                        return Dir.Trim()+ "/" + GetUniqueName(Image.FileName);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public static void GreetingsEmail(string mailid, string Fname, string Lname)
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
