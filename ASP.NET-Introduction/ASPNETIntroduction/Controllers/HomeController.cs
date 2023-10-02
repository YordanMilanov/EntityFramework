using System.Diagnostics;
using ASPNETIntroduction.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETIntroduction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //ViewData -> Dictionary
            //ViewBag -> object

            
            ViewData["MyData"] = "I am inserting data using ViewData form controller in view!";
           
            //this is dynamic object -> means we can create a property by our needs 
            // in this case we just write Result and it makes it as property
            ViewBag.Result = "I am inserting data from the controller using ViewBag in view!";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}