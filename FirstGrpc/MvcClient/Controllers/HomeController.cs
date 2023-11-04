using Microsoft.AspNetCore.Mvc;
using MvcClient.Models;
using System.Diagnostics;
using Basics;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly FirstServiceDefinition.FirstServiceDefinitionClient _client;
        private readonly ILogger<HomeController> _logger;

        public HomeController(FirstServiceDefinition.FirstServiceDefinitionClient client, ILogger<HomeController> logger)
        {
            _client = client;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var firstCall = _client.Unary(new Request { Content = "Hello from MVC!" });
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