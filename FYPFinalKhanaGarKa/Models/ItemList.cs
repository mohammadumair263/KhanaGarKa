using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models
{
    public class ItemList
    {
        public int[] prices { get; set; }
        public string[] names { get; set; }
        public int quantities { get; set; }
        public int subtotal { get; set; }
        public int total { get; set; }
    }
}
