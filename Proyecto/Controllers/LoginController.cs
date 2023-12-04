using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.Controllers
{
    public class LoginController : Controller
    {
        private readonly ProyectoGraphiclabsContext? _context;

        ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string Email, string Contrasena)
        {
            Roles roles = new Roles();
            var model = (from u in db.Usuarios
                         where u.Email == Email
                         where u.Contrasena == Encrypt.GetSHA256(Contrasena)
            select new Usuario
                         {
                             Id = u.Id,
                             Apodo = u.Apodo,
                             Email = u.Email,
                             Contrasena = u.Contrasena,
                             Telefono = u.Telefono,
                             Fecha = u.Fecha,
                             Imagen = u.Imagen,
                             IdRoles = u.IdRoles
                         }).FirstOrDefault();


            if (model != null)
            {
                var claims = new List<Claim>
                {
                    new Claim("Id", model.Id.ToString()),
                    new Claim(ClaimTypes.Name, model.Apodo),
                    new Claim(ClaimTypes.Email, model.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                if (model.IdRoles == 1)
                {
                    return RedirectToAction("Perfil", "Layout3", new { model.Id });
                }
                else
                {
                    return RedirectToAction("Perfil", "Login", new { model.Id });
                }

            }

            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(Usuario? model)
        {
            byte[] bytes;
            try
            {
                if (ModelState.IsValid)
                {
                    using (ProyectoGraphiclabsContext db = new ProyectoGraphiclabsContext())
                    {
                        using (Stream fs = model.File.OpenReadStream())
                        {
                            using (BinaryReader br = new(fs))
                            {
                                bytes = br.ReadBytes((int)fs.Length);
                                model.Imagen = Convert.ToBase64String(bytes, 0, bytes.Length);
                                var modeloTabla = new Usuario();
                                modeloTabla.Apodo = model.Apodo;
                                modeloTabla.Email = model.Email;
                                modeloTabla.Contrasena = Encrypt.GetSHA256(model.Contrasena);
                                modeloTabla.Telefono = model.Telefono;
                                modeloTabla.Fecha = model.Fecha = DateTime.Now;
                                modeloTabla.Imagen = model.Imagen;
                                modeloTabla.IdRoles= model.IdRoles = 2;
                                db.Usuarios.Add(modeloTabla);
                                db.SaveChanges();
                            }
                        }

                        return Redirect("~/Layout2/Index");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }


        public async Task<ActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Home/Index");
        }

        public ActionResult Perfil(byte? id)
        {
            ViewData["Producto"] = (from a in db.Productos
                                                join u in db.Usuarios on
                                                 a.IdUsuario equals u.Id join
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

        [HttpGet]
        public ActionResult EditarPerfil(byte id)
        {
            var usuario = db.Usuarios.FirstOrDefault(u => u.Id == id);

            if (usuario!=null)
            {
                var model = new Usuario()
                {
                    Id = usuario.Id,
                    Apodo = usuario.Apodo,
                    Email = usuario.Email,
                    Contrasena = usuario.Contrasena,
                    Telefono = usuario.Telefono,
                    Fecha = usuario.Fecha,
                    Imagen = usuario.Imagen
                };
                return View(model);
            }
            return View(usuario);
        }


        [HttpPost]
        public ActionResult EditarPerfil(Usuario model)
        {
            if (ModelState.IsValid)
            {
                byte[] bytes;
                using (Stream fs = model.File.OpenReadStream())
                {
                    using (BinaryReader br = new(fs))
                    {
                        bytes = br.ReadBytes((int)fs.Length);
                        model.Imagen = Convert.ToBase64String(bytes, 0, bytes.Length);
                        var usuario = db.Usuarios.Find(model.Id);

                        if (usuario!=null)
                        {
                            usuario.Apodo = model.Apodo;
                            usuario.Telefono = model.Telefono;
                            usuario.Imagen = model.Imagen;
                            usuario.Contrasena = model.Contrasena;
                            db.SaveChanges();
                            return RedirectToAction("Perfil", "Login", new { model.Id });
                        }
                    }
                }
            }
            return RedirectToAction("EditarPerfil", "Login", new { model.Id });
        }

    }
}
