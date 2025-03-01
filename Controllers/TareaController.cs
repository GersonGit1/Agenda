using Agenda.Filters;
using Agenda.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Security.Claims;

namespace Agenda.Controllers
{
    [ServiceFilter(typeof(SessionValidationFilter))] //protegemos el controlador de manera que no permita solicitudes sin que el
    //usuario esté autenticado. esa lógica la definimos en SessionValidationFilter
    public class TareaController : Controller
    {
        // GET: TareaController
        public ActionResult Index()
        {
            return View("VerTareas");
        }

        public JsonResult Tareas()
        {
            string? IdUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int Id = Convert.ToInt32(IdUsuario);
            MantenimientoTarea Mtarea = new();
            var tareas = Mtarea.MostrarTareas(Id);
            return tareas;
        }
        // GET: TareaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TareaController/Create
        //[ServiceFilter(typeof(SessionValidationFilter))]
        public ActionResult Create()
        {
            return View("Crear");
        }

        // POST: TareaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                MantenimientoTarea Mtarea = new();
                Tarea tarea = new();
                string? IdUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                int Id = Convert.ToInt32(IdUsuario);

                tarea.Titulo = collection["titulo"];
                tarea.Descripcion = collection["descripcion"];
                tarea.fecha = DateTime.Parse(collection["fecha"]);
                tarea.Fin = DateTime.Parse(collection["Fin"]);
                tarea.IdUsuario = Id;
                //tarea.Notificar = DateTime.Parse(collection["Notificacion"]);

                Mtarea.AñadirTarea(tarea);
                return RedirectToAction("Index");
            }
            catch 
            {
                return View("Crear");
            }
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public ActionResult ActualizarFechas([FromBody] UpdateTarea evento)
        {
            try
            {
                if (evento == null)
                {
                    throw new Exception("El evento recibido es null");
                }

                MantenimientoTarea Mtarea = new();
                Tarea tarea = new()
                {
                    fecha = Convert.ToDateTime(evento.Start),
                    Fin = Convert.ToDateTime(evento.End),
                    Id = evento.Id
                };
               

                Mtarea.ActualizarTarea(tarea);
                return Ok(new { message = "El evento se ha recibido correctamente" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errorcito: {ex.Message}");
                return StatusCode(500, new {error = ex.Message});
            }
        }

        // GET: TareaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TareaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TareaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TareaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
