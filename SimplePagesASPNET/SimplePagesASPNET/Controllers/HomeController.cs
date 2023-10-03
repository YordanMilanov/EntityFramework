using Microsoft.AspNetCore.Mvc;
using SimplePagesASPNET.ViewModels;
using System.Diagnostics;

namespace SimplePagesASPNET.Controllers
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "This is an ASP.NET Core MVC App.";
            return View();
        }

        public IActionResult Numbers()
        {
            List<int> numbers = new List<int>();
            for (int i = 0;i <= 50; i++)
            {
                numbers.Add(i);
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult NumbersToN()
        {
            ViewData["count"] = -1;
            return this.View();
        }
        [HttpPost]
        public IActionResult NumbersToN(int count = -1)
        {
            ViewData["count"] = count;
            return this.View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
