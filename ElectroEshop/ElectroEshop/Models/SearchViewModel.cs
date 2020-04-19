using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroEshop.Models
{
    public class SearchViewModel
    {
        public PagedList.IPagedList<ProductsForList> products { get; set; }
        public int totalCount { get; set; }
        public string serachWord { get; set; }
    }
    public class WishViewModel
    {
        public PagedList.IPagedList<ProductsForList> products { get; set; }
        public int totalCount { get; set; }
    }
}