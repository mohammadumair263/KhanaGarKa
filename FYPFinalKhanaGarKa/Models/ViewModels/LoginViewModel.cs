using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class LoginViewModel
    {

        public string Role { get; set; }

        [Required(ErrorMessage = "Please Enter CNIC")]
        [MaxLength(13, ErrorMessage = "Enter CNIC like XXXXXXXXXXXXX")]
        [MinLength(13, ErrorMessage = "Enter CNIC like XXXXXXXXXXXXX")]
        [RegularExpression("[0-9]+", ErrorMessage = "Your CNIC can only contain numbers")]
        public string Cnic { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
