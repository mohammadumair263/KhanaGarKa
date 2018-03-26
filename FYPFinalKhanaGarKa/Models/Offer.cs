using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FYPFinalKhanaGarKa.Models
{
    public partial class Offer
    {
        public int OfferId { get; set; }

        [Range(0,100,ErrorMessage ="Please Enter discount from 0 to 10")]
        public string Percentage { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [RegularExpression("[0-9]+", ErrorMessage = "Price can only contain numaric value")]
        public int Price { get; set; }

        [Required(ErrorMessage = "OfferName is Required")]
        [MaxLength(50, ErrorMessage = "Length should be not more than 50 charaters")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "OfferName can only contain alphanumaric value")]
        public string OfferName { get; set; }

        [MaxLength(200, ErrorMessage = "Length should be not more than 200 charaters")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Description can only contain alphanumaric value")]
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int ChefId { get; set; }

        public Chef Chef { get; set; }
    }
}
