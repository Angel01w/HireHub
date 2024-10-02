using HirehubWeb.Interfaces;
using HirehubWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HirehubWeb.Controllers
{
    public class CapacitacionesController : Controller
    {
        private readonly IBaseRepository<Training> _capacitacionessrepositorio;

        public CapacitacionesController(IBaseRepository<Training> capacitacionessrepositorio)
        {
            _capacitacionessrepositorio = capacitacionessrepositorio;
        }

        // GET: CompetenciasController
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Data(Competency competency)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            // Obtener los datos desde el repositorio
            var result = await _capacitacionessrepositorio.GetAll();

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
                    (isNumericSearch && u.TrainingID == searchId) ||    // Buscar por ID si es numérico
                    u.Description.ToLower().Contains(searchValue) ||        // Buscar por Descripción
                    u.Institution.ToLower().Contains(searchValue)                // Buscar por Intistucion
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
        public async Task<JsonResult> GuardarAsync(Training training)
        {
            try
            {
                // Asignar el estado correctamente

                Result<bool> result;

                if (training.TrainingID == 0)
                {
                    // Si es un nuevo registro
                    result = await _capacitacionessrepositorio.Add(training);
                }
                else
                {
                    // Si es una actualización de un registro existente
                    result = await _capacitacionessrepositorio.Update(training);
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

        [HttpPost]
        public async Task<JsonResult> Eliminar([FromBody] Training training)
        {
            try
            {
                if (training.TrainingID > 0)
                {
                    var result = await _capacitacionessrepositorio.Delete(training);

                    // Verificar si la eliminación fue exitosa
                    if (result.Success)
                    {
                        return Json(new { resultado = true });
                    }
                    else
                    {
                        if (result.ErrorMessage == "Error al Eliminar: An error occurred while saving the entity changes. See the inner exception for details.")
                        {
                            result.ErrorMessage = "Debe reasignar los Capacitaciones o eliminarlos antes de eliminar la cafeteria.";
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


    }
}
