using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectroEshop.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
        [Route("Error")]
        [AllowAnonymous]
        public ActionResult Error()
        {
            ViewBag.errorTitle = "Error";
            return View("Error");
        }
        public ActionResult NotFound(string aspxerrorpath)
        {
            ViewBag.errorTitle = "ERROR 404";
            ViewBag.errorMessage = "Didn't found path: " + aspxerrorpath;
            return View("Error");
        }
        public ActionResult ProductNotFound(int id)
        {
            ViewBag.errorTitle = "Product Not Found";
            ViewBag.errorMessage = "Didn't found product with id " + id;
            return View("Error");
        }
    }
}