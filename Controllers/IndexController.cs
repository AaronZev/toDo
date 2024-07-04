using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using toDoList.Custom;
using toDoList.Models;

namespace toDoList.Controllers
{
    public class IndexController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Utilities _utilities;


        public IndexController(ApplicationDbContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        //Consultas de nivel backend
        [HttpPost("Login")]
        public async Task<IActionResult> GetUser(string username, string password)
        {
            try
            {
                var user = await _context.W_USER.FirstOrDefaultAsync(u => u.USERNAME == username && u.PASSWORD == _utilities.encrypt(password));

                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSucess = true, token = _utilities.generateJwt(user), title = "Éxito", message = "Loggeo Exitoso.", data = user });
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSucess = true, token = "", title = "Error", message = "Error en el inicio de sesión." });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false,  title = "Error", message = "Ocurrió un error al intentar grabar los datos." });

            }

        }

        [HttpPost("UserRegister")]
        public async Task<IActionResult> UserRegister(string username, string password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Verifica si el nombre de usuario ya existe
                    var existingUser = await _context.W_USER.FirstOrDefaultAsync(u => u.USERNAME == username);
                    if (existingUser != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "El nombre de usuario ya existe.", title = "Error" });
                    }

                    var passwordEncript = _utilities.encrypt(password);
                    W_USER user = new W_USER
                    {
                        USERNAME = username,
                        PASSWORD = passwordEncript
                    };

                    _context.W_USER.Add(user);
                    await _context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se registró correctamente el usuario.", title = "Éxito" });
                }

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Datos inválidos.", title = "Error" });
            }
            catch (Exception) {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error al intentar grabar los datos." , title = "Error" });
            }
            
        }

    }
}
