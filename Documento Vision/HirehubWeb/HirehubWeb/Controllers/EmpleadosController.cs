using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using HirehubWeb.Models;

namespace HirehubWeb.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index2()
        {
            return View();
        }


        // Obtener la lista de empleados
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Employees.ToListAsync();
            return Json(empleados);
        }

        // Obtener un empleado por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Json(employee);
        }

        // Crear un nuevo empleado
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.HireDate = DateTime.Now;
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
                return Ok(employee);
            }
            return BadRequest(ModelState);
        }

        // Actualizar un empleado
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee updatedEmployee)
        {
            if (id != updatedEmployee.EmployeeID)
            {
                return BadRequest();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            employee.Name = updatedEmployee.Name;
            employee.Identification = updatedEmployee.Identification;
            employee.Department = updatedEmployee.Department;
            employee.Position = updatedEmployee.Position;
            employee.MonthlySalary = updatedEmployee.MonthlySalary;
            employee.Status = updatedEmployee.Status;

            await _context.SaveChangesAsync();
            return Ok(employee);
        }

        // Eliminar un empleado
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok(employee);
        }
    }
}
