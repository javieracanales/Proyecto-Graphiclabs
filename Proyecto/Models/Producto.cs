using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models;

public partial class Producto
{
    public byte Id { get; set; }

    public byte? IdUsuario { get; set; }

    [Required(ErrorMessage = "Debe elegir una categoría")]
    public byte? IdCategoria { get; set; }

    [Required(ErrorMessage = "Debe ingresar un título")]
    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public int? Precio { get; set; }

    public string? Imagen { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual ICollection<Comentario> Comentarios { get; } = new List<Comentario>();

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    [Required(ErrorMessage = "Debe elegir una categoría")]
    public virtual Categoria? IdCategoriaNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Debe subir una foto para su vista previa")]
    public IFormFile? File { get; set; }

}
