using HirehubWeb.Interfaces;
using HirehubWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HirehubWeb.Controllers
{
    public class CandidatosController : Controller
    {
        private readonly IBaseRepository<Candidate> _candidatosrepositorio;

        public CandidatosController(IBaseRepository<Candidate> capacitacionessrepositorio)
        {
            _candidatosrepositorio = capacitacionessrepositorio;
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
            var result = await _candidatosrepositorio.GetAll();

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
                    (isNumericSearch && u.CandidateID == searchId) ||    // Buscar por ID si es numérico
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
        public async Task<JsonResult> GuardarAsync(Candidate candidate)
        {
            try
            {
                // Asignar el estado correctamente

                Result<bool> result;

                if (candidate.CandidateID == 0)
                {
                    // Si es un nuevo registro
                    result = await _candidatosrepositorio.Add(candidate);
                }
                else
                {
                    // Si es una actualización de un registro existente
                    result = await _candidatosrepositorio.Update(candidate);
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
        public async Task<JsonResult> Eliminar([FromBody] Candidate candidate)
        {
            try
            {
                if (candidate.CandidateID > 0)
                {
                    var result = await _candidatosrepositorio.Delete(candidate);

                    // Verificar si la eliminación fue exitosa
                    if (result.Success)
                    {
                        return Json(new { resultado = true });
                    }
                    else
                    {
                        if (result.ErrorMessage == "Error al Eliminar: An error occurred while saving the entity changes. See the inner exception for details.")
                        {
                            result.ErrorMessage = "Debe reasignar los Candidato o eliminarlos antes de eliminar la cafeteria.";
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
