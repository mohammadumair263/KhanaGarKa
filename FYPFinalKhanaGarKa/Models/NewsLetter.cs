using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class NewsLetter
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Your Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
