using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYPFinalKhanaGarKa.Models
{
    public class ItemGroup
    {
        public int Subtotal { get; set; }

        public int Total { get; set; }

        public string OrderType { get; set; }

        public IList<CartItems> Items { get; set; }
    }
}
