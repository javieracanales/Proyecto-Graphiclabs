using System;
using System.Collections.Generic;

namespace Proyecto.Models;

public partial class Comentario
{
    public byte Id { get; set; }

    public byte? IdUsuario { get; set; }

    public byte? IdProducto { get; set; }

    public string? Comentario1 { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
    public virtual Producto? IdProductoNavigation { get; set;}

}
