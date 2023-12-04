using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Proyecto.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.Controllers
{
    public class Layout2Controller : Controller
    {

        ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext();
        public IActionResult Index(string busqueda)
        {
            List<Producto> lista;
            lista = (
               from a in db.Productos
               join
                u in db.Usuarios on
                a.IdUsuario equals u.Id
               join
                c in db.Categoria on
                a.IdCategoria equals c.Id
               select new Producto
               {
                   Id = a.Id,
                   IdUsuarioNavigation = u,
                   IdCategoriaNavigation = c,
                   Titulo = a.Titulo,
                   Descripcion = a.Descripcion,
                   Precio = a.Precio,
                   Imagen = a.Imagen
               }).ToList();

            foreach (var item in lista)
            {
                List<Favorito> favoritos = (from f in db.Favoritos
                                            where f.IdProducto == item.Id
                                            select new Favorito
                                            {
                                                Id = f.Id
                                            }).ToList();
                item.Favoritos = favoritos;
            }

            // lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).ToList();

            if (!string.IsNullOrEmpty(busqueda))
            {
                lista = (
               from a in db.Productos
               join
                u in db.Usuarios on
                a.IdUsuario equals u.Id
               join
                c in db.Categoria on
                a.IdCategoria equals c.Id
               where a.Titulo.Contains(busqueda)
               where a.Descripcion.Contains(busqueda)
               where u.Apodo.Contains(busqueda)
               select new Producto
               {
                   Id = a.Id,
                   IdUsuarioNavigation = u,
                   IdCategoriaNavigation = c,
                   Titulo = a.Titulo,
                   Descripcion = a.Descripcion,
                   Precio = a.Precio,
                   Imagen = a.Imagen,
               }).ToList();
            }

            return View(lista);
        }

        public ActionResult IndexProducto(byte? id)
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
            var model = (from c in db.Comentarios
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
                         }).FirstOrDefault();

            if (model == null)
            {
                return RedirectToAction("IndexProducto", "Layout2", new { id });
            }

            return View(model);

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

        public ActionResult MasPopular()
        {
            List<Producto> lista;
            lista = db.Productos.Include(p => p.Favoritos).Include(p => p.IdUsuarioNavigation).Include(p => p.IdCategoriaNavigation).OrderByDescending(p => p.Favoritos.Count()).ToList();

            return View(lista);
        }

    }


}

