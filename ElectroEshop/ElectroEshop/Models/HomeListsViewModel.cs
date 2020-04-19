using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ElectroEshop.Models
{
    public class HomeListsViewModel
    {
        public List<Product> newLaptopsList;
        public List<Product> newSmartphonesList;
        public List<Product> newCamerasList;
        public List<Product> newAccessoriesList;

        public List<Product> tendLaptopsList;
        public List<Product> tendSmartphonesList;
        public List<Product> tendCamerasList;
        public List<Product> tendAccessoriesList;

        public List<Product> topSellingList;

        public HomeListsViewModel()
        {
            newLaptopsList = new List<Product>();
            newSmartphonesList = new List<Product>();
            newCamerasList = new List<Product>();
            newAccessoriesList = new List<Product>();

            tendLaptopsList = new List<Product>();
            tendSmartphonesList = new List<Product>();
            tendCamerasList = new List<Product>();
            tendAccessoriesList = new List<Product>();

            topSellingList = new List<Product>();
        }

    }
    public class blankViewModel
    {
        [Required]
        [Display(Name = "As su dviems")]
        public bool RememberMee { get; set; }

        [Required]
        [Display(Name = "As cia su viena")]
        public bool RememberMe { get; set; }

        [Display(Name = "Name")]
        public bool boolSomething { get; set; }
    }
}