using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models.ViewModels
{
    public class RegisterViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Your FirstName is Required")]
        [MaxLength(20, ErrorMessage = "Maximum length of your FirstName should not be more than 20 charaters")]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Your FirstName can only contain letters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Your LastName is Required")]
        [MaxLength(20, ErrorMessage = "Maximum length of your LastName should not be more than 20 charaters")]
        [RegularExpression("[a-zA-z]+", ErrorMessage = "Your LastName can only contain letters")]
        public string LastName { get; set; }

        public string Gender { get; set; }

        [Required(ErrorMessage = "Your Date of Birth is Required")]
        [DataType(DataType.Date)]
        public DateTime Dob { get; set; }

        [Required(ErrorMessage = "Your CNIC is Required")]
        [MaxLength(13, ErrorMessage = "Length of your Cnic should be 13 charaters")]
        [MinLength(13, ErrorMessage = "Length of your Cnic should be 13 charaters")]
        [RegularExpression("[0-9]+", ErrorMessage = "Your CNIC can only contain numbers")]
        public string Cnic { get; set; }

        [Required(ErrorMessage = "Your Phone No. is Required")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Your Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
