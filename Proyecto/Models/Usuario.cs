using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Proyecto.Models;

public partial class Usuario
{
    public byte Id { get; set; }

    public string? Apodo { get; set; }

    public string? Email { get; set; }

    public string? Contrasena { get; set; }

    public int? Telefono { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Imagen { get; set; }

    public byte? IdRoles { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; } = new List<Comentario>();

    public virtual ICollection<Favorito> Favoritos { get; } = new List<Favorito>();

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();

    public virtual ICollection<Ban> Baneos { get; set; } = new List<Ban>();

    public virtual Roles? IdRolesNavigation { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Debe subir una foto para su perfil")]
    public IFormFile? File { get; set; }
}
