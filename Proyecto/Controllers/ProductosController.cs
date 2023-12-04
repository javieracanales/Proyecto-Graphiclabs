using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto.Helpers;
using Proyecto.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.Controllers
{
    public class ProductosController : Controller
    {

        ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext();

        public IActionResult IndexComentario(byte? id)
        {
                ViewData["Comentarios"] = (from c in db.Comentarios
                                           join
                                               u in db.Usuarios on c.IdUsuario equals u.Id
                                           join p in db.Productos on c.IdProducto equals p.Id
                                           where p.Id == id
                                           select new Comentario
                                           {
                                               IdUsuarioNavigation = u,
                                               IdProductoNavigation = p,
                                               Comentario1 = c.Comentario1,
                                               Fecha = c.Fecha
                                           }).ToList();
            //var model = (from c in db.Comentarios
            //             join
            //                 u in db.Usuarios on c.IdUsuario equals u.Id
            //             join p in db.Productos on c.IdProducto equals p.Id
            //             where p.Id == id
            //             select new Comentario
            //             {
            //                 IdUsuarioNavigation = u,
            //                 IdProductoNavigation = p,
            //                 Comentario1 = c.Comentario1,
            //                 Fecha = c.Fecha
            //             }).FirstOrDefault();

            var model = db.Comentarios.Include(c => c.IdUsuarioNavigation).Include(c => c.IdProductoNavigation).Where(c => c.IdProductoNavigation.Id == id).FirstOrDefault();

                if (model == null)
                {
                    return RedirectToAction("Index", "Productos", new { id });
                }

                return View(model);
        }

        public ActionResult Index(byte? id)
        {
            var model = (from a in db.Productos
                         join
                          u in db.Usuarios on
                          a.IdUsuario equals u.Id
                         join
                             c in db.Categoria on
                             a.IdCategoria equals c.Id
                         where a.Id == id
                         select new Producto
                         {
                             Id = a.Id,
                             IdUsuarioNavigation = u,
                             IdCategoriaNavigation = c,
                             Titulo = a.Titulo,
                             Descripcion = a.Descripcion,
                             Precio = a.Precio,
                             Imagen = a.Imagen,
                         }).FirstOrDefault();

            return View(model);
        }

        public ActionResult Agregar(int? id)
        {
            if (id == null)
            {
                ViewBag.idCategoria = new SelectList(db.Categoria, "id", "nombre");
            }
            else
            {
                ViewBag.idCategoria = new SelectList(db.Categoria.Where(c => c.Id == id), "id", "nombre");
            }

            return View();
        }

        public ActionResult AgregarProducto(int? id)
        {
            return Redirect("~/Home/index");
        }

        public ActionResult Arte()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).Where(p => p.IdCategoriaNavigation.Id == 2).ToList();

            return View(lista);
        }

        public ActionResult Fotografia()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).Where(p => p.IdCategoriaNavigation.Id == 12).ToList();

            return View(lista);
        }

        public ActionResult Wallpaper()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).Where(p => p.IdCategoriaNavigation.Id == 1).ToList();

            return View(lista);
        }

        public ActionResult MenorPrecio()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).OrderBy(p => p.Precio).ToList();

            return View(lista);
        }

        public ActionResult MayorPrecio()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).OrderByDescending(p => p.Precio).ToList();

            return View(lista);
        }

        public ActionResult AlfabetoAsc()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).OrderBy(p => p.Titulo).ToList();

            return View(lista);
        }

        public ActionResult AlfabetoDesc()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).OrderByDescending(p => p.Titulo).ToList();

            return View(lista);
        }

        public ActionResult Agregar()
        {
           return View();
        }

        public ActionResult MasPopular()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).OrderByDescending(p => p.Favoritos.Count()).ToList();

            return View(lista);
        }

        public ActionResult ModalLike()
        {
            return View();
        }
    }


}

