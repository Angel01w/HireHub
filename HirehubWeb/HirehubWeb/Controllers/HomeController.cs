using HirehubWeb.Interfaces;
using HirehubWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HirehubWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBaseRepository<Position> _positionsrepositorio;


        public HomeController(ILogger<HomeController> logger, IBaseRepository<Position> positionsrepositorio)
        {
            _logger = logger;
            _positionsrepositorio = positionsrepositorio;   
        }

        public async Task<IActionResult> Index()
        {
            var lista = await _positionsrepositorio.GetAll();

            return View(lista.Data);
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
