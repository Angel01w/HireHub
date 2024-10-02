using HirehubWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HirehubWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Index(string Correo, string Contrasena)
        {
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contrasena))
            {
                ViewBag.Error = "Campos vacíos";
                return View();
            }

            // Encriptar la contraseña ingresada
            string hashedPassword = ComputeSha256Hash(Contrasena);

            // Buscar el usuario en la base de datos con el correo y la contraseña encriptada
            Users usuario =  _context.Users.Where(x => x.Email == Correo && x.PasswordHash == hashedPassword).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            // Asignar valores a ViewBag
            ViewBag.UserName = usuario.Username;
            ViewBag.UserEmail = usuario.Email;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Name, usuario.Username),
                new Claim(ClaimTypes.Role, usuario.Role),
                // Agrega más claims si es necesario
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // Configurar propiedades adicionales de autenticación si es necesario
            };

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            if (usuario.Role == "Candidate")
            {
                return RedirectPermanent("/Home/Index");

            }
            return RedirectPermanent("/Candidatos/Index");

        }

        // Vista de Registro
        public IActionResult Registrar()
        {
            return View();
        }
        // Acción POST para registrar un nuevo usuario
        [HttpPost]
        public IActionResult Registrar(string Nombre, string Correo, string Contrasena, string ConfirmarContrasena)
        {
            // Verificar que los campos no estén vacíos
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contrasena) || string.IsNullOrEmpty(ConfirmarContrasena))
            {
                return Json(new { resultado = false, message = "Todos los campos son obligatorios" });
            }

            // Verificar que las contraseñas coincidan
            if (Contrasena != ConfirmarContrasena)
            {
                return Json(new { resultado = false, message = "Las contraseñas no coinciden" });
            }

            // Verificar si ya existe un usuario con el mismo correo
            if (_context.Users.Any(x => x.Email == Correo))
            {
                return Json(new { resultado = false, message = "Ya existe un usuario con este correo" });
            }

            // Encriptar la contraseña
            string hashedPassword = ComputeSha256Hash(Contrasena);

            // Crear un nuevo usuario
            Users user = new Users();
            user.Username = Nombre;
            user.Email = Correo;
            user.PasswordHash = hashedPassword;
            user.Role = "Candidate";
            user.CreationDate = DateTime.Now;
            user.UserType= "Candidate";
            // Guardar el usuario en la base de datos
            _context.Users.Add(user);
            _context.SaveChanges();

            // Retornar JSON de éxito
            return Json(new { resultado = true, message = "Usuario registrado exitosamente" });
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
