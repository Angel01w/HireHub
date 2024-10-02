using HirehubWeb.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        public IActionResult Index(string Correo, string Contrasena)
        {
            if (Contrasena == null || Correo == null)
            {
                ViewBag.Error = "Campos vacios";

            }
            User? usuario = new User();

            usuario = _context.Users.Where(x => x.Email == Correo && x.PasswordHash == Contrasena).FirstOrDefault();

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }
            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Email,usuario.Email),
            // Agrega más claims si es necesario
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                // Configurar propiedades adicionales de autenticación si es necesario
            };

            return RedirectPermanent("/Home/Index");



        }


    }
}


