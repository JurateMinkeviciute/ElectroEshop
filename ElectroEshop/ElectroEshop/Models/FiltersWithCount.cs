using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroEshop.Models
{
    public class CategoryWithCount
    {
        public Category category { get; set; }
        public int countCategories { get; set; }
    }
    public class BrandWithCount
    {
        public Brand brand { get; set; }
        public int countBrands { get; set; }
    }
}