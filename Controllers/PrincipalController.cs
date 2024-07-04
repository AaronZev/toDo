using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using toDoList.Custom;
using toDoList.Models;
using toDoList.Models.toDoList.Models;

namespace toDoList.Controllers
{
    public class PrincipalController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly Utilities _utilities;
        public IActionResult Index()
        {
            return View();
        }

        public PrincipalController(ApplicationDbContext context, Utilities utilities)
        {
            _context = context;
            _utilities = utilities;
        }

        public IActionResult Task()
        {
            return View();
        }
        
        [HttpPost("TaskRegister")]
        public async Task<IActionResult> TaskRegister(W_TASK task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.W_TASK.Add(task);
                    await _context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se registró correctamente la tarea.", title = "Éxito" });
                }

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Datos inválidos.", title = "Error" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error al intentar grabar los datos.", title = "Error" });
            }

        }

        [HttpGet("GetTask")]
        public async Task<IActionResult> GetTask(DateTime? fecha,int? prioridad, int? estado, int usuario, string? status)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var query = _context.W_TASK.AsQueryable();

                    if (fecha != null)
                    {
                        query = query.Where(x => x.DATE_CONFIG.Date == fecha);
                    }

                    if (prioridad != null)
                    {
                        query = query.Where(x => x.PRIORITY == prioridad);
                    }

                    if (estado != null)
                    {
                        query = query.Where(x => x.STATE == estado);
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        query = query.Where(x => x.STATUS == status);
                    }

                    var tasks = await query.Where(x => x.C_ID_USER == usuario).ToListAsync();



                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, response = tasks, title = "Éxito" });
                }

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Datos inválidos.", title = "Error" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error al intentar realizar la operación.", title = "Error" });
            }

        }

        [HttpGet("GetTaskId")]
        public async Task<IActionResult> GetTask(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var data = await _context.W_TASK.FindAsync(id);

                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, response = data, title = "Éxito" });
                }

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Datos inválidos.", title = "Error" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error al intentar realizar la operación.", title = "Error" });
            }

        }

        [HttpPut("TaskUpdate")]
        public async Task<IActionResult> TaskUpdate(W_TASK task)
        {
            try
            {
                var existingTask = await _context.W_TASK.FindAsync(task.C_ID_TASK);

                if (existingTask == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, message = "Tarea no encontrada.", title = "Error" });
                }

                existingTask.TITLE = task.TITLE;
                existingTask.DATE_CONFIG = task.DATE_CONFIG;
                existingTask.PRIORITY = task.PRIORITY;
                existingTask.STATE = task.STATE;
                existingTask.DESCRIPTION = task.DESCRIPTION;
                existingTask.STATUS = task.STATUS;
                existingTask.C_ID_USER = task.C_ID_USER;

                _context.W_TASK.Update(existingTask);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Tarea actualizada correctamente.", title = "Éxito" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = "Ocurrió un error al intentar actualizar la tarea.", title = "Error" });
            }
        }

        [HttpPut("TaskDelete")]
        public async Task<IActionResult> TaskDelete(int id, string estado)
        {
            try
            {
                var existingTask = await _context.W_TASK.FindAsync(id);

                if (existingTask == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { isSuccess = false, message = "Tarea no encontrada.", title = "Error" });
                }

                existingTask.STATUS = estado;


                _context.W_TASK.Update(existingTask);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Tarea actualizada correctamente.", title = "Éxito" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { isSuccess = false, message = "Ocurrió un error al intentar actualizar la tarea.", title = "Error" });
            }
        }



    }
}
