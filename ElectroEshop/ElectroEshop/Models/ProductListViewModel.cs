using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroEshop.Models
{
    public class ProductListViewModel
    {
        public string Category { get; set; }
        public List<CategoryWithCount> categoriesWithCount;
        public List<BrandWithCount> brandsWithCount;
        public List<ProductsForList> topProducts;

        public ProductListViewModel()
        {
            categoriesWithCount = new List<CategoryWithCount>();
            brandsWithCount = new List<BrandWithCount>();
            topProducts = new List<ProductsForList>();
        }
        public PagedList.IPagedList<ProductsForList> products { get; set; }
        public int totalProducts { get; set; }
    }
    public class ProductsForList
    {
        public Product product { get; set; }
        public string category_name { get; set; }
        public bool inWishList { get; set; }
    }
}