using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime Dob { get; set; }
        public string Cnic { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Category { get; set; }
        public string Role { get; set; }
        public string ImgUrl { get; set; }
    }
}
