using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ElectroEshop.Models
{
    public class ProductViewModel
    {
        public List<string> colors;
        public List<Photo> photos;

        public ProductViewModel()
        {
            colors = new List<string>();
            photos = new List<Photo>();
        }
        public PagedList.IPagedList<ElectroEshop.Reviewer> reviewers { get; set; }
        public List<Product> relatedProducts { get; set; }
        public string category_name { get; set; }
        public int reviewsCount { get; set; }
        public Product product { get; set; }
        public CountRating countRating { get; set; }
    }
    public class CountRating
    {
        public decimal avgRating { get; set; }
        public int[] stars { get; set; }
        public int[] percentStars { get; set; }
    }
}