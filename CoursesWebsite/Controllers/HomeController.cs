using CoursesWebsite.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CoursesWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceData<Category> _serviceData;

        public HomeController(ILogger<HomeController> logger, IServiceData<Category> serviceData)
        {
            _logger = logger;
            _serviceData = serviceData;
        }

        public IActionResult Index()
        {
            var categories = _serviceData.GetItems();
            return View(categories);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
