using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using HirehubWeb.Models;
using HirehubWeb.Interfaces;

namespace HirehubWeb.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IBaseRepository<Employee> _empleadossrepositorio;


        public EmpleadosController(ApplicationDbContext context, IBaseRepository<Employee> empleadossrepositorio)
        {
            _context = context;
            _empleadossrepositorio = empleadossrepositorio;
        }


        public IActionResult Index2()
        {
            return View();
        }


        
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Data(Employee employee)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            // Obtener los datos desde el repositorio
            var result = await _empleadossrepositorio.GetAll();

            // Verificar si la operación fue exitosa
            if (!result.Success)
            {
                // En caso de error, devolver el mensaje de error a DataTables
                return Json(new
                {
                    draw = draw,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    error = result.ErrorMessage
                });
            }

            var query = result.Data;

            // Aplicar el filtro de búsqueda si es necesario
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchValue = searchValue.ToLower();

                // Verificar si el valor de búsqueda es un número, para buscar por ID
                bool isNumericSearch = int.TryParse(searchValue, out int searchId);

                query = query.Where(u =>
                    (isNumericSearch && u.EmployeeID == searchId) ||    // Buscar por ID si es numérico
                    u.Identification.ToLower().Contains(searchValue) ||        // Buscar por Descripción
                    u.Name.ToLower().Contains(searchValue)                // Buscar por Intistucion
                );
            }

            // Mejorar el conteo: calcular el total de registros filtrados después del filtro
            var totalRecords = query.Count();
            var data = query.Skip(skip).Take(pageSize).ToList(); // Obtener los datos paginados

            return Json(new
            {
                draw = draw,
                recordsFiltered = totalRecords,  // Total de registros filtrados
                recordsTotal = totalRecords,     // Total de registros
                data = data
            });
        }


        [HttpPost]
        public async Task<JsonResult> Eliminar([FromBody] Employee employee)
        {
            try
            {
                if (employee.EmployeeID > 0)
                {
                    var result = await _empleadossrepositorio.Delete(employee);

                    // Verificar si la eliminación fue exitosa
                    if (result.Success)
                    {
                        return Json(new { resultado = true });
                    }
                    else
                    {
                        if (result.ErrorMessage == "Error al Eliminar: An error occurred while saving the entity changes. See the inner exception for details.")
                        {
                            result.ErrorMessage = "Debe reasignar los Idioma o eliminarlos antes de eliminar la cafeteria.";
                        }
                        // Asignar el estado correctamente
                        return Json(new { resultado = false, mensaje = result.ErrorMessage });
                    }
                }
                return Json(new { resultado = false, mensaje = "ID inválido" });
            }
            catch (Exception ex)
            {

                return Json(new { resultado = false, mensaje = ex.Message });
            }
        }



        [HttpPost]
        public async Task<JsonResult> GuardarAsync(Employee employee)
        {
            try
            {
                // Asignar el estado correctamente

                Result<bool> result;

                if (employee.EmployeeID == 0)
                {
                    // Si es un nuevo registro
                    result = await _empleadossrepositorio.Add(employee);
                }
                else
                {
                    // Si es una actualización de un registro existente
                    result = await _empleadossrepositorio.Update(employee);
                }

                // Verificar el resultado
                if (result.Success)
                {
                    return Json(new { resultado = true });
                }
                else
                {
                    return Json(new { resultado = false, mensaje = result.ErrorMessage });
                }
            }
            catch (Exception ex)
            {
                return Json(new { resultado = false, mensaje = ex.Message });
            }
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
