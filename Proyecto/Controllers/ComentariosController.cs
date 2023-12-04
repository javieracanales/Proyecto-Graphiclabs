using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.Controllers
{
    public class ComentariosController : Controller
    {

        ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext();
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AgregarComentario(Comentario model, byte? id)
        {
            var idUser = User.Claims.Where(s => s.Type == "Id").Select(s => Convert.ToByte(s.Value)).FirstOrDefault();

                if (ModelState.IsValid)
                {
                    var modeloTabla = new Comentario();
                    modeloTabla.IdUsuario = model.IdUsuario = idUser;
                    modeloTabla.IdProducto = id;
                    modeloTabla.Comentario1 = model.Comentario1;
                    modeloTabla.Fecha = model.Fecha = DateTime.Now;
                    db.Comentarios.Add(modeloTabla);
                    db.SaveChanges();
                }

            return RedirectToAction("IndexComentario", "Layout2", new { id });
        }
        
    }
}

