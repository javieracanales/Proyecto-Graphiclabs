using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Proyecto.Helpers;
using Proyecto.Models;
using Proyecto.Providers;
using static System.Net.Mime.MediaTypeNames;


namespace Proyecto.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
 
    }

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

    public ActionResult IndexFavorito(string? busqueda)
    {
        List<Favorito> lista;

        lista = (from f in db.Favoritos
                 join
                 u in db.Usuarios on f.IdUsuario equals u.Id
                 join p in db.Productos on f.IdProducto equals p.Id
                 select new Favorito
                 {
                     Id = f.Id,
                     IdUsuarioNavigation = u,
                     IdProductoNavigation = p,
                     Fecha = f.Fecha
                 }).ToList();

        if (!string.IsNullOrEmpty(busqueda))
        {
            lista = (from f in db.Favoritos
                     join
                     u in db.Usuarios on f.IdUsuario equals u.Id
                     join p in db.Productos on f.IdProducto equals p.Id
                     where p.Titulo.Contains(busqueda)
                     where u.Apodo.Contains(busqueda)
                     where p.Descripcion.Contains(busqueda)
                     select new Favorito
                     {
                         Id = f.Id,
                         IdUsuarioNavigation = u,
                         IdProductoNavigation = p,
                         Fecha = f.Fecha
                     }).ToList();
        }
        return View(lista);
    }

    public ActionResult Modal()
    {
        return View();
    }
    public ActionResult Agregar(byte? id)
    {
        if (id == null)
        {
            ViewBag.IdCategoria = new SelectList(db.Categoria, "Id", "Nombre");
        }
        else
        {
            ViewBag.IdCategoria = new SelectList(db.Categoria.Where(c => c.Id == id), "Id", "Nombre");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Agregar(Producto? producto)
    {
        byte[] bytes;
        var id = User.Claims.Where(s => s.Type == "Id").Select(s => Convert.ToByte(s.Value)).FirstOrDefault();
        using (Stream fs = producto.File.OpenReadStream())
        {
            using (BinaryReader br = new(fs))
            {
                bytes = br.ReadBytes((int)fs.Length);
                producto.Imagen = Convert.ToBase64String(bytes, 0, bytes.Length);
                var modeloTabla = new Producto();
                modeloTabla.IdUsuario = producto.IdUsuario = id;
                modeloTabla.IdCategoria = producto.IdCategoria;
                modeloTabla.Titulo = producto.Titulo;
                modeloTabla.Descripcion = producto.Descripcion;
                modeloTabla.Precio = producto.Precio;
                modeloTabla.Imagen = producto.Imagen;
                modeloTabla.Fecha = producto.Fecha = DateTime.Now;
                db.Productos.Add(producto);
                db.SaveChanges();
                return Redirect("~/Layout2/Index");
            }
        }
    }

    public ActionResult EditarModal()
    {
        return View();
    }

    [HttpGet]
    public ActionResult Eliminar(byte id)
    {
            var UserTable = db.Productos.Find(id);
            db.Productos.Remove(UserTable);
            db.SaveChanges();

        return Redirect("~/Home/Index");
    }


    public ActionResult LasReglas()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

