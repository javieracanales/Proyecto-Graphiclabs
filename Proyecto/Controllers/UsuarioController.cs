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
    public class UsuarioController : Controller
    {
        ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext();

        public ActionResult MenorPrecio(byte? id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                    join u in db.Usuarios on
                                     a.IdUsuario equals u.Id
                                    join
                                        c in db.Categoria on
                                        a.IdCategoria equals c.Id
                                    where u.Id == id
                                    orderby a.Precio ascending
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

        public ActionResult MayorPrecio(byte id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                    join u in db.Usuarios on
                                     a.IdUsuario equals u.Id
                                    join
                                        c in db.Categoria on
                                        a.IdCategoria equals c.Id
                                    where u.Id == id
                                    orderby a.Precio descending
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

        public ActionResult AlfabetoAsc( byte id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                    join u in db.Usuarios on
                                     a.IdUsuario equals u.Id
                                    join
                                        c in db.Categoria on
                                        a.IdCategoria equals c.Id
                                    where u.Id == id
                                    orderby a.Titulo ascending
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

        public ActionResult AlfabetoDesc(byte id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                    join u in db.Usuarios on
                                     a.IdUsuario equals u.Id
                                    join
                                        c in db.Categoria on
                                        a.IdCategoria equals c.Id
                                    where u.Id == id
                                    orderby a.Titulo descending
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


        public ActionResult MasPopular(byte id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                    join u in db.Usuarios on
                                     a.IdUsuario equals u.Id
                                    join
                                        c in db.Categoria on
                                        a.IdCategoria equals c.Id
                                    where u.Id == id
                                    orderby a.Favoritos.Count descending
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
    }
}

