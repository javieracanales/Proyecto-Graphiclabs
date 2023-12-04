using System;
using System.Collections.Generic;

namespace Proyecto.Models;

public partial class Favorito
{
    public byte Id { get; set; }

    public byte? IdUsuario { get; set; }

    public byte? IdProducto { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Producto IdProductoNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; }
}
