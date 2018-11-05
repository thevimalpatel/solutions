using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebApi
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public class Trolley
    {
        public Product[] Products { get; set; }
        public Specials[] Specials { get; set; }
        public Quantity[] Quantities { get; set; }
    }

    public class ShopperHistory
    {
        public string CustomerId { get; set; }
        public Product[] Products { get; set; }
    }

    public class Specials
    {
        public Quantity[] Quantities { get; set; }
        public int Total { get; set; }
    }

    public class Quantity
    {
        public string Name { get; set; }
        public int ProductQuantity { get; set; }
    }

}

