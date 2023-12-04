using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.Controllers
{
    public class FavoritosController : Controller
    {
        ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext();

        [HttpPost]
        public IActionResult Agregar(byte? idPro)
        {
            var id = User.Claims.Where(s => s.Type == "Id").Select(s => Convert.ToByte(s.Value)).FirstOrDefault();
            var model = db.Favoritos.Where(f=> f.IdProducto == idPro && f.IdUsuario == id).FirstOrDefault();

            if (model!= null)
            {
                db.Favoritos.Remove(model);
                db.SaveChanges();
                return Redirect("~/Layout2/Index");
            }
            else
            {
                var modeloTabla = new Favorito();
                modeloTabla.IdUsuario = id;
                modeloTabla.IdProducto = idPro;
                modeloTabla.Fecha = DateTime.Now;
                db.Favoritos.Add(modeloTabla);
                db.SaveChanges();
                return Redirect("~/Layout2/Index");
            }
        }
    }
}

