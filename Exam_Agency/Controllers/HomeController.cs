using Business.Services.Abstracts;
using Exam_Agency.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Exam_Agency.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPortfolioServices _portfolioServices;
        public HomeController(ILogger<HomeController> logger, IPortfolioServices portfolioServices)
        {
            _logger = logger;
            _portfolioServices = portfolioServices;
        }

        public IActionResult Index()
        {
            var list = _portfolioServices.GetAllPortfolio();
            return View(list);
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