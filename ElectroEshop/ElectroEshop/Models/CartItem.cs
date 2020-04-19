using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroEshop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Photo { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int? Discount { get; set; }
        public int Quantity { get; set; }
    }
    public class WishCount
    {
        public int Count { get; set; }
    }
}