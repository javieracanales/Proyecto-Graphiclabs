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
    public class Layout3Controller : Controller
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

        public ActionResult Usuarios()
        {
            var model = (from u in db.Usuarios
                           join
                        r in db.Roles on u.IdRoles equals r.Id
                           select new Usuario
                           {
                               Id = u.Id,
                               Apodo = u.Apodo,
                               Email = u.Email,
                               Telefono = u.Telefono,
                               Fecha = u.Fecha,
                               IdRolesNavigation = r
                           }).ToList();
            ViewData["Baneos"] = (from b in db.Ban
                                  join
                                u in db.Usuarios on b.IdUsuario equals u.Id
                                  select new Ban
                                  {
                                    Id = b.Id,
                                    Descripcion = b.Descripcion,
                                    Estado = b.Estado,
                                    IdUsuarioNavigation = u
                                  }).ToList();
            return View(model);
        }

        public ActionResult Perfil(byte? id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                    join u in db.Usuarios on
                                     a.IdUsuario equals u.Id
                                    join
                                        c in db.Categoria on
                                        a.IdCategoria equals c.Id
                                    where u.Id == id
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

            ViewData["Favorito"] = (from f in db.Favoritos
                                    join
                                    u in db.Usuarios on f.IdUsuario equals u.Id
                                    join p in db.Productos on f.IdProducto equals p.Id
                                    where f.IdUsuario == id
                                    select new Favorito
                                    {
                                        Id = f.Id,
                                        IdUsuarioNavigation = u,
                                        IdProductoNavigation = p,
                                        Fecha = f.Fecha
                                    }).ToList();

            var model = (from u in db.Usuarios
                         where u.Id == id
                         select new Usuario
                         {
                             Id = u.Id,
                             Apodo = u.Apodo,
                             Email = u.Email,
                             Telefono = u.Telefono,
                             Fecha = u.Fecha,
                             Imagen = u.Imagen
                         }).FirstOrDefault();

            return View(model);
        }

        //public ActionResult Eliminar()
        //{
        //    return View();
        //}


        public ActionResult Eliminar(byte? id)
        {
            var productos = db.Productos.Include(p => p.IdCategoriaNavigation).Include(p => p.IdUsuarioNavigation).Where(p => p.IdUsuarioNavigation.Id == id).ToList();
            foreach (var producto in productos)
            {
                var favoritos = db.Favoritos.Include(f => f.IdUsuarioNavigation).Include(f => f.IdProductoNavigation).Where(f => f.IdProductoNavigation.Id == producto.Id).ToList();
                db.Favoritos.RemoveRange(favoritos);
                db.SaveChanges();
                var comentarios = db.Comentarios.Include(c => c.IdUsuarioNavigation).Include(c => c.IdProductoNavigation).Where(c => c.IdProductoNavigation.Id == producto.Id).ToList();
                db.Comentarios.RemoveRange(comentarios);
                db.SaveChanges();
            }
            db.Productos.RemoveRange(productos);
            db.SaveChanges();

            var usuario = db.Usuarios.FirstOrDefault();
            db.Usuarios.Remove(usuario);
            db.SaveChanges();
            return Redirect("~/Layout3/Usuarios");
        }

        public ActionResult EliminarPublicaciones(byte id)
        {
            var productos = db.Productos.Include(p => p.IdCategoriaNavigation).Include(p => p.IdUsuarioNavigation).Where(p => p.Id == id).ToList();
            foreach (var producto in productos)
            {
                var favoritos = db.Favoritos.Include(f => f.IdUsuarioNavigation).Include(f => f.IdProductoNavigation).Where(f => f.IdProductoNavigation.Id == producto.Id).ToList();
                db.Favoritos.RemoveRange(favoritos);
                db.SaveChanges();
                var comentarios = db.Comentarios.Include(c => c.IdUsuarioNavigation).Include(c => c.IdProductoNavigation).Where(c => c.IdProductoNavigation.Id == producto.Id).ToList();
                db.Comentarios.RemoveRange(comentarios);
                db.SaveChanges();
            }
            db.Productos.RemoveRange(productos);
            db.SaveChanges();
            return Redirect("~/Layout3/Index");
        }


        [HttpGet]
        public ActionResult Banear()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Banear(Ban model)
        {
            var id = db.Usuarios.Find(model.Id);

            if (ModelState.IsValid)
            {
                var modeloTabla = new Ban();
                modeloTabla.Descripcion = model.Descripcion;
                modeloTabla.Estado = model.Estado = false;
                modeloTabla.IdUsuario = model.IdUsuario = 1;
                db.Ban.Add(model);
                db.SaveChanges();
            }

            return Redirect("~/Layout3/Usuarios");
        }
    }
}

