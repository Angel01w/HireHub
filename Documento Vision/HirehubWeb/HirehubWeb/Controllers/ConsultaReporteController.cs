using HirehubWeb.Interfaces;
using HirehubWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HirehubWeb.Controllers
{
    public class ConsultaReporteController : Controller
    {
        private readonly IBaseRepository<Candidate> _candidateRepository;
        private readonly ApplicationDbContext _context;

        public ConsultaReporteController(IBaseRepository<Candidate> candidateRepository, ApplicationDbContext context)
        {
            _candidateRepository = candidateRepository;
            _context = context;
        }

        // GET: CompetenciasController
        public ActionResult Index()
        {
            return View();
        }

        // Método para filtrar empleados por rango de fechas
        [HttpGet]
        public async Task<IActionResult> FiltrarPorFechaIngreso(DateTime startDate, DateTime endDate)
        {
            // Filtrar empleados cuyo HireDate esté entre startDate y endDate
            var empleados = await _context.Employees
                .Where(e => e.HireDate >= startDate && e.HireDate <= endDate)
                .ToListAsync();

            // Retornar los resultados en formato JSON (puede ser para mostrar en una tabla en el frontend)
            return Json(empleados);
        }

        // GET: CompetenciasController
        public ActionResult Index2()
        {
            return View();
        }


        // Método para filtrar candidatos por puesto, competencias y capacitaciones
        [HttpGet]
        public async Task<IActionResult> FiltrarCandidatos(string desiredPosition, string competency, string training)
        {
            // Obtener todos los candidatos
            var query = _context.Candidates.AsQueryable();

            // Filtrar por puesto (DesiredPosition)
            if (!string.IsNullOrEmpty(desiredPosition))
            {
                query = query.Where(c => c.DesiredPosition == desiredPosition);
            }

            // Filtrar por competencia (KeyCompetencies)
            if (!string.IsNullOrEmpty(competency))
            {
                query = query.Where(c => c.KeyCompetencies.Contains(competency));
            }

            // Filtrar por capacitación (KeyTrainings)
            if (!string.IsNullOrEmpty(training))
            {
                query = query.Where(c => c.KeyTrainings.Contains(training));
            }

            // Ejecutar la consulta y devolver los resultados
            var candidatosFiltrados = await query.ToListAsync();

            return Json(candidatosFiltrados);
        }
    }
}
