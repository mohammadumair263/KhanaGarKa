﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class DeliveryBoy
    {
        public DeliveryBoy()
        {
            Orders = new HashSet<Orders>();
        }

        public int DeliveryBoyId { get; set; }
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
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ImgUrl { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}
