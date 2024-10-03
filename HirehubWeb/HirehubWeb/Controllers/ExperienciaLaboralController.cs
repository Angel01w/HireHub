using HirehubWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HirehubWeb.Controllers
{
    public class ExperienciaLaboralController : Controller
    {
        private readonly ApplicationDbContext  _context;

        public ExperienciaLaboralController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción para cargar la vista
        public IActionResult Index()
        {
            return View();
        }

        // Acción para devolver los datos en formato JSON
        [HttpGet]
        public async Task<IActionResult> GetWorkExperiencesJson()
        {
            var experiences = await _context.WorkExperience.ToListAsync();
            return Json(experiences);
        }
    }
}

